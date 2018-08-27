using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour, IPointerClickHandler {

	GameController gameController;

	public TileProperties properties;
	public Image image;
	public Sprite nullImage;
	public Sprite expiredImage;
	public int x, y;
	public bool isEmpty;

	public int spawned = 0;
	public int expired = 0;
	public int lifeTime = 5;
	public int deathTime = 3;
	public int age = 0;
	public Text statusText;
	public Image statusImage;

	public string state = "Empty";
	
	public Sprite collectionImage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateTurn (int turnNumber) {
		if (state == "Planted") {
			age++;

			if (statusText) {
				statusText.text = (lifeTime - age).ToString ();
			}

			if (lifeTime - age <= 0) {
				Expire ();
			}
		} else if (state == "Expired") {
			age++;

			if (statusText) {
				//statusText.text = (-(lifeTime - age)).ToString ();
				statusText.text = (-(age - lifeTime - deathTime)).ToString ();
			}
		}

		if (lifeTime - age <= -deathTime) {
			ClearProperties ();
		}
	}

	public bool CanBePlanted () {
		if (state == "Empty" || state == "Harvested") {
			return true;
		}
		return false;
	}

	public void Expire () {
		if (expiredImage) {
			image.sprite = expiredImage;
		}

		if (statusText) {
			statusText.text = (deathTime).ToString ();
		}

		state = "Expired";
	}

	public void OnPointerClick(PointerEventData pointerEventData) {
		gameController.PlantOnTile (this);
	}

	public void ResetTile () { // Ehhhh or ClearProperties?
		if (nullImage) {
			image.sprite = nullImage;
		} else {
			image.sprite = null;
		}

		spawned = 0;

		if (statusImage) {
			statusImage.gameObject.SetActive (false);
		}

		state = "Empty";
		age = 0;
	}

	public void ClearTile (float duration) {
		ClearTile (duration, 0f);
	}

	public void ClearTile (float duration, float delay) {
		// Spawn an effect
		if (collectionImage) {
			image.sprite = collectionImage;
		}

		state = "Harvesting";

		Invoke ("ClearTile", duration);
	}

	public void ClearTile () {

		ClearProperties ();
	}

	public void SetProperties (TileProperties tp) {
		properties.color = tp.color;
		properties.shape = tp.shape;
		properties.sprite = tp.sprite;

		image.sprite = properties.sprite;

		spawned = gameController.turnNumber;

		if (statusImage) {
			statusImage.gameObject.SetActive (true);
		}
		
		state = "Planted";
	}

	public void ClearProperties () {
		properties.color = null;
		properties.shape = null;
		properties.sprite = null;

		if (nullImage) {
			image.sprite = nullImage;
		} else {
			image.sprite = null;
		}

		spawned = 0;

		if (statusImage) {
			statusImage.gameObject.SetActive (false);
		}

		state = "Empty";
		age = 0;
	}

	public void Init (int xPos, int yPos, int life, int death) {
		x = xPos;
		y = yPos;
		lifeTime = life;
		deathTime = death;

		gameController = GameObject.FindGameObjectWithTag 
			("GameController").GetComponent<GameController> ();

		spawned = 0;

		image = GetComponent<Image> ();

		ClearProperties ();
	}
}
