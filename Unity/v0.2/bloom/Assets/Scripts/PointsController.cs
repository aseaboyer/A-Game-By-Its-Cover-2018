using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsController : MonoBehaviour {

	public Text pointUI;
	public float points;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float GetPoints () {
		return points;
	}

	public void AddPoints (float added) {
		points += added;

		UpdatePoints ();
	}

	public void UpdatePoints () {
		if (pointUI) {
			pointUI.text = points.ToString ();
		}
	}
}
