using System.Collections;
using UnityEngine;

// Plays music for a scene without interrupting on scene transition if the same piece is already playing
namespace Sound
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField]
        private string clip;

        private void OnEnable()
        {
            StartCoroutine(StartMusic());
        }

        private IEnumerator StartMusic()
        {
            while (AudioManager.Instance == null)
            {
                yield return null;
            }

            AudioManager.Instance.PlayMusic(clip);
        }
    }
}