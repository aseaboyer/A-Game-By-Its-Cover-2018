using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour {

	public GameController gameController;

	public int tileCountX, tileCountY;
	public GameObject gridTile;
	//public GameObject[,] tiles;
	public TileController[,] tiles;
	public List<TileProperties> tileTypes = new List<TileProperties> ();
	public List<Vector2> emptyPositions = new List<Vector2> ();
	//Vector2 tileSizes;

	public GameObject instructions;
	public Text turnCounterText;

	public float tileShiftDuration = 0.2f;

	// Use this for initialization
	void Start () {
		InitBoard ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int CountEmptyTiles () {
		int ct = 0;

		for (int x = 0; x < tileCountX; x++) {
			for (int y = 0; y < tileCountY; y++) {
				if (tiles [x, y].CanBePlanted ()) {
					ct++;
				}
			}
		}

		return ct;
	}

	public void UpdatePlantStates (int turnNumber) {
		for (int x = 0; x < tileCountX; x++) {
			for (int y = 0; y < tileCountY; y++) {
				TileController item = tiles [x, y].GetComponent<TileController> ();
				item.UpdateTurn (turnNumber);
			}
		}
	}

	public bool FindMatches (int x, int y) {
		// Check JUST the selected row and column for a match
		int rowMatch = MatchesRow (x, y);
		int colMatch = MatchesColumn (x, y);

		if (rowMatch + colMatch > 0) {
			int pts = 0;
			float startClearing = Time.time;

			if (colMatch > 0) {
				/*Debug.Log ("Reset tiles in col "  + x + 
					" Score points: " + tileCountY + " * " +
					colMatch + " = " + (tileCountY * colMatch));
				*/
				pts += (colMatch * tileCountX);

				// reset the column
				ClearColumn (x, y);
			}

			if (rowMatch > 0) {
				/*Debug.Log ("Reset tiles in row "  + y + 
					" Score points: " + tileCountX + " * " +
					rowMatch + " = " + (tileCountX * rowMatch));
				*/
				pts += (rowMatch * tileCountY);

				// reset the row
				ClearRow (x, y);
			}

			gameController.AddPoints (pts);

			// Tell the GameController how long to shift tiles

			return true;
		}
		// else nothing here
		return false;
	}

	public void ClearColumn (int row, int col) {
		for (int y = 0; y < tileCountY; y++) {
			TileController tc = tiles [row, y].GetComponent<TileController> ();
			tc.ClearTile (tileShiftDuration);
		}
	}

	public void ClearRow (int row, int col) {
		for (int x = 0; x < tileCountX; x++) {
			//Debug.Log ("Row check: " + x + ", " + row);
			TileController tc = tiles [x, col].GetComponent<TileController> ();
			tc.ClearTile (tileShiftDuration);
		}
	}

	public int MatchesColumn (int row, int col) {
		// compile the column items into a list and see if it matches
		int matchesType = 0;
		List<TileController> tileList = new List<TileController> ();

		// ITERATE OVER THE MATRIX AND PUSH TO THE LIST
		for (int y = 0; y < tileCountY; y++) {
			//Debug.Log ("Row check: " + x + ", " + row);
			TileController tc = tiles [row, y];//.GetComponent<TileController> ();
			/*
			Debug.Log ("Check: " + tc.x + " (" + row + ")" + ", " +
				tc.y + " (" + col + ")" + " - " + 
				tc.properties.color + " " + tc.properties.shape);
			*/
			if (tc.state != "Planted") {
				//Debug.Log ("Incomplete col!");
				return 0;
			}

			tileList.Add (tc);
		}

		if (tileList.Count >= tileCountY) {
			if (SameColor (tileList)) {
				matchesType++;
			}
			if (SameShape (tileList)) {
				matchesType++;
			}

			// return 0 if not a matches, 1 if color or shape, 2 if color AND shape
			return matchesType;
		
		} else { // not a full column!
			return 0;
		}
	}

	public int MatchesRow (int row, int col) {
		// compile the row items into a list and see if it matches
		int matchesType = 0;
		List<TileController> tileList = new List<TileController> ();
		
		// ITERATE OVER THE MATRIX AND PUSH TO THE LIST
		for (int x = 0; x < tileCountX; x++) {
			//Debug.Log ("Row check: " + x + ", " + row);
			TileController tc = tiles [x, col].GetComponent<TileController> ();
			/*
			Debug.Log ("Check: " + tc.x + " (" + row + ")" + ", " +
				tc.y + " (" + col + ")" + " - " + 
				tc.properties.color + " " + tc.properties.shape);
			*/
			if (tc.state != "Planted") {
				//Debug.Log ("Incomplete row!");
				return 0;
			}

			tileList.Add (tc);
		}

		if (tileList.Count >= tileCountX) {
			if (SameColor (tileList)) {
				matchesType++;
			}
			if (SameShape (tileList)) {
				matchesType++;
			}

			// return 0 if not a matches, 1 if color or shape, 2 if color AND shape
			return matchesType;

		} else { // not a full row!
			return 0;
		}
	}

	public bool SameShape (List<TileController> tileList) {
		if (tileList.Count <= 0) {
			return false;
		}

		string s = tileList [0].properties.shape;

		foreach (TileController tile in tileList) {
			if (tile.properties.shape != s) {
				return false;
			}
		}

		return true;
	}

	public bool SameColor (List<TileController> tileList) {
		if (tileList.Count <= 0) {
			return false;
		}

		string c = tileList [0].properties.color;

		foreach (TileController tile in tileList) {
			if (tile.properties.color != c) {
				return false;
			}
		}

		return true;
	}

	public bool MatchesColor (GridItemController a, GridItemController b) {
		if (a.color == b.color) {
			return true;
		}
		return false;
	}

	public TileProperties RandomTile () {
		return tileTypes [Random.Range (0, tileTypes.Count)];
	}

	void InitBoard () {
		string levelData = PlayerPrefs.GetString ("StoredLevelData");
		Debug.Log (levelData);
		string[] levelValues = levelData.Split (':');

		tileCountX = int.Parse (levelValues [0]);
		tileCountY = int.Parse (levelValues [1]);
		int lifeTime = int.Parse (levelValues [2]);
		int deathTime = int.Parse (levelValues [3]);

		tiles = new TileController[tileCountX, tileCountY];

		RectTransform rectTrans = GetComponent<RectTransform> ();
		Vector2 boardDimmensions = new Vector2 (rectTrans.rect.width, rectTrans.rect.height);

		Vector2 newTileSizes = boardDimmensions;
		newTileSizes.x /= tileCountX;
		newTileSizes.y /= tileCountY;
		//tileSizes = newTileSizes;

		// Set the initial tiles that the player will click on to plant
		if (gridTile) {
			for (int x = 0; x < tileCountX; x++) {
				for (int y = 0; y < tileCountY; y++) {

					GameObject newTile = Instantiate (gridTile);
					RectTransform newRect = newTile.GetComponent<RectTransform> ();

					// put the tile in the array
					tiles [x, y] = newTile.GetComponent<TileController> ();

					// add the new tile to the board
					newTile.transform.SetParent (transform);

					// position the tile
					newRect.transform.position = new Vector2 (
						newTileSizes.x * x,
						newTileSizes.y * y
					);

					// resize the tile
					newRect.sizeDelta = new Vector2 (newTileSizes.x, newTileSizes.y);

					// init the tileController script
					TileController tc = newTile.GetComponent<TileController> ();
					tc.Init (x, y, lifeTime, deathTime);
				}
			}
		} else {
			Debug.Log ("Grid Tile undefined in GridController");
		}
	}
}
