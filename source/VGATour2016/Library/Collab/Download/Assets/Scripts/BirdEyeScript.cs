using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdEyeScript : MonoBehaviour {


	public GameObject birdEyeTarget;
	public GameObject puttingHolder;
//	private GameObject gameGround;
//	private DefaultTrackableEventHandler eventHandler;
	// Use this for initialization
//	void Start () {
////		gameGround = GameObject.Find("GameGround");	
////		eventHandler = GetComponent<DefaultTrackableEventHandler>();
//		Debug.Log ("Hello");
//	}
//
//
//	// Update is called once per frame
//	void Update () {
//		/*
//		int count = this.transform.childCount;
//		for (int i = count - 1; i >= 0; i--) {
//			Destroy (this.transform.GetChild(i).gameObject);
//		}
//		if (eventHandler.currentStatus == TrackableBehaviour.Status.DETECTED ||
//		    eventHandler.currentStatus == TrackableBehaviour.Status.TRACKED ||
//		    eventHandler.currentStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
//			Debug.Log (count);
//			for (int i = 0; i < gameGround.transform.childCount; i++) {
//				GameObject aChild = Instantiate (gameGround.transform.GetChild(i).gameObject);
//				aChild.transform.parent = this.transform;
//			}
//		}
//		*/
//	}


	// Use this for initialization
	void Start () {

		// used for remembering ball gameobjects (for activating inactive ones)
		GlobalData.balls = new List<GameObject> ();

		// TODO: delete this after merging with selection scene, this is used for testing purposes
		GlobalData.course = 2;
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
		currentHole.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
		currentHole.transform.localEulerAngles = new Vector3 (0, 90, 0);
		courseObject.SetActive(false);
		//		Debug.Log("start");
		//		Debug.Log(course);
		//		Debug.Log(hole);

		// add ball to be child of current hole, set it to be inactive
		foreach (Player p in GlobalData.players) {
			GameObject ball = GameObject.Find (p.ball);
			ball.transform.localPosition = currentHole.transform.localPosition + Vector3.up;
			//			ball.transform.parent = currentHole.transform.parent.parent;
			//			ball.transform.localPosition = Vector3.up;
			//			ball.transform.localPosition = currentHole.transform.parent.localPosition + Vector3.up;
			ball.transform.parent = currentHole.transform;
			ball.transform.localPosition = Vector3.up;
			ball.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
			// TODO: update its local scale
			GlobalData.balls.Add (ball);
			ball.SetActive (false);
		}
		this.transform.parent = birdEyeTarget.transform;
		this.transform.localPosition = Vector3.zero;
		this.transform.localScale = Vector3.one;
		this.transform.localEulerAngles = Vector3.zero;
			
		// set current turn to 0 (first player)
		GlobalData.turn = 0;

		// activate ball for next (first) player
		SetNextTurn ();

		//		GameObject ball = GameObject.Find("Ball1");
		//		ball.transform.parent = currentHole.transform;
		//		ball.transform.localPosition = Vector3.up;
	}

	void SetNextTurn() {
		GameObject ball = GlobalData.balls[GlobalData.turn];
		ball.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		if (GlobalData.state == GlobalData.State.BallMove) {
			if (this.transform.childCount > 0) {
				Destroy(this.transform.GetChild (0).gameObject);
			}
			GameObject currentHole = Instantiate(puttingHolder.transform.GetChild(0).gameObject);
			currentHole.transform.parent = this.transform;
			currentHole.transform.localPosition = Vector3.zero;
			currentHole.transform.localScale = Vector3.one * 0.2f;
			currentHole.transform.localEulerAngles = new Vector3 (0, 90, 0);
		}
		// avoid having the putting view rotate on bad image target detection
//		transform.rotation = Quaternion.identity;
//		transform.localEulerAngles = new Vector3 (0, 90, 0);
	}
}
