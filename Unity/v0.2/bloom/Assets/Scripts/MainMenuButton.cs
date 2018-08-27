using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuButton : MonoBehaviour, IPointerClickHandler {

	public MainMenuController mainMenuController;
	public string levelData = "";

	public Text highScoreText;

	// Use this for initialization
	void Start () {
		LoadHighScore ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadHighScore () {
		float highScore = PlayerPrefs.GetFloat (levelData + "HighScore", 0f);

		if (highScoreText) {
			highScoreText.text = "(High: " + highScore + ")";
		}
	}

	public void OnPointerClick (PointerEventData pointerEventData) {
		mainMenuController.LoadScene (levelData);
	}
}
