using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public AudioMixer musicMixer;
    public AudioMixer SFXMixer;
    public void SetLevel(float sliderValue)
    {
        musicMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        SFXMixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
    }
}
