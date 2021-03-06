using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour {

	public int tileCountX, tileCountY;
	public GameObject playerTile;
	public int playerX, playerY;
	public GameObject[,] tiles;
	public List<GameObject> tileTypes = new List<GameObject> ();
	public List<Vector2> emptyPositions = new List<Vector2> ();

	public int minimumMatch = 3;

	public float shiftDuration = 1f;
	public int turnLimit = 26;
	public int turnNumber = 0;
	public bool tilesShifting;
	public float stopShiftingTiles;

	public GameObject instructions;
	public Text turnCounterText;

	// Use this for initialization
	void Start () {
		turnNumber = 0;

		UpdateTurnCounter ();

		InitBoard ();

		tilesShifting = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!tilesShifting) {

			// Wait for player input
			if (Input.GetButtonUp ("Horizontal")) {
				if (Input.GetAxis ("Horizontal") > 0) {
					MovePlayer ("right");
				} else if (Input.GetAxis ("Horizontal") < 0) {
					MovePlayer ("left");
				}
			}

			if (Input.GetButtonUp ("Vertical")) {
				if (Input.GetAxis ("Vertical") > 0) {
					MovePlayer ("up");
				} else if (Input.GetAxis ("Vertical") < 0) {
					MovePlayer ("down");
				}
			}

		} else { // tiles are shifting
			//see if they should stop
			if (Time.time >= stopShiftingTiles) {
				StopShifting ();
			}
		}
	}

	void FindMatches () {
		for (int x = 0; x <= tileCountX - minimumMatch; x++) {
			for (int y = 0; y <= tileCountY - minimumMatch; y++) {
				if (tiles [x, y].transform.tag == "Tile") {
					Debug.Log ("Search started at " + x + ", " + y);

					FindHorizontalMatches (x, y);
					FindVerticalMatches (x, y);
				} else {
					Debug.Log ("Not a tile at " + x + ", " + y);
				}
			}
		}
	}

	bool FindHorizontalMatches (int x, int y) {
		int matchesColor = 0;
		int matchesShape = 0;
		int matchTypes = 0;

		GridItemController a = tiles [x, y].GetComponent <GridItemController> ();

		for (int i = x + 1; i <= minimumMatch - 1; i++) {
			if (tiles [i, y].transform.tag == "Tile") {
				GridItemController b = tiles [i, y].GetComponent <GridItemController> ();
				if (a.color == b.color) {
					matchesColor++;
				}
				if (a.shape == b.shape) {
					matchesShape++;
				}

			} else {
				// Never going to match if we are matching against a non-tile4!
				return false;
			}
		}

		if (matchesColor == minimumMatch) {
			matchTypes++;
		}

		if (matchesShape == minimumMatch) {
			matchTypes++;
		}

		if (matchTypes > 1) {
			Debug.Log ("That was a bonus match!");
		}

		if (matchTypes > 0) {
			Debug.Log ("Horizontal match found starting at " + x + ", " + y);
		}

		return false;
	}

	bool FindVerticalMatches (int x, int y) {

		return false;
	}

	void StopShifting () {
		// Show Instructions
		ShowInstructions (true);

		tilesShifting = false;

		turnNumber++;

		UpdateTurnCounter ();
	}

	void MovePlayer (string dir) {
		//Debug.Log ("Move player " + dir);

		bool validMove = false;
		// ignore moves if the player would go offscreen

		if (dir == "up") {
			validMove = ShiftPlayer (0, 1);
			
		} else if (dir == "down") {
			validMove = ShiftPlayer (0, -1);

		} else if (dir == "right") {
			validMove = ShiftPlayer (1, 0);

		} else if (dir == "left") {
			validMove = ShiftPlayer (-1, 0);
		}

		//Debug.Log ("Valid Moved? " + validMove);

		if (validMove) {
			// Hide instructions
			ShowInstructions (false);

			tilesShifting = true;

			stopShiftingTiles = shiftDuration + Time.time;

			UpdateCanvas ();
		}

	}

	bool ShiftPlayer (int xChange, int yChange) {
		int newX = playerX + xChange,
		newY = playerY + yChange;

		if (newX < 0 || newY < 0 ||
		    newX > tileCountX || newY > tileCountY) {
			Debug.Log ("Player left bounds!");
			return false;
		}

		// Shift the objects
		GameObject tempTile = tiles [newX, newY]; // Ref the other tile
		tiles [newX, newY] = tiles [playerX, playerY];
		tiles [playerX, playerY] = tempTile;

		// Swap physical positions
		RectTransform aRect = tiles [playerX, playerY].GetComponent<RectTransform> ();
		RectTransform bRect = tiles [newX, newY].GetComponent<RectTransform> ();
		Vector2 aPos = aRect.position;
		Vector2 bPos = bRect.position;
		aRect.position = bPos;
		bRect.position = aPos;

		// Tell the two items to shift
		/*float stopTime = shiftDuration + Time.time;
		GridItemController gic = tempTile.GetComponent<GridItemController> ();
		gic.StartShifting (new Vector2 (), stopTime);
		PlayerController pic = tiles [newX, playerX].GetComponent<PlayerController> ();
		pic.StartShifting (new Vector2 (), stopTime);*/

		// Lastly, update the stored player position
		playerX = newX;
		playerY = newY;

		// return true if the move is good!
		return true;
	}

	void UpdateTurnCounter () {
		if (turnCounterText) {
			turnCounterText.text = (turnLimit - turnNumber).ToString ();
		}

		if (turnNumber >= turnLimit) {
			Debug.Log ("Game Over! Level failed.");
		}
	}

	void ShowInstructions (bool state) {
		if (instructions) {
			instructions.SetActive (state);
		}
	}

	GameObject RandomTile () {
		return tileTypes [Random.Range (0, tileTypes.Count)];
	}

	GameObject ReplaceTile () {
		return tileTypes [Random.Range (0, tileTypes.Count)];
	}

	void InitBoard () {
		tiles = new GameObject[tileCountX, tileCountY];

		RectTransform rectTrans = GetComponent<RectTransform> ();
		Vector2 boardDimmensions = new Vector2 (rectTrans.rect.width, rectTrans.rect.height);

		Vector2 newTileSizes = boardDimmensions;
		newTileSizes.x /= tileCountX;
		newTileSizes.y /= tileCountY;

		for (int x = 0; x < tileCountX; x++) {
			for (int y = 0; y < tileCountY; y++) {

				GameObject newTile;

				if (x == playerX && y == playerY) {
					// Create the player tile
					newTile = Instantiate (playerTile);

				} else {
					// create a new version of a tile
					newTile = Instantiate (RandomTile ());
				}

				RectTransform newRect = newTile.GetComponent<RectTransform> ();

				// put the tile in the array
				tiles [x, y] = newTile;

				// add the new tile to the board
				newTile.transform.SetParent (transform);

				// position the tile
				newRect.transform.position = new Vector2 (
					newTileSizes.x * x,
					newTileSizes.y * y
				);

				// resize the tile
				newRect.sizeDelta = new Vector2 (newTileSizes.x, newTileSizes.y);
			}
		}

		FindMatches ();

		UpdateCanvas ();
	}

	private void UpdateCanvas () {
		Canvas.ForceUpdateCanvases ();
	}
}
