using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    private AudioOptionsManager.Channel channel;
    [SerializeField]
    private TextMeshProUGUI valueLabel;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(sliderValue =>
        {
            AudioOptionsManager.SetChannel(channel, sliderValue);
            SetValue(sliderValue);
        });
    }

    private void OnEnable()
    {
        var value = AudioOptionsManager.GetChannel(channel);
        slider.value = value;
        SetValue(value);
    }

    private void SetValue(float value)
    {
        valueLabel.text = ((int)(value * 100)).ToString();
    }
}