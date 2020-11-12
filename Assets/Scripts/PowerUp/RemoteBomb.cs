using Bomberfox.Player;
using UnityEngine;

namespace Bomberfox.PowerUp
{
	public class RemoteBomb : MonoBehaviour, IPowerUp
	{
		[SerializeField] private GameObject remoteBombPrefab = null;
		public Bomb.Type Type { get; } = Bomb.Type.Special;

		public void AddToPlayer(PlayerController pc)
		{
			pc.ReceiveNewBomb(this);
		}

		public GameObject GetPrefab()
		{
			return remoteBombPrefab;
		}

		public void Remove()
		{
			Destroy(gameObject);
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
