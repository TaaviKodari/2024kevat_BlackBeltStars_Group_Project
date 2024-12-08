using UnityEngine;

namespace Portal
{
    public class PortalArrow : MonoBehaviour
    {
        public Transform portal;
        [SerializeField]
        private Vector2 margin;

        private void Update()
        {
            var cam = Camera.main;
            if (portal == null || cam == null) return;

            var arrowRect = new Rect(cam.pixelRect.position + margin, cam.pixelRect.size - margin * 2);

            var portalScreenPos = cam.WorldToScreenPoint(portal.position);

            var aboveArrowPos = portalScreenPos + Vector3.up * 128;
            if (arrowRect.Contains(aboveArrowPos))
            {
                transform.position = aboveArrowPos;
                transform.rotation = Quaternion.identity;
                return;
            }

            var belowArrowPos = portalScreenPos + Vector3.down * 128;
            if (arrowRect.Contains(belowArrowPos))
            {
                transform.position = belowArrowPos;
                transform.rotation = Quaternion.Euler(0, 0, 180);
                return;
            }

            var angle = Mathf.Atan2(portalScreenPos.y - arrowRect.center.y, portalScreenPos.x - arrowRect.center.x) * Mathf.Rad2Deg + 90;
            transform.position = PointOnRect(arrowRect, angle - 90);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Based on: https://discussions.unity.com/t/finding-point-on-a-bounds-2d-rectangle-using-its-centre-and-an-angle/170079/2
        // Heavily tweaked to make it work correctly
        private static Vector2 PointOnRect(Rect bounds, float angle)
        {
            var dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            var e = bounds.size / 2;
            var x = e.y * dir.x / dir.y;
            var y = e.x * dir.y / dir.x;
            var x2 = e.x * Mathf.Sign(dir.x);
            var y2 = e.y * Mathf.Sign(dir.y);
            return bounds.center + (Mathf.Abs(y) < e.y ? new Vector2(x2, dir.x > 0 ? y : -y) : new Vector2(dir.y > 0 ? x : -x, y2));
        }
    }
}