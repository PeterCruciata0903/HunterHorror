using System;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudio : Singleton<PlayerAudio>
{
    //Each sound is stored in the sound array and possesses a volume, pitch, clip, and source.
    //public static AudioManager Instance { get; private set; }
    [SerializeField]
    public Sound[] sounds;
    private int noiseCycle; //indexes across a list of sounds.
    GameObject self;
    PlayerMovement playerMovement;
    public void Start()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.soundName == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        if (!s.isPlaying)
        {
            s.source.Play();
            return;
        }
    }

    //No need for loop play thus far

    //public void loopPlay(string name, int loops)
    //{
    //    Sound s = Array.Find(sounds, sound => sound.soundName == name);
    //    float timeSinceLastPlayed = 0f;
    //    timeSinceLastPlayed += Time.deltaTime;
    //    if (s == null)
    //    {
    //        Debug.LogWarning("Sound: " + name + " not found");
    //        return;
    //    }
    //    if (!s.isPlaying)
    //    {
    //        s.source.Play();
    //        return;
    //    }
    //    if (timeSinceLastPlayed > s.length * (float)loops)
    //    {
    //        s.source.Stop();
    //        return;
    //    }
    //}

    public void playerWalkingNoise() //Repeating Walking Sound
    {
        Play("Walk");
    }
    public void playerSprintingNoise() //Repeating Sprinting Sound
    {
        Play("Sprint");
    }
    public void playerHurt()
    {
        Play("Pain");
    }
    public void playerJump()
    {
        Play("Jump");
    }
}
