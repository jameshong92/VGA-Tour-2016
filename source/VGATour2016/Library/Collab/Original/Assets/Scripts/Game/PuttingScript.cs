using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuttingScript : MonoBehaviour {

	// StateToPuttSet()
	// StateBackToIdle()


	public GameObject birdEyeHolder;
	public GameObject puttingTarget;
	public GameObject clubHolder;
	public GameObject courses;

	private Vector3 displacement;

//	private Transform FindBall() {
//		Debug.Log ("in else if");
//		foreach (Transform child in this.transform.GetChild(0)) {
//			Debug.Log ("checking " + child.name);
//			if (child.CompareTag (GlobalData.players [GlobalData.turn].ball)) {
//				return child;
//			}
//		}
//		return null;
//	}


	// Use this for initialization
	void Start () {

//		// used for remembering ball gameobjects (for activating inactive ones)
//		GlobalData.balls = new List<GameObject> ();
//
//		// TODO: delete this after merging with selection scene, this is used for testing purposes
//		GlobalData.course = 2;
//		GlobalData.players = new List<Player> ();
//
//		// initialize two players with ball names Ball1 and Ball2 
//		Player p = new Player ("Harry");
//		p.ball = "Ball1";
//		GlobalData.players.Add (p);
//
//		// remove TODO

		BeginNewHole ();
	}

	void Update() {
//		Debug.Log ("current player" + GlobalData.players[GlobalData.turn].name);
	}


	// create Current hole in Putt view
	// create and initialize the player's ball 
	//
	void BeginNewHole() {
		// find course and hole from global data
		string course = "Course" + GlobalData.course;
		string hole = "Hole" + GlobalData.hole;
		courses.SetActive (true);
		GameObject courseObject = courses.transform.Find(course).gameObject;
		GameObject holeObject = courseObject.transform.Find(hole).gameObject;

		birdEyeHolder.GetComponent<BirdEyeScript>().CopyHole(holeObject);

		GameObject currentHole = Instantiate(holeObject);
		courses.SetActive (false);
		currentHole.transform.parent = this.transform;
		currentHole.transform.localPosition = Vector3.zero;
		currentHole.transform.localScale = Vector3.one * 0.5f;
		currentHole.transform.localEulerAngles = new Vector3 (0, 90, 0);
		courseObject.SetActive(false);

		GlobalData.balls = new List<GameObject>();
		// add ball to be child of current hole, set it to be inactive
		foreach (Player p in GlobalData.players) {
			// GameObject ball = GameObject.Find (p.ball); 
			// GameObject ball = Instantiate(p.g_ball);
			GameObject ball = p.g_ball;
			if (ball.GetComponent<BallScript> () == null) {
				ball.AddComponent<BallScript> ();
			}
			//ball.transform.localPosition = currentHole.transform.localPosition + Vector3.up;
			//			ball.transform.parent = currentHole.transform.parent.parent;
			//			ball.transform.localPosition = Vector3.up;
			//			ball.transform.localPosition = currentHole.transform.parent.localPosition + Vector3.up;
			ball.transform.parent = currentHole.transform;
			ball.transform.localPosition = Vector3.up * 0.3f;
			ball.transform.localScale = Vector3.one * 0.2f; 
			// TODO: update its local scale
			GlobalData.balls.Add (ball);
			ball.SetActive (false);

			p.isFinished = false; // At the beginning of a new hole, all players are in 
		}
		this.transform.parent = puttingTarget.transform;
		this.transform.localPosition = Vector3.zero;
		this.transform.localScale = Vector3.one;
		this.transform.localEulerAngles = Vector3.zero;

		// set current turn to -1 because GlobalData.NextTurn() gives the turn to the NEXT player
		// we want 0-th player to play... kind of hacky
		GlobalData.turn = -1;
		GlobalData.NextTurn ();
		GlobalData.state = GlobalData.State.TurnBegin;
		//		GameObject ball = GameObject.Find("Ball1");
		//		ball.transform.parent = currentHole.transform;
		//		ball.transform.localPosition = Vector3.up;

		GameObject currentFootPosition = new GameObject ("currentFootPosition");
		currentFootPosition.transform.parent = currentHole.transform;
	}
		

	public void StateToPuttSet() {
		
		GlobalData.state = GlobalData.State.PuttSet;
//		GameObject currentHole;
		clubHolder.SetActive (true);
		foreach (Transform t in clubHolder.transform) {
			Debug.Log ("in foreach!!!!!");
			Debug.Log (t.name);
			if (t.name.Equals (GlobalData.players [GlobalData.turn].name + "Club")) {
				t.GetComponent<Renderer> ().enabled = true;
				clubHolder.GetComponent<GolfClubScript>().SetCurrentClub (t.gameObject);
				Debug.Log ("setting club to active");
				Debug.Log (t.name);
			} else if (t.name.Contains("Club")) {
				t.GetComponent<Renderer> ().enabled = false;
			}
		}
	}

	public void StateBackToIdle() {
		GlobalData.state = GlobalData.State.Idle;
		clubHolder.SetActive (false);
		// GlobalData.currentClub = clubHolder;
	}
}
