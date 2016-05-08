using UnityEngine;
using System.Collections;
using Vuforia;

public class ReceiverScript : MonoBehaviour {

	public GameObject sideView;
	public GameObject travel;
	public GameObject puttingHolder;
	public GameObject clubHolder;

	private bool sideViewRotationDetector;
	private NetworkView nView;

	// Use this for initialization
	void Start () {
		sideViewRotationDetector = false;
		nView = GetComponent<NetworkView> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void changeSideView(int newSideView) {
		sideView.GetComponent<VGADefaultTrackableEventHandler> ().OnTrackableStateChanged (
			TrackableBehaviour.Status.TRACKED, 
			TrackableBehaviour.Status.NOT_FOUND);

		GlobalData.currentSideView = newSideView;
		sideView.name = GlobalData.sideViews [GlobalData.currentSideView];

		sideView.GetComponent<VGADefaultTrackableEventHandler> ().OnTrackableStateChanged (
			TrackableBehaviour.Status.NOT_FOUND, 
			TrackableBehaviour.Status.TRACKED);
	}

	[RPC]
	void SetAccInput(Vector3 input) {
		if (sideView.transform.childCount > 0) { // side view begin tracked
			if (GlobalData.state == GlobalData.State.Idle) {
				// Birdeye being tracked
				int direction = 1;
				if (input.x < 0) {
					direction = -1;
				}
				float rotation = input.x * direction;
				if (rotation > 0.7f && sideViewRotationDetector == false) {
					sideViewRotationDetector = true;
				}
				if (rotation < 0.1f && sideViewRotationDetector == true) {
					sideViewRotationDetector = false;
					int newSideView = GlobalData.currentSideView + direction;
					if (newSideView == -1) {
						newSideView = GlobalData.sideViews.Length - 1;
					} else if (newSideView == GlobalData.sideViews.Length) {
						newSideView = 0;
					}
					changeSideView(newSideView);
				}
			}
		} 
		if (puttingHolder.transform.parent.name.Equals ("Putting")) {
			// if putting view is being tracked
			if (GlobalData.state == GlobalData.State.Putt) {
				clubHolder.GetComponent<GolfClubScript> ().SetAccInput (input);
			}
		}
	}

	[RPC]
	void SetHoldInput(bool input) {
		if (puttingHolder.transform.parent.name.Equals ("Putting")) {
			// if putting view is being tracked
			if (GlobalData.state == GlobalData.State.Idle) {
				travel.GetComponent<TravelScript> ().SetHoldInput (input);
			} else if (GlobalData.state == GlobalData.State.Putt) {
				clubHolder.GetComponent<GolfClubScript> ().SetHoldInput (input);
			}
		}
	}

	[RPC]
	void SetTouchInput(int input) {
		if (puttingHolder.transform.parent.name.Equals ("Putting")) {
			// if putting view is being tracked
			if (input == 0) {
				// tap
				if (GlobalData.state == GlobalData.State.TurnBegin) {
					GlobalData.state = GlobalData.State.Idle;
				} else if (GlobalData.state == GlobalData.State.Idle) {
					if (travel.GetComponent<TravelScript>().IsInsideHalo()) {
						changeSideView (0); // 0 is BirdEye view; set side view back to birdeye
						puttingHolder.GetComponent<PuttingScript> ().StateToPuttSet ();
						Debug.Log("moving to putt set state");
					} else {
						Debug.Log("move closer to ball!!");
					}
				} else if (GlobalData.state == GlobalData.State.PuttSet) {
					clubHolder.GetComponent<GolfClubScript> ().StateToPutt ();
				}
			} else if (input == 4) {
				if (GlobalData.state == GlobalData.State.PuttSet) {
					puttingHolder.GetComponent<PuttingScript> ().StateBackToIdle ();
				} else if (GlobalData.state == GlobalData.State.Putt) {
					clubHolder.GetComponent<GolfClubScript> ().StateBackToPuttSet ();
				}
			}
		}
	}

	[RPC]
	void SendVibration(bool trigger) {
	}

	public void Vibrate() {
		nView.RPC ("SendVibration", RPCMode.All, true);
	}
}
