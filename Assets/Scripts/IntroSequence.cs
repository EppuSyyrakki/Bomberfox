using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
	private bool end = false;
	private bool ready = false;
	private Animator a;

	private void Awake()
	{
		a = GetComponent<Animator>();
	}

    // Update is called once per frame
    private void Update()
    {
        // press menu button to skip to game
        if (Input.GetButtonDown("Menu")) GameManager.Instance.GoToGame();

		// press any key to...
        if (Input.anyKeyDown)
        {
			// go to next if ready
	        if (ready)
	        {
		        a.SetTrigger("NextAnimation");
		        ready = false;
		        return;
	        }

	        AnimatorClipInfo[] info = a.GetCurrentAnimatorClipInfo(0);
	        a.Play(info[0].clip.name, 0, 1f);
	        ready = true;
        }

        a.ResetTrigger("NextAnimation");
	}

	// called from animation event
    private void ReadyToContinue()
    {
	    ready = true;
    }

    // called from animation event
    private void End()
    {
		Invoke(nameof(InvokeGame), 1f);
		StartCoroutine(FindObjectOfType<FadeOutUI>().FadeBlackOutSquare());
	}

	private void InvokeGame()
	{
		GameManager.Instance.GoToGame();
	}
}
