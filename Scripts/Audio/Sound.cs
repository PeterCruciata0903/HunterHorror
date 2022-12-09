using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string soundName;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 0.3f)]
    public float pitch;

    public float length; // length of clip
    public bool isPlaying; // is the clip playing?

    [HideInInspector]
    public AudioSource source;

    public bool loop;

    private void Start()
    {
        length = clip.length;
    }
    private void Update()
    {
        isPlaying = source.isPlaying;
    }
}
