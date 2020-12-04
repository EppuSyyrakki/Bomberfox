using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHider : MonoBehaviour
{
	[SerializeField]
	private Button[] buttons = null;

	private void EnableButtons()
	{
		foreach (Button b in buttons)
		{
			b.interactable = true;
		}
	}

	private void DisableButtons()
	{
		foreach (var button in buttons)
		{
			button.interactable = false;
		}
	}
}
