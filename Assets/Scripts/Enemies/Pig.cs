﻿using System;
using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;

namespace Bomberfox.Enemies
{
	public class Pig : Enemy
	{
		[SerializeField] private float chargeSpeed = 3.5f;

		private Vector3 chargeDir = Vector3.zero;
		private Vector3 nextTarget = Vector3.zero;
		
		private bool chargeMovementStarted = false;
		private bool specialTriggered = false;
		
		public override void Update()
		{
			if (!SpecialMove)
			{
				// If we're not doing the special move, do the regular update
				base.Update();
				return;
			}

			// if we haven't seen player, don't do special
			if (PlayerLastSeen == Nowhere)
			{
				ResetSpecials();
				base.Update();
				return;
			}

			// Do this on the first update of this special move
			if (!specialTriggered)
			{
				UpdateAnimator(PlayerLastSeen);
				Anim.SetTrigger("Special");
				specialTriggered = true;
			}

			if (!chargeMovementStarted) return;

			if (transform.position == nextTarget)
			{
				if (Space != null) Destroy(Space);

				LastPosition = transform.position;
				Vector3 checkPos = transform.position + chargeDir;

				if (CollisionHandler.CheckPigCharge(checkPos))
				{
					nextTarget = checkPos;
					ReserveSpace(checkPos);
				}
				else
				{
					Anim.SetTrigger("EndSpecial");
					chargeMovementStarted = false;
				}
			}

			if (!IsAlive) return;

			transform.position = Vector3.MoveTowards(transform.position, nextTarget,
				chargeSpeed * Time.deltaTime);
		}

		// called from animation event at the end of StartCharge animation clip
		private void StartCharge()
		{
			chargeDir = transform.InverseTransformPoint(PlayerLastSeen).normalized;
			nextTarget = transform.position;
			chargeMovementStarted = true;
		}

		// called from animation event at end of StopCharge animation clip
		private void EndCharge()
		{
			ResetSpecials();
			PlayerLastSeen = Nowhere;
			DefineRandomDirection();
			CurrentTarget = transform.position + Direction;
			UpdateAnimator(CurrentTarget);
		}

        public override void StartDeath()
        {
            base.StartDeath();
            AudioManager.instance.OneShotSound("PigDeath");
        }

        private void ResetSpecials()
        {
	        Anim.ResetTrigger("EndSpecial");
	        Anim.ResetTrigger("Special");
			SpecialMove = false;
	        SpecialMoveTimer = 0;
	        specialTriggered = false;
	        chargeMovementStarted = false;
        }
	}
}
