using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileProperties {

	public string color;
	public string shape;
	public Sprite sprite;

	TileProperties (string c, string s, Sprite spr) {
		color = c;
		shape = s;
		sprite = spr;
	}
}
