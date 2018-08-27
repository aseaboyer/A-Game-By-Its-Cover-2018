using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItemController : MonoBehaviour {

	public string color;
	public string shape;

	public bool isShifting = false;
	public float stopShifting = 0;
	public Vector2 destination;

	// Use this for initialization
	void Start () {
		isShifting = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartShifting (Vector2 dest, float endTime) {
		stopShifting = endTime;
	}

	public bool matches (string c, string s) {
		if (c == color && s == shape) {
			return true;
		}

		return false;
	}
}
