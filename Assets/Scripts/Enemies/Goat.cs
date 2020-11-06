using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox.Enemies
{
	public class Goat : Enemy
	{
		[SerializeField, Range(2, 5)] private int teleportRange = 3;

		private bool hasMoved = false;
		private bool disableInvoked = false;

		public override void Update()
		{
			if (!SpecialMove)
			{
				base.Update();
				return;
			}

			if (!disableInvoked)
			{
				Anim.SetTrigger("Special");
				Invoke(nameof(DisableCollider), 0.25f);
				disableInvoked = true;
			}
		}

		private void MovePosition()
		{
			Vector3 startPos = transform.position;

			for (int i = 0; i < 20; i++)	// try 20 times to find a new spot to move to.
			{
				Vector3 tryDestination = startPos + new Vector3(
					Random.Range(-teleportRange, teleportRange + 1), 
					Random.Range(-teleportRange, teleportRange + 1));

				if (CollisionHandler.CheckPosition(tryDestination))
				{
					transform.position = tryDestination;
					hasMoved = true;
					break;
				}
			}
		}

		private void DisableCollider()
		{
			GetComponent<Collider2D>().enabled = false;
		}

		private void EndSpecial()
		{
			disableInvoked = false;
			SpecialMove = false;
			GetComponent<Collider2D>().enabled = true;
			DefineRandomDirection();
			CurrentTarget = transform.position + Direction;
		}
	}
}
