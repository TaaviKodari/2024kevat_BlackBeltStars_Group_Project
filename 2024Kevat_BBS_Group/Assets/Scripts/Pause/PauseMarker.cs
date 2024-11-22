using System.Linq;
using UnityEngine;

namespace Pause
{
    // A script used to mark the game paused
    // While any one of these is enabled the game should be paused
    // Allows for stacking of pauses from multiple sources (e.g. console and pause menu)
    public class PauseMarker : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log("Pausing game due to marker", this);
            PauseManager.Pause();
        }

        private void OnDisable()
        {
            if (FindObjectsOfType<PauseMarker>().All(it => it == this || !it.enabled))
            {
                Debug.Log("Unpausing game due to marker", this);
                PauseManager.Unpause();
            }
        }
    }
}