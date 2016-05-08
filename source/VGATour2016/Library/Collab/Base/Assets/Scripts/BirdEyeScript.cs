using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdEyeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

		// used for remembering ball gameobjects (for activating inactive ones)
		GlobalData.balls = new List<GameObject> ();

		// TODO: delete this after merging with selection scene, this is used for testing purposes
		GlobalData.players = new List<Player> ();
		for (int i = 1; i < 3; i++) {
			// initialize two players with ball names Ball1 and Ball2 
			Player p = new Player (i.ToString ());
			p.ball = "Ball" + i.ToString ();
			GlobalData.players.Add (p);
		}
		// remove TODO


		BeginNewHole ();
	}

	void BeginNewHole() {
		// find course and hole from global data
		string course = "Course" + GlobalData.course;
		string hole = "Hole" + GlobalData.hole;
		GameObject courseObject = GameObject.Find(course);
		GameObject holeObject = courseObject.transform.Find(hole).gameObject;
		GameObject currentHole = Instantiate(holeObject);
		currentHole.transform.parent = this.transform;
		currentHole.transform.localPosition = Vector3.zero;
		Debug.Log("start");
		Debug.Log(course);
		Debug.Log(hole);
		courseObject.SetActive(false);

		// add ball to be child of current hole, set it to be inactive
		foreach (Player p in GlobalData.players) {
			GameObject ball = GameObject.Find (p.ball);
			ball.transform.localPosition = currentHole.transform.localPosition + Vector3.up;
//			ball.transform.parent = currentHole.transform.parent.parent;
//			ball.transform.localPosition = Vector3.up;
//			ball.transform.localPosition = currentHole.transform.parent.localPosition + Vector3.up;
			ball.transform.parent = currentHole.transform;
			ball.transform.localPosition = Vector3.up;
			// TODO: update its local scale
			GlobalData.balls.Add (ball);
			ball.SetActive (false);
		}

		// set current turn to 0 (first player)
		GlobalData.turn = 0;

		// activate ball for next (first) player
		SetNextTurn ();

//		GameObject ball = GameObject.Find("Ball1");
//		ball.transform.parent = currentHole.transform;
//		ball.transform.localPosition = Vector3.up;
	}

	// Update is called once per frame
	void Update () {

	}

	void SetNextTurn() {
		GameObject ball = GlobalData.balls[GlobalData.turn];
		ball.SetActive (true);
	}
}
