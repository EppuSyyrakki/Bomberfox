using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox
{
    public class ShockWave : MonoBehaviour
    {
        private Vector3 direction;
        private float fadeOutTime;
        private float fadeDelay;
        private float timer;
        private SpriteRenderer spriteRenderer;
        private float lerpTime;
        private CollisionHandler collisionHandler;
        private bool blocked;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            collisionHandler = GetComponent<CollisionHandler>();
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= fadeDelay)
            {
                float alpha = Mathf.Lerp(1f, 0f, lerpTime);
                Color color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
                lerpTime += Time.deltaTime / fadeOutTime;
            }
        }

        public void SetDirection(Vector3 direction)
        {
            this.direction = direction;
        }

        public void SetTimes(float fadeDelay, float fadeOutTime)
        {
            this.fadeDelay = fadeDelay;
            this.fadeOutTime = fadeOutTime;
        }
        
        /// <summary>
        /// Continues instantiating clones of gameObject after initial explosion. If a block is encountered,
        /// will not continue instantiating.
        /// </summary>
        /// <param name="distance">Distance from the original explosion</param>
        public void Continue(int distance, GameObject shockWavePrefab)
        {
            Vector3 position = direction * distance + transform.position;

            if (collisionHandler.CheckPosition(position) && !blocked)
            {
	            GameObject obj = Instantiate(shockWavePrefab, position, Quaternion.identity, transform);
	            obj.GetComponent<ShockWave>().SetTimes(fadeDelay, fadeOutTime);
            }
            else blocked = true;
        }
    }
}
