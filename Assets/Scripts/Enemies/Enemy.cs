﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bomberfox.Player;

namespace Bomberfox.Enemies
{
    [RequireComponent(typeof(CollisionHandler))]
    public class Enemy : MonoBehaviour
    {
	    public readonly Vector3 Nowhere = Vector2.one * 1000;
	    private readonly Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
	    private float spawnTimer, spawnTime;

		[SerializeField] private float speed = 10f, lookDistance = 5f;
	    [SerializeField] private GameObject reservedSpace = null;
	    [SerializeField, Range(0, 100)] private int specialMoveChance = 10;
	    [SerializeField, Range(1f, 20f)] private float specialMoveCoolDown = 5f;
        [SerializeField, Range(1, 100)] private int powerUpChance = 30;

        public bool IsAlive { private set; get; } = true;
	    public Vector3 Direction { get; private set; } = Vector3.zero;
		public CollisionHandler CollisionHandler { get; private set; }
		public bool SpecialMove { get; set; }
		public Animator Anim { get; set; }
		public GameObject Space { get; set; }
		public Vector3 PlayerLastSeen { get; set; }
		public Vector3 LastPosition { private get; set; } = Vector3.zero;
		public float SpecialMoveTimer { private get; set; }
		public Vector3 CurrentTarget { get; set; }

		private void Awake()
        {
	        CollisionHandler = GetComponent<CollisionHandler>();
	        Anim = GetComponent<Animator>();
        }

        public virtual void Start()
        {
	        spawnTime = Anim.GetCurrentAnimatorStateInfo(0).length + 0.1f;
	        PlayerLastSeen = Nowhere;
	        CurrentTarget = transform.position + Direction;
        }

        public virtual void Update()
        {
	        spawnTimer += Time.deltaTime;
	        SpecialMoveTimer += Time.deltaTime;
	        LookForPlayer();

			// if just spawned or are dead, don't update at all
			if (spawnTimer < spawnTime || IsAlive == false) return;

			// if we are at target, get new target and update the animator according to target's direction
			if (transform.position == CurrentTarget)
			{
				LastPosition = transform.position;

				if (Space != null) Destroy(Space);

				// get a random number to see if we should do the special move
				if (SpecialMoveTimer > specialMoveCoolDown)
				{
					if (Random.Range(1, 101) < specialMoveChance)
					{
						SpecialMove = true;
						return;
					}

					SpecialMoveTimer = 0;
				}

				// if we are at where the player was last seen, reset the player seen position
				if (transform.position == PlayerLastSeen) PlayerLastSeen = Nowhere;

				// if no special move, move normally
				SetNewTarget();
			}

			// if we got this far we're not doing special, so move to our current target
	        MoveToCurrentTarget();
        }

        private void LookForPlayer()
        {
	        for (int i = 0; i < 4; i++)
	        {
				// look in all vectors saved in directions array
		        var check = Physics2D.Raycast(
			        transform.position + directions[i],
			        directions[i],
			        lookDistance);
		        
				// if the raycast hit something & that something has PlayerController component
		        if (check && check.transform.gameObject.TryGetComponent(out PlayerController player))
		        {
					// show sight line in scene view TODO remove debug line
					Debug.DrawLine(transform.position + directions[i], player.transform.position, Color.cyan);

					// if the player in invulnerable, we don't "see" them
			        if (player.isInvulnerable) PlayerLastSeen = Nowhere;

			        // get the player's position rounded to whole numbers
			        Vector3 pos = new Vector3(
				        Mathf.RoundToInt(check.transform.position.x),
				        Mathf.RoundToInt(check.transform.position.y),
				        0);

			        PlayerLastSeen = pos;
				}
	        }
        }

        private void SetNewTarget()
        {
	        Vector3 pos = transform.position;

			if (PlayerLastSeen != Nowhere)	// if we have seen the player
	        {
		        if (PlayerLastSeen.y > pos.y) Direction = Vector3.up;
				if (PlayerLastSeen.y < pos.y) Direction = Vector3.down;
				if (PlayerLastSeen.x > pos.x) Direction = Vector3.right;
				if (PlayerLastSeen.x < pos.x) Direction = Vector3.left;
	        }

			// check in the Direction if we can move
			if (CollisionHandler.CheckPosition((pos + Direction)))
			{
				CurrentTarget = pos + Direction;
				return;
			}

			// if we got this far we can't move, so create a new random direction
			DefineRandomDirection();
	        CurrentTarget = pos + Direction;
        }

        private void MoveToCurrentTarget()
        {
			// if we haven't reserved a space and it's free, do that and exit this method
	        if (Space == null && CollisionHandler.CheckPosition(CurrentTarget))
	        {
				UpdateAnimator(CurrentTarget);
				ReserveSpace(CurrentTarget);
		        return;		// TODO maybe not needed? check
	        }
			
			// if we got this far, we've reserved a space. Now move toward it
			transform.position = Vector3.MoveTowards(
				transform.position,
				CurrentTarget,
				speed * Time.deltaTime);
        }

        protected void ReserveSpace(Vector3 pos)
        {
	        Space = Instantiate(reservedSpace, pos, Quaternion.identity, transform.parent);
        }

