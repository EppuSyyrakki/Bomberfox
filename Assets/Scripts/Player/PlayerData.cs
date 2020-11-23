using UnityEngine;
using System.Collections;

namespace Bomberfox.Player
{
	public class PlayerData
	{
		public int Health { get; }
		public int BombCount { get; }
		public int BombRange { get; }
		public bool HasShield { get; }
		public GameObject[] Specials { get; } = new GameObject[3];

		/// <summary>
		/// Use when transitioning between levels to store Player data
		/// </summary>
		public PlayerData(int health, int bombCount, int bombRange, bool hasShield, GameObject[] specials)
		{
			Health = health;
			BombCount = bombCount;
			BombRange = bombRange;
			HasShield = hasShield;
			Specials = specials;
		}

		/// <summary>
		/// Use to initialize Player data when starting a new game
		/// </summary>
		public PlayerData()
		{
			Health = 1;
			BombCount = 1;
			BombRange = 2;
			HasShield = false;
			Specials = new GameObject[3];
		}
	}
}