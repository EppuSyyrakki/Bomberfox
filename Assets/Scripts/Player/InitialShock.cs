using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Bomberfox.Player
{
	public class InitialShock : MonoBehaviour
	{
		[SerializeField]
		private GameObject shockWavePrefab = null;

		private Vector3[] directions;
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
		public void BeginExploding(Bomb.ShockType type)
		{
			if (type == Bomb.ShockType.Full)
			{
				Vector3 pos = transform.position;
				
				for (int x = -range; x <= range; x++)
				{
					for (int y = -range; y <= range; y++)
					{
						if (Mathf.Abs(x) == range && Mathf.Abs(y) == range) continue;

						Vector3 location = new Vector3(pos.x + x, pos.y + y, 0);
						GameObject obj = Instantiate(shockWavePrefab, location, Quaternion.identity, transform);
						ShockWave sw = obj.GetComponent<ShockWave>();
						sw.Direction = Vector3.zero;
						sw.SetDelay(fadeDelay);
						shockWaves.Add(sw);
					}
				}

				return;
			}

			directions = new Vector3[0];

			if (type == Bomb.ShockType.XandY || type == Bomb.ShockType.Full) 
				directions = new Vector3[4] {Vector3.up, Vector3.right, Vector3.down, Vector3.left};
			else if (type == Bomb.ShockType.X)
				directions = new Vector3[2] {Vector3.left, Vector3.right};
			else if (type == Bomb.ShockType.Y)
				directions = new Vector3[2] {Vector3.up, Vector3.down};

			for (int i = 0; i < directions.Length; i++)
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

