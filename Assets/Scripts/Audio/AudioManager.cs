using UnityEngine.Audio;
using System;
using Bomberfox;
using Bomberfox.Player;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public Music[] musics;
    public Sound[] sounds;

    public static AudioManager instance;

    public AudioMixer masterMixer;

    public float maxMusic = 1f;
    public float maxSound = 0.75f;

    public bool musicEnabled = true;
    public bool soundsEnabled = true;

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
	    Bomb[] bombs = FindObjectsOfType<Bomb>();

	    foreach (Bomb bomb in bombs)
	    {
		    bomb.PauseSound(pause);
	    }

        foreach (Sound sound in sounds)
	    {
		    if (pause) sound.source.Pause();
		    else sound.source.UnPause();
	    }
    }

    public void MuteMusic()
    {
        foreach (Music m in musics)
        {
            m.source.volume = 0f;
        }

        musicEnabled = false;
    }

    public void MuteSound()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = 0f;
        }

        masterMixer.SetFloat("Bomb", -80f);
        soundsEnabled = false;
    }

    public void EnableMusic()
    {
        foreach (Music m in musics)
        {
            m.source.volume = maxMusic;
        }

        musicEnabled = true;
    }

    public void EnableSound()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume;
        }

        masterMixer.SetFloat("Bomb", 0f);
        soundsEnabled = true;
    }
}

