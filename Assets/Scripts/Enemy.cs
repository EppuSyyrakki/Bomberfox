using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bomberfox
{
    [RequireComponent(typeof(CollisionHandler))]
    public class Enemy : MonoBehaviour
    {
		private readonly Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

	    [SerializeField] private float speed = 10f, lookDistance = 5f;
	    [SerializeField] private GameObject reservedSpace;

	    private bool spaceIsReserved;
	    private GameObject space;
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
            
			// if we are at target, get new target and update the animator according to target's direction
			if (transform.position == currentTarget)
			{
				Destroy(space);
				spaceIsReserved = false;
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
		        
		        // draw debug line for the ray TODO remove debug lines
		        if (check)
		        {
			        Debug.DrawLine(transform.position + directions[i], check.transform.position, Color.cyan);
				}
		        
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
	        if (!spaceIsReserved)
	        {
		        space = Instantiate(reservedSpace, currentTarget, Quaternion.identity, transform.parent);
		        spaceIsReserved = true;
	        }
	        else
	        {
		        transform.position = Vector3.MoveTowards(
			        transform.position,
			        currentTarget,
			        speed * Time.deltaTime);
			}
        }

        private void DefineRandomDirection()
        {
	        int i = 0;

			// TODO while loop is not very good. Make up something else.
	        while (i < 30) // limit searches to avoid endless loop if no free direction found
	        {
		        int directionIndex = Random.Range(0, 4);
                Vector3 direction = directions[directionIndex];

		        if (collisionHandler.CheckPosition(transform.position + direction))
		        {
			        randomDirection = direction;
			        break;
		        }

		        i++;

		        if (i == 30)
		        {
					print("Couldn't find a free direction, stopping");
			        randomDirection = Vector3.zero;
		        }
	        }
        }

        private void UpdateAnimator()
        {
	        Vector3 target = transform.InverseTransformPoint(currentTarget).normalized;

	        if (target == Vector3.up)
	        {
				animator.SetTrigger("FacingUp");
				animator.ResetTrigger("FacingDown");
				animator.ResetTrigger("FacingRight");
				animator.ResetTrigger("FacingLeft");
			}
			
	        if (target == Vector3.right)
	        {
		        animator.SetTrigger("FacingRight");
				animator.ResetTrigger("FacingLeft");
				animator.ResetTrigger("FacingDown");
				animator.ResetTrigger("FacingUp");
			}
			
	        if (target == Vector3.down)
	        {
		        animator.SetTrigger("FacingDown");
		        animator.ResetTrigger("FacingLeft");
		        animator.ResetTrigger("FacingRight");
		        animator.ResetTrigger("FacingUp");
			}
			
	        if (target == Vector3.left)
	        {
		        animator.SetTrigger("FacingLeft");
				animator.ResetTrigger("FacingDown");
				animator.ResetTrigger("FacingUp");
				animator.ResetTrigger("FacingRight");
			}
		}

        public void Kill()
        {
            Destroy(gameObject);
        }
    }
}
