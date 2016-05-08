using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {
	private NetworkView nView;

	public GameObject message;
	// Acceleration Input Variables
	private Vector3 accInput;

	// Touch Detection Variables
	private float touchInput;
	float startTime;
	Vector2 startPos;
	bool couldBeSwipe;
	float comfortZone = 1500f;
	float holdBufferDist = 50f;
	float minHorizSwipeDist = 200f;
	float minVertSwipeDist = 100f;
	float maxSwipeTime = 0.5f;
	float minHoldTime = 0.2f;

	public static float lastActionTime;

	// Hold Variables
	bool onHold = false;

	[RPC]
	void SendVibration(bool trigger) {
		if (trigger) {
			Handheld.Vibrate ();
		}
	}

	void SendAccInput() {
		nView.RPC ("SetAccInput", RPCMode.Server, Input.acceleration);
	}

	[RPC]
	void SetAccInput(Vector3 input) {
		accInput = input;
	}

	void SendHoldInput(bool onHold) {
		message.GetComponent<Text> ().text = "On Hold...";
		lastActionTime = Time.time;
		if (!onHold) {
			message.GetComponent<Text> ().text = "";
		}
		nView.RPC("SetHoldInput", RPCMode.Server, onHold);
	}

	[RPC]
	void SetHoldInput(bool input) {
		onHold = input;
	}

	void SendTouchInput(int touchInput) {
		if (touchInput == 0) {
			message.GetComponent<Text> ().text = "Tapped";
			lastActionTime = Time.time;
		} else if (touchInput == 1) {
			message.GetComponent<Text> ().text = "Swiped Up";
			lastActionTime = Time.time;
		} else if (touchInput == 2) {
			message.GetComponent<Text> ().text = "Swiped Right";
			lastActionTime = Time.time;
		} else if (touchInput == 3) {
			message.GetComponent<Text> ().text = "Swiped Down";
			lastActionTime = Time.time;
		} else if (touchInput == 4) {
			message.GetComponent<Text> ().text = "Swiped Left";
			lastActionTime = Time.time;
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
			if (lastActionTime > 0 && Time.time - lastActionTime >= 2) {
				message.GetComponent<Text> ().text = "Waiting on actions...";
			};
			// Acceleration Input
			SendAccInput ();
			var touchTime = Time.time - startTime;
			// Hold Detection
			if (onHold && touchTime > minHoldTime) {
				SendHoldInput (true);
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
					if (onHold && touchTime > minHoldTime) {
						SendHoldInput (false);
					} else if (swipeDist == 0) {
						SendTouchInput (0);
					} else if (couldBeSwipe && (touchTime < maxSwipeTime) && (Mathf.Abs(horizSwipeDist) > minHorizSwipeDist)) {
						var horizSwipeDirection = Mathf.Sign (touch.position.x - startPos.x);
						if (horizSwipeDirection > 0) {
							SendTouchInput (2);
						} else {
							SendTouchInput (4);
						}
					} else if (couldBeSwipe && (touchTime < maxSwipeTime) && (Mathf.Abs(vertSwipeDist) > minVertSwipeDist)) {
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
