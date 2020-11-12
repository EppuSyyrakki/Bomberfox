﻿using Bomberfox.Enemies;
using UnityEngine;

namespace Bomberfox
{
	public class Progression : MonoBehaviour
	{
		[Header("Starting values:"), SerializeField, Range(0, 100), Tooltip("Percentage chance")]
		private int blockChance = 25;

		[SerializeField, Range(0, 100), Tooltip("Percentage chance")] 
		private int obstacleChance = 25;

		[SerializeField, Range(1, 20), Tooltip("Enemies in first level")] 
		private int enemies = 2;

		[SerializeField, Range(0.1f, 1f), Tooltip("this value * original speed")]
		private float enemySpeed;

		[SerializeField, Range(0.1f, 1f), Tooltip("this value * original chance")]
		private float enemySpecialChance = 0.5f;

		[SerializeField, Range( 1f, 2f), Tooltip("this value * original time")]
		private float enemySpecialCooldown = 1f;

		[Header("Per-level increases:"), SerializeField, Range(0, 50), Tooltip("Added to the chance on every level")]
		private int blockChanceIncrease = 2;

		[SerializeField, Range(0, 50), Tooltip("Added to the chance on every level")] 
		private int obstacleChanceIncrease = 3;

		[Header("Enemy difficulty increases:"), SerializeField, Range(1, 5), Tooltip("Additional enemy every n:th level")]
		private int addEnemyFrequency = 1;

		[SerializeField, Range(0, 0.05f), Tooltip("Added to the enemy speed every level")]
		private float enemySpeedIncrease = 0.005f;

		[SerializeField, Range(0.01f, 0.1f), Tooltip("Added to enemy special chance every level")]
		private float enemySpecialChanceIncrease = 0.05f;

		[SerializeField, Range(-0.1f, 0f), Tooltip("Reduced from enemy special cooldown every level")]
		private float enemySpecialCooldownDecrease = -0.005f;

		[Header("Introduce enemy @ level:"), SerializeField, Range(0, 20)]
		private int goatLevel = 0;

		[SerializeField, Range(0, 20)]
		private int pigLevel = 0;

		[SerializeField, Range(0, 20)]
		private int bunnyLevel = 0;

		/// <summary>
		/// What environment "theme" to use. 0 = normal
		/// </summary>
		public int GetPresetLevel(int currentLevel)
		{
			return 0;
		}

		/// <summary>
		/// Chance of creating a block in a free and not-surrounded position.
		/// </summary>
		public int GetRandomBlockChance(int currentLevel)
		{
			return 50;
		}

		/// <summary>
		/// Chance of creating an obstacle in a free position.
		/// </summary>
		public int GetRandomObstacleChance(int currentLevel)
		{
			return 25;
		}

		/// <summary>
		/// Number of enemies to be created.
		/// </summary>
		public int GetEnemyCount(int currentLevel)
		{
			return Mathf.FloorToInt(currentLevel / addEnemyFrequency + enemies);
		}

		/// <summary>
		/// Types of enemies to be created. index 0 = Goat, 1 = Pig, 2 = Bunny.
		/// </summary>
		public bool[] GetEnemyTypes(int currentLevel)
		{
			bool[] enemies = new bool[3];

			if (currentLevel >= goatLevel) enemies[0] = true;

			if (currentLevel >= pigLevel) enemies[1] = true;

			if (currentLevel >= bunnyLevel) enemies[2] = true;

			if (enemies[0] == false && enemies[1] == false && enemies[2] == false)
			{
				Debug.LogError("No enemy available. Set at least one enemy to introduce at level " 
				               + currentLevel + " in the Progression component of LevelBuilder.");
			}

			return enemies;
		}

		/// <summary>
		/// Adjust EnemyData parameters according to current level.
		/// </summary>
		/// <returns></returns>
		public EnemyData GetEnemyData(int currentLevel, EnemyData data)
		{
			return data;
		}
	}
}
