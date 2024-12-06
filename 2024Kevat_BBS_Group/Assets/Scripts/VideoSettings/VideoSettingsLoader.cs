using System.IO;
using UnityEngine;

namespace VideoSettings
{
    public static class VideoSettingsLoader
    {
        public static VideoSettings CurrentSettings;

        [RuntimeInitializeOnLoadMethod]
        private static void OnLoad()
        {
            Load();
            Apply();
        }

        public static void Load()
        {
            if (File.Exists("options/video.json"))
            {
                var save = JsonUtility.FromJson<VideoSettingsSave>(File.ReadAllText("options/video.json"));
                CurrentSettings.FullScreenMode = save.fullScreenMode;
                CurrentSettings.Resolution = DecodeResolution(save.resolution);
            }
            else
            {
                CurrentSettings = new VideoSettings
                {
                    FullScreenMode = FullScreenMode.ExclusiveFullScreen,
                    Resolution = Screen.currentResolution
                };
            }
        }

        public static void Save()
        {
            var save = new VideoSettingsSave
            {
                fullScreenMode = CurrentSettings.FullScreenMode,
                resolution = CurrentSettings.Resolution.ToString()
            };

            Directory.CreateDirectory("options");
            File.WriteAllText("options/video.json", JsonUtility.ToJson(save));
        }

        public static void Apply()
        {
            Screen.SetResolution(
                CurrentSettings.Resolution.width,
                CurrentSettings.Resolution.height,
                CurrentSettings.FullScreenMode,
                CurrentSettings.Resolution.refreshRateRatio
            );
        }

        private static Resolution DecodeResolution(string input)
        {
            foreach (var resolution in Screen.resolutions)
            {
                if (input == resolution.ToString())
                {
                    return resolution;
                }
            }

            return Screen.currentResolution;
        }
    }
}