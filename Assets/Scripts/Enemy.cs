using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox
{
    [RequireComponent(typeof(CollisionHandler))]
    public class Enemy : MonoBehaviour
    {
		private readonly Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

	    [SerializeField] private float speed = 10f, lookDistance = 5f;

        private Vector3 playerLastSeen;
        private Vector3 randomDirection;
        private Vector3 currentTarget;
        private readonly Vector3 nowhere = Vector2.one * 1000;
        private CollisionHandler collisionHandler;
        private Animator animator;

        private void Awake()
        {
	        collisionHandler = GetComponent<CollisionHandler>();
	        animator = GetComponent<Animator>();
        }

        private void Start()
        {
	        playerLastSeen = nowhere;
            DefineRandomDirection();
            currentTarget = transform.position + randomDirection;
            animator.SetBool("Moving", true);
        }

        private void Update()
        {
	        // check if we can see the player somewhere
			playerLastSeen = LookForPlayer();	
            
			// if we are at target, get new target
	        if (transform.position == currentTarget) SetNewTarget();
	        
			// move to our current target
            MoveToCurrentTarget();
            UpdateAnimator();
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

                // showing debug lines for the raycasts
                Debug.DrawLine(transform.position + directions[i], transform.position + directions[i] * (lookDistance + 1), Color.cyan);

		        if (check && check.transform.CompareTag("Player"))
		        {
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
					// if it's free, change the target
					currentTarget = transform.position + nextTarget;
				}
				else
				{
					// if the player left a bomb in front of us, get a new random target
					DefineRandomDirection();
					currentTarget = transform.position + randomDirection;
				}
	        }
	        else
	        {
				// check in the previous random direction if we can move
		        if (collisionHandler.CheckPosition((transform.position + randomDirection)))
		        {
			        currentTarget = transform.position + randomDirection;
		        }
		        else
		        {
					// if we can't move, get a new random direction and set it as current target
					DefineRandomDirection();
					currentTarget = transform.position + randomDirection;
				}
	        }
        }

        private void MoveToCurrentTarget()
        {
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

		        if (collisionHandler.CheckPosition(transform.position + direction))
		        {
			        randomDirection = directions[directionIndex];
			        break;
		        }

		        i++;
	        }
        }

        private void UpdateAnimator()
        {
	        Vector3 pos = transform.position;

	        if (currentTarget.y > pos.y)
	        {
				animator.SetTrigger("FacingUp");
	        }
			else if (currentTarget.x > pos.x)
	        {
		        animator.SetTrigger("FacingRight");
	        }
			else if (currentTarget.y < pos.y)
	        {
		        animator.SetTrigger("FacingDown");
	        }
			else if (currentTarget.x < pos.x)
	        {
		        animator.SetTrigger("FacingLeft");
	        }
		}

        public void Kill()
        {
            Destroy(gameObject);
        }
    }
}
