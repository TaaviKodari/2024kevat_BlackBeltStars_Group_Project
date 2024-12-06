using System;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace VideoSettings
{
    public class VideoSettingController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown windowModeDropdown;
        [SerializeField]
        private TMP_Dropdown resolutionDropdown;

        private Resolution[] visibleResolutions;

        private void Awake()
        {
            windowModeDropdown.onValueChanged.AddListener(SetWindowMode);
            resolutionDropdown.onValueChanged.AddListener(SetResolution);

            visibleResolutions = Screen.resolutions
                .Where(res => Mathf.Approximately(res.width / (float) res.height, 16 / 9f)) // Only show 19:6 resolutions
                .OrderByDescending(res => res.refreshRateRatio.value) // Largest refresh rate first so that gets picked
                .Distinct(new ResolutionComparer()) // Only one refresh rate per resolution
                .OrderByDescending(res => res.height) // Largest resolution first
                .ToArray();

            resolutionDropdown.options = visibleResolutions.Select(res => new TMP_Dropdown.OptionData($"{res.width} x {res.height}")).ToList();
        }

        private void OnEnable()
        {
            windowModeDropdown.value = VideoSettingsLoader.CurrentSettings.FullScreenMode switch
            {
                FullScreenMode.ExclusiveFullScreen => 2,
                FullScreenMode.FullScreenWindow => 1,
                FullScreenMode.MaximizedWindow => 0,
                FullScreenMode.Windowed => 0,
                _ => throw new ArgumentOutOfRangeException()
            };
            resolutionDropdown.value = Array.IndexOf(visibleResolutions, VideoSettingsLoader.CurrentSettings.Resolution);
        }

        private void SetWindowMode(int mode)
        {
            VideoSettingsLoader.CurrentSettings.FullScreenMode = mode switch
            {
                0 => FullScreenMode.Windowed,
                1 => FullScreenMode.ExclusiveFullScreen,
                2 => FullScreenMode.FullScreenWindow,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }

        private void SetResolution(int index)
        {
            VideoSettingsLoader.CurrentSettings.Resolution = visibleResolutions[index];
        }

        [UsedImplicitly]
        public void Apply()
        {
            VideoSettingsLoader.Save();
            VideoSettingsLoader.Apply();
        }
    }
}