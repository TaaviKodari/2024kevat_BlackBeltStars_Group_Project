using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

public static class AudioOptionsManager
{
    public static AudioOptions Options { get; private set; }

    public static void SetChannel(Channel channel, float value)
    {
        var options = Options;
        switch (channel)
        {
            case Channel.Master:
                options.masterVolume = value;
                break;
            case Channel.Sfx:
                options.sfxVolume = value;
                break;
            case Channel.Music:
                options.musicVolume = value;
                break;
        }
        Options = options;
        Save();
    }

    public static float GetChannel(Channel channel)
    {
        return channel switch
        {
            Channel.Master => Options.masterVolume,
            Channel.Sfx => Options.sfxVolume,
            Channel.Music => Options.musicVolume,
            _ => 1
        };
    }

    private static void Save()
    {
        var audioManager = Object.FindObjectOfType<AudioManager>();
        if (audioManager != null) audioManager.UpdateMixervolume();
        
        Directory.CreateDirectory("options");
        File.WriteAllText("options/audio.json", JsonUtility.ToJson(Options));
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Load()
    {
        if (File.Exists("options/audio.json"))
        {
            Options = JsonUtility.FromJson<AudioOptions>(File.ReadAllText("options/audio.json"));
        }
        else
        {
            Options = new AudioOptions
            {
                musicVolume = 1, sfxVolume = 1, masterVolume = 1
            };
        }
    }
    
    public enum Channel
    {
        Master, Sfx, Music
    }

    [Serializable]
    public struct AudioOptions
    {
        public float musicVolume;
        public float sfxVolume;
        public float masterVolume;
    }
}
