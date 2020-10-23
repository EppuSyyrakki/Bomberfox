using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
	// include mid directions to avoid no-go places in level
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
	private Transform blocksParent = null;
	private Transform obstaclesParent = null;
	private Transform enemiesParent = null;

	// bottom left and top right corners to limit creation loops
	private Vector3Int min = new Vector3Int(-7, -4, 0);
	private Vector3Int max = new Vector3Int(7, 4, 0);
	
	private int blocksLevel = 0;
	private int obstaclesLevel = 0;
	private List<Vector3> reserved = new List<Vector3>();

	[SerializeField] private GameObject[] blockPrefabs;
	[SerializeField] private GameObject[] obstaclePrefabs;
	[SerializeField] private GameObject[] enemyPrefabs;
	[SerializeField] private GameObject playerPrefab;

	private void Awake()
	{
		FindParentObjects();
	}

	private void Start()
	{
		AddReservedLocations();
		CreatePlayer();
		CreateBlocks();
	}

	private void FindParentObjects()
	{
		blocksParent = GameObject.Find("Blocks").transform;
		obstaclesParent = GameObject.Find("Obstacles").transform;
		enemiesParent = GameObject.Find("Enemies").transform;
	}
	
	private void AddReservedLocations()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			reserved.Add(transform.GetChild(i).position);
		}
	}

	private void CreatePlayer()
	{
		Quaternion q = Quaternion.identity;
		Vector3 v = reserved[Random.Range(0, reserved.Count)];
		Instantiate(playerPrefab, v, q, null);
	}

	private void CreateBlocks()
	{
		for (int y = min.y; y <= max.y; y++)
		{
			for (int x = min.x; x <= max.x; x++)
			{
				CreateNormalBlock(x, y);
				CreateRandomBlock(x, y);
				CreateObstacles(x, y);
			}
		}
	}

	private void CreateObstacles(int x, int y)
	{
		Quaternion q = Quaternion.identity;
		Vector3 v = new Vector3(x, y, 0);

		if (reserved.Contains(v)) return;

		if (Random.Range(0, 101) < 66)
		{
			Instantiate(obstaclePrefabs[obstaclesLevel], v, q, obstaclesParent);
		}
	}

	private void CreateNormalBlock(int x, int y)
	{
		if ((x % 2 == 0 && Mathf.Abs(y) == 3) || (Mathf.Abs(y) == 1) && Mathf.Abs(x) == 6)
		{
			Quaternion q = Quaternion.identity;
			Vector3 v = new Vector3(x, y, 0);
			Instantiate(blockPrefabs[blocksLevel], v, q, blocksParent);
			reserved.Add(v);
		}
	}

	private void CreateRandomBlock(int x, int y)
	{
		Quaternion q = Quaternion.identity;
		Vector3 v = new Vector3(x, y, 0);

		if (reserved.Contains(v)) return;

		if (Random.Range(0, 101) < 30 && LocationNotSurrounded(v))
		{
			Instantiate(blockPrefabs[blocksLevel], v, q, blocksParent);
			reserved.Add(v);
		}
	}

	private bool LocationNotSurrounded(Vector3 v)
	{
		int blockedDirections = 0;

		foreach (Vector3 dir in directions)
		{
			if (reserved.Contains(v + dir)) blockedDirections++;

			if (blockedDirections > 1) return false;
		}

		return true;
	}
}
