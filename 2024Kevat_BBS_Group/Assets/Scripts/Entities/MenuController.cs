using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public MasterInput input { get; private set; }
    public GameObject PauseMenuUI;
    public GameObject OptionsMenuUI;
    public GameObject BuildUI;
    //add more states when needed
    public enum MenuStates{None, Pause, Options};
    public MenuStates CurrentMenuState;

    private void Awake()
    {
        input = new MasterInput();
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
        SwitchMenu();
        if(input.Player.Menu.triggered)
        {
            switch(CurrentMenuState)
            {
                //from what state
                case MenuStates.None:
                    //to what state
                    CurrentMenuState = MenuStates.Pause;
                    break;
                case MenuStates.Pause:
                    CurrentMenuState = MenuStates.None;
                    break;
                case MenuStates.Options:
                    CurrentMenuState = MenuStates.Pause;
                    break;
            }
        }
    }

    private void SwitchMenu()
    {
        switch(CurrentMenuState)
        {
            case MenuStates.None:
                BuildUI.SetActive(true);
                PauseMenuUI.SetActive(false);
                OptionsMenuUI.SetActive(false);
                break;
            case MenuStates.Pause:
                BuildUI.SetActive(false);
                PauseMenuUI.SetActive(true);
                OptionsMenuUI.SetActive(false);
                break;
            case MenuStates.Options:
                BuildUI.SetActive(false);
                PauseMenuUI.SetActive(false);
                OptionsMenuUI.SetActive(true);
                break;
        }
    }
    //buttons
    public void Resume()
    {
        CurrentMenuState = MenuStates.None;
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
        CurrentMenuState = MenuStates.Options;
    }
    public void MenuBackPause()
    {
        CurrentMenuState = MenuStates.Pause;
    }
}