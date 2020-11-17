﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Bomberfox.Player
{
    public class Bomb : MonoBehaviour
    {
	    private enum Trigger
	    {
            Timer = 0,
            Remote
	    }

	    public enum Type
	    {
            Normal = 0,
            Special
	    }

	    public enum ShockType
	    {
		    XandY = 0,
		    X,
		    Y,
		    Full
	    }

	    public bool HasRemote
	    {
		    get
		    {
			    if (trigger == Trigger.Remote) return true;
			    return false;
		    }
	    }

        [SerializeField]
	    private Type type = Type.Normal;

	    [SerializeField]
	    private Trigger trigger = Trigger.Timer;

	    [SerializeField] 
	    public ShockType shockType = ShockType.XandY;

        [SerializeField, Tooltip("How far the explosion goes"), Range(1, 10)]
        public int range = 3;

        [SerializeField, Tooltip("Speed of the explosion, n:th of a second"), Range(10, 100)]
        public float speed = 25;
        
        [SerializeField, Tooltip("In seconds"), Range(0, 1f)]
        public float fadeDelay = 0.3f;

        [SerializeField, Tooltip("In seconds")]
        private float bombTimer = 2f;
        
        [SerializeField]
        private GameObject explosionPrefab = null;

        public bool IsTriggered { private get; set; }

        private PlayerController owner = null;
        private Collider2D collider2d;
        
        private void Awake()
        {
	        collider2d = GetComponent<Collider2D>();
        }

        private void Start()
        {
	        if (type == Type.Normal)
	        {
		        range = owner.bombRange;
	        }
        }

        // Update is called once per frame
        private void Update()
        {
			if (trigger == Trigger.Timer)
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
        }

        private void OnDestroy()
        {
            if (type == Type.Normal) owner.ChangeCurrentBombs(-1);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
	        collider2d.isTrigger = false;
        }

        // Creates the explosion that destroys this gameObject
        public void Explode()
        {
            GameManager.Instance.ExplodedBombs++;
	        Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);

	        if (type == Type.Special) owner.specialBomb = null;
        }

        public void SetOwnerAndInit(PlayerController owner)
        {
	        this.owner = owner;
	        
            if (type == Type.Normal) owner.ChangeCurrentBombs(1);
        }

        public void MuteBomb()
        {
            
        }
    }
}
