using UnityEngine;
using System.Collections;

public class Player {

	public string name { get; set; }
	public string golfClub { get; set; }
	public string ball { get; set; }
	public Vector3 prevPosition { get; set; }
	public int shotCount { get; set; }
	public int score { get; set; }


	public Player(string name) {
		this.name = name;
	}
}
