using UnityEngine;
using System.Collections;

public class TravelScript : MonoBehaviour {

	public GameObject arCamera;
	public GameObject puttingHolder;

	private Vector2 touchOrigin = -Vector2.one; //Used to store location of screen touch origin for mobile controls.
	private float distance = 5f;
	private bool onHold;


	// Use this for initialization
	void Start () {
	}
		
	public void SetHoldInput(bool input) {
		onHold = input;
	}
	
	// Update is called once per frame
	void Update () {
//		if (Network.isServer) {

			if (onHold && GlobalData.state == GlobalData.State.Idle) {
				Debug.Log ("pressing");

				Vector3 forward = arCamera.transform.forward;
				forward.y = 0f;
				Vector3 unitVector = forward.normalized;
				if (puttingHolder.transform.childCount > 0) {
					GameObject holeObject = puttingHolder.transform.GetChild (0).gameObject;
					holeObject.transform.position = holeObject.transform.position - unitVector * distance * Time.deltaTime;
				}
			}
//		}
	}
}
