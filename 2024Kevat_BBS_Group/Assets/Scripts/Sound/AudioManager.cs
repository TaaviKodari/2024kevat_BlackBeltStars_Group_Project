using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup SFXMixerGroup;
    [SerializeField] private AudioMixerGroup masterMixerGroup;
    public Sound[] sounds;
    
    void Awake()
    {
        Instance = this;
        
        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            switch(s.audioType)
            {
                case Sound.AudioTypes.SFX:
                    s.source.outputAudioMixerGroup = SFXMixerGroup;
                    break;
                case Sound.AudioTypes.Music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
                case Sound.AudioTypes.Master:
                    s.source.outputAudioMixerGroup = masterMixerGroup;
                    break;
            }
        }
    }
    
    // Plays an audio clip unless it is already playing
    public void PlayFull(string clipName)
    {
        var s = Array.Find(sounds, sound => sound.name == clipName);
        if(s == null){
            Debug.LogWarning("Sound: '" + clipName + "' not found!");
            return;
        }
        if(!s.source.isPlaying)
        {
            s.source.Play();
        }
        
    }
    
    // Plays an audio clip, stopping all previous instances of it
    public void PlayStop(string clipName)
    {
        var s = Array.Find(sounds, sound => sound.name == clipName);
        if(s == null){
            Debug.LogWarning("Sound: '" + clipName + "' not found!");
            return;
        }
        s.source.Stop();
        s.source.Play();
    }
    
    // Plays an audio clip, allowing the clip to overlap if played multiple timess
    public void PlayOver(string clipName)
    {
        var s = Array.Find(sounds, sound => sound.name == clipName);
        if(s == null){
            Debug.LogWarning("Sound: '" + clipName + "' not found!");
            return;
        }
        s.source.Play();
    }
    
    //template for sound clips
    public void UpdateMixervolume()
    {
        musicMixerGroup.audioMixer.SetFloat("MusicVol", Mathf.Log10(AudioOptionsManager.MusicVol)*20);
        SFXMixerGroup.audioMixer.SetFloat("SFXVol", Mathf.Log10(AudioOptionsManager.SFXVol)*20);
        masterMixerGroup.audioMixer.SetFloat("MasterVol", Mathf.Log10(AudioOptionsManager.MasterVol)*20);
    }
}
