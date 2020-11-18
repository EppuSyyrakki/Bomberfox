using System;
using System.Collections;
using System.Collections.Generic;
using Bomberfox.PowerUp;
using UnityEngine;

namespace Bomberfox.Player
{
    public class PlayerController : MonoBehaviour
    {
        private enum Direction  // helper for animator to decide which facing to use
        {
            None,
            Up,
            Right,
            Down,
            Left
        }

       [SerializeField]
        private float speed = 10f;

        // How much health the player has in the beginning
        [SerializeField] 
        private int playerStartingHealth = 1;

        // What is the player's max health
        [SerializeField] 
        private int playerMaxHealth = 5;

        // How many bombs the player can drop at the same time
        [SerializeField] 
        private int maxBombs = 1;

        // the range of player's normal bomb
        public int bombRange = 2;
        [SerializeField]
        private int maxBombRange = 5;

        // the ultimate maximum how many bombs player can achieve through powerups
        [SerializeField] 
        private int maxBombLimit = 5;
        
        // the prefab of the basic normal bomb
        [SerializeField]
        private GameObject normalBomb = null;
        // the prefab of the special bomb
        public GameObject specialBomb = null;
        // reference to the special bomb instantiated on the map
        public GameObject Special { get; set; }

        private int currentBombs = 0;   // The amount of bombs currently in the game
        private Animator animator;
        private CollisionHandler collisionHandler;
        private PolygonCollider2D playerCollider;
        private Rigidbody2D rb;
        private Vector2 movement;
        private bool isAlive = true;
        
        private Health healthSystem;
        public bool isInvulnerable = false;    // Is the player invulnerable or not
        public bool specialUsed = false;
        public bool isShieldOn = false;

        [SerializeField, Tooltip("How long the player is invulnerable after taking damage")]
        private float invulnerabilityTimer = 5;

        private void Awake()
        {
	        animator = GetComponent<Animator>();
	        collisionHandler = GetComponent<CollisionHandler>();
	        playerCollider = GetComponent<PolygonCollider2D>();
	        rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
	        PlayerData data = GameManager.Instance.Player;
	        playerStartingHealth = data.Health;
	        maxBombs = data.BombCount;
	        bombRange = data.BombRange;
	        specialBomb = data.SpecialBomb;
	        healthSystem = new Health(playerMaxHealth, playerStartingHealth);
        }

        private void Update()
        {
	        ProcessInput();
            UpdateAnimator();
        }

        private void FixedUpdate()
        {
	        if (isAlive)
	        {
		        rb.MovePosition(rb.position + movement * (speed * Time.fixedDeltaTime));
            }
        }
        
        /// <summary>
        /// Checks input and creates a bomb if maxBombs allows.
        /// </summary>
        private void ProcessInput()
        {
	        movement.x = Input.GetAxis("Horizontal");
	        movement.y = Input.GetAxis("Vertical");

	        float pythagoras = ((movement.x * movement.x) + (movement.y * movement.y));

	        if (pythagoras > (speed * speed))
	        {
		        float magnitude = Mathf.Sqrt(pythagoras);
		        float multiplier = speed / magnitude;
		        movement.x *= multiplier;
		        movement.y *= multiplier;
	        }

            if (Input.GetButtonDown("Fire1") && currentBombs < maxBombs)
            {
                CreateNormalBomb();
            }

            if (Input.GetButtonDown("Fire2") && specialUsed)
            {
	            TryExplodeSpecial();
            }

            if (Input.GetButtonDown("Fire2") && !specialUsed)
            {
	            TryCreateSpecial();
            }
        }

        private void CreateNormalBomb()
        {
	        Vector3 pos = new Vector3(
		        Mathf.RoundToInt(transform.position.x),
		        Mathf.RoundToInt(transform.position.y),
		        0);

	        if (collisionHandler.CheckPosition(pos))
	        {
		        GameObject bomb = Instantiate(normalBomb, pos, Quaternion.identity);
		        bomb.GetComponent<Bomb>().SetOwnerAndInit(this);
            }
        }

        private void TryCreateSpecial()
        {
	        if (specialBomb == null) return;

	        Vector3 pos = new Vector3(
		        Mathf.RoundToInt(transform.position.x),
		        Mathf.RoundToInt(transform.position.y),
		        0);

	        if (collisionHandler.CheckPosition(pos))
	        {
		        Special = Instantiate(specialBomb, pos, Quaternion.identity);
		        Special.GetComponent<Bomb>().SetOwnerAndInit(this);
		        specialUsed = true;
	        }
        }

