using System;
using System.Collections;
using System.Collections.Generic;
using Bomberfox.PowerUp;
using Bomberfox.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Bomberfox.Player
{
    public class PlayerController : MonoBehaviour
    {
	    [SerializeField]
        private float speed = 10f;

        // How much health the player has in the beginning
        [SerializeField] 
        public int playerStartingHealth = 1;

        // What is the player's max health
        [SerializeField] 
        private int playerMaxHealth = 5;

        // How many bombs the player can drop at the same time
        [SerializeField] 
        public int maxBombs = 1;
        // the ultimate maximum how many bombs player can achieve through powerups
        [SerializeField]
        private int maxBombLimit = 5;

        // the range of player's normal bomb
        public int bombRange = 1;
        [SerializeField]
        private int maxBombRange = 3;
        
        // the prefab of the basic normal bomb
        [SerializeField]
        private GameObject normalBomb = null;
        
        // The special bomb storage
        public GameObject megaBomb = null;
        public GameObject remoteBomb = null;
        public GameObject mineBomb = null;
        // reference to the remote bomb instantiated on the map - needed to be able to blow it up
        private GameObject remote = null;

        [SerializeField]
        private GameObject shield = null;

        private int currentBombs = 0;   // The amount of bombs currently in the game
        private Animator animator;
        private CollisionHandler collisionHandler;
        private CircleCollider2D playerCollider;
        private Rigidbody2D rb;
        private Vector2 movement;
        private bool movementEnabled = false;
        
        
        public Health healthSystem;
        public bool isInvulnerable = false;    // Is the player invulnerable or not
        public bool hasShield = false;

        [SerializeField, Tooltip("Time interval for step sounds")]
        private float stepTime = 1;
        private float stepTimer;
        private bool step1 = true;
        private bool step2 = false;

        [SerializeField, Tooltip("How long the player is invulnerable after taking damage")]
        private float invulnerabilityTimer = 2;

        private void Awake()
        {
	        animator = GetComponent<Animator>();
	        collisionHandler = GetComponent<CollisionHandler>();
	        playerCollider = GetComponent<CircleCollider2D>();
	        rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
	        PlayerData data = GameManager.Instance.Player;
	        playerStartingHealth = data.Health;
	        maxBombs = data.BombCount;
	        bombRange = data.BombRange;
	        hasShield = data.HasShield;
	        megaBomb = data.Specials[0];
	        remoteBomb = data.Specials[1];
	        mineBomb = data.Specials[2];
	        healthSystem = new Health(playerMaxHealth, playerStartingHealth);
            stepTimer = stepTime;
        }

        private void Update()
        {
	        if (Input.GetButtonDown("Menu"))
	        {
		        if (FindObjectOfType<PauseMenuUI>().HelpActive) return;

		        TogglePauseMenu();
		        return;
	        }

	        if (!movementEnabled) return;
            
	        ProcessInput();
	        UpdateAnimator();

            if (hasShield && !shield.activeSelf) shield.SetActive(true);
            else if (!hasShield && shield.activeSelf) shield.SetActive(false);

            if (Math.Abs(movement.x) >= 1 || Math.Abs(movement.y) >= 1)
            {
                StepSounds();
            }
        }

        private void StepSounds()
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer < 0)
            {
                stepTimer = stepTime;

                if (step1)
                {
                    AudioManager.instance.PlaySound("Step1");
                    step1 = false;
                    step2 = true;
                }
                else if (step2)
                {
                    AudioManager.instance.PlaySound("Step2");
                    step1 = true;
                    step2 = false;
                }
            }

        }

        private void FixedUpdate()
        {
	        if (!movementEnabled) return;
	        
	        rb.MovePosition(rb.position + movement * (speed * Time.fixedDeltaTime));
        }

        private void TogglePauseMenu()
        {
	        if (GameManager.Instance.isPaused) GameManager.Instance.isPaused = false;
	        else GameManager.Instance.isPaused = true;

	        GameObject inGameCanvas = GameObject.FindGameObjectWithTag("InGameCanvas");

	        if (inGameCanvas != null)
	        {
		        if (GameManager.Instance.isPaused)
		        {
			        inGameCanvas.GetComponent<PauseMenuUI>().ActivateMenu();
			        DisableMovement();
		        }
		        else
		        {
			        inGameCanvas.GetComponent<PauseMenuUI>().DeactivateMenu();
			        EnableMovement();
		        }
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

            if (Input.GetButtonDown("NormalBomb") && currentBombs < maxBombs)
            {
	            CreateBomb(normalBomb);
            }

            if (Input.GetButtonDown("MegaBomb") && megaBomb != null)
            {
	            CreateBomb(megaBomb);
	            megaBomb = null;
            }

            if (Input.GetButtonDown("RemoteBomb") && remote != null)
            {
	            remote.GetComponent<Bomb>().Explode();
	            remote = null;
            }
            else if (Input.GetButtonDown("RemoteBomb") && remoteBomb != null && remote == null)
            {
	            CreateBomb(remoteBomb);
	            remoteBomb = null;
            }

            if (Input.GetButtonDown("MineBomb") && mineBomb != null)
            {
	            CreateBomb(mineBomb);
	            mineBomb = null;
            }
        }

        private void CreateBomb(GameObject special)
        {
	        Vector3 pos = new Vector3(
		        Mathf.RoundToInt(transform.position.x),
		        Mathf.RoundToInt(transform.position.y),
		        0);

	        if (collisionHandler.CheckPosition(pos))
	        {
		        GameObject newBomb = Instantiate(special, pos, Quaternion.identity);
		        newBomb.GetComponent<Bomb>().SetOwnerAndInit(this);

		        if (newBomb.GetComponent<Bomb>().type == Bomb.Type.Remote) remote = newBomb;
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
            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
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
        /// Checks whether the player collided with an enemy and if so, calls for the correct method.
        /// </summary>
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Player"))
            {
                TakeDamage();
            }
        }

        /// <summary>
        /// Turns on the collider after player's invulnerability has ended.
        /// </summary>
        private void TurnOnCollider()
        {
            isInvulnerable = false;
            Physics2D.IgnoreLayerCollision(8, 9, false);
        }

        private void StartDeath()
        {
            AudioManager.instance.OneShotSound("PlayerDeath");
            animator.SetTrigger("Die");
            Destroy(playerCollider);
            DisableMovement();
        }


        // called from animation event at the end of Death animation
        private void EndDeath()
        {
            AudioManager.instance.StopGameMusic();
            AudioManager.instance.PlayMusic("DeathSong");
            GameManager.Instance.TotalDeaths++;
            GameManager.Instance.UpdateTotalStats();
            GameManager.Instance.GoToDeathMenu();
        }

        // called from animation event at the end of Spawn animation
        // and from GameManager when menu button pressed
        public void EnableMovement()
        {
	        movementEnabled = true;
        }

        // called from LevelEndKey to prevent movement and from GameManager
        // when menu button pressed
        public void DisableMovement()
        {
	        movementEnabled = false;
        }

        /// <summary>
        /// Checks if the player should take damage.
        /// If the player is shielded, removes the shield from the player and starts the invulnerability.
        /// If the player is not shielded, player takes damage before they become invulnerable for a moment.
        /// </summary>
        public void TakeDamage()
        {
            if (hasShield)
            {
                isInvulnerable = true;
                Physics2D.IgnoreLayerCollision(8, 9, true);
                hasShield = false;
                Invoke(nameof(TurnOnCollider), invulnerabilityTimer);
                SetTransparency(0.5f);
                Invoke(nameof(DisableTransparency), invulnerabilityTimer);
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
                    AudioManager.instance.OneShotSound("PlayerHurt");
                    isInvulnerable = true;
                    Physics2D.IgnoreLayerCollision(8, 9, true);
                    Invoke(nameof(TurnOnCollider), invulnerabilityTimer);
                    SetTransparency(0.5f);
                    Invoke(nameof(DisableTransparency), invulnerabilityTimer);
                }
            }
        }

        private void DisableTransparency()
        {
			SetTransparency(1f);
        }

        private void SetTransparency(float alpha)
        {
	        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>(true);

	        foreach (SpriteRenderer sr in renderers)
	        {
		        Color old = sr.color;
		        Color newColor = new Color(old.r, old.g, old.b, alpha);
		        sr.color = newColor;
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
            hasShield = true;
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
	        if (powerUp.Type == Bomb.Type.Mega) megaBomb = powerUp.GetPrefab();
            else if (powerUp.Type == Bomb.Type.Remote) remoteBomb = powerUp.GetPrefab();
            else if (powerUp.Type == Bomb.Type.Mine) mineBomb = powerUp.GetPrefab();
        }

        public PlayerData GetPlayerData()
        {
	        GameObject[] specials = {megaBomb, remoteBomb, mineBomb};
            return new PlayerData(healthSystem.GetHealth(), maxBombs, bombRange, hasShield, specials);
        }
    }
}
