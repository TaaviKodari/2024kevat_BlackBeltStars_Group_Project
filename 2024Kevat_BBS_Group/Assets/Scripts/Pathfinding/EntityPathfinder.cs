using UnityEngine;
using Random = UnityEngine.Random;

namespace Pathfinding
{
    // Script used on any GameObject that needs to pathfind
    // Handles updating paths when necessary and calculating the correct travel direction
    // Other scripts must be used to set the target position and to move the object
    public class EntityPathfinder : MonoBehaviour
    {
        private PathfindingManager manager;

        [Tooltip("Maximum distance away from the start that will be searched during pathfinding")]
        [SerializeField]
        private float maxDistance = 50;
        [Tooltip("Offset from object origin for the pathfinding position. Rendered as a gray box gizmo")]
        [SerializeField]
        private Vector2 navPosOffset = new(0, 0);
        [Tooltip("The maximum speed that the movement direction can change at in degrees per second.")]
        [SerializeField]
        private float rotationSpeed = 90;

        private float recalculationTimer = 2;
        private Vector2 pathTarget;
        private Vector2 target;
        private Path path;
        private int pathIndex;
        private bool hasTarget;
        
        private Vector2 moveDirection;
        
        private void Start()
        {
            manager = FindObjectOfType<PathfindingManager>();
        }

        private void Update()
        {
            // Periodically recalculate paths in order to avoid getting stuck on bad ones
            recalculationTimer -= Time.deltaTime;
            if (recalculationTimer < 0)
            {
                RecomputePath();
                // Introduce random timing here to avoid all enemies recomputing during the same frame
                recalculationTimer = Random.Range(1.5f, 2.5f);
            }
            UpdatePath();
            UpdateDirection();
        }

        // Renders lines and boxes that help with debugging pathfinding
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
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(GetNavPos(), GetNavPos() + moveDirection);
        }

        // Sets the pathfinding target. If it differs enough from the precious target, then the path will be recomputed.
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

        // Clears the current pathfinding target
        public void ClearTarget()
        {
            hasTarget = false;
            path = null;
            pathIndex = 0;
        }

        private void UpdateDirection()
        {
            // Make sure there is a path to walk along
            if (!hasTarget || path == null)
            {
                moveDirection = Vector2.zero;
                return;
            }

            var pathNodes = path.Nodes;
            if (pathIndex >= pathNodes.Count)
            {
                moveDirection = Vector2.zero;
                return;
            }

            // Calculate direction
            var aimPos = GetAimPos();
            var currentPos = GetNavPos();
            var targetDirection = (aimPos - currentPos).normalized;
            
            // If we're still, instantly change direction
            if (moveDirection.sqrMagnitude < 0.01f)
            {
                moveDirection = targetDirection;
                return;
            }

            // Calculate how much we are allowed to rotate and limit the amount we want to rotate to that.
            var allowedAngle = rotationSpeed * Time.deltaTime;
            var angle = Mathf.Clamp(Vector2.SignedAngle(moveDirection, targetDirection), -allowedAngle, allowedAngle);

            // Rotate the movement direction
            moveDirection = Quaternion.AngleAxis(angle, Vector3.forward) * moveDirection;
        }

        // Gets the direction the pathfinder wants to move in
        public Vector2 GetDirection()
        {
            return moveDirection;
        }

        // Gets the point we are currently walking towards
        private Vector2 GetAimPos()
        {
            var pathNodes = path.Nodes;
            // If we only have one node left, walk there
            if (pathIndex == pathNodes.Count - 1)
            {
                return manager.GetNodePos(pathNodes[pathIndex]);
            }

            // Walk towards the point between the next two nodes for a smoother path.
            // TODO: A better interpolation would be nice
            var nearPos = manager.GetNodePos(pathNodes[pathIndex]);
            var farPos = manager.GetNodePos(pathNodes[pathIndex + 1]);
            return new Vector2((nearPos.x + farPos.x) / 2, (nearPos.y + farPos.y) / 2);
        }

        // Recalculates the path to the target
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

        // Updates which node along the path we are walking to
        // Also makes sure that we update the path if we get too far away from it
        private void UpdatePath()
        {
            if (!hasTarget || path == null) return;
            var pathNodes = path.Nodes;
            
            var nodePos = manager.GetNodePos(pathNodes[pathIndex]);
            var currentPos = GetNavPos();
            var sqrDistance = (nodePos - currentPos).sqrMagnitude;
            if (sqrDistance > 25) RecomputePath();
            if (sqrDistance > 0.1) return;
            
            if (pathIndex + 1 >= pathNodes.Count)
            {
                ClearTarget();
            }
            else
            {
                pathIndex++;
            }
        }

        // Gets the position the pathfinding agent is currently at
        private Vector2 GetNavPos()
        {
            return (Vector2)transform.position + navPosOffset;
        }
    }
}