using UnityEngine;

public class InputManager : MonoBehaviour {
	private NetworkView nView;

	// Acceleration Input Variables
	private Vector3 accInput;

	// Touch Detection Variables
	private float touchInput;
	float startTime;
	Vector2 startPos;
	bool couldBeSwipe;
	float comfortZone = 1500f;
	float holdBufferDist = 50f;
	float minHorizSwipeDist = 100f;
	float minVertSwipeDist = 150f;
	float maxSwipeTime = 0.5f;

	// Hold Variables
	bool onHold = false;

	void SendAccInput() {
		nView.RPC("SetAccInput", RPCMode.Server, Input.acceleration);
	}

	[RPC]
	void SetAccInput(Vector3 input) {
		accInput = input;
	}

	void SendHoldInput(bool onHold) {
		Debug.Log ("On Hold");
		if (!onHold) {
			Debug.Log ("Off Hold");
		}
		nView.RPC("SetHoldInput", RPCMode.Server, onHold);
	}

	[RPC]
	void SetHoldInput(bool input) {
		onHold = input;
	}

	void SendTouchInput(int touchInput) {
		Debug.Log (touchInput);
		if (touchInput == 0) {
			Debug.Log ("Tap");
		} else if (touchInput == 1) {
			Debug.Log ("Swipe Up");
		} else if (touchInput == 2) {
			Debug.Log ("Swipe Right");
		} else if (touchInput == 3) {
			Debug.Log ("Swipe Down");
		} else if (touchInput == 4) {
			Debug.Log ("Swipe Left");
		}
		nView.RPC("SetTouchInput", RPCMode.Server, touchInput);
	}

	[RPC]
	void SetTouchInput(int input) {
		touchInput = input;
	}
	
	void Start()
	{
		nView = GetComponent<NetworkView> ();
	}

	void Update()
	{
		if (Network.peerType == NetworkPeerType.Client) {
			var swipeTime = Time.time - startTime;
			// Acceleration Input
			SendAccInput ();
			// Hold Detection
			if (onHold && swipeTime > maxSwipeTime) {
				SendHoldInput (onHold);
			}
			// Swipe Detection
			if (Input.touchCount > 0) {
				var touch = Input.touches[0];
				switch (touch.phase) {

				case TouchPhase.Began:
					onHold = true;
					couldBeSwipe = true;
					startPos = touch.position;
					startTime = Time.time;
					break;

				case TouchPhase.Moved:
					if (Mathf.Abs (touch.position.x - startPos.x) > comfortZone &&
					    Mathf.Abs (touch.position.y - startPos.y) > comfortZone) {
						couldBeSwipe = false;
					}
					var swipeDist = (touch.position - startPos).magnitude;
					if (swipeDist > holdBufferDist) {
						onHold = false;
					}
					break;

				case TouchPhase.Ended:
					swipeDist = (touch.position - startPos).magnitude;
					var horizSwipeDist = (touch.position.x - startPos.x);
					var vertSwipeDist = (touch.position.y - startPos.y);
					if (onHold && swipeTime > maxSwipeTime) {
						SendHoldInput (false);
					} else if (swipeDist == 0) {
						SendTouchInput (0);
					} else if (couldBeSwipe && (swipeTime < maxSwipeTime) && (Mathf.Abs(horizSwipeDist) > minHorizSwipeDist)) {
						var horizSwipeDirection = Mathf.Sign (touch.position.x - startPos.x);
						if (horizSwipeDirection > 0) {
							SendTouchInput (2);
						} else {
							SendTouchInput (4);
						}
					} else if (couldBeSwipe && (swipeTime < maxSwipeTime) && (Mathf.Abs(vertSwipeDist) > minVertSwipeDist)) {
						var vertSwipeDirection = Mathf.Sign (touch.position.y - startPos.y);
						if (vertSwipeDirection > 0) {
							SendTouchInput (1);
						} else {
							SendTouchInput (3);
						}
					}
					onHold = false;
					break;
				}
			}
		}
	}
}
