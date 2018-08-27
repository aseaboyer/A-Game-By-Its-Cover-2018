using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverModalController : MonoBehaviour {

	public GameObject modalWindow;
	public Text modalText;

	public string mainMenuName = "Start Scene";

	// Use this for initialization
	void Start () {
		HideModal ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GameEnd (float score) {
		Debug.Log ("Final Score: " + score);
		string levelName = PlayerPrefs.GetString ("StoredLevelData");
		Debug.Log ("Level name: " + levelName);
		float existingScore = PlayerPrefs.GetFloat (levelName + "HighScore");
		Debug.Log ("Existing Score: " + existingScore);
		// Set a bunch of text
		if (modalText) {
			string resultsText = "";

			if (score <= existingScore) {
				resultsText = "Your score: " + score +
					"\n" + "The high score: " + existingScore;

			} else {
				resultsText = "Score: " + score + 
					"\n" + "You beat the old high score!";

				PlayerPrefs.SetFloat (levelName + "HighScore", score);
			}

			modalText.text = resultsText;
		}

		DisplayModal ();
	}

	public void HideModal () {
		if (modalWindow) {
			modalWindow.SetActive (false);
		}
	}

	public void DisplayModal () {
		if (modalWindow) {
			modalWindow.SetActive (true);
		}
	}

	public void ReturnToMenu () {
		SceneManager.LoadScene (mainMenuName);
	}
}
