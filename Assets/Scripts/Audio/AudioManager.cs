using UnityEngine.Audio;
using System;
using Bomberfox;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
    }

    public void StopPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

    public void StopPlayingGameMusic()
    {
        StopPlaying("GameMusic1");
        StopPlaying("GameMusic2");
        StopPlaying("GameMusic3");
    }

    public void CheckGameMusic()
    {
        if (GameManager.Instance.CurrentLevel == 1)
        {
            Play("GameMusic1");
        }
        else if (GameManager.Instance.CurrentLevel == 2)
        {
            StopPlaying("GameMusic1");
            Play("GameMusic2");
        }
        else if (GameManager.Instance.CurrentLevel == 3)
        {
            StopPlaying("GameMusic2");
            Play("GameMusic3");
        }
    }
}

