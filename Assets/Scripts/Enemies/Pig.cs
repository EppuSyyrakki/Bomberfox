using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;

namespace Bomberfox.Enemies
{
	public class Pig : Enemy
	{
		[SerializeField] private float chargeSpeed = 3.5f;

		public override void Update()
		{
			if (!SpecialMove)
			{
				// If we're not doing the special move, do the regular update
				base.Update();
			}
			else
			{
				print("enemy at " + transform.position + ", player seen at " + playerLastSeen);
				SpecialMove = false;
			}
		}

	}
}
