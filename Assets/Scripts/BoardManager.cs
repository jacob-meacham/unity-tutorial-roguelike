using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count {
		public int min;
		public int max;

		public Count (int min, int max) {
			this.min = min;
			this.max = max;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(5, 9);
	public Count foodCount = new Count(1, 5);
	public GameObject exit;
	public List<GameObject> floor;
	public List<GameObject> walls;
	public List<GameObject> food;
	public List<GameObject> enemies;
	public List<GameObject> outerWalls;

	private Transform boardParent;
	private List<Vector3> openGridPositions = new List<Vector3>();

	public void SetupScene(int level) {
		// Fill out walls and floor
		GenerateBoard ();

		PlaceObjects (walls, wallCount.min, wallCount.max);
		PlaceObjects (food, foodCount.min, foodCount.max);

		int numEnemies = (int)Mathf.Log (level, 2f);
		PlaceObjects (enemies, numEnemies, numEnemies);

		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
	}

	void GenerateBoard() {
		boardParent = new GameObject ("Board").transform;
		openGridPositions.Clear ();

		for (int col = -1; col <= columns; col++) {
			for (int row = -1; row <= rows; row++) {
				var toInstantiate = floor.RandomElement ();

				if (col == -1 || col == columns || row == -1 || row == rows) {
					toInstantiate = outerWalls.RandomElement ();
				} else {
					// Also peg this as a possible position for an object to go
					openGridPositions.Add(new Vector3(col, row, 0f));
				}

				GameObject instance = Instantiate (toInstantiate, new Vector3 (col, row, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardParent);
			}
		}			
	}

	Vector3 RandomGridPosition() {
		int randomElement = Random.Range (0, openGridPositions.Count);
		Vector3 pos = openGridPositions[randomElement];
		openGridPositions.RemoveAt (randomElement);

		return pos;
	}

	void PlaceObjects(IList<GameObject> prefabs, int min, int max) {
		int numToPlace = Random.Range (min, max);
		for (int i = 0; i < numToPlace; i++) {
			var toInstantiate = prefabs.RandomElement ();
			Vector3 pos = RandomGridPosition ();

			GameObject instance = Instantiate (toInstantiate, pos, Quaternion.identity) as GameObject;
			instance.transform.SetParent (boardParent);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
