using UnityEngine;
using System.Collections;

public class GolfClubScript : MonoBehaviour {

	public GameObject puttingHolder;
	public GameObject birdeyeHolder;
	public GameObject arCamera;
	public GameObject arrow;
	public GameObject receiver;
	public AudioSource hardHit;
	public AudioSource softHit;

	private float speed = 100f;
	private Vector3 d0;
	private Vector3 d1;
	private Vector3 d2;
//	private bool forwardOnly;
	private Vector3 clubOrient;
	private float vol = 1.0f;
	private Vector3 accInput;
	private bool onHold;
	private GameObject currentClub;

	void Start()
	{
//		forwardOnly = false;
		foreach (Player p in GlobalData.players) {
			p.g_club.transform.parent = this.transform;
			p.g_club.transform.localPosition = Vector3.zero;
			p.g_club.transform.localScale = Vector3.one * 14;
			p.g_club.SetActive(true);
			p.g_club.GetComponent<Renderer>().enabled = false;
		}
	}

	private Transform FindBall() {
		foreach (Transform child in puttingHolder.transform.GetChild(0).transform) {
			if (child.name.Equals(GlobalData.players[GlobalData.turn].name + "Ball")) {
				return child;
			}
		}
		return null;
	}

	public void SetCurrentClub(GameObject t) {
		currentClub = t;
	}

	public void StateBackToPuttSet() {
		GlobalData.state = GlobalData.State.PuttSet;
	}

	public void StateToPutt() {
		GlobalData.state = GlobalData.State.Putt;
		clubOrient = currentClub.transform.eulerAngles;
	}
		
	void StateToBallMove() {
		GlobalData.state = GlobalData.State.BallMove;
		onHold = false;
		this.gameObject.SetActive(false);
	}

	public void SetAccInput(Vector3 input) {
		accInput = input;
	}

	public void SetHoldInput(bool input) {
		onHold = input;
	}

	void Update()
	{
		if (!puttingHolder.transform.parent.name.Contains ("Temp") && GlobalData.state == GlobalData.State.Putt) {
//			d2 = Input.acceleration;
			d2 = accInput;
			if (d2.z >= 0 && d2.x < 0) {
				d2.x = -1f - (1f + d2.x);	
				if (d2.x < -1.5f) {
					d2.x = -1.5f;
				}
			} else if (d2.z >= 0 && d2.x > 0) {
				d2.x = 1f + (1f - d2.x);
				if (d2.x > 1.5f) {
					d2.x = 1.5f;
				}
			}
			if (d1.x < 0 && d2.x - d1.x > 1.5f && !onHold) {
				d2.x = -1.5f;
			}
			Vector3 d = d2 * 0.5f + d1 * 0.5f;
			if (onHold) {
				if (d.x < d1.x) {
					d.x = d1.x;
				}
			}

//			currentClub.transform.eulerAngles = new Vector3(d.x * speed, clubOrient.y, clubOrient.z);
			currentClub.transform.eulerAngles = new Vector3(clubOrient.x, clubOrient.y, - d.x * speed) + new Vector3(0, 0, 90);

			float angle = currentClub.transform.eulerAngles.z;
//			Debug.Log ("before: " + angle);
//			Debug.Log ("x : " + d.x);
//			Debug.Log ("y : " + d.y);
//			Debug.Log ("z : " + d.z);
//			if (angle > 360) {
//				angle = angle - 360;
//				Debug.Log ("if after: " + angle);
//			}
			if (angle < 100 && onHold) { // originally 90 degrees, but for better user experience, added 10 degrees
				// TODO: CHECK DIRECTION OF FORCE AND FIX CURRENT CLUB SWING MOTION
				Transform ball = FindBall ();
				Rigidbody r = ball.GetComponent<Rigidbody> ();
				r.useGravity = true;
//				Vector3 force = currentClub.transform.position - ball.position - currentClub.transform.forward.normalized * 2.5f;
				Vector3 force = arrow.transform.right;
				force.y = 0;
//				Debug.Log("Speed : " + (d.x-d1.x));
				float s = ((d.x - d1.x) + (d1.x - d0.x)) / 2f;
				r.AddForce (force.normalized * -1 * s * 4000);
				hardHit.Play ();
				receiver.GetComponent<ReceiverScript> ().Vibrate ();
				StateToBallMove();
			}
			d0 = d1;
			d1 = d;
		} else if (!puttingHolder.transform.parent.name.Contains ("Temp") && GlobalData.state == GlobalData.State.PuttSet) {
			Transform ball = FindBall ();
			Vector3 d = arCamera.transform.up;
			d.y = 0;
			currentClub.transform.position = ball.transform.position + Vector3.Cross (d, Vector3.down).normalized * 4 + new Vector3(0, 47, 0);
			currentClub.transform.position -= currentClub.transform.forward.normalized * 11f;
//			currentClub.transform.position += currentClub.transform.forward.normalized * 4f;
//			currentClub.transform.localPosition += new Vector3 (0, 0, 2.5f);
			arrow.transform.position = ball.transform.position - Vector3.Cross (d, Vector3.down).normalized * 10;

			arrow.transform.position -= arrow.transform.forward.normalized * 2f;	
			Quaternion orientation = Quaternion.LookRotation(Vector3.Cross(d, Vector3.down));
			currentClub.transform.rotation = orientation;
			currentClub.transform.eulerAngles += new Vector3 (-18, -90, 90);
			arrow.transform.localScale = Vector3.one * 6;
			arrow.transform.rotation = orientation;
			arrow.transform.eulerAngles += new Vector3 (0, -90, 0);
		}
	}

//	public void ForwardOnly() 
//	{
////		forwardOnly = !forwardOnly;
//	}
//
}
