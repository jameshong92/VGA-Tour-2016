using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdEyeScript : MonoBehaviour {

	public GameObject birdEyeTarget;
	public GameObject puttingHolder;

	private List<GameObject> balls;

	void Start() {
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
//		if (GlobalData.state == GlobalData.State.TurnBegin) {
//			return;
//		}
		balls [GlobalData.turn].SetActive (true);
//		if (GlobalData.players [GlobalData.turn].shotCount == 0 &&
//		    GlobalData.state == GlobalData.State.Idle ||
//		    GlobalData.state == GlobalData.State.Putt ||
//		    GlobalData.state == GlobalData.State.PuttSet) {
//			balls [GlobalData.turn].transform.localPosition = GlobalData.players [GlobalData.turn].prevPosition;
//		} else {
//			balls [GlobalData.turn].transform.localPosition = GlobalData.players [GlobalData.turn].prevPosition;
//		}
		balls [GlobalData.turn].transform.localPosition = GlobalData.players [GlobalData.turn].g_ball.transform.localPosition;
	}
}
