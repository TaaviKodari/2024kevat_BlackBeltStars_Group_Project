using System.Collections.Generic;
using System.Linq;
using GameState;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MainMenu
{
    // Manages the new game popup
    public class NewGameDialog : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField nameField;
        [SerializeField]
        private TMP_Text errorText;
        [FormerlySerializedAs("button")]
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private SceneTransition transition;

        private HashSet<string> existingSaves;

        private void Start()
        {
            existingSaves = SaveManager.LoadGames().Select(save => save.SaveName).ToHashSet();

            startButton.onClick.AddListener(Begin);
            startButton.interactable = false;
            nameField.onValueChanged.AddListener(value =>
            {
                var trimmed = value.Trim();
                var canStart = true;
                if (trimmed == "")
                {
                    canStart = false;
                    errorText.text = "Save name must not be empty";
                } else if (existingSaves.Contains(value.ReplaceInvalidFileNameCharacters()))
                {
                    canStart = false;
                    errorText.text = "A save with this name already exists";
                }
                else
                {
                    errorText.text = "";
                }

                startButton.interactable = canStart;
            });
        }

        private void Begin()
        {
            var name = nameField.text.Trim();
            if (name == "") return;
            var saveGame = new SaveGame
            {
                name = name,
                SaveName = name.ReplaceInvalidFileNameCharacters()
            };
            var stateManager = GameStateInitializer.CreateStateManager(saveGame);
            stateManager.GenerateMaps();
            transition.LoadScene("WorldSelect");
        }
    }
}