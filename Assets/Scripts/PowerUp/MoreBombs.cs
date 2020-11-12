using Bomberfox.Player;
using UnityEngine;

namespace Bomberfox.PowerUp
{
	public class MoreBombs : MonoBehaviour, IPowerUp
	{
		public Bomb.Type Type { get; } = Bomb.Type.Normal;

		public void AddToPlayer(PlayerController pc)
		{
			pc.AddMaxBombs();
		}

		public void Remove()
		{
			Destroy(gameObject);
		}

		public GameObject GetPrefab()
		{
			return null;	// no associated prefab with this PowerUp
		}
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				PlayerController pc = other.gameObject.GetComponent<PlayerController>();
				AddToPlayer(pc);
				Remove();
			}
		}
	}
}
