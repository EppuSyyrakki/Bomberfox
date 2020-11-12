using Bomberfox.Player;
using UnityEngine;

namespace Bomberfox.PowerUp
{
	public interface IPowerUp
	{
		void AddToPlayer(PlayerController pc);
		void Remove();
		GameObject GetPrefab();
		Bomb.Type Type { get; }
	}
}

