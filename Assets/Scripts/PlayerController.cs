using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Bomberfox
{
    public class PlayerController : MonoBehaviour
    {
        public enum Direction
        {
            Down,
            Left,
            Up,
            Right
        }

        [SerializeField]
        private float speed = 10f;

        [SerializeField]
        private GameObject bombPrefab = null;

        private Animator animator;

        private Direction direction;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            ProcessMovement();
            ProcessFire();
        }

        private void ProcessMovement()
        {
            // Get input values and calculate offsets to previous position
            float xOffset = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float yOffset = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            // Restrict "player" from moving out of screen, numbers from pixels per unit of graphic. Temporary fix.
            float newX = Mathf.Clamp(transform.position.x + xOffset, 0.5f, 15.5f);
            float newY = Mathf.Clamp(transform.position.y + yOffset, 0.5f, 8.5f);

            // Move "player"
            transform.position = new Vector2(newX, newY);
        }

        public void ProcessFire()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            }
        }

        private void UpdateAnimator()
        {

        }
    }
}
