using GameState;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private InGameManager gameManager;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<InGameManager>();
    }
    
    private void Update()
    {
        if (gameManager.Input.Menu.Pause.triggered)
        {
            animator.SetTrigger("key/menu");
        }
    }
    
    public void OpenMenu()
    {
        Debug.Log("Open Menu");
        SceneManager.LoadScene("MainMenu");
    }
    
    public void MenuQuit()
    {
        Debug.Log("Quitting from game menu");
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}