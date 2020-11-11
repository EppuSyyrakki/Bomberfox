﻿using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
	private readonly Vector3[] directions =
	{
		Vector3.up, 
		Vector3.up + Vector3.right, 
		Vector3.right, 
		Vector3.right + Vector3.down,
		Vector3.down, 
		Vector3.down + Vector3.left,
		Vector3.left,
		Vector3.left + Vector3.up
	};
	private readonly Vector3[] directionsSimple =
	{
		Vector3.up,
		Vector3.right,
		Vector3.down,
		Vector3.left,
	};

	private Transform enemiesParent = null;
	private Transform initialObstaclesParent = null;

	// bottom left and top right corners to limit creation loops
	private Vector3Int min = new Vector3Int(-7, -4, 0);
	private Vector3Int max = new Vector3Int(7, 4, 0);

	private int presetLevel = 0;
	private int blocksLevel = 0;
	private int obstaclesLevel = 0;
	private int randomBlockChance = 33;
	private int randomObstacleChance = 25;
	private int enemyCount = 6;
	private List<Vector3Int> freePositions = new List<Vector3Int>();

	[SerializeField, Header("Level prefabs")] 
	private GameObject[] presets = null;
	[SerializeField] 
	private GameObject[] blocks = null;
	[SerializeField] 
	private GameObject[] obstacles = null;

	[SerializeField, Header("Characters")] 
	private GameObject[] enemies = null;
	[SerializeField] 
	private GameObject player = null;
	
	private void Awake()
	{
		FindParentObjects();
		InitFreePositions();
	}

	private void Start()
	{
		GetBuildParameters(GameManager.Instance.CurrentLevel);
		Preset();
		Player();
		RandomBlocks();
		CheckForDeadEnds();
		Obstacles();
		KeyObstacle();
		Enemies();
        StartCoroutine(FindObjectOfType<FadeOutUI>().ShowBlackOutSquare());
        Physics2D.IgnoreLayerCollision(8, 9, false);
	}
	
	private void FindParentObjects()
	{
		enemiesParent = GameObject.Find("Enemies").transform;
		initialObstaclesParent = GameObject.Find("Initial Obstacles").transform;

		if (enemiesParent == null || initialObstaclesParent == null)
		{
			Debug.Log("One of the required parent GameObjects is missing from scene.");
		}
	}

	private void GetBuildParameters(int currentLevel)
	{
		// change build parameters according to current level
	}

	private void Preset()
	{
		if (presets[0] != null)
		{
			Instantiate(
				presets[Random.Range(0, presets.Length)],
				Vector2.zero,
				Quaternion.identity,
				initialObstaclesParent);
		}
		else
		{
			Debug.LogError("Error loading level presets. Check LevelBuilder for missing references.");
		}

		ReplaceAndRemove(GameObject.FindGameObjectsWithTag("Obstacle"), obstacles[presetLevel]);
		ReplaceAndRemove(GameObject.FindGameObjectsWithTag("Block"), blocks[presetLevel]);
	}

	private void ReplaceAndRemove(GameObject[] toRemove, GameObject replacement)
	{
		List<Vector3Int> positions = new List<Vector3Int>();

		for (int i = toRemove.Length - 1; i >= 0; i--)
		{
			positions.Add(Vector3ToInt(toRemove[i].transform.position));
			// Destroy(toRemove[i]);
		}

		foreach (Vector3Int pos in positions)
		{
			// Instantiate(replacement, pos, Quaternion.identity, initialObstaclesParent.GetChild(0));
			freePositions.Remove(pos);
		}
	}

	/// <summary>
	/// Populate the freePositions list with all available coordinates
	/// </summary>
	private void InitFreePositions()
	{
		for (int y = min.y; y <= max.y; y++)
		{
			for (int x = min.x; x <= max.x; x++)
			{
				Vector3Int pos = new Vector3Int(x, y, 0);
				freePositions.Add(pos);
			}
		}
	}

	/// <summary>
	/// Instantiate the player to the position of a random child of this game object
	/// and remove the child positions from free positions.
	/// </summary>
	private void Player()
	{
		//// remove initial obstacles from freePositions list
		//foreach (Transform obstacle in initialObstaclesParent.GetChild(0).GetComponentsInChildren<Transform>())
		//{
		//	Vector3Int toRemove = Vector3ToInt(obstacle.position);
		//	freePositions.Remove(toRemove);
		//}

		// create player
		Quaternion q = Quaternion.identity;
		Vector3 v = transform.GetChild(Random.Range(0, transform.childCount)).position;
		Instantiate(player, v, q, null);

		// remove player start locations from freePositions
		foreach (Transform child in GetComponentsInChildren<Transform>())
		{
			Vector3Int toRemove = Vector3ToInt(child.position);
			freePositions.Remove(toRemove);
		}
	}

	private static Vector3Int Vector3ToInt(Vector3 toChange)
	{
		return new Vector3Int(Mathf.RoundToInt(toChange.x), Mathf.RoundToInt(toChange.y), 0);
	}
	
	/// <summary>
	/// Create blocks at random and remove them from the free positions. Doesn't create a block if there are
	/// more than 2 blocks around a location. Removes from free positions if block instantiated.
	/// </summary>
	/// <param name="chance">the percentage chance to create a block</param>
	private void RandomBlocks()
	{
		Vector3Int[] freePositionsCopy = freePositions.ToArray();

		foreach (Vector3 v in freePositionsCopy)
		{
			if (Random.Range(0, 101) < randomBlockChance && LocationNotSurrounded(v))
			{
				Quaternion q = Quaternion.identity;
				Instantiate(blocks[blocksLevel], v, q, initialObstaclesParent.GetChild(0));
				freePositions.Remove(Vector3ToInt(v));
			}
		}
	}

	/// <summary>
	/// Checks if any location in level is surrounded by blocks in 4 directions. If found, removes one at random.
	/// </summary>
	private void CheckForDeadEnds()
	{
		for (int y = min.y; y <= max.y; y++)
		{
			for (int x = min.x; x <= max.x; x++)
			{
				if (CheckPositionForDeadEnd(new Vector3(x, y)))
				{
					RemoveOneFromAround(new Vector3(x, y));
				}
			}
		}
	}

	private bool CheckPositionForDeadEnd(Vector3 pos)
	{
		int numberOfBlocks = 0;

		foreach (Vector3 dir in directionsSimple)
		{
			Collider2D collider = Physics2D.OverlapPoint(pos + dir);
			
			if (collider != null && collider.gameObject.CompareTag("Block")) numberOfBlocks++;
		}

		return numberOfBlocks == 4;
	}

	private void RemoveOneFromAround(Vector3 pos)
	{
		Vector3 randomDir = directionsSimple[Random.Range(0, directionsSimple.Length)];
		Collider2D collider = Physics2D.OverlapPoint(pos + randomDir);
		freePositions.Add(Vector3ToInt(collider.gameObject.transform.position));
		Destroy(collider.gameObject);
	}

	/// <summary>
	/// Creates obstacles at random to free positions and removes it from the list.
	/// </summary>
	/// <param name="chance">Percentage chance to create an obstacle</param>
	private void Obstacles()
	{
		Vector3Int[] freePositionsCopy = freePositions.ToArray();

		foreach (Vector3 v in freePositionsCopy)
		{
			if (Random.Range(0, 101) < randomObstacleChance)
			{
				Quaternion q = Quaternion.identity;
				Instantiate(obstacles[obstaclesLevel], v, q, initialObstaclesParent.GetChild(0));
				freePositions.Remove(Vector3ToInt(v));
			}
		}
	}

	private void KeyObstacle()
	{
		Obstacle[] allObstacles = initialObstaclesParent.GetChild(0).GetComponentsInChildren<Obstacle>();

		if (allObstacles == null)
		{
			Debug.LogError("Couldn't find a place to hide the key.");
			return;
		}

		Obstacle keyObstacle = allObstacles[Random.Range(0, allObstacles.Length)];
		keyObstacle.IsKey = true;
	}

	private void Enemies()
	{
		if (enemyCount > freePositions.Count) enemyCount = freePositions.Count;

		for (int i = enemyCount; i > 0; i--)
		{
			Vector3Int[] freePositionsCopy = freePositions.ToArray();
			Vector3Int v = freePositionsCopy[Random.Range(0, freePositionsCopy.Length)];
			Quaternion q = Quaternion.identity;
			Instantiate(enemies[Random.Range(0, enemies.Length)], v, q, enemiesParent);
			freePositions.Remove(v);
		}
	}

	private bool LocationNotSurrounded(Vector3 v)
	{
		int blockedDirections = 0;

		foreach (Vector3 dir in directions)
		{
			if (!freePositions.Contains(Vector3ToInt(v + dir))) blockedDirections++;

			if (blockedDirections > 2) return false;
		}

		return true;
	}
}
