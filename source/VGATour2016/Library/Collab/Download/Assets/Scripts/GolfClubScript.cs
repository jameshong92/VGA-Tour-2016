using UnityEngine;
using System.Collections;

public class GolfClubScript : MonoBehaviour {

	public GameObject puttingHolder;
	public GameObject birdeyeHolder;
	public GameObject arCamera;
	public AudioSource hardHit;
	public AudioSource softHit;

	private float speed = 100f;
	private Vector3 d1;
	private Vector3 d2;
	private bool forwardOnly;
	private Vector3 clubOrient;
	private float vol = 1.0f;
	private Vector3 accInput;
	private bool onHold;

	void Start()
	{
		forwardOnly = false;
	}

	private Transform FindBall() {
//		Debug.Log ("in else if");
		foreach (Transform child in puttingHolder.transform.GetChild(0).transform) {
//			Debug.Log ("checking " + child.name);
			if (child.CompareTag (GlobalData.players [GlobalData.turn].ball)) {
				return child;
			}
		}
		return null;
	}

	private Transform FindBirdeyeBall() {
//		Debug.Log ("in else if");
		foreach (Transform child in birdeyeHolder.transform.GetChild(0).transform) {
//			Debug.Log ("checking " + child.name);
			if (child.CompareTag (GlobalData.players [GlobalData.turn].ball)) {
				return child;
			}
		}
		return null;
	}

	public void StateToPutt() {
		GlobalData.state = GlobalData.State.Putt;
		clubOrient = this.transform.eulerAngles;
	}
		
	void StateToBallMove() {
		GlobalData.state = GlobalData.State.BallMove;
		onHold = false;
		this.gameObject.SetActive(false);
	}

	public void SetAccInput(Vector3 input) {
		accInput = input;
		Debug.Log (accInput);
	}

	public void SetHoldInput(bool input) {
		onHold = input;
	}

	void Update()
	{
//		if (GlobalData.state == GlobalData.State.Putt || GlobalData.state == GlobalData.State.PuttSet) {
			if (!puttingHolder.transform.parent.name.Contains ("Temp") && GlobalData.state == GlobalData.State.Putt) {
//				d2 = Input.acceleration;
				d2 = accInput;
				Vector3 d = d2 * 0.5f + d1 * 0.5f;
				if (forwardOnly) {
					if (d.x < d1.x) {
						d.x = d1.x;
					}
				}

				transform.eulerAngles = new Vector3(d.x * speed, clubOrient.y, clubOrient.z);
				float angle = transform.eulerAngles.x + 180;
				if (angle > 360) {
					angle = angle - 360;
				}

				if (angle > 180 && onHold) {
					Transform ball = FindBall ();
					Rigidbody r = ball.GetComponent<Rigidbody> ();
					Vector3 force = this.transform.position - ball.position;
					force.y = 0;
					Debug.Log("Speed : " + (d.x-d1.x));
					r.AddForce (force.normalized * -1 * (d.x - d1.x) * 4000);
//					ball = FindBirdeyeBall ();
//					r = ball.GetComponent<Rigidbody> ();
//					r.AddForce (force.normalized * -500);
					hardHit.Play ();
					StateToBallMove();
				}
				d1 = d;
			} else if (!puttingHolder.transform.parent.name.Contains ("Temp") && GlobalData.state == GlobalData.State.PuttSet) {
				// this.transform.GetChild (0).gameObject.GetComponent<Renderer> ().enabled = true;
				Transform ball = FindBall ();
				//Vector3 cameraToBall = ball.transform.position - arCamera.transform.position;
				//cameraToBall.y = 0;
				Vector3 d = arCamera.transform.up;
				d.y = 0;
				this.transform.position = ball.transform.position + Vector3.Cross (d, Vector3.down).normalized * 2;
				Quaternion orientation = Quaternion.LookRotation(Vector3.Cross(d, Vector3.down));
				this.transform.rotation = orientation;
			}

//		} else if (Network.isClient) {
//			//			Debug.Log ("this is client");
//			Application.ExternalCall ("console.log", Network.player.ToString ());
//		} else {
//			//			Debug.Log ("what am i");
//		}

	}

	public void ForwardOnly() 
	{
		forwardOnly = !forwardOnly;
	}

}
