using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bomberfox
{
    [RequireComponent(typeof(CollisionHandler))]
    public class Enemy : MonoBehaviour
    {
	    private readonly Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

        public enum Direction  // helper for animator to decide which facing to use
        {
            Right,
            Left,
            Up,
            Down,
            None
        }

	    [SerializeField] private float speed = 10f, lookDistance = 5f;

        [NonSerialized]
        public int randomDirection;

        private Vector3 moveTarget;
        private Vector3 playerLastSeen; // point where player was detected. own position if not detected
        private Vector3 currentDirection;
        private Direction moveDirection;
        private CollisionHandler collisionHandler;
        private Animator animator;
        private float lookTimer;

        [SerializeField] 
        private LayerMask layerMask;
        
        private void Awake()
        {
	        collisionHandler = GetComponent<CollisionHandler>();
	        animator = GetComponent<Animator>();
        }

        private void Start()
        {
	        moveTarget = transform.position;
            animator.SetBool("Moving", true);
        }

        private void Update()
        {
	        lookTimer += Time.deltaTime;

	        if (lookTimer > 0.5f)
	        {
		        playerLastSeen = LookForPlayer();
		        lookTimer = 0;
	        }

            Move();
	        UpdateAnimator();
        }

        /// <summary>
        /// Searches for tag Player in layerMask field.
        /// </summary>
        /// <returns>Position of player rounded to int if found, or own position if not found </returns>
        private Vector3 LookForPlayer()
        {
	        for (int i = 0; i < 4; i++)
	        {
		        var check = Physics2D.Raycast(
			        transform.position + directions[i],
			        directions[i],
			        lookDistance);
                Debug.DrawLine(transform.position + directions[i], transform.position + directions[i] * (lookDistance + 1), Color.cyan, 0.25f);

		        if (check && check.transform.CompareTag("Player"))
		        {
                    print("found player");
			        Vector3 pos = new Vector3(
				        Mathf.RoundToInt(check.transform.position.x),
				        Mathf.RoundToInt(check.transform.position.y),
				        0);
			        return pos;
		        }
	        }

	        return transform.position;
        }

        private void Move()
        {
	        if (transform.position == playerLastSeen)
	        {
		        // MoveToTarget();
	        }
        }

        private void UpdateAnimator()
        {
	        // if the moving direction changes, tell the animator so it can swap to another state
        }

        /// <summary>
        /// Checks if the enemy should move to the same direction or switch direction. Moves the enemy.
        /// </summary>


        /// <summary>
        /// Checks if the enemy can still move to the same direction.
        /// </summary>
        private bool IsDirectionFree()
        {
            Vector3 nextPos = moveTarget + currentDirection;
            if (nextPos != moveTarget && collisionHandler.CheckPosition(nextPos)) return true;

            return false;
        }

        /// <summary>
        /// Defines a new random position for the enemy to move. Checks if new position is free.
        /// </summary>
        /// <returns>Current transform plus a direction vector.</returns>
        private Vector3 MoveRandom()
        {
            randomDirection = Random.Range(0, 4);

            Vector3 direction = new Vector3();
            if (randomDirection == 0) direction = Vector3.right;
            else if (randomDirection == 1) direction = Vector3.left;
            else if (randomDirection == 2) direction = Vector3.up;
            else if (randomDirection == 3) direction = Vector3.down;

            // add new direction to current location
            Vector3 nextPos = moveTarget + direction;

            // if the new position is not own position, doesn't have collider and tag "Block", return it
            if (nextPos != moveTarget && collisionHandler.CheckPosition(nextPos))
            {
                currentDirection = direction;
                return nextPos;
            }

            // otherwise return own position
            return moveTarget;
        }

        /// <summary>
        /// Sets the moveDirection animation helper according to input
        /// </summary>
        /// <returns>the direction which the player should be facing</returns>
        private Direction DefineMoveDirection()
        {
            if (randomDirection == 0) return Direction.Right;
            else if (randomDirection == 1) return Direction.Left;
            else if (randomDirection == 2) return Direction.Up;
            else if (randomDirection == 3) return Direction.Down;
            else return Direction.None;
        }

        private void MoveToTarget(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                moveTarget,
                speed * Time.deltaTime);
        }

        public void Kill()
        {
            Destroy(gameObject);
        }
    }
}
