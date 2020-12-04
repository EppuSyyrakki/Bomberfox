using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox.UI
{
	public class AudioUICheck : MonoBehaviour
	{
		[SerializeField]
		private GameObject MusicOn = null;
		[SerializeField]
		private GameObject MusicOff = null;
		[SerializeField]
		private GameObject SoundOn = null;
		[SerializeField]
		private GameObject SoundOff = null;

		void Start()
		{
			if (AudioManager.instance.soundsEnabled)
			{
				SoundOn.SetActive(true);
				SoundOff.SetActive(false);
			}
			
			if (!AudioManager.instance.soundsEnabled)
			{
				SoundOn.SetActive(false);
				SoundOff.SetActive(true);
			}

			if (AudioManager.instance.musicEnabled)
			{
				MusicOn.SetActive(true);
				MusicOff.SetActive(false);
			}

			if (!AudioManager.instance.musicEnabled)
			{
				MusicOn.SetActive(false);
				MusicOff.SetActive(true);
			}
		}
	}
}
