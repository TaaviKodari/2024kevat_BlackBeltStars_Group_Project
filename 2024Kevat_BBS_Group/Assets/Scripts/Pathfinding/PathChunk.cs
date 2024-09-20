using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pathfinding
{
    public class PathChunk
    {
        private static readonly int CollisionMask = LayerMask.GetMask("Buildings", "Terrain Obstacles");
        private readonly Vector2Int pos;
        private readonly PathNode[,] nodes;
        private readonly PathfindingManager manager;

        public PathChunk(Vector2Int pos, PathfindingManager manager)
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

        public void RenderGizmos()
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

        public void Update()
        {
            var colliderArray = new Collider2D[4];
            for (var x = 0; x < nodes.GetLength(0); x++)
            {
                for (var y = 0; y < nodes.GetLength(1); y++)
                {
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

                    nodes[x, y] = new PathNode(cost);
                }
            }
        }

        public PathNode GetNode(NodePos nodePos)
        {
            return nodes[nodePos.SubPos.x, nodePos.SubPos.y];
        }
    }

    public readonly struct PathNode
    {
        public readonly NodeType Type;

        public PathNode(NodeType type)
        {
            Type = type;
        }
    }

    public static class NodeTypes
    {
        public static float Cost(this NodeType nodeType)
        {
            return nodeType switch
            {
                NodeType.Open => 0,
                NodeType.Blocked => 5,
                NodeType.Building => float.PositiveInfinity,
                _ => throw new ArgumentOutOfRangeException(nameof(nodeType), nodeType, "Unexpected node type")
            };
        }

        public static NodeType Max(NodeType a, NodeType b)
        {
            return a.Cost() > b.Cost() ? a : b;
        }
    }

    public enum NodeType
    {
        Open, Blocked, Building
    }
    
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