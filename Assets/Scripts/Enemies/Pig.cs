using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;

namespace Bomberfox.Enemies
{
	public class Pig : Enemy
	{
		private bool invoked = false;

		public override void Update()
		{
			if (!SpecialMove)
			{
				base.Update();
			}
			else
			{
				if (!invoked)
				{
					print(gameObject.name + " is doing the special move");
					Invoke(nameof(ResetSpecial), 1f);
					invoked = true;
				}
			}
		}

		private void ResetSpecial()
		{
			SpecialMove = false;
		}
	}
}
