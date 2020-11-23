using Bomberfox.Player;
using UnityEngine;

namespace Bomberfox.PowerUp
{
	public abstract class PowerUpBase : MonoBehaviour
	{
		[SerializeField] 
		private GameObject bombPrefab = null;

		[SerializeField]
		private string floatingText = "";

		[SerializeField]
		private GameObject textObject = null;

		public virtual Bomb.Type Type { get; }

		public virtual bool AddToPlayer(PlayerController pc)
		{
			return false;
		}

		public GameObject GetPrefab()
		{
			return bombPrefab;
		}

		public void Remove()
		{
			GameObject help = Instantiate(textObject, transform.position, Quaternion.identity);
			PowerUpText puText = help.GetComponent<PowerUpText>();
			puText.SetText(floatingText);
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
