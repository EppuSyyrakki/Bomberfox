using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bomberfox.Player;

namespace Bomberfox.Enemies
{
    [RequireComponent(typeof(CollisionHandler))]
    public class Enemy : MonoBehaviour
    {
	    [SerializeField] private float speed = 10f, lookDistance = 5f;
	    [SerializeField] private GameObject reservedSpace = null;
	    [SerializeField, Range(0, 100)] public int specialMoveChance = 10;
	    [SerializeField] private float specialMoveCoolDown = 5f;

	    private readonly Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
		private readonly Vector3 nowhere = Vector2.one * 1000;
		
	    private Vector3 randomDirection = Vector3.zero;
        private Vector3 currentTarget;
        private CollisionHandler collisionHandler;
        private float spawnTimer, spawnTime, specialMoveTimer;
        private bool isAlive = true;

        public bool SpecialMove { get; set; }
        [HideInInspector]
		public Animator animator;
		[HideInInspector]
		public Vector3 playerLastSeen;
		[HideInInspector]
		public bool spaceIsReserved;
		[HideInInspector]
		public GameObject space;

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

        public virtual void Update()
        {
	        spawnTimer += Time.deltaTime;
	        specialMoveTimer += Time.deltaTime;

			// if just spawned or are dead, don't update at all
	        if (spawnTimer < spawnTime || isAlive == false) return;

	        // if we are at where the player was last seen, reset the player seen position
	        if (transform.position == playerLastSeen) playerLastSeen = nowhere;

	        LookForPlayer();

			// if we are at target, get new target and update the animator according to target's direction
			if (transform.position == currentTarget)
			{
				if (spaceIsReserved)
				{
					Destroy(space);
					spaceIsReserved = false;
				}

				// get a random number to see if we should do the special move
				if (Random.Range(1, 101) < specialMoveChance && specialMoveTimer > specialMoveCoolDown)
				{
					SpecialMove = true;
					specialMoveTimer = 0;
				}
				else
				{
					// if no special move, move normally
					SetNewTarget();
					UpdateAnimator();
				}
			}
	        
			// move to our current target if we are not doing the special move
	        if (!SpecialMove) MoveToCurrentTarget();
        }

        private void LookForPlayer()
        {
	        for (int i = 0; i < 4; i++)
	        {
				// look in all vectors saved in directions array
		        var check = Physics2D.Raycast(
			        transform.position + directions[i],
			        directions[i],
			        lookDistance);
		        
				// if the raycast hit something & that something has PlayerController component
		        if (check && check.transform.gameObject.TryGetComponent(out PlayerController player))
		        {
					// show sight line in scene view
					Debug.DrawLine(transform.position + directions[i], player.transform.position, Color.cyan);

					// if the player in invulnerable, we don't "see" them
			        if (player.isInvulnerable) playerLastSeen = nowhere;

			        // get the player's position rounded to whole numbers
			        Vector3 pos = new Vector3(
				        Mathf.RoundToInt(check.transform.position.x),
				        Mathf.RoundToInt(check.transform.position.y),
				        0);

					// if the player is at the same row or column, save the rounded player position
			        if (pos.x == Mathf.RoundToInt(transform.position.x) 
			            || pos.y == Mathf.RoundToInt(transform.position.y))
			        {
				        playerLastSeen = pos;
					}
			        else
			        {
				        playerLastSeen = nowhere;
			        }
		        }
	        }
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
		        ReserveSpace(currentTarget);
		        return;
	        }
			
			// if we got this far, we've reserved a space. Now move toward it
			transform.position = Vector3.MoveTowards(
				transform.position,
				currentTarget,
				speed * Time.deltaTime);
        }

        public void ReserveSpace(Vector3 pos)
        {
	        space = Instantiate(reservedSpace, pos, Quaternion.identity, transform.parent);
	        spaceIsReserved = true;
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

			// TODO Destroy(GetComponent<Collider2D>());
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
