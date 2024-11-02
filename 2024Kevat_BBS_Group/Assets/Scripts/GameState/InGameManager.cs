using AtomicConsole;
using AtomicConsole.Engine;
using UnityEngine;

namespace GameState
{
    public class InGameManager : MonoBehaviour
    {
        public static InGameManager Instance { get; private set; }
        
        private int portalPlacementLayerMask;

        [SerializeField]
        private SceneTransition transition;

        public MasterInput Input { get; private set; }
        public bool Paused { get; private set; }

        private int antsKilled;
        private int wavesSurvived;
        private GameStateManager manager;

        public GameObject portalPrefab;
        public Transform playerTransform;
        private Vector2 PortalLocation;
        private bool portalActive = false;
        public GameObject portalPopUp;
        public PopUpManager popUpManager;
        
        private void Awake()
        {
            Instance = this;
            Input = new MasterInput();
            Input.Enable();
        }

        private void Start()
        {
            manager = FindObjectOfType<GameStateManager>();
            popUpManager = FindObjectOfType<PopUpManager>();
            portalPlacementLayerMask = LayerMask.GetMask("Buildings", "Terrain Obstacles");
        }

        public void Pause()
        {
            if (Paused) return;
            Input.Player.Disable();
            Input.Building.Disable();
            Time.timeScale = 0f;
            Paused = true;
        }

        public void Unpause()
        {
            if (!Paused) return;
            Input.Player.Enable();
            Input.Building.Enable();
            Time.timeScale = 1f;
            Paused = false;
        }

        public void PlayerTouchedPortal()
        {
            SendToLobby();
        }
        
        [AtomicCommand(name:"SpawnPortal",group:"world",description:"Spawns the portal")]
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
        }

        private Vector2 PickPortalLocation()
        {
            var playerPos = (Vector2)playerTransform.position;
            var angle = Random.value * 2 * Mathf.PI;
            var distance = 10 + (Random.value - 0.5f) * 5;

            return playerPos + new Vector2(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);
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