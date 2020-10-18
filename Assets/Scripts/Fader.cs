using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox
{
	public class Fader : MonoBehaviour
	{
		private SpriteRenderer spriteRenderer;
		private float lerpTime;

		public bool Fade { private get; set; }
		public float fadeOutTime = 1f;

		// Start is called before the first frame update
		void Start()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		// Update is called once per frame
		void Update()
		{
			if (Fade)
			{
				FadeOut();
			}
		}

		private void FadeOut()
		{
			float alpha = Mathf.Lerp(1f, 0f, lerpTime);
			lerpTime += Time.deltaTime / fadeOutTime;
		}
	}
}

