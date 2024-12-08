using System.Linq;
using AtomicConsole;
using Pause;
using Portal;
using UnityEngine;
using UnityEngine.Events;

namespace GameState
{
    public class InGameManager : MonoBehaviour
    {
        public static InGameManager Instance { get; private set; }

        [SerializeField]
        private SceneTransition transition;

        public MasterInput Input { get; private set; }

        private int antsKilled;
        private int wavesSurvived;
        private GameStateManager manager;

        [SerializeField]
        private UnityEvent onWin;
        [SerializeField]
        private UnityEvent onLose;
        
        private void Awake()
        {
            Instance = this;
            Input = new MasterInput();
            Input.Enable();
            PauseManager.OnPause.AddListener(OnPause);
            PauseManager.OnUnpause.AddListener(OnUnpause);
        }

        private void Start()
        {
            manager = FindObjectOfType<GameStateManager>();
        }

        private void OnDestroy()
        {
            PauseManager.OnPause.RemoveListener(OnPause);
            PauseManager.OnUnpause.RemoveListener(OnUnpause);
        }

        private void OnPause()
        {
            Input.Building.Disable();
            Input.Player.Disable();
        }

        private void OnUnpause()
        {
            Input.Building.Enable();
            Input.Player.Enable();
        }

        [AtomicCommand("GameState", "FadeGameEnd", "Fade to game end screen. Arguments: <won: bool>")]
        public void EndGame(bool won)
        {
            if (won)
            {
                manager.currentSaveGame.resources.diamonds += manager.currentMap.diamondCount;
                onWin.Invoke();
            }
            else
            {
                onLose.Invoke();
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
            FindObjectOfType<PortalManager>().SpawnPortal();
        }

        private bool HasWon()
        {
            return GetGoalProgress() >= GetGoalTarget();
        }

        public int GetGoalTarget()
        {
            return manager.currentMap.goal switch
            {
                KillAntsMapGoal goal => goal.amount,
                SurviveWavesMapGoal goal => goal.amount,
                _ => int.MaxValue
            };
        }

        public int GetGoalProgress()
        {
            return manager.currentMap.goal switch
            {
                KillAntsMapGoal => antsKilled,
                SurviveWavesMapGoal => wavesSurvived,
                _ => 0
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