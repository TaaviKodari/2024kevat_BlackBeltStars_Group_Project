using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pathfinding
{
    // The main manager of pathfinding
    // Keeps all the chunks and pathfinding settings
    public class PathfindingManager : MonoBehaviour
    {
        [SerializeField]
        internal Vector2Int chunkSize = new(10, 10);
        [SerializeField]
        internal Vector2 nodeDensity = new(0.5f, 0.5f);
        [SerializeField]
        internal Vector2 colliderSize = new(2.2f, 1.2f);
        [SerializeField]
        internal DebugOptions debugOptions;

        private readonly PathFinder pathFinder;
        private readonly Dictionary<Vector2Int, PathChunk> chunks = new();

        public PathfindingManager()
        {
            pathFinder = new PathFinder(this);
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var chunk in chunks.Values)
            {
                // chunk.RenderGizmos();
            }
        }

        // Finds a path
        [CanBeNull]
        public Path FindPath(Vector2 from, Vector2 to, float maxDistance = 50)
        {
            return pathFinder.FindPath(GetClosestNode(from), FindValidNodeNear(GetClosestNode(to)), maxDistance);
        }

        // Updates the nodes in chunks within a certain area
        public void UpdateChunks(Vector2 pos, Vector2 size)
        {
            size += colliderSize;
            var min = pos - size / 2;
            var max = pos + size / 2;
            var minNode = GetClosestNode(min);
            var maxNode = GetClosestNode(max);

            for (var x = minNode.ChunkPos.x; x <= maxNode.ChunkPos.x; x++)
            {
                for (var y = minNode.ChunkPos.y; y <= maxNode.ChunkPos.y; y++)
                {
                    UpdateChunk(new Vector2Int(x, y));
                }
            }
        }

        private void UpdateChunk(Vector2Int chunkPos)
        {
            if (!chunks.TryGetValue(chunkPos, out var chunk))
            {
                chunk = new PathChunk(chunkPos, this);
                chunks[chunkPos] = chunk;
            }
            
            chunk.Update();
        }

        // Gets the in world position of a node
        public Vector2 GetNodePos(NodePos node)
        {
            return node.Pos * nodeDensity;
        }
        
        // Gets the closes node to an in world position
        public NodePos GetClosestNode(Vector2 pos)
        {
            return new NodePos(new Vector2Int(Mathf.RoundToInt(pos.x / nodeDensity.x), Mathf.RoundToInt(pos.y / nodeDensity.y)), this);
        }

        // Walks randomly to nodes around target in search of a valid one
        private NodePos FindValidNodeNear(NodePos pos)
        {
            var moves = 0;
            var candidate = pos;
            while (GetNode(candidate).Type == NodeType.Blocked && moves < 5)
            {
                candidate = new NodePos(pos.Pos + new Vector2Int(Random.Range(-1, 1), Random.Range(-1, 1)), this);
                moves++;
            }

            return candidate;
        }
        
        // Gets the data of a node at a certain positon
        public PathNode GetNode(NodePos nodePos)
        {
            if (chunks.TryGetValue(nodePos.ChunkPos, out var chunk))
            {
                return chunk.GetNode(nodePos);
            }

            return new PathNode(0);
        }
    }

    [Serializable]
    internal class DebugOptions
    {
        public Gradient costGradient = new();
    }
}