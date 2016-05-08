using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuttingScript : MonoBehaviour {

	public GameObject birdEyeHolder;
	public GameObject puttingTarget;
	public GameObject golfClub;

	private Vector3 displacement;

	// Use this for initialization
	void Start () {

	}

	private Transform FindBall() {
		Debug.Log ("in else if");
		foreach (Transform child in this.transform.GetChild(0)) {
			Debug.Log ("checking " + child.name);
			if (child.CompareTag (GlobalData.players [GlobalData.turn].ball)) {
				return child;
			}
		}
		return null;
	}

	// Update is called once per frame
	void LateUpdate () {
		// avoid having the putting view rotate on bad image target detection
//		transform.rotation = Quaternion.identity;
	}

	public void StateToPuttSet() {
		GlobalData.state = GlobalData.State.PuttSet;
		GameObject currentHole;

		if (this.transform.childCount > 0) {
//			SetDisplacement ();
			// update position to moved one
			GlobalData.players [GlobalData.turn].prevPosition = this.transform.GetChild (0).position;
			displacement = Vector3.zero;
			Destroy (this.transform.GetChild (0).gameObject);
			CreateHole ();
//			SetHolePosition (currentHole);
		} else {
			GlobalData.players [GlobalData.turn].prevPosition = this.transform.position;
			CreateHole ();
//			SetHolePosition (currentHole);
			SetDisplacement ();
		}

//		this.transform.parent = puttingTarget.transform;
//		this.transform.localPosition = Vector3.zero;
//		this.transform.localScale = Vector3.one;
//		this.transform.localEulerAngles = Vector3.zero;
	}

	private void CreateHole() {
		GameObject holeObject;
		holeObject = birdEyeHolder.transform.GetChild (0).gameObject;
		GameObject currentHole = Instantiate(holeObject);
		Debug.Log (currentHole.name);
		currentHole.transform.parent = this.transform;
		currentHole.transform.localPosition = Vector3.zero;
		currentHole.transform.localScale = Vector3.one * 20;
		currentHole.transform.localEulerAngles = new Vector3 (0, 90, 0);
		golfClub.SetActive (true);
		GlobalData.currentClub = golfClub;

		SetHolePosition (currentHole);
	}

	private void SetDisplacement() {
		Transform ball = FindBall ();
		displacement = this.transform.position - ball.position + new Vector3(0f, 1.5f, 0f); //1.5 for ball center
		Debug.Log ("transform position: " + this.transform.position);
		Debug.Log ("ball position: " + ball.position);
		Debug.Log ("displacement: " + displacement);
	}

	private void SetHolePosition(GameObject currentHole) {
		Debug.Log ("SET HOLE POSITON SET HOLE POSITONSET HOLE POSITONSET HOLE POSITONSET HOLE POSITONSET HOLE POSITONSET HOLE POSITONSET HOLE POSITONSET HOLE POSITONSET HOLE POSITONSET HOLE POSITONSET HOLE POSITONSET HOLE POSITON");
		Debug.Log ("set position of " + this.transform.GetChild (0).name);
//		Transform ball = FindBall();
//		Debug.Log (this.transform.GetChild(0).position - ball.position);
//		Vector3 displacement = this.transform.GetChild(0).position - ball.position;
		Debug.Log ("local: " + this.transform.localPosition);
		currentHole.transform.position = GlobalData.players [GlobalData.turn].prevPosition + displacement;
		Debug.Log ("world updated: " + currentHole.transform.position);
		Debug.Log ("local updated: " + currentHole.transform.localPosition);
		GlobalData.players [GlobalData.turn].prevPosition = currentHole.transform.position;
	}
}
