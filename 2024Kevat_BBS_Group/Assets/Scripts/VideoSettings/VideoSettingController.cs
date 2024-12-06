using System;
using System.Collections.Generic;
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
            visibleResolutions = Screen.resolutions
                .Where(res => Mathf.Approximately(res.width / (float) res.height, 16 / 9f)) // Only show 19:6 resolutions
                .OrderByDescending(res => res.refreshRateRatio.value) // Largest refresh rate first so that gets picked
                .Distinct(new ResolutionComparer()) // Only one refresh rate per resolution
                .OrderByDescending(res => res.height) // Largest resolution first
                .ToArray();

            resolutionDropdown.options = new List<TMP_Dropdown.OptionData> { new("") };
            resolutionDropdown.options.AddRange(visibleResolutions.Select(res => new TMP_Dropdown.OptionData($"{res.width} x {res.height}")));
        }

        private void OnEnable()
        {
            windowModeDropdown.value = Screen.fullScreenMode switch
            {
                FullScreenMode.ExclusiveFullScreen => 1,
                FullScreenMode.FullScreenWindow => 2,
                FullScreenMode.MaximizedWindow => 0,
                FullScreenMode.Windowed => 0,
                _ => throw new ArgumentOutOfRangeException()
            };
            resolutionDropdown.value = Array.FindIndex(visibleResolutions,
                    res => res.width == Screen.width && res.height == Screen.height) + 1; // -1 becomes 0
        }

        [UsedImplicitly]
        public void Apply()
        {
            var fullScreenMode = windowModeDropdown.value switch
            {
                0 => FullScreenMode.Windowed,
                1 => FullScreenMode.ExclusiveFullScreen,
                2 => FullScreenMode.FullScreenWindow,
                _ => throw new ArgumentOutOfRangeException()
            };

            var resolution = resolutionDropdown.value == 0 ?
                Screen.currentResolution :
                visibleResolutions[resolutionDropdown.value - 1];
            Screen.SetResolution(resolution.width, resolution.height, fullScreenMode, resolution.refreshRateRatio);
        }
    }
}