using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using Bomberfox.Enemies;
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

	// bottom left and top right corners to limit creation loops
	private Vector3Int min = new Vector3Int(-7, -4, 0);
	private Vector3Int max = new Vector3Int(7, 4, 0);

	private Progression progression;
	private Vector3 playerStart;
	private Transform enemiesParent = null;
	private Transform initialObstaclesParent = null;
	private int currentLevel = 0;	// start from 1 on first level
	private int presetLevel = 0;	// start from 0 on first level (used as index)
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
		progression = GetComponent<Progression>();
	}

	private void Start()
	{
		currentLevel = GameManager.Instance.CurrentLevel;
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
	}

	private void Preset()
	{
		Instantiate(
				presets[Random.Range(0, presets.Length)],
				Vector3.zero,
				Quaternion.identity,
				initialObstaclesParent);
		presetLevel = progression.GetPresetLevel(currentLevel);

		if (presetLevel > 0)
		{
			List<GameObject> presetObstacles = new List<GameObject>();
			List<GameObject> presetBlocks = new List<GameObject>();

			foreach (Transform child in initialObstaclesParent.GetChild(0))
			{
				if (child.CompareTag("Obstacle")) presetObstacles.Add(child.gameObject);
				else if (child.CompareTag("Block")) presetObstacles.Add(child.gameObject);
			}

			ReplaceAndRemove(presetObstacles.ToArray(), obstacles[presetLevel]);
			ReplaceAndRemove(presetBlocks.ToArray(), blocks[presetLevel]); 
		}
	}

	/// <summary>
	/// Remove and replace all GameObjects in an array.
	/// </summary>
	/// <param name="toRemove">The array of GameObjects to remove</param>
	/// <param name="replacement">The GameObject to replace the originals</param>
	private void ReplaceAndRemove(GameObject[] toRemove, GameObject replacement)
	{
		for (int i = toRemove.Length - 1; i >= 0; i--)
		{
			Vector3 pos = toRemove[i].transform.position;
			Destroy(toRemove[i]);
			Instantiate(replacement, pos, Quaternion.identity, initialObstaclesParent.GetChild(0));
			freePositions.Remove(Vector3ToInt(pos));
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
		playerStart = transform.GetChild(Random.Range(0, transform.childCount)).position;
		Instantiate(player, playerStart, q, null);

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
				Instantiate(blocks[presetLevel], v, q, initialObstaclesParent.GetChild(0));
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
				Instantiate(obstacles[presetLevel], v, q, initialObstaclesParent.GetChild(0));
				freePositions.Remove(Vector3ToInt(v));
			}
		}
	}

	private void KeyObstacle()
	{
		Obstacle[] allObstacles = initialObstaclesParent.GetChild(0).GetComponentsInChildren<Obstacle>();
		Vector3 player = playerStart;

		if (allObstacles == null)
		{
			Debug.LogError(name + " couldn't find a place to hide the key.");
			return;
		}

		int loopAttempts = 0;

		while (true)
		{
			loopAttempts++;

			if (loopAttempts > 100)
			{
				Debug.LogError(name + " couldn't move the key away from player start.");
				break;
			}

			Obstacle keyObstacle = allObstacles[Random.Range(0, allObstacles.Length)];
			Vector3 key = keyObstacle.transform.position;

			// check if key is in the same quarter of the map as player. If so, try again
			if ((player.x < 0 && key.x < 0) && (player.y < 0 && key.y < 0)) continue;
			if ((player.x < 0 && key.x < 0) && (player.y > 0 && key.y > 0)) continue;
			if ((player.x > 0 && key.x > 0) && (player.y > 0 && key.y > 0)) continue;
			if ((player.x > 0 && key.x > 0) && (player.y < 0 && key.y < 0)) continue;

			keyObstacle.IsKey = true;
			break;
		}
	}

	private void Enemies()
	{
		enemyCount = progression.GetEnemyCount(currentLevel);
		GameObject[] enemyTypes = EnemyTypes(progression.GetEnemyTypes(currentLevel));

		if (enemyCount > freePositions.Count) enemyCount = freePositions.Count;
		
		for (int i = enemyCount; i > 0; i--)
		{
			Vector3Int[] freePositionsCopy = freePositions.ToArray();
			Vector3Int v = freePositionsCopy[Random.Range(0, freePositionsCopy.Length)];
			Quaternion q = Quaternion.identity;
			GameObject enemyObject = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Length)], v, q, enemiesParent);
			Enemy enemy = enemyObject.GetComponent<Enemy>();
			EnemyData data = enemy.GetData();
			enemy.SetData(progression.GetEnemyData(currentLevel, data));
			freePositions.Remove(v);
		}
	}

	private GameObject[] EnemyTypes(bool[] enemyTypes)
	{
		List<GameObject> availableEnemies = new List<GameObject>();

		if (enemyTypes[0]) availableEnemies.Add(enemies[0]);
		if (enemyTypes[1]) availableEnemies.Add(enemies[1]);
		if (enemyTypes[2]) availableEnemies.Add(enemies[2]);

		return availableEnemies.ToArray();
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
