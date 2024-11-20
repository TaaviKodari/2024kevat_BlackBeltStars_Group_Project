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
            AudioManager.Instance.PlayMusic(clip);
        }
    }
}