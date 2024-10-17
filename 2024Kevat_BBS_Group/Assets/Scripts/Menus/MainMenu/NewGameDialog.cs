using GameState;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MainMenu
{
    // Manages the new game popup
    public class NewGameDialog : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField nameField;
        [FormerlySerializedAs("button")]
        [SerializeField]
        private Button startButton;

        private void Start()
        {
            startButton.onClick.AddListener(Begin);
            startButton.interactable = false;
            nameField.onValueChanged.AddListener(value => startButton.interactable = value.Trim() != "");
        }

        private void Begin()
        {
            var name = nameField.text.Trim();
            if (name == "") return;
            var saveGame = new SaveGame
            {
                SaveName = name
            };
            var stateManager = GameStateInitializer.CreateStateManager(saveGame);
            stateManager.GenerateMaps();
            SceneManager.LoadScene("WorldSelect");
        }
    }
}