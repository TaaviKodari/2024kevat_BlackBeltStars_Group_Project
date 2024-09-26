using UnityEngine;

namespace GameState
{
    // Creates a game state if not present when loading a scene directly from the editor
    public class GameStateInitializer : MonoBehaviour
    {
        [SerializeField]
        private SaveGame defaultSave;
        
        public void Awake()
        {
            if (FindObjectOfType<GameStateManager>() == null)
            {
                var stateObject = new GameObject("Game State");
                var stateComponent = stateObject.AddComponent<GameStateManager>();
                stateComponent.LoadGame(defaultSave);
                stateComponent.currentSaveGame.SaveName = "Dev Game";
            }
            DestroyImmediate(gameObject);
        }
    }
}