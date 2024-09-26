using UnityEngine;

namespace GameState
{
    public class GameStateManager : MonoBehaviour
    {
        public SaveGame currentSaveGame;
        public MapStats currentMap;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void NewGame(string saveName)
        {
            currentSaveGame = default;
            currentSaveGame.SaveName = saveName;
        }

        public void LoadGame(SaveGame game)
        {
            currentSaveGame = game;
        }
        
        public void Save()
        {
            SaveManager.SaveGame(currentSaveGame);
        }
    }
}