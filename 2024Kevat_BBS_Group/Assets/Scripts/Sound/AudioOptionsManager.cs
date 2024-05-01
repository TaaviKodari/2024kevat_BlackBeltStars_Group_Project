using UnityEngine;
using TMPro;

public class AudioOptionsManager : MonoBehaviour
{
    public static float MusicVol { get; private set; }
    public static float SFXVol { get; private set; }
    [SerializeField] private TextMeshProUGUI musicvalue;
    [SerializeField] private TextMeshProUGUI SFXvalue;
    public void OnMusicSliderValueChange(float value)
    {
        MusicVol = value;
        musicvalue.text = ((int)(value * 100)).ToString();
        AudioManager.instance.UpdateMixervolume();
    }
    public void OnSFXSliderValueChange(float value)
    {
        SFXVol = value;
        SFXvalue.text = ((int)(value * 100)).ToString();
        AudioManager.instance.UpdateMixervolume();
    }
}
