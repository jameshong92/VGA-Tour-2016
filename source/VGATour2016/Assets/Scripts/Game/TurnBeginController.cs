using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnBeginController : MonoBehaviour {

	public GameObject text;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// temporary, -- test display with one script
//		if (GlobalData.state == GlobalData.State.TurnBegin) {
//			this.GetComponent<Canvas> ().enabled = true;
//			text.GetComponent<Text> ().text = GlobalData.players [GlobalData.turn].name + "'s shot #" +
//			(GlobalData.players [GlobalData.turn].shotCount + 1) + "\n";
//			text.GetComponent<Text> ().text += "Tap to begin your turn";
//		} else {
//			this.GetComponent<Canvas> ().enabled = false;
//		}
	}
}
