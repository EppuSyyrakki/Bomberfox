﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox.Player
{
    public class ShockWave : MonoBehaviour
    {
        public Vector3 Direction { get; set; }
        public bool Blocked { get; set; }
        public bool Penetration { get; private set; }
        private float fadeDelay;
        private float timer;
        private CollisionHandler collisionHandler;
        private Fader fader;

        private void Awake()
        {
	        collisionHandler = GetComponent<CollisionHandler>();
            fader = GetComponent<Fader>();
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= fadeDelay)
            {
	            fader.Fade = true;
            }
        }
        
        public void SetParameters(float fadeDelay, bool penetration)
        {
            this.fadeDelay = fadeDelay;
            this.Penetration = penetration;
        }
        
        /// <summary>
        /// Continues instantiating clones of gameObject after initial explosion. If a block is encountered with CollisionHandler,
        /// will not continue instantiating.
        /// </summary>
        /// <param name="distance">Distance from the original explosion</param>
        public void Continue(int distance, GameObject shockWavePrefab)
        {
            Vector3 position = Direction * distance + transform.position;

            if (!Blocked && collisionHandler.CheckPosition(position))
            {
                GameObject obj = Instantiate(shockWavePrefab, position, Quaternion.identity, transform);
	            obj.GetComponent<ShockWave>().SetParameters(fadeDelay, Penetration);
            }
            else Blocked = true;
        }
    }
}
