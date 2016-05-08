using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour {

	public AudioSource cheer;
	public AudioSource unimpressed;
	public AudioSource cup;

	private GameObject objectInHole;
	private bool ballInHole; // used to check for bounced balls, should only call once

	// Use this for initialization
	void Start () {
		ballInHole = false;
	}

	// Update is called once per frame
	void Update () {
		if (!cup.isPlaying && !cheer.isPlaying && ballInHole) {
			cheer.Play ();
			ballInHole = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (!cup.isPlaying && !cheer.isPlaying && !ballInHole
				&& (objectInHole == null || !objectInHole.name.Equals(other.transform.gameObject.name))) {
			Debug.Log (objectInHole);
			objectInHole = other.transform.gameObject;
			Destroy (objectInHole); // TODO state change to idle; 
			ballInHole = true;
//			if (ballInHole) {
//				ballInHole = false;

				cup.Play ();
//			}
		}
	}

	void OnTriggerStay(Collider other) {
//		if (!cup.isPlaying && !cheer.isPlaying && objectInHole != null) {
		if (!cup.isPlaying && !cheer.isPlaying && ballInHole) {
			cheer.Play ();
			//			other.transform.GetComponent<Collider> ().enabled = false;
			//			other.transform.GetComponent<Renderer> ().enabled = false;
//			Destroy (objectInHole); // TODO state change to idle; 
//			objectInHole = null;
			ballInHole = false;
		}
	}
}
