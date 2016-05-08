using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ToolbarControl : MonoBehaviour {

	public Camera cam;
	public GameObject finger;
	public GameObject BallSelect;
	public GameObject ClubSelect;
	public GameObject go;
	public Text BallText;
	public Text ClubText;

	// variable from device -- left/right swipe, tap
	private int input;
	private NetworkView nview;

	// for selecting ball/club
	private int ballNum;
	private int clubNum;
	private float ballDegree;
	private float clubDegree;
	private GameObject selected;

	// for storing the current selected ball/club info
	private int index;
	private Quaternion rot;
	private Quaternion original_rot;
	private Vector3 scale;

	private int ball_or_club;
	private int left_or_right;
	private int playerIndex;
	private int playerNum;

	// for platform rotation animation
	private int frame;
	private float degree;

	// Use this for initialization
	void Start () {

		nview = GetComponent<NetworkView> ();

		//hard coded 3 hypothetical players
		//		GlobalData.players = new List<Player>();
		//		GlobalData.players.Add (new Player ("Peter"));
		//		GlobalData.players.Add (new Player ("Harry"));
		//		GlobalData.players.Add (new Player ("James")); 
		playerIndex = 0;
		playerNum = GlobalData.players.Count;

		index = 0;
		ballNum = BallSelect.transform.childCount;
		ballDegree = 360 / ballNum;
		clubNum = ClubSelect.transform.childCount;
		clubDegree = 360 / clubNum;
		ballDegree /= 10; clubDegree /= 10;

		//setup ballselect platform
		SetUpBall();

		frame = 0; 

//		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}

	// Update is called once per frame
	void Update () {
		if (ball_or_club == 0) { 		// Ball platform
			if (left_or_right == 1) {	// rotate left
				if (frame < 10) {
					rot = BallSelect.transform.rotation;
					Vector3 angle = new Vector3 (0, ballDegree, 0);
					Quaternion delta = Quaternion.Euler (angle);
					BallSelect.transform.rotation = rot * delta;
					frame++;
				} else {
					UpdateIndex ();
					select_ (BallSelect.transform.GetChild (index).gameObject);
					frame = 0; left_or_right = 0;
				}
			} else if (left_or_right == 2) {	// rotate right
				if (frame < 10) {
					rot = BallSelect.transform.rotation;
					Vector3 angle = new Vector3 (0, -1 * ballDegree, 0);
					Quaternion delta = Quaternion.Euler (angle);
					BallSelect.transform.rotation = rot * delta;
					frame++;
				} else {
					UpdateIndex ();
					select_ (BallSelect.transform.GetChild (index).gameObject);
					frame = 0; left_or_right = 0;
				}
			}
		} else if (ball_or_club == 1) {	// Club platform
			if (left_or_right == 1) {
				if (frame < 10) {
					rot = ClubSelect.transform.rotation;
					Vector3 angle = new Vector3 (0, clubDegree, 0);
					Quaternion delta = Quaternion.Euler (angle);
					ClubSelect.transform.rotation = rot * delta;
					frame++;
				} else {
					UpdateIndex ();
					select_ (ClubSelect.transform.GetChild (index).gameObject);
					frame = 0; left_or_right = 0;
				}
			} else if (left_or_right == 2) {
				if (frame < 10) {
					rot = ClubSelect.transform.rotation;
					Vector3 angle = new Vector3 (0, -1 * clubDegree, 0);
					Quaternion delta = Quaternion.Euler (angle);
					ClubSelect.transform.rotation = rot * delta;
					frame++;
				} else {
					UpdateIndex ();
					select_ (ClubSelect.transform.GetChild (index).gameObject);
					frame = 0; left_or_right = 0;
				}
			}
		}
	}

	[RPC]
	void SetTouchInput (int touchInput) {
		input = touchInput;
		touch ();
	}

	[RPC]
	void SetAccInput (Vector3 input) {

	}

	public void test() {
		input = 0;
		touch ();
	}

	//called every time a player swipes or taps his device screen
	void touch () {
		if (ball_or_club == 0) {  	// ball state
			if (input == 4) { 		// rotate left
				left_or_right = 1;
				deselect_ (BallSelect.transform.GetChild (index).gameObject);
			}
			else if (input == 2) {	// rotate right
				left_or_right = 2;
				deselect_ (BallSelect.transform.GetChild (index).gameObject);
			}
			else if (input == 0) {	// select ball
				//				GlobalData.players [playerIndex].ball = BallSelect.transform.GetChild (index).gameObject;
				GlobalData.players[playerIndex].g_ball = Instantiate (BallSelect.transform.GetChild (index).gameObject);
				GlobalData.players[playerIndex].g_ball.name = GlobalData.players[playerIndex].name + "Ball";
				GlobalData.players[playerIndex].g_ball.SetActive (false);
				//				copy_and_hide (GlobalData.players [playerIndex].g_ball, BallSelect.transform.GetChild (index).gameObject);
				SetUpClub ();
			}
		} 
		else if (ball_or_club == 1) { 	// club state
			if (input == 4) {		// rotate left
				left_or_right = 1;
				deselect_ (ClubSelect.transform.GetChild (index).gameObject);
			}
			else if (input == 2) {	// rotate right
				left_or_right = 2;
				deselect_ (ClubSelect.transform.GetChild (index).gameObject);
			}
			else if (input == 0) {	// select club
				//				GlobalData.players [playerIndex].golfClb = ClubSelect.transform.GetChild (index).gameObject;
				GlobalData.players [playerIndex].g_club = Instantiate (ClubSelect.transform.GetChild (index).GetChild(0).gameObject);
				GlobalData.players[playerIndex].g_club.name = GlobalData.players[playerIndex].name + "Club";
				GlobalData.players [playerIndex].g_club.SetActive (false);
				//				copy_and_hide (GlobalData.players [playerIndex].g_club, ClubSelect.transform.GetChild (index).gameObject);
				if (playerIndex < playerNum - 1) {
					playerIndex++;
					SetUpBall ();
				} else {
					ClubSelect.transform.parent.gameObject.SetActive (false);
					ClubText.text = "Nice! Touch anywhere to move on to the game.";
					ball_or_club = -1;
				}
			}
		}
		else {
			LoadGameScene ();
		}
	}

	// using button
	public void cast_ray() {

		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay (finger.transform.position);
		if (Physics.Raycast (ray, out hit, 1000)) {
			selected = hit.collider.gameObject;


			if (selected.transform.tag == "BallButton" && frame == 0) {

				if (selected.transform.name == "Left Button_") {
					// turn the platform left
					left_or_right = 1;
					deselect_ (BallSelect.transform.GetChild (index).gameObject);
				}

				if (selected.transform.name == "Right Button_") {
					left_or_right = 2;
					deselect_ (BallSelect.transform.GetChild (index).gameObject);
				}

				if (selected.transform.name == "Select Button_") {
					// add selected ball to player target, move on to clubselect
					GlobalData.players [playerIndex].g_ball = Instantiate (BallSelect.transform.GetChild (index).gameObject);
					GlobalData.players [playerIndex].g_ball.SetActive (false);
					//					copy_and_hide (GlobalData.players [playerIndex].g_ball, BallSelect.transform.GetChild (index).gameObject);
					SetUpClub ();
				}
			}

			if (selected.transform.tag == "ClubButton") {
				if (selected.transform.name == "Left Button_") {
					left_or_right = 1;
					deselect_ (ClubSelect.transform.GetChild (index).gameObject);
				}

				if (selected.transform.name == "Right Button_") {
					left_or_right = 2;
					deselect_ (ClubSelect.transform.GetChild (index).gameObject);
				}

				if (selected.transform.name == "Select Button_") {
					//					GlobalData.players [playerIndex].golfClub = ClubSelect.transform.GetChild (index).gameObject;
					GlobalData.players [playerIndex].g_club = Instantiate (ClubSelect.transform.GetChild (index).gameObject);
					GlobalData.players [playerIndex].g_club.SetActive (false);
					//					copy_and_hide (GlobalData.players [playerIndex].g_club, ClubSelect.transform.GetChild (index).gameObject);

					if (playerIndex < playerNum - 1) {
						playerIndex++;
						SetUpBall ();
					} else {
						ClubSelect.transform.parent.gameObject.SetActive (false);
						ClubText.text = "Nice! Touch anywhere to move on to the game.";
						ball_or_club = -1;
					}
				}
			}
		}
	}

	void UpdateIndex () {
		if (left_or_right == 1) { 			//left
			index++;
			if (ball_or_club == 0) {		
				if (index == ballNum) {
					index = 0;
				}
			} else {
				if (index == clubNum) {
					index = 0;
				}
			}
		} else if (left_or_right == 2) {	//right
			index--;
			if (ball_or_club == 0) {
				if (index < 0) {
					index = ballNum - 1;
				}
			} else {
				if (index < 0) {
					index = clubNum - 1;
				}
			}
		}
	}

	void SetUpBall() {

		if (scale.magnitude != 0) {
			deselect_ (ClubSelect.transform.GetChild (index).gameObject);
			ClubSelect.transform.rotation = original_rot;
		}
		ClubSelect.transform.parent.gameObject.SetActive (false);
		ClubText.gameObject.SetActive (false);

		index = 0; ball_or_club = 0;
		original_rot = BallSelect.transform.rotation;
		BallSelect.transform.parent.gameObject.SetActive (true);
		select_ (BallSelect.transform.GetChild (index).gameObject);
		BallText.text = GlobalData.players [playerIndex].name + ", please select your ball.";
		BallText.gameObject.SetActive (true);
		//		initializePos (BallSelect);
	}

	void SetUpClub() {
		Debug.Log ("club");
		//remove all ball select items
		deselect_ (BallSelect.transform.GetChild (index).gameObject);
		BallSelect.transform.rotation = original_rot;
		BallSelect.transform.parent.gameObject.SetActive (false);
		BallText.gameObject.SetActive (false);

		//set up club select platform
		index = 0; ball_or_club = 1;
		original_rot = ClubSelect.transform.rotation;
		ClubSelect.transform.parent.gameObject.SetActive(true);
		select_ (ClubSelect.transform.GetChild (index).gameObject);
		ClubText.text = GlobalData.players [playerIndex].name + ", please select your club.";
		ClubText.gameObject.SetActive (true);
		//		initializePos (ClubSelect);

	}

	void select_(GameObject go) {
		go.SendMessage ("selectObj", SendMessageOptions.DontRequireReceiver);
		scale = go.transform.localScale;
		go.transform.localScale *= 2;
		if (ball_or_club == 1)
			go.transform.Translate (3, 0, 0);
	}

	void deselect_(GameObject go) {
		go.SendMessage ("deselectObj", SendMessageOptions.DontRequireReceiver);
		go.transform.localScale = scale;
		if (ball_or_club == 1)
			go.transform.Translate (-3, 0, 0);
	}


	public void LoadGameScene() {

		for (int i = 0; i < GlobalData.players.Count; i++){
			DontDestroyOnLoad (GlobalData.players [i].g_ball);
			DontDestroyOnLoad (GlobalData.players [i].g_club);
		}

		Application.LoadLevel ("Game");
	}

	// sets the ball or club selection platform to look at the user
	void initializePos(GameObject g) {
		Debug.Log ("init");
		//		go.transform.position = new Vector3 (x, y, z);
		//		g.transform.LookAt (go.transform, Vector3.up);
		//		g.transform.LookAt (cam.transform.parent.transform, Vector3.up);
		go.transform.position = new Vector3 
			(-cam.transform.parent.transform.position.x, g.transform.position.y, -cam.transform.parent.transform.position.z);
		g.transform.LookAt (go.transform);

		Debug.Log (g.transform.position.x);
		Debug.Log (g.transform.position.y);
	}

	// copies the selected ball or club, changes 
	void copy_and_hide(GameObject copy, GameObject go) {
		copy.transform.parent = go.transform.parent;
		copy.transform.localScale = go.transform.localScale;
		//		copy.transform.parent = GAMEOBJECT.transform;
		//		copy.transform.position = GAMEOBJECT.transform.position;
		//		copy.SetActive(false);
	}
}
