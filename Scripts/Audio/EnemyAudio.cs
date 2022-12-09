using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class EnemyAudio : Singleton<EnemyAudio>
{
    //Each sound is stored in the sound array and possesses a volume, pitch, clip, and source.
    //public static AudioManager Instance { get; private set; }

    public Sound[] sounds;
    private int noiseCycle; //indexes across a list of sounds.
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
           // Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        if (!s.isPlaying)
        {
            s.source.Play();
            return;
        }
    }
    public void PlayWithDelay(string name, float delay) //useful for random and stressful sounds
    {
        float countdown = 0f;
        countdown -= Time.deltaTime;
        Sound s = Array.Find(sounds, sound => sound.soundName == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        if (countdown > 0 || s.isPlaying)
        {
            return;
        }
        if (countdown <= 0)
        {
            countdown = UnityEngine.Random.Range(0, delay);
            s.source.Play();
        }
    }

    public void aggressiveNoise() //2 cycling sounds from a source
    {
        Play("Hunting");
    }
    public void stalkingNoise() //Sounds that reference player movement - walking
    {
        PlayWithDelay("Stalking", 15f);
    }
    public void trackingNoise() //Sounds that reference player movement - sprinting
    {
        Play("Tracking");
    }
    public void eatingNoise() //Hunter makes eating sounds
    {
        Play("Attacking");
    }
    public void enemyBreathingNoise() //Plays most of the time, until tracking
    {
        PlayWithDelay("Breathing", 9f);
    }
    public void clickingNoise()
    {
        Play("Clicking");
    }
}
