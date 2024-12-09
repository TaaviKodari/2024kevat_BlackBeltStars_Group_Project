using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Sound
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField]
        private AudioMixer mixer;
        [SerializeField]
        private AudioSource musicSource;
        [SerializeField]
        private AudioSource sfxSource;

        [SerializeField]
        private ConfiguredSound[] sounds;

        private string currentMusic;
        private readonly Dictionary<string, ConfiguredSound> soundLookup = new();

        private void Awake()
        {
            Instance = this;

            foreach (var sound in sounds)
            {
                soundLookup[sound.name] = sound;
            }
        }

        private void Start()
        {
            UpdateMixerVolumes();
        }

        public void PlayMusic(string clipName)
        {
            if (clipName == currentMusic) return;
            currentMusic = clipName;

            if (!soundLookup.TryGetValue(clipName, out var sound))
            {
                Debug.LogWarning("Sound: '" + clipName + "' not found!");
                return;
            }

            StartCoroutine(FadeMusic(sound.clip));
        }

        private IEnumerator FadeMusic(AudioClip newClip)
        {
            const float normalVolume = 1f;
            const float delay = 1f;

            while (musicSource.volume > 0)
            {
                musicSource.volume -= normalVolume * Time.deltaTime / delay;
                yield return null;
            }
            musicSource.Stop();
            musicSource.volume = normalVolume;
            musicSource.clip = newClip;
            musicSource.Play();
        }

        public void PlaySfx(string clipName)
        {
            if (!soundLookup.TryGetValue(clipName, out var sound))
            {
                Debug.LogWarning("Sound: '" + clipName + "' not found!");
                return;
            }

            sfxSource.pitch = sound.pitch * Random.Range(0.85f, 1.15f);
            sfxSource.PlayOneShot(sound.clip, sound.volume);
        }

        public void UpdateMixerVolumes()
        {
            mixer.SetFloat("MusicVol", Mathf.Log10(AudioOptionsManager.Options.musicVolume)*20);
            mixer.SetFloat("SFXVol", Mathf.Log10(AudioOptionsManager.Options.sfxVolume)*20);
            mixer.SetFloat("MasterVol", Mathf.Log10(AudioOptionsManager.Options.masterVolume)*20);
        }

        public static bool CheckPeriod(float delay)
        {
            var lastStep = (int)((Time.time - Time.deltaTime) / delay);
            var step = (int)(Time.time / delay);

            return step > lastStep;
        }
    }
}
