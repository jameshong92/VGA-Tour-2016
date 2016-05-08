using UnityEngine;
using System.Collections;

public class BallCameraScript : MonoBehaviour {

	Quaternion rotation;

	// Use this for initialization
	void Start () {
		rotation = transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
//		transform.rotation = rotation;

		transform.position = transform.parent.position;
		transform.LookAt(transform.position + transform.parent.GetComponent<Rigidbody>().velocity);

		Vector3 position = transform.parent.position - (transform.rotation * Vector3.forward * 1.7f + Vector3.zero);
		transform.position = position;
	}
}
