using Bomberfox.Player;
using UnityEngine;

namespace Bomberfox.PowerUp
{
	public abstract class PowerUpBase : MonoBehaviour
	{
		[SerializeField] private GameObject bombPrefab = null;

		public virtual Bomb.Type Type { get; }

		public virtual bool AddToPlayer(PlayerController pc)
		{
			print("called from base");
			return false;
		}

		public GameObject GetPrefab()
		{
			return bombPrefab;
		}

		public void Remove()
		{
			Destroy(gameObject);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.gameObject.TryGetComponent(out PlayerController pc)) return;
            
            if (AddToPlayer(pc))
            {
	            GameManager.Instance.CollectedPU++;
	            Remove();
			}
		}
	}
}
