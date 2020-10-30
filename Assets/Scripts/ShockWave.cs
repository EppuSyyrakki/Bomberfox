using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox
{
    public class ShockWave : MonoBehaviour
    {
        public Vector3 Direction { get; set; }
        public bool Blocked { get; set; }
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
        
        public void SetDelay(float fadeDelay)
        {
            this.fadeDelay = fadeDelay;
        }
        
        /// <summary>
        /// Continues instantiating clones of gameObject after initial explosion. If a block is encountered,
        /// will not continue instantiating.
        /// </summary>
        /// <param name="distance">Distance from the original explosion</param>
        public void Continue(int distance, GameObject shockWavePrefab)
        {
            Vector3 position = Direction * distance + transform.position;

            if (!Blocked && collisionHandler.CheckPosition(position))
            {
                GameObject obj = Instantiate(shockWavePrefab, position, Quaternion.identity, transform);
	            obj.GetComponent<ShockWave>().SetDelay(fadeDelay);
            }
            else Blocked = true;
        }
    }
}
