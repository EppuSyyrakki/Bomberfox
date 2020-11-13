using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;

namespace Bomberfox.Enemies
{
	public class Bunny : Enemy
	{
		[SerializeField, Range(2, 5)] private int jumpRange = 3;

		private bool disableInvoked = false;
		private bool isJumping = false;
		private Vector3 startPos;
		private float lerpT;
		private float jumpTime;
		private float endTime;

		public override void Start()
		{
			base.Start();
			
			// get the animation lengths to set up the special move
			RuntimeAnimatorController ac = Anim.runtimeAnimatorController;

			for (int i = 0; i < ac.animationClips.Length; i++)
			{
				if (ac.animationClips[i].name == "Jump")
					jumpTime = ac.animationClips[i].length;
				if (ac.animationClips[i].name == "StopJump")
					endTime = ac.animationClips[i].length;
			}
		}

		public override void Update()
		{
			if (!SpecialMove)
			{
				base.Update();
				return;
			}

			if (!disableInvoked && IsAlive)
			{
				if (GetDestination())
				{
					Invoke(nameof(DisableCollider), Time.deltaTime);
					Anim.SetTrigger("Special");
					startPos = transform.position;
				}
				else
				{
					EndSpecial();
				}

				disableInvoked = true;
			}

			if (isJumping)
			{
				lerpT += Time.deltaTime / jumpTime;
				transform.position = Vector3.Lerp(startPos, Space.transform.position, lerpT);
			}
		}

		private bool GetDestination()
		{
			Vector3 startPos = transform.position;

			for (int i = 0; i < 20; i++)    // try 20 times to find a new spot to jump to.
			{
				Vector3 tryDestination = startPos + new Vector3(
					Random.Range(-jumpRange, jumpRange + 1),
					Random.Range(-jumpRange, jumpRange + 1));

				if (CollisionHandler.CheckPosition(tryDestination))
				{
					ReserveSpace(tryDestination);
					return true;
				}
			}

			return false;
		}

		// called from animation event at the end of StartJump
		private void StartJump()
		{
			isJumping = true;
		}


		// called from animation event at the end of Jump
		private void EndJump()
		{
			Invoke(nameof(EndSpecial), endTime);
		}

		private void DisableCollider()
		{
			GetComponent<BoxCollider2D>().enabled = false;
		}

		private void EndSpecial()
		{
			isJumping = false;
			lerpT = 0;
			SpecialMoveTimer = 0;
			Destroy(Space);
			disableInvoked = false;
			SpecialMove = false;
			GetComponent<BoxCollider2D>().enabled = true;
			DefineRandomDirection();
			CurrentTarget = transform.position + Direction;
		}

		public override void StartDeath()
		{
			base.StartDeath();
			AudioManager.instance.OneShotSound("RabbitDeath");
		}
	}
}
