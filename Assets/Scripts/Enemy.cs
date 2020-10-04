using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox
{
    public class Enemy : MonoBehaviour
    {
        public enum Direction  // helper for animator to decide which facing to use
        {
            Right,
            Left,
            Up,
            Down,
            None
        }

        [SerializeField]
        public float speed = 10f;

        public int randomDirection;

        // public Animator animator;
        private Vector3 moveTarget;
        private Vector3 currentDirection;
        private Direction moveDirection;
        public CollisionHandler collisionHandler;

        public void SetStartingPoint(Vector3 start)
        {
            moveTarget = start;
        }

        /// <summary>
        /// Checks if the enemy should move to the same direction or switch direction. Moves the enemy.
        /// </summary>
        public void Move()
        {
            // animator.SetBool("Running", true);

            if (transform.position == moveTarget)
            {
                if (IsDirectionFree())
                {
                    moveTarget += currentDirection;
                }
                else
                {
                    moveTarget = DefineNextPosition();
                    // moveDirection = DefineMoveDirection()
                    // animator.SetBool("Running", false);
                }
            }
            else
            {
                MoveToTarget(moveTarget);
            }
        }

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
        private Vector3 DefineNextPosition()
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
