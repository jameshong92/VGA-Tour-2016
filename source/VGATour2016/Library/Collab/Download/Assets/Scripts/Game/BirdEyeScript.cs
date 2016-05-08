using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class BirdEyeScript : MonoBehaviour {

	public GameObject birdEyeTarget;
	public GameObject puttingHolder;
	public GameObject receiver;

	private bool showPutting;

	void Start() {
		showPutting = !puttingHolder.transform.parent.name.Contains("Temp");	
	}

	// Update is called once per frame
	void Update () {
		if (GlobalData.state == GlobalData.State.BallMove) {
			if (this.transform.childCount > 0) {
				Destroy(this.transform.GetChild (0).gameObject);
			}
			GameObject currentHole = Instantiate(puttingHolder.transform.GetChild(0).gameObject);
			currentHole.transform.parent = this.transform;
			currentHole.transform.localPosition = Vector3.zero;
			currentHole.transform.localScale = Vector3.one * 0.5f;
			currentHole.transform.localEulerAngles = new Vector3 (0, 90, 0);
		}

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
