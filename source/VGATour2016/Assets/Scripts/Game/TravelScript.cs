using UnityEngine;
using System.Collections;

public class TravelScript : MonoBehaviour {

	public GameObject arCamera;
	public GameObject puttingHolder;
	public GameObject puttingTarget;
	public GameObject foot;
//	public GameObject currentFootPosition;

	private Vector2 touchOrigin = -Vector2.one; //Used to store location of screen touch origin for mobile controls.
	private float distance = 20f;
	private bool onHold;

	// Use this for initialization
	void Start () {
//		foot.transform.position = puttingTarget.transform.position + new Vector3 (0, 2f, 0);
		InitPosition();
	}

	public void InitPosition() {
		foot.transform.position = puttingTarget.transform.position + new Vector3 (0, 2f, 0);

		// MOST RECENT ADD
		foreach (Player p in GlobalData.players) {
			p.prevPosition = Vector3.zero;
		}
	}
		
	public void SetHoldInput(bool input) {
		onHold = input;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if (puttingTarget.transform.childCount == 0) {
			onHold = false;
		}

		if (onHold && GlobalData.state == GlobalData.State.Idle) {
			Vector3 forward = foot.transform.forward;
			forward.y = 0f;
			Vector3 unitVector = forward.normalized;
			if (puttingHolder.transform.childCount > 0) {
				GameObject holeObject = puttingHolder.transform.GetChild(0).gameObject;
				holeObject.transform.position = holeObject.transform.position - unitVector * distance * Time.deltaTime;
				GlobalData.players [GlobalData.turn].prevPosition = holeObject.transform.localPosition;
			}
		}

		if (GlobalData.state == GlobalData.State.Idle && Physics.Raycast (puttingTarget.transform.position + new Vector3 (0, 500, 0), -Vector3.up, out hit)) {
			foot.transform.GetChild (0).gameObject.GetComponent<Renderer> ().enabled = true;
			foot.transform.position = hit.point + new Vector3 (0, 1f, 0);
			GameObject currentFootPosition = puttingHolder.transform.GetChild(0).Find ("currentFootPosition").gameObject;
			currentFootPosition.transform.position = foot.transform.position;

			foot.transform.localScale = Vector3.one;

			Vector3 d = arCamera.transform.up;
			d.y = 0;

			Quaternion orientation = Quaternion.LookRotation (Vector3.Cross (d, Vector3.down));
			this.transform.rotation = orientation;
			foot.transform.rotation = orientation;
			foot.transform.eulerAngles += new Vector3 (0, -90, 0);
		} else {
			foot.transform.GetChild (0).gameObject.GetComponent<Renderer> ().enabled = false;
		}
	}

	public bool IsInsideHalo() {
		float distance = (foot.transform.position - GlobalData.balls [GlobalData.turn].transform.position).magnitude;
		if (distance > 15f) {
			return false;
		} else {
			return true;
		}
	}
}
