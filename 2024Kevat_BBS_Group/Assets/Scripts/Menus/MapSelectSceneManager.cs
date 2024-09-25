using System;
using GameState;
using UnityEngine;

public class MapSelectSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] mapCards = new GameObject[3];

    private GameStateManager stateManager;

    public void Start()
    {
        stateManager = FindObjectOfType<GameStateManager>();
    }

    public void PlayMap(int cardIndex)
    {
        Debug.Log("Playing map " + cardIndex);
    }

    public void Save()
    {
        stateManager.Save();
    }

    public void CloseGame()
    {
        Save();
    }
}