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
        private float totalTime;
        private float timer;
        private readonly Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
        private List<ShockWave> shockWaves = new List<ShockWave>();
        private BoxCollider2D boxCollider;
        private CollisionHandler collisionHandler;
        private Fader fader;

        private void Awake()
        {
            parentBomb = transform.parent.gameObject;
            collisionHandler = GetComponent<CollisionHandler>();
            fader = GetComponent<Fader>();
        }

        private void Start()
        {
            FetchBombParameters();
            transform.parent = null;
            Destroy(parentBomb);
            PlayerController.currentBombs -= 1;
            BeginExploding();
            StartCoroutine(nameof(ContinueShocks));
            totalTime = speed * range + fadeDelay + fader.fadeOutTime;    // time to wait before destroying gameObject
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= fadeDelay)
            {
	            fader.Fade = true;
            }
            
            if (timer > totalTime)
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
        }

        /// <summary>
        /// Loops 4 directions, creates shocks and gives them directions to continue in. Adds the initial shocks to list.
        /// </summary>
        private void BeginExploding()
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3 position = transform.position + directions[i];
                GameObject obj = Instantiate(shockWavePrefab, position, Quaternion.identity, transform);
                ShockWave sw = obj.GetComponent<ShockWave>();
                sw.Direction = directions[i];
                sw.SetDelay(fadeDelay);

                if (collisionHandler.CheckPosition(position)) // check if the position is free
                {
	                shockWaves.Add(sw);
                }
            }
        }

        /// <summary>
        /// Waits for 'speed' amount of time and creates new shocks from the initial shocks. Sends a distance to the
        /// initial shocks to instantiate at. Loops for 'range' amount of cycles. The check for blocking objects is done
        /// inside the Continue method in ShockWave script.
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
