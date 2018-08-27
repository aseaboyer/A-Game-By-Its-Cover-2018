using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public bool isShifting = false;
	public float stopShifting = 0;
	public Vector2 destination;

	// Use this for initialization
	void Start () {
		isShifting = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isShifting) {

		}
	}

	public void StartShifting (Vector3 dest, float endTime) {
		stopShifting = endTime;
	}
}
