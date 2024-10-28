using AtomicConsole.Engine;
using GameState;
using UnityEngine;

[RequireComponent(typeof(AtomicConsoleEngine), typeof(PauseMarker))]
public class ConsolePauseChecker : MonoBehaviour
{
    private AtomicConsoleEngine engine;
    private PauseMarker marker;

    private void Awake()
    {
        engine = GetComponent<AtomicConsoleEngine>();
        marker = GetComponent<PauseMarker>();
    }

    private void Update()
    {
        marker.enabled = engine.isVisible;
        if (InGameManager.Instance != null) {
            if (engine.isVisible)
            {
                InGameManager.Instance.Input.Menu.Disable();
            }
            else
            {
                InGameManager.Instance.Input.Menu.Enable();
            }
        }
    }
}