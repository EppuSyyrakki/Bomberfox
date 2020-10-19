using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField, Tooltip("How far the explosion goes"), Range(1, 10)]
        public int range = 1;

        [SerializeField, Tooltip("Speed of the explosion, n:th of a second"), Range(10, 100)]
        public float speed = 10;
        
        [SerializeField, Tooltip("In seconds"), Range(0, 1f)]
        public float fadeDelay = 0.5f;

        [SerializeField, Tooltip("In seconds")]
        private float bombTimer = 2f;
        
        [SerializeField]
        private GameObject explosionPrefab = null;

        private PlayerController owner = null;

        // Update is called once per frame
        void Update()
        {
            if (bombTimer > 0)
            {
                bombTimer -= Time.deltaTime;
            }
            else
            {
                Explode();
            }
        }

        // Creates the explosion that destroys this gameObject
        public void Explode()
        {
            owner.ChangeCurrentBombs(-1);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);
        }

        public void SetOwnerAndInit(PlayerController owner)
        {
            this.owner = owner;
            owner.ChangeCurrentBombs(1);
        }
    }
}
