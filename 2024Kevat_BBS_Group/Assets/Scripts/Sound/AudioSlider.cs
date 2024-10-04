using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Links a slider to audio options
[RequireComponent(typeof(Slider))]
public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    private AudioChannel channel;
    [Tooltip("A TMP text that's used to show the value of the slider")]
    [SerializeField]
    private TextMeshProUGUI valueLabel;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(sliderValue =>
        {
            AudioOptionsManager.SetChannelVolume(channel, sliderValue);
            SetLabelText(sliderValue);
        });
    }

    private void OnEnable()
    {
        var value = AudioOptionsManager.GetChannelVolume(channel);
        slider.value = value;
        SetLabelText(value);
    }

    private void SetLabelText(float value)
    {
        valueLabel.text = ((int)(value * 100)).ToString();
    }
}