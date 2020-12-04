using UnityEngine.Audio;
using System;
using Bomberfox;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public Music[] musics;
    public Sound[] sounds;

    public static AudioManager instance;

    public AudioMixer masterMixer;

    public float maxMusic = 1f;
    public float maxSound = 0.75f;

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

        // Loads the musics
        foreach (Music m in musics)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume;
            m.source.loop = m.loop;
        }

        // Loads the sounds
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.volume;
            s.source.loop = s.loop;
        }
    }

    void Update()
    {
        if (GameManager.Instance.isMusicOn)
        {
            EnableMusic();
        }
        else if (!GameManager.Instance.isMusicOn)
        {
            MuteMusic();
        }

        if (GameManager.Instance.isSoundOn)
        {
            EnableSound();
        }
        else if (!GameManager.Instance.isSoundOn)
        {
            MuteSound();
        }
    }

    public void PlayMusic(string name)
    {
        Music m = Array.Find(musics, music => music.name == name);
        if (m == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        m.source.Play();
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
    }

    // Plays sound effects on top of each others
    public void OneShotSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        s.source.PlayOneShot(s.clip, s.volume);
    }

    public AudioSource GetSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return null;
        }

        return s.source;
    }

    // Stops playing the said music
    public void StopMusic(string name)
    {
        Music m = Array.Find(musics, music => music.name == name);
        if (m == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        m.source.Stop();
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

    // Stops all the game's background musics from playing. Useful for example when returning to main menu.
    public void StopGameMusic()
    {
        StopMusic("GameMusic1");
        StopMusic("GameMusic2");
        StopMusic("GameMusic3");
    }

    // Checks which version of the background music the game should play and plays it
    public void CheckGameMusic()
    {
        if (GameManager.Instance.CurrentLevel == 1)
        {
            PlayMusic("GameMusic1");
        }
        else if (GameManager.Instance.CurrentLevel == 4)
        {
            StopMusic("GameMusic1");
            PlayMusic("GameMusic2");
        }
        else if (GameManager.Instance.CurrentLevel == 7)
        {
            StopMusic("GameMusic2");
            PlayMusic("GameMusic3");
        }
    }

    public void VictoryMusic()
    {
        int sound = UnityEngine.Random.Range(1, 3);

        if (sound == 1)
        {
            PlaySound("Victory1");
        }
        else if (sound == 2)
        {
            PlaySound("Victory2");
        }
    }

    public void PauseAllSounds(bool pause)
    {
	    if (pause) MuteBomb();
	    if (!pause) EnableBomb();

        foreach (Sound sound in sounds)
	    {
		    if (pause) sound.source.Pause();
		    else sound.source.UnPause();
	    }
    }

    public void ToggleMusic()
    {
	    if (GameManager.Instance.isMusicOn) GameManager.Instance.isMusicOn = false;
	    else GameManager.Instance.isMusicOn = true;
    }

    public void ToggleSound()
    {
	    if (GameManager.Instance.isSoundOn) GameManager.Instance.isSoundOn = false;
        else GameManager.Instance.isSoundOn = true;
    }

    public void MuteMusic()
    {
        foreach (Music m in musics)
        {
            m.source.volume = 0f;
        }
    }

    public void MuteSound()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = 0f;
        }

        MuteBomb();
    }

    public void EnableMusic()
    {
        foreach (Music m in musics)
        {
            m.source.volume = maxMusic;
        }
    }

    public void EnableSound()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume;
        }

        EnableBomb();
    }

    public void MuteBomb()
    {
        masterMixer.SetFloat("Bomb", -80f);
        print("bomb channel at -80");
    }

    public void EnableBomb()
    {
        masterMixer.SetFloat("Bomb", 0f);
    }
}

