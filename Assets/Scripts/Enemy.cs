using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bomberfox
{
    [RequireComponent(typeof(CollisionHandler))]
    public class Enemy : MonoBehaviour
    {
	    [SerializeField] private float speed = 10f, lookDistance = 5f;
	    [SerializeField] private GameObject reservedSpace = null;

	    private readonly Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
		private readonly Vector3 nowhere = Vector2.one * 1000;
		private bool spaceIsReserved;
	    private GameObject space;
		private Vector3 playerLastSeen;
        private Vector3 randomDirection = Vector3.zero;
        private Vector3 currentTarget;
        private CollisionHandler collisionHandler;
        private Animator animator;
        private float spawnTimer = 0f;
		private float spawnTime = 0f;
		private bool isAlive = true;

        private void Awake()
        {
	        collisionHandler = GetComponent<CollisionHandler>();
	        animator = GetComponent<Animator>();
        }

        private void Start()
        {
	        spawnTime = animator.GetCurrentAnimatorStateInfo(0).length + 0.1f;
	        playerLastSeen = nowhere;
	        currentTarget = transform.position + randomDirection;
        }

        private void Update()
        {
	        spawnTimer += Time.deltaTime;

			// if just spawned or dead, don't update at all
	        if (spawnTimer < spawnTime || isAlive == false) return;

	        // check if we can see the player somewhere
			playerLastSeen = LookForPlayer();	
            
			// if we are at target, get new target and update the animator according to target's direction
			if (transform.position == currentTarget)
			{
				if (spaceIsReserved)
				{
					Destroy(space);
					spaceIsReserved = false;
				}

				SetNewTarget();
				UpdateAnimator();
			}
	        
			// move to our current target
             MoveToCurrentTarget();
        }

        private Vector3 LookForPlayer()
        {
	        for (int i = 0; i < 4; i++)
	        {
				// look in all vectors saved in directions array
		        var check = Physics2D.Raycast(
			        transform.position + directions[i],
			        directions[i],
			        lookDistance);
		        
		        if (check && check.transform.gameObject.TryGetComponent(out PlayerController player))
		        {
					// show sight line in scene view
					Debug.DrawLine(transform.position + directions[i], player.transform.position, Color.cyan);

			        if (player.isInvulnerable) return nowhere;

			        // if we see the player, return the players position rounded to whole numbers
			        Vector3 pos = new Vector3(
				        Mathf.RoundToInt(check.transform.position.x),
				        Mathf.RoundToInt(check.transform.position.y),
				        0);
			        return pos;
		        }
	        }

			// if we can't see the player, return a "null" vector outside game area
	        return nowhere;
        }

        private void SetNewTarget()
        {
	        if (playerLastSeen != nowhere)	// if we have seen the player
	        {
				// get the players position as a direction relative to us, with magnitude of 1
		        Vector3 nextTarget = transform.InverseTransformPoint(playerLastSeen).normalized;

		        // check that position in case the player left a bomb on the way
				if (collisionHandler.CheckPosition(nextTarget))	
				{
					// if it's free, change the target and return
					currentTarget = transform.position + nextTarget;
					return;
				}
	        }
	        else // if we haven't seen the player
	        {
				// check in the previous random direction if we can move
		        if (collisionHandler.CheckPosition((transform.position + randomDirection)))
		        {
			        currentTarget = transform.position + randomDirection;
			        return;
		        }
	        }

			// if we got this far we can't move, so create a new random direction
	        DefineRandomDirection();
	        currentTarget = transform.position + randomDirection;
        }

        private void MoveToCurrentTarget()
        {
			// if we haven't reserved a space and it's free, do that and exit this method
	        if (!spaceIsReserved && collisionHandler.CheckPosition(currentTarget))
			{
				space = Instantiate(reservedSpace, currentTarget, Quaternion.identity, transform.parent);
				spaceIsReserved = true;
				return;
			}
			
			// if we got this far, we've reserved a space. Now move toward it
			transform.position = Vector3.MoveTowards(
				transform.position,
				currentTarget,
				speed * Time.deltaTime);
        }

        private void DefineRandomDirection()
        {
	        int i = 0;

	        while (i < 10) // limit searches to avoid endless loop if no free direction found
	        {
		        int directionIndex = Random.Range(0, 4);
                Vector3 direction = directions[directionIndex];

				// if we find a free direction, assign that and exit this method
		        if (collisionHandler.CheckPosition(transform.position + direction))
		        {
			        randomDirection = direction;
			        return;
		        }

		        i++;
	        }

			// if we got this far, we can't move anywhere so assign a zero vector
	        randomDirection = Vector3.zero;
		}

        private void UpdateAnimator()
        {
			// change current target into a direction (with magnitude of 1) relative to our location 
	        Vector3 target = transform.InverseTransformPoint(currentTarget).normalized;

	        if (target == Vector3.up) SetTrigger("FacingUp");
	        if (target == Vector3.right) SetTrigger("FacingRight");
			if (target == Vector3.down) SetTrigger("FacingDown");
			if (target == Vector3.left) SetTrigger("FacingLeft");
			
			// if the enemy is "stuck", display moving down animation
			if (target == Vector3.zero) SetTrigger("FacingDown");
        }

		// Set one facing trigger and reset others
        private void SetTrigger(string triggerToSet)
        {
			animator.SetTrigger(triggerToSet);

			if (triggerToSet != "FacingUp") animator.ResetTrigger("FacingUp");
			if (triggerToSet != "FacingRight") animator.ResetTrigger("FacingRight");
			if (triggerToSet != "FacingDown") animator.ResetTrigger("FacingDown");
			if (triggerToSet != "FacingLeft") animator.ResetTrigger("FacingLeft");
		}

        public void StartDeath()
        {
	        if (space != null) Destroy(space);
			animator.SetTrigger("Die");
			isAlive = false;
        }

		// triggered from animation event
        private void EndDeath()
        {
	        Destroy(gameObject);
		}
    }
}
