using UnityEngine;

namespace Bomberfox.Player
{
    public class Bomb : MonoBehaviour
    {
	    public enum Type
	    {
            Normal = 0,
            Mega,
            Remote,
            Mine
	    }

	    public enum ShockType
	    {
		    XandY = 0,
		    X,
		    Y,
		    Full
	    }

	    private readonly Vector3[] mineTriggerDir = {Vector3.zero, Vector3.up, Vector3.right, Vector3.down, Vector3.left};

        public bool penetration = false;

	    public Type type = Type.Normal;

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

        private PlayerController owner = null;
        private BoxCollider2D boxCollider2d;

        private void Awake()
        {
	        boxCollider2d = GetComponent<BoxCollider2D>();
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
			if (type == Type.Normal || type == Type.Mega)
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
            else if (type == Type.Mine)
			{
				Vector3 pos = transform.position;

				foreach (Vector3 dir in mineTriggerDir)
				{
					Collider2D collider = Physics2D.OverlapCircle(pos + dir * 0.75f, 0.25f);

					if (collider != null && collider.gameObject.CompareTag("Enemy")) Explode();
				}
			}
        }

        private void OnDestroy()
        {
            if (type == Type.Normal) owner.ChangeCurrentBombs(-1);
        }
        
        // change the collider from trigger to real collider when player moves away from over it.
        private void OnTriggerExit2D(Collider2D other)
        {
	        boxCollider2d.isTrigger = false;
        }

        // Creates the explosion that destroys this gameObject
        public void Explode()
        {
            GameManager.Instance.ExplodedBombs++;

            if (type == Type.Normal)
            {
                AudioManager.instance.OneShotSound("Explosion");
            }
            else if (type == Type.Mega)
            {
                AudioManager.instance.OneShotSound("ExplosionBiggest");
            }
            else if (type == Type.Remote)
            {
	            AudioManager.instance.OneShotSound("ExplosionBigger");
            }
            else if (type == Type.Mine)
            {
	            AudioManager.instance.OneShotSound("Landmine");
            }

            Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);
        }

        public void SetOwnerAndInit(PlayerController owner)
        {
	        this.owner = owner;
	        
            if (type == Type.Normal) owner.ChangeCurrentBombs(1);
        }

        public void PauseSound(bool enable)
        {
	        if (enable) GetComponent<AudioSource>().Pause();
	        else GetComponent<AudioSource>().UnPause();
        }
    }
}
