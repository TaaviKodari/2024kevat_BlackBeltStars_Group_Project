using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameState
{
    public class LiveGameTracker : MonoBehaviour
    {
        public static LiveGameTracker Instance { get; private set; }
        
        private int antsKilled;
        private int wavesSurvived;
        private GameStateManager manager;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            manager = FindObjectOfType<GameStateManager>();
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
            manager.GenerateMaps();
            SceneManager.LoadScene("WorldSelect");
        }

        public void LoseGame()
        {
            manager.GenerateMaps();
            SceneManager.LoadScene("WorldSelect");
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