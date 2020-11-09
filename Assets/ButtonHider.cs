using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHider : MonoBehaviour
{
	[SerializeField]
	private GameObject startButton, quitButton;

	private void EnableButtons()
	{
		startButton.SetActive(true);
		quitButton.SetActive(true);
	}

	private void DiableButtons()
	{
		startButton.SetActive(false);
		quitButton.SetActive(false);
	}
}
