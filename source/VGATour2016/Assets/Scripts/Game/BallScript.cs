using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallScript : MonoBehaviour {
	static int numMoving = 0;
	Rigidbody rigidBody;
	Vector3 beforePuttPosition;
	Vector3 lastPosition;
	bool isMoving;
	bool stopping;
	float stoppingStartTime;
	public float buffer = 0.08f;

//	public bool inHole
	private float dropThresh = -1f;
	private GameObject uicontrol;
	private bool outofbound;
	Behaviour halo;


	// Use this for initialization
	void Start () {
		halo = (Behaviour) GetComponent("Halo"); 
		halo.enabled = false;
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
		Debug.Log (halo);
		rigidBody = GetComponent<Rigidbody> ();
		lastPosition = this.transform.position;
		isMoving = false;
		uicontrol = GameObject.Find ("UI_game view");
		outofbound = false;
	}

	public void StateToTurnBegin() {
		if (GlobalData.state == GlobalData.State.BallMove) {
			GlobalData.state = GlobalData.State.TurnBegin;
//			GlobalData.players [GlobalData.turn].shotCount += 1;

			Debug.Log (GlobalData.turn);

			GlobalData.NextTurn ();

			Debug.Log ("statetoturnbegin shot++");
			Debug.Log (GlobalData.turn);

		}
	}

	void BallOutOfBound() {
		this.transform.position = beforePuttPosition;
		rigidBody.Sleep ();
		outofbound = false;

	}

	// Update is called once per frame
	void Update () {
		if (this.transform.parent.parent.name.Contains ("BirdEye")) {
			return;
		}
		if (GlobalData.state == GlobalData.State.Idle) {
			halo.enabled = true;
		} else {
			halo.enabled = false;
		}

		if (GlobalData.state == GlobalData.State.BallMove) {
			if (!isMoving && (transform.position - lastPosition).magnitude > buffer) {
				beforePuttPosition = lastPosition;
				isMoving = true;
				stopping = false;
				numMoving = numMoving + 1;
			} else if (!GlobalData.goalin && isMoving && (transform.position - lastPosition).magnitude <= buffer && stopping && Time.time - stoppingStartTime > 1.5f) {
				Debug.Log ("Ball stopped");
				Debug.Log ((transform.position - lastPosition).magnitude);
				isMoving = false;
				rigidBody.Sleep ();
				numMoving = numMoving - 1;
				Debug.Log (GlobalData.playersLeft);
				Debug.Log (GlobalData.goalin);
				if (GlobalData.playersLeft != 0 && !GlobalData.goalin) {
					Debug.Log ("in if for g.playerleft != 0 and not goalin");
					StateToTurnBegin ();	
				}
			}
			if (!stopping && (transform.position - lastPosition).magnitude <= buffer) {
				stoppingStartTime = Time.time;
			}
			stopping = ((transform.position - lastPosition).magnitude <= buffer);
			if (transform.position.y < dropThresh && !outofbound) {
				outofbound = true;
				uicontrol.GetComponent<UIController>().hideMessage();
				uicontrol.GetComponent<UIController>().displayError("OutOfBounds");
				Invoke ("BallOutOfBound", 4);
//				BallOutOfBound();
			}
		}
		lastPosition = transform.position;
	}

	void OnCollisionEnter(Collision other) {
	}

}
