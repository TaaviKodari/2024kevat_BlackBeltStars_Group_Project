using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Sound
{
    public string name;
    [FormerlySerializedAs("audioType")]
    public AudioChannel channel;
    public AudioClip clip; 
    [Range(0f, 1f)]
    public float volume; 
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;
    [NonSerialized]
    public AudioSource Source;
}
