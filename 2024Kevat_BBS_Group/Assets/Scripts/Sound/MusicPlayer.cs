using UnityEngine;

// Plays music for a scene without interrupting on scene transition if the same piece is already playing
public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    private string clip;

    private void OnEnable()
    {
        AudioManager.Instance.SwitchMusic(clip);
    }

    private void Update()
    {
        AudioManager.Instance.PlayFull(clip);
    }
}