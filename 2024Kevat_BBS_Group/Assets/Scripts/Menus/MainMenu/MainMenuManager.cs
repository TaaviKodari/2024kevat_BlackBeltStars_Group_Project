using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

namespace MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public Button exitButton;
    
        void Start()
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();

            // In the Editor, you can stop playing with this line (only works in the Editor):
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }

        void Update()
        {
            AudioManager.Instance.PlayFull("MainMenu");
        }
    }
}
