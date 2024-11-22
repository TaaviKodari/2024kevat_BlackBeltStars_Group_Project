using UnityEngine;
using UnityEngine.Events;

namespace Pause
{
    public static class PauseManager
    {
        public static bool Paused { get; private set; }
        public static readonly UnityEvent OnPause = new();
        public static readonly UnityEvent OnUnpause = new();

        public static void Pause()
        {
            if (Paused) return;
            OnPause.Invoke();
            Time.timeScale = 0f;
            Paused = true;
        }

        public static void Unpause()
        {
            if (!Paused) return;
            OnUnpause.Invoke();
            Time.timeScale = 1f;
            Paused = false;
        }
    }
}