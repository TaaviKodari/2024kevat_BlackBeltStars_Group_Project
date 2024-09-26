using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

public class MainMenuManager : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;
    
    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
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

    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("WorldSelect");
    }

    void Update()
    {
        AudioManager.Instance.PlayFull("MainMenu");
    }
}
