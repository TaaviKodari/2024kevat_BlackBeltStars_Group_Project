using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] 
    private AudioMixerGroup musicMixerGroup;
    [SerializeField] 
    private AudioMixerGroup sfxMixerGroup;
    [SerializeField] 
    private AudioMixerGroup masterMixerGroup;
    
    [SerializeField]
    private Sound[] sounds;
    private readonly Dictionary<string, AudioSource> sources = new();
    
    private void Awake()
    {
        Instance = this;
        
        // Add audio source components to the manager for each sound
        foreach (var sound in sounds) {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.loop = sound.loop;
            source.outputAudioMixerGroup = sound.channel switch
            {
                AudioChannel.Sfx => sfxMixerGroup,
                AudioChannel.Music => musicMixerGroup,
                AudioChannel.Master => masterMixerGroup,
                _ => throw new InvalidOperationException($"Sound {sound.name} has unknown channel: {sound.channel}")
            };
            
            sound.Source = source;
            sources[sound.name] = source;
        }
        
        UpdateMixerVolumes();
    }

    public void SwitchMusic(string clipName)
    {
        if (!sources.TryGetValue(clipName, out var newSource))
        {
            Debug.LogWarning("Sound: '" + clipName + "' not found!");
            return;
        }

        foreach (var source in sources.Values)
        {
            if (source.outputAudioMixerGroup != musicMixerGroup) continue;
            if (source == newSource) continue;
            StartCoroutine(FadeOut(source, 1f));
        }
        
        if(!newSource.isPlaying)
        {
            newSource.Play();
        }
    }

    private static IEnumerator FadeOut(AudioSource source, float delay)
    {
        var normalVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= normalVolume * Time.deltaTime / delay;
            yield return null;
        }
        source.Stop();
        source.volume = normalVolume;
    }
    
    // Plays an audio clip unless it is already playing
    public void PlayFull(string clipName)
    {
        if (!sources.TryGetValue(clipName, out var source))
        {
            Debug.LogWarning("Sound: '" + clipName + "' not found!");
            return;
        }

        if(!source.isPlaying)
        {
            source.Play();
        }
    }
    
    // Plays an audio clip, stopping all previous instances of it
    public void PlayStop(string clipName)
    {
        if (!sources.TryGetValue(clipName, out var source))
        {
            Debug.LogWarning("Sound: '" + clipName + "' not found!");
            return;
        }

        source.Stop();
        source.Play();
    }
    
    // Plays an audio clip, allowing the clip to overlap if played multiple timess
    public void PlayOver(string clipName)
    {
        if (!sources.TryGetValue(clipName, out var source))
        {
            Debug.LogWarning("Sound: '" + clipName + "' not found!");
            return;
        }

        source.Play();
    }
    
    public void UpdateMixerVolumes()
    {
        musicMixerGroup.audioMixer.SetFloat("MusicVol", Mathf.Log10(AudioOptionsManager.Options.musicVolume)*20);
        sfxMixerGroup.audioMixer.SetFloat("SFXVol", Mathf.Log10(AudioOptionsManager.Options.sfxVolume)*20);
        masterMixerGroup.audioMixer.SetFloat("MasterVol", Mathf.Log10(AudioOptionsManager.Options.masterVolume)*20);
    }
}
