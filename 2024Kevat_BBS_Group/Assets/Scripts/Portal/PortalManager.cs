using AtomicConsole;
using UnityEngine;

namespace Portal
{
    public class PortalManager : MonoBehaviour
    {
        private int portalPlacementLayerMask;

        public GameObject portalPrefab;
        private bool portalActive;
        public GameObject portalPopUp;
        public Transform playerTransform;
        public PopUpManager popUpManager;
        [SerializeField]
        private PortalArrow indicatorArrow;

        private void Start()
        {
            portalPlacementLayerMask = LayerMask.GetMask("Buildings", "Terrain Obstacles");
        }

        [AtomicCommand("PortalManager", "SpawnPortal", "Spawns the portal")]
        public void SpawnPortal()
        {
            if (portalActive) return;
            portalActive = true;

            var portalLocation = PickPortalLocation();
            var portal = Instantiate(portalPrefab, portalLocation, Quaternion.identity);

            for (var i = 0; i < 5; i++)
            {
                if (Physics2D.OverlapBox(portalLocation, Vector2.one, 0, portalPlacementLayerMask))
                {
                    print($"Portal placement failed at ${portalLocation}, retrying");
                    portalLocation = PickPortalLocation();
                    portal.transform.position = portalLocation;
                }
                else
                {
                    break;
                }
            }

            print($"Spawned the portal at {portalLocation}");
            popUpManager.SetActivePopUp(portalPopUp, true);

            indicatorArrow.portal = portal.transform;
            indicatorArrow.gameObject.SetActive(true);
        }

        private Vector2 PickPortalLocation()
        {
            var playerPos = (Vector2)playerTransform.position;
            var angle = Random.value * 2 * Mathf.PI;
            var distance = 10 + (Random.value - 0.5f) * 5;

            return playerPos + new Vector2(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);
        }
    }
}