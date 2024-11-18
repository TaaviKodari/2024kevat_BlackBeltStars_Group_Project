using System.Linq;
using AtomicConsole;
using UnityEngine;
using UnityEngine.Events;

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
        private bool portalActive = false;
        public GameObject portalPopUp;
        public PopUpManager popUpManager;

        [SerializeField]
        private UnityEvent onWin;
        [SerializeField]
        private UnityEvent onLose;
        
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

        [AtomicCommand("GameState", "FadeGameEnd", "Fade to game end screen. Arguments: <won: bool>")]
        public void EndGame(bool won)
        {
            if (won)
            {
                onWin.Invoke();
            }
            else
            {
                onLose.Invoke();
            }
        }
        
        [AtomicCommand("GameState", "SpawnPortal", "Spawns the portal")]
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

        private bool HasWon()
        {
            return manager.currentMap.goal switch
            {
                KillAntsMapGoal goal => antsKilled >= goal.amount,
                SurviveWavesMapGoal goal => wavesSurvived >= goal.amount,
                _ => false
            };
        }

        [AtomicCommand("GameState", "NextRound", "Open the map select screen for the next round")]
        public void PrepareNextRound()
        {
            manager.GenerateMaps();
            transition.LoadScene("WorldSelect");
        }

        [AtomicCommand("GameState", "RollBack", "Load the last save with the same name")]
        public void RollBackGame()
        {
            var saves = SaveManager.LoadGames().FindAll(game => game.SaveName == manager.currentSaveGame.SaveName);
            if (saves.Count == 0)
            {
                Debug.LogError("Cannot load previous save (not found)", this);
                AbandonGame();
                return;
            }
            if (saves.Count > 0)
            {
                Debug.LogWarning("Multiple saves with the same name found, loading first", this);
            }
            manager.LoadGame(saves.First());
            transition.LoadScene("WorldSelect");
        }

        [AtomicCommand("GameState", "AbandonGame", "Return to main menu")]
        public void AbandonGame()
        {
            Destroy(manager.gameObject);
            transition.LoadScene("MainMenu");
        }
    }
}