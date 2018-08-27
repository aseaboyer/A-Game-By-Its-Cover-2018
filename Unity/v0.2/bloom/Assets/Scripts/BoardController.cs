using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour {

	public Vector2 tileCount;
	public GameObject[,] tiles;
	public List<GameObject> tileTypes = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		Debug.Log (GetComponent<SpriteRenderer> ().bounds.size);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
