using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
	private bool paused;

    // Update is called once per frame
    private void Update()
    {
        // press menu button to skip to game
        if (Input.GetButtonDown("Menu")) GameManager.Instance.GoToGame();

        if (paused)
        {
	        Time.timeScale = 0f;

	        if (Input.anyKeyDown)
	        {
		        paused = false;
		        Time.timeScale = 1f;
            }
        }
    }

    // called from animation event
    private void Pause()
    {
	    paused = true;
    }

    // called from animation event
    private void End()
    {
	    GameManager.Instance.GoToGame();
    }
}
