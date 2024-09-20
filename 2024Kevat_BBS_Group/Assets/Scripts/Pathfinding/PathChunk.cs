using System;
using UnityEditor;
using UnityEngine;

namespace Pathfinding
{
    // A chunk of pathfinding nodes
    // This class handles storing and updating the nodes in a chunk
    internal class PathChunk
    {
        // Layer mask for the types of objects that affect pathfinding.
        private static readonly int CollisionMask = LayerMask.GetMask("Buildings", "Terrain Obstacles");
        private readonly Vector2Int pos;
        private readonly PathNode[,] nodes;
        private readonly PathfindingManager manager;

        internal PathChunk(Vector2Int pos, PathfindingManager manager)
        {
            this.pos = pos;
            var size = manager.chunkSize;
            nodes = new PathNode[size.x, size.y];
            this.manager = manager;
        }

        private Vector2 GetNodePos(Vector2Int node)
        {
            var chunkWorldSize = manager.chunkSize * manager.nodeDensity;
            return chunkWorldSize * pos + node * manager.nodeDensity;
        }

        internal void RenderGizmos()
        {
            for (var x = 0; x < nodes.GetLength(0); x++)
            {
                for (var y = 0; y < nodes.GetLength(1); y++)
                {
                    var nodePos = GetNodePos(new Vector2Int(x, y));
                    
                    var editorCamera = SceneView.currentDrawingSceneView.camera;
                    var viewportPos = editorCamera.WorldToViewportPoint(nodePos);
                    if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1) continue;

                    var cost = nodes[x, y].Type.Cost();
                    Gizmos.color = cost > 10 ? Color.black : manager.debugOptions.costGradient.Evaluate(cost / 10);
                    Gizmos.DrawCube(nodePos, new Vector3(0.1f, 0.1f, 0.1f));
                }
            }
        }
        
        internal void Update()
        {
            // We use a single array for all collision checks to avoid allocations
            var colliderArray = new Collider2D[4];
            
            for (var x = 0; x < nodes.GetLength(0); x++)
            {
                for (var y = 0; y < nodes.GetLength(1); y++)
                {
                    // Loop over every matching container in every node and combine them for a node cost
                    var colliders = Physics2D.OverlapBoxNonAlloc(GetNodePos(new Vector2Int(x, y)), manager.colliderSize, 0, colliderArray, CollisionMask);
                    var cost = NodeType.Open;
                    for (var i = 0; i < colliders; i++)
                    {
                        var layer = colliderArray[i].gameObject.layer;
                        if (layer == LayerMask.NameToLayer("Buildings"))
                        {
                            cost = NodeTypes.Max(cost, NodeType.Building);
                        } 
                        else if (layer == LayerMask.NameToLayer("Terrain Obstacles"))
                        {
                            cost = NodeTypes.Max(cost, NodeType.Blocked);
                        } 
                    }

                    // Update the node with the cost
                    nodes[x, y] = new PathNode(cost);
                }
            }
        }

        // Gets a node from this chunk.
        // Providing a position outside this chunk will cause the wrong node to be returned.
        internal PathNode GetNode(NodePos nodePos)
        {
            return nodes[nodePos.SubPos.x, nodePos.SubPos.y];
        }
    }

    // Data associated with a pathfinding node.
    public readonly struct PathNode
    {
        public readonly NodeType Type;

        public PathNode(NodeType type)
        {
            Type = type;
        }
    }

    // Utilities for working with node types
    public static class NodeTypes
    {
        // Calculates the cost of a node type
        public static float Cost(this NodeType nodeType)
        {
            return nodeType switch
            {
                NodeType.Open => 0,
                NodeType.Blocked => float.PositiveInfinity,
                NodeType.Building => 5,
                _ => throw new ArgumentOutOfRangeException(nameof(nodeType), nodeType, "Unexpected node type")
            };
        }

        // Gets the more expensive of two node types
        public static NodeType Max(NodeType a, NodeType b)
        {
            return a.Cost() > b.Cost() ? a : b;
        }
    }

    public enum NodeType
    {
        Open, Blocked, Building
    }
    
    // A node position
    // Additionally stores the chunk it belongs to and its position within said chunk for convenience and fast access
    public readonly struct NodePos : IEquatable<NodePos>
    {
        public readonly Vector2Int Pos;
        public readonly Vector2Int ChunkPos;
        public readonly Vector2Int SubPos;

        public NodePos(Vector2Int pos, PathfindingManager manager)
        {
            Pos = pos;
            ChunkPos = new Vector2Int(Mathf.FloorToInt((float) pos.x / manager.chunkSize.x), Mathf.FloorToInt((float) pos.y / manager.chunkSize.y));
            SubPos = pos - ChunkPos * manager.chunkSize;
        }

        public override string ToString()
        {
            return $"{nameof(NodePos)}{Pos}";
        }

        public bool Equals(NodePos other)
        {
            return Pos.Equals(other.Pos);
        }

        public override bool Equals(object obj)
        {
            return obj is NodePos other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Pos.GetHashCode();
        }
    }
}