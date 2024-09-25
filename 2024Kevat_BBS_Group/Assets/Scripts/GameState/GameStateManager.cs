using UnityEngine;

namespace GameState
{
    public class GameStateManager : MonoBehaviour
    {
        private SaveGame currentSaveGame;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public ref SaveGame GetSaveGameRef()
        {
            return ref currentSaveGame;
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