using System.Collections.Generic;
using GameState;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    // Manages the load game popup
    public class LoadGameDialog : MonoBehaviour
    {
        [SerializeField]
        private SaveSelectButton buttonPrefab;
        [SerializeField]
        private Button loadButton;
        [SerializeField]
        private Transform buttonHolder;
        [SerializeField]
        private SceneTransition transition;
        
        private int selectedSave;
        private List<SaveGame> games;

        private void Start()
        {
            loadButton.onClick.AddListener(Load);
        }

        private void OnEnable()
        {
            selectedSave = -1;
            loadButton.interactable = false;
            games = SaveManager.LoadGames();
            
            // Destroy old buttons and create new ones
            foreach (Transform child in buttonHolder)
            {
                Destroy(child.gameObject);
            }
            for (var i = 0; i < games.Count; i++)
            {
                var button = Instantiate(buttonPrefab, buttonHolder);
                button.Index = i;
                button.SaveName = games[i].SaveName;
                button.Dialog = this;
            }
        }

        private void Load()
        {
            if (selectedSave == -1) return;
            GameStateInitializer.CreateStateManager(games[selectedSave]);
            transition.LoadScene("WorldSelect");
        }

        public void SelectSave(int index)
        {
            selectedSave = index;
            loadButton.interactable = true;
        }
    }
}
