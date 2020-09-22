﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

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
            timer = 0;
        }

        private void Update()
        {
            if (timer >= fadeDelay)
            {
                float alpha = Mathf.Lerp(1f, 0f, lerpTime);
                print(lerpTime);
                Color color = sr.color;
                color.a = alpha;
                sr.color = color;
            }

            lerpTime += Time.deltaTime / fadeOutTime;
            timer += Time.deltaTime;

            if (lerpTime > 1)
            {
                Destroy(gameObject);
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