        private void TryExplodeSpecial()
        {
	        if (Special != null)
	        {
		        Bomb bomb = Special.GetComponent<Bomb>();
		        
		        if (bomb.HasRemote)
		        {
			        bomb.Explode();
			        Special = null;
		        }
	        }
        }

        /// <summary>
        /// Updates animator component by setting triggers that define which direction player is facing.
        /// </summary>
        private void UpdateAnimator()
        {
            float lastInputX = Input.GetAxis("Horizontal");
            float lastInputY = Input.GetAxis("Vertical");

            // Movement code for Blend Tree
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                animator.SetBool("Walking", true);
                if (lastInputX > 0)
                {
                    animator.SetFloat("LastMoveX", 1f);
                }
                else if (lastInputX < 0)
                {
                    animator.SetFloat("LastMoveX", -1f);
                }
                else
                {
                    animator.SetFloat("LastMoveX", 0f);
                }

                if (lastInputY > 0)
                {
                    animator.SetFloat("LastMoveY", 1f);
                }
                else if (lastInputY < 0)
                {
                    animator.SetFloat("LastMoveY", -1f);
                }
                else
                {
                    animator.SetFloat("LastMoveY", 0f);
                }
            }
            else
            {
                animator.SetBool("Walking", false);
            }
        }

        /// <summary>
        /// Sets the moveDirection animation helper according to input.
        /// </summary>
        /// <returns>the direction which the player should be facing</returns>
        private Direction DefineMoveDirection()
        {
	        if (Input.GetAxis("Vertical") > 0) return Direction.Up;
            if (Input.GetAxis("Horizontal") > 0) return Direction.Right;
            if (Input.GetAxis("Vertical") < 0) return Direction.Down;
            if (Input.GetAxis("Horizontal") < 0) return Direction.Left;

            return Direction.None;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Player"))
            {
                TakeDamage();
            }
        }

        private void TurnOnCollider()
        {
            isInvulnerable = false;
            Physics2D.IgnoreLayerCollision(8, 9, false);
            Debug.Log("I can take damage again");
        }

        private void StartDeath()
        {
            AudioManager.instance.OneShotSound("PlayerDeath");
            animator.SetTrigger("Die");
            Destroy(playerCollider);
            isAlive = false;
        }


        // called from animation event at the end of Death animation
        private void EndDeath()
        {
            AudioManager.instance.StopGameMusic();
            AudioManager.instance.PlayMusic("DeathSong");
            GameManager.Instance.TotalDeaths++;
            GameManager.Instance.GoToDeathMenu();
        }

        public void TakeDamage()
        {
            if (isShieldOn)
            {
                isInvulnerable = true;
                Physics2D.IgnoreLayerCollision(8, 9, true);
                Debug.Log("Ha, I had a shield!");
                isShieldOn = false;
                Invoke("TurnOnCollider", invulnerabilityTimer);
            }
            else
            {
                healthSystem.Damage(1);

                if (healthSystem.GetHealth() <= 0)
                {
                    StartDeath();
                }
                else
                {
                    isInvulnerable = true;
                    // playerCollider.enabled = !playerCollider.enabled; // disables colliding with everything on the level
                    Physics2D.IgnoreLayerCollision(8, 9, true);
                    Debug.Log("Ouch, I took damage!");
                    Debug.Log(healthSystem.GetHealth());
                    Invoke("TurnOnCollider", invulnerabilityTimer);
                }
            }
        }
        
        public void ChangeCurrentBombs(int change)
        {
	        currentBombs += change;
        }

        public void AddMaxBombs()
        {
	        if (maxBombs < maxBombLimit) maxBombs++;
	        else maxBombs = maxBombLimit;
        }

        public void AddShield()
        {
            isShieldOn = true;
        }

        public void AddMoreRange()
        {
	        if (bombRange + 1 <= maxBombRange)
	        {
		        bombRange++;
	        }
        }

        public void AddHealth()
        {
            healthSystem.Heal(1);

        }

        public void ReceiveNewBomb(PowerUpBase powerUp)
        {
	        specialBomb = powerUp.GetPrefab();
	        specialUsed = false;
        }

        public PlayerData GetPlayerData()
        {
            return new PlayerData(healthSystem.GetHealth(), maxBombs, bombRange, specialBomb);
        }
    }
}