        protected void DefineRandomDirection()
        {
	        int i = 0;

	        while (i < 10) // limit searches to avoid endless loop if no free direction found
	        {
		        int directionIndex = Random.Range(0, 4);
                Vector3 direction = directions[directionIndex];

				// if we find a free direction, assign that and exit this method
		        if (CollisionHandler.CheckPosition(transform.position + direction))
		        {
			        Direction = direction;
			        return;
		        }

		        i++;
	        }

			// if we got this far, we can't move anywhere so assign a zero vector
	        Direction = Vector3.zero;
		}

        public void GoBack()
        {
	        CurrentTarget = LastPosition;
        }

        protected void UpdateAnimator(Vector3 moveTarget)
        {
			// change moveTarget into a vector relative to our location 
	        Vector3 target = transform.InverseTransformPoint(moveTarget);

			if (target.y > 0)
			{
				Anim.SetBool("FacingUp", true);
				Anim.SetBool("FacingRight", false);
				Anim.SetBool("FacingDown", false);
				Anim.SetBool("FacingLeft", false);
			}
			if (target.y < 0)
			{
				Anim.SetBool("FacingUp", false);
				Anim.SetBool("FacingRight", false);
				Anim.SetBool("FacingDown", true);
				Anim.SetBool("FacingLeft", false);
			}
			if (target.x > 0)
			{
				Anim.SetBool("FacingUp", false);
				Anim.SetBool("FacingRight", true);
				Anim.SetBool("FacingDown", false);
				Anim.SetBool("FacingLeft", false);
			}
			if (target.x < 0) 
			{
				Anim.SetBool("FacingUp", false);
				Anim.SetBool("FacingRight", false);
				Anim.SetBool("FacingDown", false);
				Anim.SetBool("FacingLeft", true);
			}
			
			// if the enemy is "stuck", display moving down animation
			if (target == Vector3.zero)
			{
				Anim.SetBool("FacingUp", false);
				Anim.SetBool("FacingRight", false);
				Anim.SetBool("FacingDown", true);
				Anim.SetBool("FacingLeft", false);
			}
        }


		/* Old Trigger -method for animation
		if (target == Vector3.up) SetTrigger("FacingUp");
		if (target == Vector3.right) SetTrigger("FacingRight");
		if (target == Vector3.down) SetTrigger("FacingDown");
		if (target == Vector3.left) SetTrigger("FacingLeft");

		// if the enemy is "stuck", display moving down animation
		if (target == Vector3.zero) SetTrigger("FacingDown");
	}

	// Set one facing trigger and reset others
	private void SetTrigger(string triggerToSet)
	{
		Anim.SetTrigger(triggerToSet);

		if (triggerToSet != "FacingUp") Anim.ResetTrigger("FacingUp");
		if (triggerToSet != "FacingRight") Anim.ResetTrigger("FacingRight");
		if (triggerToSet != "FacingDown") Anim.ResetTrigger("FacingDown");
		if (triggerToSet != "FacingLeft") Anim.ResetTrigger("FacingLeft");
	}
		*/

		public virtual void StartDeath()
        {
	        if (Space != null) Destroy(Space);

	        Destroy(GetComponent<Collider2D>());
			Anim.SetTrigger("Die");
			IsAlive = false;
            GameManager.Instance.KilledEnemies++;
            GameManager.Instance.LevelProgression += 5;

            if (GameManager.Instance.IsFirstKill)
            {
                ExtraBombPowerUp();
                GameManager.Instance.IsFirstKill = false;
            }
            else
            {
                PowerUp();
			}
        }

        public EnemyData GetData()
        {
			return new EnemyData(speed, specialMoveChance, specialMoveCoolDown);
        }

        public void SetData(EnemyData data)
        {
	        speed = data.Speed;
	        specialMoveChance = data.SpecialChance;
	        specialMoveCoolDown = data.SpecialCoolDown;

	        if (specialMoveCoolDown < 0) specialMoveCoolDown = 0;
        }

		// triggered from animation event
        private void EndDeath()
        {
	        Destroy(gameObject);
		}

		// When enemy dies, this determines if the enemy will randomly drop a power up or not
        private void PowerUp()
        {
            int chance = Random.Range(0, 101);

            if (GameManager.Instance.CurrentLevel < 6)
            {
                if (chance <= powerUpChance)
                {
                    GameObject powerUp = GameManager.Instance.GetPowerUp();
                    Instantiate(powerUp, transform.position, Quaternion.identity);
                }
			} 
            else if (GameManager.Instance.CurrentLevel < 11)
            {
                if (chance <= (powerUpChance - 5))
                {
                    GameObject powerUp = GameManager.Instance.GetPowerUp();
                    Instantiate(powerUp, transform.position, Quaternion.identity);
                }
			}
			else
            {
                if (chance <= (powerUpChance - 10))
                {
                    GameObject powerUp = GameManager.Instance.GetPowerUp();
                    Instantiate(powerUp, transform.position, Quaternion.identity);
                }
			}


        }

        private void ExtraBombPowerUp()
        {
            GameObject pu = GameManager.Instance.GetFirstExtraBomb();
            Instantiate(pu, transform.position, Quaternion.identity);
        }
    }
}
