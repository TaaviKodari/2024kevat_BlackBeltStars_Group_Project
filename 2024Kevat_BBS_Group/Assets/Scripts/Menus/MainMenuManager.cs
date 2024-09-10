using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
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
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    
    }

    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Game");
        
    }

    void Update()
    {
        FindObjectOfType<AudioManager>().PlayFull("MainMenu");
    }
}
