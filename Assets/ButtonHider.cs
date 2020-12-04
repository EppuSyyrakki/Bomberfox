using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bomberfox.UI
{
	public class ButtonHider : MonoBehaviour
	{
		[SerializeField]
		private Button[] buttons = null;

		private void Awake()
		{
			GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
		}

		private void EnableButtons()
		{
			foreach (Button b in buttons)
			{
				b.interactable = true;
			}

			// set start game button as default selected
			Button btn = GameObject.Find("StartGameButton").GetComponent<Button>();
			EventSystem es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
			es.SetSelectedGameObject(null);
			es.SetSelectedGameObject(btn.gameObject);
		}

		private void DisableButtons()
		{
			foreach (var button in buttons)
			{
				button.interactable = false;
			}
		}
	}
}
