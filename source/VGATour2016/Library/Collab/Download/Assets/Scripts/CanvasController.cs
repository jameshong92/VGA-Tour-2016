using UnityEngine;
using System.Collections;

public class CanvasController : MonoBehaviour {

	GlobalData.State currentState;
	public Canvas idleCanvas;
	public Canvas puttSetCanvas;
	public Canvas puttCanvas;
	public Canvas ballMoveCanvas;

	// Use this for initialization
	void Start () {
		currentState = GlobalData.state;
		OnStateChange();
	}

	void OnStateChange() {
		currentState = GlobalData.state;
		Debug.Log ("OnStateChange");
		Debug.Log (currentState);
		idleCanvas.enabled = false;
		puttSetCanvas.enabled = false;
		puttCanvas.enabled = false;
		ballMoveCanvas.enabled = false;
		if (currentState == GlobalData.State.Idle) {
			idleCanvas.enabled = true;
		} else if (currentState == GlobalData.State.PuttSet) {
			puttSetCanvas.enabled = true;
		} else if (currentState == GlobalData.State.Putt) {
			puttCanvas.enabled = true;
		} else if (currentState == GlobalData.State.BallMove) {
			ballMoveCanvas.enabled = true;
		}
	}

	// Update is called once per frame
	void Update () {
		if (currentState != GlobalData.state) {
			OnStateChange();
		}
	}
}
