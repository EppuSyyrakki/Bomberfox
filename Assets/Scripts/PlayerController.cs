using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox
{
    public class PlayerController : MonoBehaviour
    {
        private enum Direction  // helper for animator to decide which facing to use
        {
            None,
            Up,
            Right,
            Down,
            Left
        }

        [SerializeField]
        private float speed = 10f;

        // How many bombs the player can drop at the same time
        [SerializeField] 
        private int maxBombs = 3;
        [SerializeField]
        private int currentBombs = 0;   // The amount of bombs currently in the game

        [SerializeField]
        private GameObject bombPrefab = null;

        private Animator animator;
        private Direction moveDirection;
        private CollisionHandler collisionHandler;
        private Rigidbody2D rb;
        private Vector2 movement;

        private void Start()
        {
            animator = GetComponent<Animator>();
            collisionHandler = GetComponent<CollisionHandler>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            ProcessInput();
            UpdateAnimator();
        }

        private void FixedUpdate()
        {
	        rb.MovePosition(rb.position + movement * (speed * Time.fixedDeltaTime));
        }

        /// <summary>
        /// Checks input and creates a bomb if maxBombs allows.
        /// </summary>
        private void ProcessInput()
        {
	        movement.x = Input.GetAxis("Horizontal");
	        movement.y = Input.GetAxis("Vertical");

            if (Input.GetButtonDown("Fire1") && currentBombs < maxBombs)
            {
                Vector3Int pos = new Vector3Int(
                    Mathf.RoundToInt(transform.position.x),
                    Mathf.RoundToInt(transform.position.y),
                    0);
                
                if (collisionHandler.CheckPosition(pos))
                {
                    GameObject bomb = Instantiate(bombPrefab, pos, Quaternion.identity);
                    bomb.GetComponent<Bomb>().SetOwnerAndInit(this);
                }
            }
        }

        /// <summary>
        /// Updates animator component by setting triggers that define which direction player is facing.
        /// </summary>
        private void UpdateAnimator()
        {
	        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
		        animator.SetBool("Running", true);
	        else
		        animator.SetBool("Running", false);
	        
	        moveDirection = DefineMoveDirection();

	        if (moveDirection == Direction.Up) animator.SetTrigger("FacingUp");
            if (moveDirection == Direction.Right) animator.SetTrigger("FacingRight");
            if (moveDirection == Direction.Down) animator.SetTrigger("FacingDown");
            if (moveDirection == Direction.Left) animator.SetTrigger("FacingLeft");
        }

        /// <summary>
        /// Sets the moveDirection animation helper according to input.
        /// </summary>
        /// <returns>the direction which the player should be facing</returns>
        private Direction DefineMoveDirection()
        {
	        if (Input.GetAxis("Vertical") > 0) return Direction.Up;
            if (Input.GetAxis("Horizontal") > 0) return Direction.Right;
            if (Input.GetAxis("Vertical") < 0) return Direction.Down;
            if (Input.GetAxis("Horizontal") < 0) return Direction.Left;

            return Direction.None;
        }

        public void ChangeCurrentBombs(int change)
        {
	        currentBombs += change;
        }
    }
}
