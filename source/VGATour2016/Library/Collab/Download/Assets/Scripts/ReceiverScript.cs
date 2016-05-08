using UnityEngine;
using System.Collections;

public class ReceiverScript : MonoBehaviour {

	public GameObject travel;
	public GameObject puttingHolder;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RPC]
	void SetAccInput(Vector3 input) {
		if (GlobalData.state == GlobalData.State.Putt) {
			GlobalData.currentClub.GetComponent<GolfClubScript> ().SetAccInput (input);
		}
	}

	[RPC]
	void SetHoldInput(bool input) {
		Debug.Log (input);
		if (GlobalData.state == GlobalData.State.Idle) {
			travel.GetComponent<TravelScript> ().SetHoldInput (input);
		} else if (GlobalData.state == GlobalData.State.Putt) {
			GlobalData.currentClub.GetComponent<GolfClubScript> ().SetHoldInput (input);
		}
	}

	[RPC]
	void SetTouchInput(float input) {
		if (input == 0f) {
			// tap
			if (GlobalData.state == GlobalData.State.Idle) {
				puttingHolder.GetComponent<PuttingScript>().StateToPuttSet ();
			} else if (GlobalData.state == GlobalData.State.PuttSet) {
				GlobalData.currentClub.GetComponent<GolfClubScript>().StateToPutt();
			}
		}
//		touchInput = input;
	}
}
