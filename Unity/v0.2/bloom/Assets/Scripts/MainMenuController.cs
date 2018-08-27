using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	public string gameSceneName = "Game Scene";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadScene (string info) {
		// store level data in prefs
		PlayerPrefs.SetString ("StoredLevelData", info);

		// load scene
		SceneManager.LoadScene (gameSceneName);
	}
}
