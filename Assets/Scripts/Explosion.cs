using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField]
        private GameObject shockWavePrefab = null;

        private GameObject parentBomb = null;
        private int range;
        private float speed;
        private float fadeDelay;
        private float fadeOutTime;
        private float totalTime;
        private float timer;
        private Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
        private List<ShockWave> shockWaves = new List<ShockWave>();
        private SpriteRenderer sr;
        private float lerpTime;
        private BoxCollider2D boxCollider;

        private void Awake()
        {
            parentBomb = transform.parent.gameObject;
            sr = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            FetchBombParameters();
            transform.parent = null;
            Destroy(parentBomb);
            BeginExploding();
            StartCoroutine(nameof(ContinueShocks));
            totalTime = speed * range + fadeDelay + fadeOutTime;    // time to wait before destroying gameObject
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
            else if (timer > totalTime)
            {
                Destroy(gameObject);
            }
        }

        private void FetchBombParameters()
        {
            Bomb bomb = parentBomb.GetComponent<Bomb>();
            range = bomb.range;
            speed = 1 / bomb.speed;
            fadeDelay = bomb.fadeDelay;
            fadeOutTime = bomb.fadeOutTime;
        }

        /// <summary>
        /// Loops 4 directions, creates shocks and gives them directions to continue in. Adds the initial shocks to list.
        /// </summary>
        private void BeginExploding()
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject obj = Instantiate(shockWavePrefab,
                    transform.position + directions[i],
                    Quaternion.identity,
                    transform);
                ShockWave s = obj.GetComponent<ShockWave>();
                s.SetDirection(directions[i]);
                s.SetTimes(fadeDelay, fadeOutTime);
                shockWaves.Add(s);
            }
        }

        /// <summary>
        /// Waits for 'speed' amount of time and creates new shocks from the initial shocks. Sends a distance to the
        /// initial shocks to instantiate at. Loops for 'range' amount of cycles.
        /// </summary>
        /// <returns>To update for 'speed' amount of seconds</returns>
        private IEnumerator ContinueShocks()
        {
            for (int i = 1; i < range; i++)
            {
                yield return new WaitForSeconds(speed);

                foreach (ShockWave s in shockWaves)
                {
                    s.Continue(i, shockWavePrefab);
                }
            }
        }
    }
}
