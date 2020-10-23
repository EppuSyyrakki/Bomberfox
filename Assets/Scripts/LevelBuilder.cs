using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelBuilder : MonoBehaviour
{
	private readonly Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
	private GameObject blocksParent = null;
	private GameObject obstaclesParent = null;
	private GameObject enemiesParent = null;
	private int levelWidth, levelHeight;
	private List<Vector2> reserved = new List<Vector2>();
	private Vector3 topLeft;

	[SerializeField] private GameObject[] blockPrefabs;
	[SerializeField] private GameObject[] obstaclePrefabs;
	[SerializeField] private GameObject[] enemyPrefabs;

	private void Awake()
	{
		FindParentObjects();
		GetLevelSize();
		topLeft = GetTopLeftVector();
		print(topLeft);
	}

	private Vector3 GetTopLeftVector()
	{
		return new Vector3(-(levelWidth - 1) / 2, (levelHeight -1) / 2, 0);	
	}

	private void FindParentObjects()
	{
		blocksParent = GameObject.Find("Blocks");
		obstaclesParent = GameObject.Find("Obstacles");
		enemiesParent = GameObject.Find("Enemies");
	}

	private void GetLevelSize()
	{
		Tilemap tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
		levelWidth = tilemap.size.x;
		levelHeight = tilemap.size.y;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawCube(topLeft, Vector3.one);
	}
}
