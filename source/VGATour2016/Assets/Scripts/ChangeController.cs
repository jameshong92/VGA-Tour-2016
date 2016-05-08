using UnityEngine;
using System;
using System.Collections;

public class ChangeController : MonoBehaviour {

	private int playerNum;
	private int itemIndex;
	private int itemNum;
	private float degree;
	private int frame;

	private bool message;
	private bool isClub;
	private int left_or_right; 
	private Quaternion rot;
	private Quaternion original_rot;
	private Vector3 scale;

	public UIController uicontrol;
	public GameObject BirdEyeHolder;

	// Use this for initialization
	void Start () {
		if (transform.name.Contains ("Club"))
			isClub = true;
		else
			isClub = false;	
		
		playerNum = GlobalData.players.Count;
		if (isClub)
			itemIndex = Int32.Parse (GlobalData.players [GlobalData.turn].g_club.tag);
		else
			itemIndex = Int32.Parse (GlobalData.players [GlobalData.turn].g_ball.tag);

		itemNum = transform.childCount;
		degree = 360 / itemNum;

		// set up rotation for user's item to face forward
		if (itemIndex != 0) {
			rot = transform.rotation;
			Vector3 angle = new Vector3 (0, degree * itemIndex, 0);
			Quaternion delta = Quaternion.Euler (angle);
			transform.rotation = rot * delta;
		}
		select_ (transform.GetChild (itemIndex).gameObject);
		degree /= 10;
		frame = 0; 
		left_or_right = -1;
		message = false;

	}
	
	// Update is called once per frame
	void Update () {

		if (left_or_right == 1) {
			if (frame < 10) {
				rot = transform.rotation;
				Vector3 angle = new Vector3 (0, degree, 0);
				Quaternion delta = Quaternion.Euler (angle);
				transform.rotation = rot * delta;
				frame++;
			} else {
				UpdateIndex ();
				select_ (transform.GetChild (itemIndex).gameObject);
				frame = 0;
				left_or_right = 0;
			}
		} else if (left_or_right == 2) {	// rotate right
			if (frame < 10) {
				rot = transform.rotation;
				Vector3 angle = new Vector3 (0, -1 * degree, 0);
				Quaternion delta = Quaternion.Euler (angle);
				transform.rotation = rot * delta;
				frame++;
			} else {
				UpdateIndex ();
				select_ (transform.GetChild (itemIndex).gameObject);
				frame = 0;
				left_or_right = 0;
			}
		}
	}

	public void touch (int input) {
		Debug.Log ("touch");
		if (input == 4) {			// rotate left
			left_or_right = 1;
			deselect_ (transform.GetChild (itemIndex).gameObject);
		} else if (input == 2) {	// rotate right
			left_or_right = 2;
			deselect_ (transform.GetChild (itemIndex).gameObject);
		} else if (input == 0) {	// select item
			if (message) {
				uicontrol.GetComponent<UIController> ().hideMessage ();
			}

			Player p = GlobalData.players [GlobalData.turn];

			// if ball
			if (!isClub) {
				//check if same ball
				if (p.g_ball.tag == transform.GetChild (itemIndex).gameObject.tag) {
					
				} else {
					Material m =transform.GetChild (itemIndex).gameObject.GetComponent<Renderer> ().material; 
					p.g_ball.GetComponent<Renderer> ().material = m;
					// ADDED THIS ROW
					BirdEyeHolder.GetComponent<BirdEyeScript> ().UpdateBall (GlobalData.turn, m);
				}
				uicontrol.GetComponent<UIController> ().displayMessage ("ballChanged");
				message = true;
			}
			// if club
			else {
				if (GlobalData.players[GlobalData.turn].g_club.tag == transform.GetChild(itemIndex).gameObject.tag) {
					
				} else {
					GameObject newClub = Instantiate (transform.GetChild (itemIndex).transform.GetChild(0).gameObject);
					newClub.transform.parent = p.g_club.transform.parent;
					newClub.transform.localPosition = p.g_club.transform.position;
					newClub.transform.localScale = p.g_club.transform.localScale;
					newClub.name = p.name + "Club";

					Destroy (p.g_club);
					p.g_club = newClub;
				}
				uicontrol.GetComponent<UIController> ().displayMessage ("clubChanged");
				message = true;
			}
		}
	}

	void UpdateIndex () {
		if (left_or_right == 1) {
			itemIndex++;
			if (itemIndex == itemNum) {
				itemIndex = 0;
			}
		}
		else if (left_or_right == 2) {
			itemIndex--;
			if (itemIndex < 0)
				itemIndex = itemNum - 1;
		}
	}

	void select_(GameObject go) {
		scale = go.transform.localScale;
		go.transform.localScale *= 2;
		if (isClub)
			go.transform.Translate (3, 0, 0);
	}

	void deselect_(GameObject go) {
		go.transform.localScale = scale;
		if (isClub)
			go.transform.Translate (-3, 0, 0);
	}
		



}
