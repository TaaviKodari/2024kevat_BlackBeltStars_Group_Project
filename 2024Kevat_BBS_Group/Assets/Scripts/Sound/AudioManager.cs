using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup SFXMixerGroup;
    [SerializeField] private AudioMixerGroup masterMixerGroup;
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
            return;
        }
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

    //dont let same audioclip overlap
    public void PlayFull(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null){
            Debug.LogWarning("Sound: '" + name + "' not found!");
            return;
        }
        if(!s.source.isPlaying)
        {
            s.source.Play();
        }
        
    }
    //stop sound then play it again
    public void PlayStop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null){
            Debug.LogWarning("Sound: '" + name + "' not found!");
            return;
        }
        s.source.Stop();
        s.source.Play();
    }
    //play audioclip over itself if triggered
    public void PlayOver(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null){
            Debug.LogWarning("Sound: '" + name + "' not found!");
            return;
        }
        s.source.Play();
    }
    //template for sound clips
    //FindObjectOfType<AudioManager>().PlayFull("name of clip");
    public void UpdateMixervolume()
    {
        musicMixerGroup.audioMixer.SetFloat("MusicVol", Mathf.Log10(AudioOptionsManager.MusicVol)*20);
        SFXMixerGroup.audioMixer.SetFloat("SFXVol", Mathf.Log10(AudioOptionsManager.SFXVol)*20);
        masterMixerGroup.audioMixer.SetFloat("MasterVol", Mathf.Log10(AudioOptionsManager.MasterVol)*20);
    }

}
