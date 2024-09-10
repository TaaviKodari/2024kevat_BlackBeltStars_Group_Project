using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private MasterInput input;
    private Animator animator;

    private void Awake()
    {
        input = new MasterInput();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        if (input.Building.ToggleBuilding.triggered)
        {
            animator.SetTrigger("key/build");
        }
        if(input.Player.Menu.triggered)
        {
            animator.SetTrigger("key/menu");
        }
    }
    
    //buttons
    public void Resume()
    {
        animator.SetTrigger("button/resume");
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
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    public void MenuOptions()
    {
        animator.SetTrigger("button/options");
    }
    
    public void MenuBackPause()
    {
        animator.SetTrigger("button/back");
    }
}