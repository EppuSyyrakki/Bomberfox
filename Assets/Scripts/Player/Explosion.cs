﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bomberfox.PowerUp;

namespace Bomberfox.Player
{
    public class Explosion : MonoBehaviour
    {
	    [SerializeField, Tooltip("Drag the prefab InitialShock here")]
	    private GameObject initialShockPrefab = null;

        private Bomb.ShockType shockType;
        private bool penetration = false;
        private GameObject parentBomb = null;
        private int range;
        private float speed;
        private float fadeDelay;
        private float totalTime;
        private float timer;

        // private BoxCollider2D boxCollider;
        
        private Fader fader;

        private void Awake()
        {
            parentBomb = transform.parent.gameObject;
            fader = GetComponent<Fader>();
        }

        private void Start()
        {
            FetchBombParameters();
            transform.parent = null;
            Destroy(parentBomb);
            totalTime = speed * range + fadeDelay + fader.fadeOutTime;    // time to wait before destroying gameObject

            GameObject shockCenter = Instantiate(initialShockPrefab, transform);
            InitialShock initialShock = shockCenter.GetComponent<InitialShock>();
            initialShock.ReceiveBombParameters(range, speed, fadeDelay, penetration);
            initialShock.BeginExploding(shockType);
            initialShock.BeginCoroutineToContinue();
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

            if (timer > totalTime * 0.5f)
            {
	            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();

	            foreach (Collider2D c in colliders)
	            {
		            Destroy(c, Time.deltaTime);
	            }
            }
        }

        private void FetchBombParameters()
        {
            Bomb bomb = parentBomb.GetComponent<Bomb>();
            range = bomb.range;
            speed = 1 / bomb.speed;
            fadeDelay = bomb.fadeDelay;
            penetration = bomb.penetration;
            shockType = bomb.shockType;
        }
    }
}
