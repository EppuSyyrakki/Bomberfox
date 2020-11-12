﻿using Bomberfox.Player;
using UnityEngine;

namespace Bomberfox.PowerUp
{
	public abstract class PowerUpBase : MonoBehaviour
	{
		[SerializeField] private GameObject bombPrefab = null;

		public virtual Bomb.Type Type { get; }

		public virtual void AddToPlayer(PlayerController pc)
		{
			return;
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
			if (other.gameObject.CompareTag("Player")
			    && other.gameObject.TryGetComponent(out PlayerController pc)
			    && pc.IsSpecialNull)
			{
				AddToPlayer(pc);
				Remove();
			}
		}
	}
}