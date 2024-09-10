using UnityEngine;
using TMPro;

public class AudioOptionsManager : MonoBehaviour
{
    public static float MusicVol { get; private set; }
    public static float SFXVol { get; private set; }
    public static float MasterVol { get; private set; }
    [SerializeField] private TextMeshProUGUI musicvalue;
    [SerializeField] private TextMeshProUGUI SFXvalue;
    [SerializeField] private TextMeshProUGUI mastervalue;
    public void OnMusicSliderValueChange(float value)
    {
        MusicVol = value;
        musicvalue.text = ((int)(value * 100)).ToString();
        AudioManager.Instance.UpdateMixervolume();
    }
    public void OnSFXSliderValueChange(float value)
    {
        SFXVol = value;
        SFXvalue.text = ((int)(value * 100)).ToString();
        AudioManager.Instance.UpdateMixervolume();
    }
    public void OnMasterSliderValueChange(float value)
    {
        MasterVol = value;
        mastervalue.text = ((int)(value * 100)).ToString();
        AudioManager.Instance.UpdateMixervolume();
    }
}
