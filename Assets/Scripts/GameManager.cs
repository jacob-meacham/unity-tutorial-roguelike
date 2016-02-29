using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	private BoardManager boardManager;
	private int level = 3;

	void Awake() {
		boardManager = GetComponent<BoardManager>();
		InitGame ();
	}

	void InitGame() {
		boardManager.SetupScene (level);
	}
}
