using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox.Player
{
	public class InitialShock : MonoBehaviour
	{
		[SerializeField]
		private GameObject shockWavePrefab = null;

		private readonly Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
		private int range;
		private float speed;
		private float fadeDelay;
		private CollisionHandler collisionHandler;
		private List<ShockWave> shockWaves = new List<ShockWave>();
		public bool[] Blocked { get; set; } = {true, true, true, true};

		private void Awake()
		{
			collisionHandler = GetComponent<CollisionHandler>();
		}

		public void BeginCoroutineToContinue()
		{
			StartCoroutine(nameof(ContinueShocks));
		}

		/// <summary>
		/// Loops 4 directions, creates shocks and gives them directions to continue in. Adds the initial shocks to list.
		/// </summary>
		public void BeginExploding()
		{
			for (int i = 0; i < 4; i++)
			{
				
				GameObject obj = Instantiate(shockWavePrefab, transform.position, Quaternion.identity, transform);
				ShockWave sw = obj.GetComponent<ShockWave>();
				sw.Direction = directions[i];
				sw.SetDelay(fadeDelay);
				shockWaves.Add(sw);
			}
		}

		/// <summary>
		/// Waits for 'speed' amount of time and creates new shocks from the initial shocks. Sends a distance to the
		/// initial shocks to instantiate at. Loops for 'range' amount of cycles. The check for blocking objects is done
		/// inside the Continue method in ShockWave script.
		/// </summary>
		/// <returns>To update for 'speed' amount of seconds</returns>
		private IEnumerator ContinueShocks()
		{
			for (int i = 0; i <= range; i++)
			{
				yield return new WaitForSeconds(speed);

				foreach (ShockWave s in shockWaves)
				{
					s.Continue(i, shockWavePrefab);
				}
			}
		}

		public void ReceiveBombParameters(int range, float speed, float fadeDelay)
		{
			this.range = range;
			this.speed = speed;
			this.fadeDelay = fadeDelay;
		}
	}
}

