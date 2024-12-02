using System;
using GameState;
using TMPro;
using UnityEngine;

public class PersistentResourceWatcher : MonoBehaviour
{
    [SerializeField]
    private Resource resource;

    private TMP_Text tmpText;
    private GameStateManager stateManager;

    private void Start()
    {
        tmpText = GetComponentInChildren<TMP_Text>();
        stateManager = FindObjectOfType<GameStateManager>();
    }

    private void Update()
    {
        tmpText.text = resource switch
        {
            Resource.Gold => stateManager.currentSaveGame.resources.gold.ToString(),
            Resource.Diamonds => stateManager.currentSaveGame.resources.diamonds.ToString(),
            _ => throw new IndexOutOfRangeException($"Unknown resource: {resource}")
        };
    }

    private enum Resource
    {
        Gold, Diamonds
    }
}