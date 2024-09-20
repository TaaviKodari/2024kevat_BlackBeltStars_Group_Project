using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pathfinding
{
    public class EntityPathfinder : MonoBehaviour
    {
        private PathfindingManager manager;

        [SerializeField]
        private float maxDistance = 50;
        [SerializeField]
        private Vector2 navPosOffset = new(0, 0);

        private float recalculationTimer = 2;
        private Vector2 pathTarget;
        private Vector2 target;
        private Path path;
        private int pathIndex;
        private bool hasTarget;
        
        private void Start()
        {
            manager = FindObjectOfType<PathfindingManager>();
        }

        private void Update()
        {
            recalculationTimer -= Time.deltaTime;
            if (recalculationTimer < 0)
            {
                RecomputePath();
                // Introduce random timing here to avoid all enemies recomputing during the same frame
                recalculationTimer = Random.Range(1.5f, 2.5f);
            }
            UpdatePath();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawCube(transform.position + (Vector3)navPosOffset, new Vector3(0.1f, 0.1f, 0.1f));
            
            if (!hasTarget || path == null) return;
            Gizmos.color = Color.cyan;
            path.DrawGizmo();
            if (pathIndex < path.Nodes.Count)
            {
                Gizmos.color = Color.green;
                var aimPos = GetAimPos();
                Gizmos.DrawCube(aimPos, new Vector3(0.1f, 0.1f, 0.1f));
                Gizmos.DrawLine(GetNavPos(), aimPos);
            }
            if (path.Nodes.Count >= 2)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(manager.GetNodePos(path.Nodes[0]), new Vector3(0.1f, 0.1f, 0.1f));
                Gizmos.DrawCube(manager.GetNodePos(path.Nodes[^1]), new Vector3(0.1f, 0.1f, 0.1f));
            }
        }

        public void SetTarget(Vector2 newTarget)
        {
            target = newTarget;
            
            // If we already have a target and the path was computed close to the new target 
            // then we just record the new target and keep the path to avoid recomputing it for small changes
            if (hasTarget && (pathTarget - newTarget).sqrMagnitude < 4)
            {
                return;
            }

            hasTarget = true;
            RecomputePath();
        }

        public void ClearTarget()
        {
            hasTarget = false;
            path = null;
            pathIndex = 0;
        }

        public Vector2 GetDirection()
        {
            if (!hasTarget || path == null) return Vector2.zero;
            var pathNodes = path.Nodes;
            if (pathIndex >= pathNodes.Count) return Vector2.zero;

            var aimPos = GetAimPos();
            var currentPos = GetNavPos();
            return (aimPos - currentPos).normalized;
        }

        private Vector2 GetAimPos()
        {
            var pathNodes = path.Nodes;
            if (pathIndex == pathNodes.Count - 1)
            {
                return manager.GetNodePos(pathNodes[pathIndex]);
            }

            var nearPos = manager.GetNodePos(pathNodes[pathIndex]);
            var farPos = manager.GetNodePos(pathNodes[pathIndex + 1]);
            return new Vector2((nearPos.x + farPos.x) / 2, (nearPos.y + farPos.y) / 2);
        }

        private void RecomputePath()
        {
            if (!hasTarget) return;
            pathTarget = target;
            var start = GetNavPos();

            path = manager.FindPath(start, target, maxDistance);
            if (path == null)
            {
                Debug.Log("Failed to compute path", this);
            }
            pathIndex = 0;
        }

        private void UpdatePath()
        {
            if (!hasTarget || path == null) return;
            var pathNodes = path.Nodes;
            
            var nodePos = manager.GetNodePos(pathNodes[pathIndex]);
            var currentPos = GetNavPos();
            var sqrDistance = (nodePos - currentPos).sqrMagnitude;
            if (sqrDistance > 0.1) return;
            if (sqrDistance > 25)
            {
                RecomputePath();
            }

            if (pathIndex + 1 >= pathNodes.Count)
            {
                ClearTarget();
            }
            else
            {
                pathIndex++;
            }
        }

        private Vector2 GetNavPos()
        {
            return (Vector2)transform.position + navPosOffset;
        }
    }
}