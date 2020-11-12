using Bomberfox.Player;
using UnityEngine;

namespace Bomberfox.PowerUp
{
	public class BigBomb : MonoBehaviour, IPowerUp
	{
		[SerializeField] private GameObject bigBombPrefab = null;
		public Bomb.Type Type { get; } = Bomb.Type.Special;

		public void AddToPlayer(PlayerController pc)
		{
			pc.ReceiveNewBomb(this);
		}

		public GameObject GetPrefab()
		{
			return bigBombPrefab;
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
