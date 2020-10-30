using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip playerMove, bombFuse, bombExplode1, bombExplode2, forestMusic;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = Resources.Load<AudioClip>("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
