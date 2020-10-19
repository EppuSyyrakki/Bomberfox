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
            Right,
            Left,
            Up,
            Down,
            None
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
        private Vector3 moveTarget;
        private Vector3 currentTarget;
        private Direction moveDirection;
        private CollisionHandler collisionHandler;

        private void Start()
        {
            animator = GetComponent<Animator>();
            collisionHandler = GetComponent<CollisionHandler>();
        }

        private void Update()
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
	            animator.SetBool("Running", true);
            }
            else
            {
	            animator.SetBool("Running", false);
            }

            ProcessMovement();
            ProcessFire();
            UpdateAnimator();
        }

        /// <summary>
        /// Sets the moveDirection animation helper according to input.
        /// </summary>
        /// <returns>the direction which the player should be facing</returns>
        private Direction DefineMoveDirection()
        {
            if (Input.GetAxis("Horizontal") > 0) return Direction.Right;
            else if (Input.GetAxis("Horizontal") < 0) return Direction.Left;
            else if (Input.GetAxis("Vertical") > 0) return Direction.Up;
            else if (Input.GetAxis("Vertical") < 0) return Direction.Down;
            else return Direction.None;
        }

        /// <summary>
        /// Process player movement according to input
        /// </summary>
        private void ProcessMovement()
        {
	        // Get input values and calculate offsets to previous position
	        float xOffset = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
	        float yOffset = Input.GetAxis("Vertical") * speed * Time.deltaTime;

	        // Restrict "player" from moving out of screen, numbers from pixels per unit of graphic. Temporary fix.
	        float newX = Mathf.Clamp(transform.position.x + xOffset, -6f, 6f);
	        float newY = Mathf.Clamp(transform.position.y + yOffset, -4f, 4f);

	        // Move "player"
	        transform.position = new Vector2(newX, newY);
        }

        /// <summary>
        /// Checks input and creates a bomb if maxBombs allows.
        /// </summary>
        private void ProcessFire()
        {
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
            if (moveDirection == Direction.Right) animator.SetTrigger("FacingRight");
            if (moveDirection == Direction.Left) animator.SetTrigger("FacingLeft");
            if (moveDirection == Direction.Up) animator.SetTrigger("FacingUp");
            if (moveDirection == Direction.Down) animator.SetTrigger("FacingDown");
        }

        public void ChangeCurrentBombs(int change)
        {
	        currentBombs += change;
        }
    }
}
