using GameState;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class NewGameDialog : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField nameField;
        [SerializeField]
        private Button button;

        private void Start()
        {
            button.onClick.AddListener(Begin);
            button.interactable = false;
            nameField.onValueChanged.AddListener(value => button.interactable = value.Trim() != "");
        }

        private void Begin()
        {
            var name = nameField.text.Trim();
            if (name == "") return;
            var saveGame = new SaveGame
            {
                SaveName = name
            };
            GameStateInitializer.CreateStateManager(saveGame);
            SceneManager.LoadScene("WorldSelect");
        }
    }
}