using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Pathfinding
{
    // A* Pathfinder adapted from https://en.wikipedia.org/wiki/A%2A_search_algorithm#Pseudocode
    internal class PathFinder
    {
        private readonly PathfindingManager manager;
        private readonly Dictionary<NodePos, PathNode> nodeCache = new();

        public PathFinder(PathfindingManager manager)
        {
            this.manager = manager;
        }

        // Main pathfinding function adapted from wikipedia pseudocode
        [CanBeNull]
        internal Path FindPath(NodePos from, NodePos to, float maxDistance)
        {
            if (Distance(from, to) > maxDistance) return null;
            
            var openSet = new PriorityQueue();
            openSet.Push(from, Distance(from, to));
            var cameFrom = new Dictionary<NodePos, NodePos>();
            var gScore = new Dictionary<NodePos, float>
            {
                [from] = 0
            };
            var fScore = new Dictionary<NodePos, float>
            {
                [from] = Distance(from, to)
            };

            while (!openSet.IsEmpty())
            {
                var current = openSet.Pop();
                if (current.Equals(to))
                {
                    nodeCache.Clear();
                    return BuildPath(cameFrom, current); // Build path and return
                }

                foreach (var neighbour in Neighbours(current))
                {
                    if (Distance(from, neighbour) > maxDistance) continue;
                    
                    var tentativeGScore = gScore.GetValueOrDefault(current, float.PositiveInfinity) + Distance(current, neighbour);
                    if (!(tentativeGScore < gScore.GetValueOrDefault(neighbour, float.PositiveInfinity))) continue;
                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentativeGScore;
                    fScore[neighbour] = tentativeGScore + Distance(neighbour, to);
                    if (!openSet.Has(neighbour)) openSet.Push(neighbour, fScore[neighbour]);
                }
            }

            nodeCache.Clear();
            return null;
        }

        // Builds the final path after we've found one
        private Path BuildPath(IDictionary<NodePos, NodePos> cameFrom, NodePos current)
        {
            var path = new List<NodePos> { current };

            while (cameFrom.TryGetValue(current, out current))
            {
                path.Add(current);
            }

            path.Reverse();
            return new Path(path, manager);
        }
        
        private float Distance(NodePos from, NodePos to)
        {
            return (to.Pos - from.Pos).magnitude + (GetNode(from).Type.Cost() + GetNode(to).Type.Cost()) / 2;
        }

        private PathNode GetNode(NodePos pos)
        {
            if (!nodeCache.TryGetValue(pos, out var node))
            {
                node = manager.GetNode(pos);
                nodeCache[pos] = node;
            }

            return node;
        }

        // Generates all neighbours of a node
        private IEnumerable<NodePos> Neighbours(NodePos pos)
        {
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x != 0 || y != 0)
                    {
                        yield return new NodePos(pos.Pos + new Vector2Int(x, y), manager);
                    }
                }
            }
        }
    }

    public class Path
    {
        private readonly PathfindingManager manager;
        public readonly IList<NodePos> Nodes;

        internal Path(IList<NodePos> nodes, PathfindingManager manager)
        {
            Nodes = nodes;
            this.manager = manager;
        }

        public void DrawGizmo()
        {
            Gizmos.DrawLineStrip(Nodes.Select(pos => (Vector3) manager.GetNodePos(pos)).ToArray(), false);
        } 
    }

    // Janky "priority queue" based on SortedSet because a proper one is too much work, and we're stuck on old .NET
    internal class PriorityQueue
    {
        private readonly SortedSet<Entry> entries = new(EntryComparer.Instance);
        private readonly HashSet<NodePos> positions = new();

        public void Push(NodePos pos, float priority)
        {
            entries.Add(new Entry { Priority = priority, Pos = pos });
            positions.Add(pos);
        }

        public bool IsEmpty()
        {
            return entries.Count == 0;
        }

        public NodePos Pop()
        {
            var min = entries.Min;
            entries.Remove(min);
            positions.Remove(min.Pos);
            return min.Pos;
        }

        public bool Has(NodePos pos)
        {
            return positions.Contains(pos);
        }

        private struct Entry
        {
            internal NodePos Pos;
            internal float Priority;
        }

        private class EntryComparer : Comparer<Entry>
        {
            public static readonly EntryComparer Instance = new();

            public override int Compare(Entry x, Entry y)
            {
                var priorityComparison = Comparer<float>.Default.Compare(x.Priority, y.Priority);
                if (priorityComparison != 0) return priorityComparison;
                var xComparison = Comparer<int>.Default.Compare(x.Pos.Pos.x, y.Pos.Pos.x);
                if (xComparison != 0) return xComparison;
                return Comparer<int>.Default.Compare(x.Pos.Pos.y, y.Pos.Pos.y);
            }
        }
    }
}