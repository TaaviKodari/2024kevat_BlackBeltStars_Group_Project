using System;
using System.Collections.Generic;
using GameState;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class LoadGameDialog : MonoBehaviour
    {
        [SerializeField]
        private GameSelectButton buttonPrefab;
        [SerializeField]
        private Button loadButton;
        [SerializeField]
        private Transform buttonHolder;
        
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
            SceneManager.LoadScene("WorldSelect");
        }

        public void SelectSave(int index)
        {
            selectedSave = index;
            loadButton.interactable = true;
        }
    }
}
