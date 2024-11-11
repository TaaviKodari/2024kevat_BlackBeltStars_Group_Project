using GameState;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MapSelect
{
    public class MapSelectSceneManager : MonoBehaviour
    {
        [SerializeField]
        private MapCard mapCard1;
        [SerializeField]
        private MapCard mapCard2;
        [SerializeField]
        private MapCard mapCard3;
        [SerializeField]
        private SceneTransition transition;

        private GameStateManager stateManager;

        public void Start()
        {
            stateManager = FindObjectOfType<GameStateManager>();
            var maps = stateManager.currentSaveGame.maps;
            mapCard1.Populate(maps.map1);
            mapCard2.Populate(maps.map2);
            mapCard3.Populate(maps.map3);
            Save();
        }

        public void PlayMap(int cardIndex)
        {
            stateManager.currentMap = cardIndex switch
            {
                1 => stateManager.currentSaveGame.maps.map1,
                2 => stateManager.currentSaveGame.maps.map2,
                3 => stateManager.currentSaveGame.maps.map3,
                _ => default
            };
            transition.LoadScene("Game");
        }

        public void Save()
        {
            stateManager.Save();
        }

        public void CloseGame()
        {
            Save();
            Destroy(stateManager.gameObject);
            transition.LoadScene("MainMenu");
        }
        
        public void OpenShop()
        {
            Save();
            transition.LoadScene("Shop");
        }
        
        public void OpenWorldSelect()
        {
            transition.LoadScene("WorldSelect");
        }
    }
}