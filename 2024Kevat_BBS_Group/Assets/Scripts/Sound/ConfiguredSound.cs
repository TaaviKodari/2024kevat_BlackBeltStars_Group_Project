using System;
using UnityEngine;

namespace Sound
{
    [Serializable]
    public class ConfiguredSound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume;
        [Range(0.1f, 3f)]
        public float pitch;
    }
}
