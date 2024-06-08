using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] longSounds, shortSounds;

    public AudioSource uiSource, extinguisherSource, boltSource, fireSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayUISound(string name)
    {
        PlaySound(name, shortSounds, uiSource);
    }

    public void PlayExtinguisherSound(string name)
    {
        PlaySound(name, longSounds, extinguisherSource);

    }

    public void PlayBoltSound(string name)
    {
        PlaySound(name, shortSounds, boltSource);
    }

    public void PlayFireSound(string name)
    {
        PlaySound(name, longSounds, fireSource);
    }

    private void PlaySound(string soundName, Sound[] soundArray, AudioSource source)
    {
        Sound sound = Array.Find(soundArray, s => s.name == soundName);

        if (sound != null)
        {
            source.clip = sound.clip;
            source.Play();
        }
        else
        {
            Debug.LogWarning("Can't find: " + soundName);
        }
    }
}

