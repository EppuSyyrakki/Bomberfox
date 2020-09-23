using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox
{
    public class ShockWave : MonoBehaviour
    {
        private Vector3 dir;
        private float fadeOutTime;
        private float fadeDelay;
        private float timer;
        private SpriteRenderer sr;
        private float lerpTime;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= fadeDelay)
            {
                float alpha = Mathf.Lerp(1f, 0f, lerpTime);
                Color color = sr.color;
                color.a = alpha;
                sr.color = color;
                lerpTime += Time.deltaTime / fadeOutTime;
            }
        }

        public void SetDirection(Vector3 dir)
        {
            this.dir = dir;
        }

        public void SetTimes(float fadeDelay, float fadeOutTime)
        {
            this.fadeDelay = fadeDelay;
            this.fadeOutTime = fadeOutTime;
        }
        
        /// <summary>
        /// Continues instantiating clones of gameObject after initial explosion.
        /// </summary>
        /// <param name="distance">Distance from the original explosion</param>
        public void Continue(int distance, GameObject shockWavePrefab)
        {
            Vector3 position = dir * distance + transform.position;
            GameObject obj = Instantiate(shockWavePrefab, position, Quaternion.identity, transform);
            obj.GetComponent<ShockWave>().SetTimes(fadeDelay, fadeOutTime);
        }
    }
}
