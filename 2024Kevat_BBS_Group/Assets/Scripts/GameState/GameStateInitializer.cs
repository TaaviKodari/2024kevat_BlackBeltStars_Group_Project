using UnityEngine;

namespace GameState
{
    // Creates a game state if not present when loading a scene directly from the editor
    // Also responsible for ensuring that the variant manager loads correctly
    public class GameStateInitializer : MonoBehaviour
    {
        [SerializeField]
        private SaveGame defaultSave;
        [SerializeField]
        private VariantManager variantManager;
        
        public void Awake()
        {
            variantManager.Init();
            if (FindObjectOfType<GameStateManager>() == null)
            {
                var manager = CreateStateManager(defaultSave);
                manager.currentSaveGame.SaveName = "Dev Game";
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