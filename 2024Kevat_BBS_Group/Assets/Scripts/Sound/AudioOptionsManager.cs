using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sound
{
    public static class AudioOptionsManager
    {
        private static readonly string SavePath = $"{Application.persistentDataPath}/options/audio.json";
        public static AudioOptions Options { get; private set; }

        // Sets the volume of a specific audio channel
        public static void SetChannelVolume(AudioChannel channel, float value)
        {
            var options = Options;
            switch (channel)
            {
                case AudioChannel.Master:
                    options.masterVolume = value;
                    break;
                case AudioChannel.Sfx:
                    options.sfxVolume = value;
                    break;
                case AudioChannel.Music:
                    options.musicVolume = value;
                    break;
            }
            Options = options;
            Save();
        }

        // Gets the volume of a specific audio channel
        public static float GetChannelVolume(AudioChannel channel)
        {
            return channel switch
            {
                AudioChannel.Master => Options.masterVolume,
                AudioChannel.Sfx => Options.sfxVolume,
                AudioChannel.Music => Options.musicVolume,
                _ => 1
            };
        }

        private static void Save()
        {
            var audioManager = Object.FindObjectOfType<AudioManager>();
            if (audioManager != null) audioManager.UpdateMixerVolumes();

            Directory.CreateDirectory(SavePath.Remove(SavePath.LastIndexOf("/", StringComparison.Ordinal)));
            File.WriteAllText(SavePath, JsonUtility.ToJson(Options));
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Load()
        {
            if (File.Exists(SavePath))
            {
                Options = JsonUtility.FromJson<AudioOptions>(File.ReadAllText(SavePath));
            }
            else
            {
                Options = new AudioOptions
                {
                    musicVolume = 1, sfxVolume = 1, masterVolume = 1
                };
            }
        }

        [Serializable]
        public struct AudioOptions
        {
            public float musicVolume;
            public float sfxVolume;
            public float masterVolume;
        }
    }

    public enum AudioChannel
    {
        Master, Sfx, Music
    }
}