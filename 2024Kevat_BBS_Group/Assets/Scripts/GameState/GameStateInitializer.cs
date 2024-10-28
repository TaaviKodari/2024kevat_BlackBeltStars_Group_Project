using AtomicConsole.Engine;
using UnityEngine;

namespace GameState
{
    // Creates a game state if not present when loading a scene directly from the editor
    // Also responsible for ensuring that the variant manager loads correctly
    public class GameStateInitializer : MonoBehaviour
    {
        [SerializeField]
        private bool startGame = true;
        [SerializeField]
        private SaveGame defaultSave;
        [SerializeField]
        private AudioManager audioManagerPrefab;
        [SerializeField]
        private VariantManager variantManager;
        [SerializeField]
        private AtomicConsoleEngine consolePrefab;
        
        public void Awake()
        {
            variantManager.Init();
            if (startGame && FindObjectOfType<GameStateManager>() == null)
            {
                var manager = CreateStateManager(defaultSave);
                manager.currentSaveGame.SaveName = "Dev Game";
                manager.GenerateMaps();
                manager.currentMap = manager.currentSaveGame.maps.map1;
            }
            if (FindObjectOfType<AudioManager>() == null)
            {
                var manager = Instantiate(audioManagerPrefab);
                manager.name = audioManagerPrefab.name;
                DontDestroyOnLoad(manager);
            }

            if (FindObjectOfType<AtomicConsoleEngine>() == null)
            {
                Instantiate(consolePrefab);
            }
            
            DestroyImmediate(gameObject);
        }

        public static GameStateManager CreateStateManager(SaveGame saveGame)
        {
            var stateObject = new GameObject("Game State");
            var stateComponent = stateObject.AddComponent<GameStateManager>();
            stateComponent.LoadGame(saveGame);
            return stateComponent;
        }
    }
}