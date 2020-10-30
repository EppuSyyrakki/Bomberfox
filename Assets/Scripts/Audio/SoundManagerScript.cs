using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip playerMove1, playerMove2, forestMusic;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        playerMove1 = Resources.Load<AudioClip>("Step1");
        playerMove2 = Resources.Load<AudioClip>("Step2");


        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound (string clip)
    {
        switch(clip)
        {
            case "walk":
                audioSrc.PlayOneShot(playerMove1);
                break;
        }
    }

    public static void StopSound()
    {
        audioSrc.Stop();
    }

}
