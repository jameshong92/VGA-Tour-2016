using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class BirdEyeScript : MonoBehaviour {

	public GameObject birdEyeTarget;
	public GameObject puttingHolder;
	public GameObject receiver;

	private List<GameObject> balls;

	private bool showPutting;

	void Start() {
		showPutting = !puttingHolder.transform.parent.name.Contains("Temp");	
	}

	public void CopyHole(GameObject holeObject) {
		if (this.transform.childCount > 0) {
			Destroy(this.transform.GetChild (0).gameObject);
		}
		GameObject currentHole = Instantiate(holeObject);
		currentHole.transform.Find ("Goal").gameObject.SetActive (false); 
		currentHole.transform.parent = this.transform;
		currentHole.transform.localPosition = Vector3.zero;
		currentHole.transform.localScale = Vector3.one;
		currentHole.transform.localEulerAngles = new Vector3 (0, 90, 0);

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
			ball.transform.parent = currentHole.transform;
			ball.transform.localScale = Vector3.one * 0.2f;
			balls.Add(ball);
			ball.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update () {
		balls [GlobalData.turn].SetActive (true);
		balls [GlobalData.turn].transform.localPosition = GlobalData.players [GlobalData.turn].g_ball.transform.localPosition;

		if (!showPutting && !puttingHolder.transform.parent.name.Contains("Temp")) {
			Debug.Log ("AAAAAAAAAAAAAAAAAAAAAAAAAAA");
			receiver.GetComponent<ReceiverScript>().changeSideView(0);
			birdEyeTarget.GetComponent<VGADefaultTrackableEventHandler> ().OnTrackableStateChanged (
				TrackableBehaviour.Status.TRACKED, 
				TrackableBehaviour.Status.NOT_FOUND);
		}
		showPutting = !puttingHolder.transform.parent.name.Contains("Temp");
	}
}
