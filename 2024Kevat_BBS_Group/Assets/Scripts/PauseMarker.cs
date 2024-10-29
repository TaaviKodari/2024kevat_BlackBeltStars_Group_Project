using System;
using System.Linq;
using GameState;
using UnityEngine;

// A script used to mark the game paused
// While any one of these is enabled the game should be paused
// Allows for stacking of pauses from multiple sources (e.g. console and pause menu)
public class PauseMarker : MonoBehaviour
{
    private void OnEnable()
    {
        if (InGameManager.Instance != null)
        {
            Debug.Log("Pausing game due to marker", this);
            InGameManager.Instance.Pause();
        }
    }

    private void OnDisable()
    {
        if (InGameManager.Instance != null && FindObjectsOfType<PauseMarker>().All(it => it == this || !it.enabled))
        {
            Debug.Log("Unpausing game due to marker", this);
            InGameManager.Instance.Unpause();
        }
    }
}