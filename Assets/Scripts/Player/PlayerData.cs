using UnityEngine;
using System.Collections;

namespace Bomberfox.Player
{
	public class PlayerData
	{
		public int Health { get; }
		public int BombCount { get; }
		public int BombRange { get; }
		public GameObject SpecialBomb { get; }

		/// <summary>
		/// Use when transitioning between levels to store Player data
		/// </summary>
		public PlayerData(int health, int bombCount, int bombRange, GameObject specialBomb)
		{
			Health = health;
			BombCount = bombCount;
			BombRange = bombRange;
			SpecialBomb = specialBomb;
		}

		/// <summary>
		/// Use to initialize Player data when starting a new game
		/// </summary>
		public PlayerData()
		{
			Health = 1;
			BombCount = 1;
			BombRange = 2;
			SpecialBomb = null;
		}
	}
}