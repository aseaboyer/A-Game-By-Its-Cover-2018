using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GridController gridController;
	public PointsController pointsController;
	public GameOverModalController gameOverModal;

	public float shiftDuration = 1f;
	public int turnNumber = 0;
	public bool tilesShifting;
	public float stopShiftingTiles;

	public GameObject instructions;
	public Image currentTile;
	public TileProperties currentTileProperties;
	public Image nextTile;
	public TileProperties nextTileProperties;

	public AudioSource audioSource;
	public List<AudioClip> plantNormal;
	public List<AudioClip> plantMatch;

	// Use this for initialization
	void Start () {
		turnNumber = 0;
		tilesShifting = false;

		GetNextTileProperties ();
		GetNextTileProperties ();

		UpdateCanvas ();
	}
	
	// Update is called once per frame
	void Update () {
		if (tilesShifting) { // tiles are shifting
			//see if they should stop
			if (Time.time >= stopShiftingTiles) {
				StopShifting ();
			}
		}
	}

	public void PlayRandomClip (List<AudioClip> clips) {
		if (clips.Count > 0 && audioSource) {
			System.Random rand = new System.Random ();
			int id = rand.Next (0, clips.Count);
			audioSource.PlayOneShot (clips [id]);

		} else {
			Debug.Log ("No clips to play!");
		}
	}

	public void EndGame () {
		// remember, there is no winning state, just more/less than old score
		float score = pointsController.GetPoints ();
		gameOverModal.GameEnd (score);
	}

	public void AddPoints (float val) {
		pointsController.AddPoints (val);
	}

	public void PlantOnTile (TileController tc) {
		// This is how turns are made

		if (tc.state == "Empty") {
			// If we can shift...
			tc.SetProperties (currentTileProperties);

			bool matchMade = gridController.FindMatches (tc.x, tc.y);

			if (matchMade) {
				PlayRandomClip (plantMatch);
			} else {
				PlayRandomClip (plantNormal);
			}

			gridController.UpdatePlantStates (turnNumber);

			GetNextTileProperties ();

			// Evaluate if tiles are consumed
			int emptyTiles = gridController.CountEmptyTiles ();

			if (emptyTiles <= 0) {
				EndGame ();
			}

		} else {
			Debug.Log ("Can't plant there, not Empty");
		}
	}

	public void GetNextTileProperties () {
		// move the next to current
		currentTileProperties = nextTileProperties;

		// fetch random new
		nextTileProperties = gridController.RandomTile ();

		// update the UI
		currentTile.sprite = currentTileProperties.sprite;
		nextTile.sprite = nextTileProperties.sprite;
	}

	void ShowInstructions (bool state) {
		if (instructions) {
			instructions.SetActive (state);
		}
	}

	void StopShifting () {
		// Show Instructions
		ShowInstructions (true);

		tilesShifting = false;

		turnNumber++;
	}

	private void UpdateCanvas () {
		Canvas.ForceUpdateCanvases ();
	}
}
