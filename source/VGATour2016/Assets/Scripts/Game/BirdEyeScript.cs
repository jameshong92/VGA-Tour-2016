using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class BirdEyeScript : MonoBehaviour {

	public GameObject birdEyeTarget;
	public GameObject puttingHolder;
	public GameObject receiver;
	public GameObject foot; // JUST ADDED
	public Font fakeReceipt;

	private List<GameObject> balls;
	private GameObject currentFoot;
	private GameObject currentHole;
	private GameObject uicontrol;

	private bool showPutting;
	private Vector3 localPosition;

	void Start() {
		showPutting = !puttingHolder.transform.parent.name.Contains("Temp");
		uicontrol = GameObject.Find ("UI_game view");
	}

	public void CopyHole(GameObject holeObject) {
		if (this.transform.childCount > 0) {
			foreach (Transform t in this.transform) {
				Destroy (t.gameObject);
			}
		}
		currentHole = Instantiate(holeObject);
		currentHole.transform.Find ("Goal").gameObject.SetActive (false); 
		currentHole.transform.parent = this.transform;
		currentHole.transform.localPosition = Vector3.zero;
		currentHole.transform.localScale = Vector3.one;
		currentHole.transform.localEulerAngles = new Vector3 (0, 90, 0);

		currentFoot = Instantiate (foot);
		balls = new List<GameObject> ();
		for (int i = 0; i < GlobalData.players.Count; i++) {
			GameObject ball = Instantiate(GlobalData.players[i].g_ball);
			if (ball.GetComponent<Rigidbody> () != null) {
				Destroy (ball.GetComponent<Rigidbody> ());
			}
			if (ball.GetComponent<Collider> ().enabled) {
				ball.GetComponent<Collider> ().enabled = false;
			}
			if (ball.GetComponent<BallScript> () != null) {
				ball.GetComponent<BallScript> ().enabled = false;
			}
			ball.name = GlobalData.players[i].name + "BirdEyeBall";
			ball.transform.parent = currentHole.transform;
			ball.transform.localScale = Vector3.one * 0.2f;
			balls.Add(ball);
			ball.SetActive(false);

			AddText (i, ball);
		}
	}

	public void AddText(int playerIndex, GameObject ball) {
		// adding name text
		GameObject text = new GameObject (GlobalData.players [playerIndex].name + "Text");
		text.transform.parent = ball.transform;
		text.transform.eulerAngles += new Vector3 (90, 0, 0);
		text.transform.localPosition = new Vector3 (0, 0, 2);
		text.transform.localScale = Vector3.one;
		text.AddComponent<TextMesh> ();
		TextMesh txt = text.GetComponent<TextMesh> ();
		txt.text = GlobalData.players [playerIndex].name;
		txt.color = ball.GetComponent<Renderer>().material.GetColor("_Color");
		txt.anchor = TextAnchor.MiddleCenter;
		txt.fontSize = 25;
		txt.alignment = TextAlignment.Center;
		txt.fontStyle = FontStyle.Bold;
		txt.font = fakeReceipt;
	}

	// ADDED: update ball to a new one
//	public void UpdateBall(int playerIndex) {
//		Debug.Log (" in update ball");
//		string ballName = GlobalData.players [playerIndex].name + "BirdEyeBall";
//		GameObject text = null;
//
//		// find ball from the list of balls
//		GameObject oldBall = balls.Find (b => b.name.Equals (ballName));
//		Vector3 ballPosition = Vector3.zero;
//		if (oldBall != null) {
//			Debug.Log ("ball found");
//			// save the current position of old ball, remove from balls list & destroy
//			ballPosition = oldBall.transform.localPosition;
//			text = oldBall.transform.GetChild (0).gameObject;
//			text.transform.parent = currentHole.transform;
////			balls.Remove (oldBall);
////			Destroy (oldBall);
//		}
//		Debug.Log ("creating ball");
//		GameObject ball = Instantiate(GlobalData.players[playerIndex].g_ball);
//		if (ball.GetComponent<Rigidbody> () != null) {
//			Destroy (ball.GetComponent<Rigidbody> ());
//		}
//		if (ball.GetComponent<Collider> ().enabled) {
//			ball.GetComponent<Collider> ().enabled = false;
//		}
//		if (ball.GetComponent<BallScript> () != null) {
//			ball.GetComponent<BallScript> ().enabled = false;
//		}
//		ball.transform.parent = currentHole.transform;
//		ball.transform.localScale = Vector3.one * 0.2f;
//		ball.transform.localPosition = ballPosition;
//		ball.name = ballName;
//		balls [GlobalData.turn] = ball;
//		if (oldBall != null) {
//			Destroy (oldBall);
//		}
//		// balls.Add(ball); // ball is active since only current user can modify the ball
//		if (text == null) {
//			AddText (playerIndex, ball);
//		} else {
//			text.transform.parent = ball.transform;
//			text.GetComponent<TextMesh> ().color = ball.GetComponent<Renderer> ().material.GetColor ("_Color");
//		}
//	}
	public void UpdateBall(int playerIndex, Material m) {
		string ballName = GlobalData.players [playerIndex].name + "BirdEyeBall";
		GameObject ball = balls.Find (b => b.name.Equals (ballName));
		ball.GetComponent<Renderer> ().material = m;
		ball.transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = m.GetColor ("_Color");
	}

	// Update is called once per frame
	void Update () {
		balls [GlobalData.turn].SetActive (true);
		balls [GlobalData.turn].transform.localPosition = GlobalData.players [GlobalData.turn].g_ball.transform.localPosition;

		if (!showPutting && !puttingHolder.transform.parent.name.Contains("Temp")) {
			receiver.GetComponent<ReceiverScript>().changeSideView(0);
			uicontrol.GetComponent<UIController> ().UpdateBirdsEye ();
			birdEyeTarget.GetComponent<VGADefaultTrackableEventHandler> ().OnTrackableStateChanged (
				TrackableBehaviour.Status.TRACKED, 
				TrackableBehaviour.Status.NOT_FOUND);
		}

		if (!showPutting && GlobalData.currentSideView == 0 && foot.transform.localScale != Vector3.zero) {
			// ADDED
			// show the current foot position in the birdeye view
			currentFoot.transform.parent = this.transform.GetChild (0).transform;
			GameObject currentFootPosition = puttingHolder.transform.GetChild(0).Find ("currentFootPosition").gameObject;
			currentFoot.transform.localPosition = currentFootPosition.transform.localPosition + new Vector3 (0, 0.5f, 0);
			currentFoot.transform.localScale = Vector3.one * 0.02f;
			currentFoot.transform.rotation = foot.transform.rotation;
			currentFoot.transform.GetChild (0).gameObject.GetComponent<Renderer> ().enabled = true;
		}
		showPutting = !puttingHolder.transform.parent.name.Contains("Temp");
	}
}
