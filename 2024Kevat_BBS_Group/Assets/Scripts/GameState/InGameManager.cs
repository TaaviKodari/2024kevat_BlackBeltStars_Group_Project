using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameState
{
    public class InGameManager : MonoBehaviour
    {
        public static InGameManager Instance { get; private set; }

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
                popUpManager.SetActivePopUp(portalPopUp, true);
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