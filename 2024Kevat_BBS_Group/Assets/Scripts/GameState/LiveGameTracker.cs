using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameState
{
    public class LiveGameTracker : MonoBehaviour
    {
        public static LiveGameTracker Instance { get; private set; }

        [SerializeField]
        private SceneTransition transition;

        private int antsKilled;
        private int wavesSurvived;
        private GameStateManager manager;

        public GameObject portalPrefab;
        public Transform playerTransform;
        private Vector2 PortalLocation;
        private bool portalActive = false;
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            manager = FindObjectOfType<GameStateManager>();
        }

        public void PlayerTouchedPortal()
        {
            SendToLobby();
        }
        
        private void SpawnPortal()
        {
            // Set adjustedPosition to player position with y increased by 10
            Vector3 adjustedPosition = playerTransform.position;
            adjustedPosition.y += 10;

            // Instantiate the portalPrefab at the adjusted position
            if (!portalActive)
            {
                portalActive = true;
                Instantiate(portalPrefab, adjustedPosition, Quaternion.identity);
            }
        }

        
        public void AddKilledAnt()
        {
            antsKilled++;
            CheckWin();
        }

        public void AddWaveSurvived()
        {
            wavesSurvived++;
            CheckWin();
        }

        private void CheckWin()
        {
            if (!HasWon()) return;
            SpawnPortal();
        }

        private void SendToLobby()
        {
            manager.GenerateMaps();
            transition.LoadScene("WorldSelect");
        }
        
        public void LoseGame()
        {
            manager.GenerateMaps();
            transition.LoadScene("WorldSelect");
        }

        private bool HasWon()
        {
            return manager.currentMap.goal switch
            {
                KillAntsMapGoal goal => antsKilled >= goal.amount,
                SurviveWavesMapGoal goal => wavesSurvived >= goal.amount,
                _ => false
            };
        }
    }
}