using UnityEngine;

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
        //have this be the main menu scene or whatever once its done
        //SceneManager.LoadScene("Menu");
    }
    
    public void MenuQuit()
    {
        Debug.Log("Quitting from game menu");
        Application.Quit(); //doesn't end test in unity when testing, works as intended i think
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