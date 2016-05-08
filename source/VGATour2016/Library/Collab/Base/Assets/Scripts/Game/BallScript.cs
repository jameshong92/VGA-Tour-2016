using UnityEngine;
using System.Collections;

/// <summary>
/// TODO Once the image gets lost state comes to chaos....
/// </summary>

public class BallScript : MonoBehaviour {
	static int numMoving = 0;
//	float SlowDownPerSecond = 2f;
	Rigidbody rigidBody;
	Vector3 lastPosition;
	bool isMoving;
	static float buffer = 0.005f;

	// Use this for initialization
	void Start () {
//		Rigidbody rb = GetComponent<Rigidbody> ();
//		rb.freezeRotation = true;
		rigidBody = GetComponent<Rigidbody> ();
		lastPosition = this.transform.position;
		isMoving = false;
	}

	void StateToTurnBegin() {
		GlobalData.state = GlobalData.State.TurnBegin;
		GlobalData.players[GlobalData.turn].shotCount += 1;
		GlobalData.NextTurn();
	}

	// Update is called once per frame
	void Update () {
		if (this.transform.parent.parent.name.Contains ("BirdEye")) {
			return;
		}
		if (GlobalData.state == GlobalData.State.BallMove) {
//			Debug.Log ("Num Moving : " + numMoving);
			if (!isMoving && (transform.position - lastPosition).magnitude > buffer) {
				isMoving = true;
				numMoving = numMoving + 1;
			} else if (isMoving && (transform.position - lastPosition).magnitude <= buffer) {
				isMoving = false;
				rigidBody.Sleep ();
				numMoving = numMoving - 1;
				if (numMoving == 0) {
					StateToTurnBegin ();	
				}
			}
			lastPosition = transform.position;
		}
//		Debug.Log (rigidBody.angularVelocity.magnitude);
//		Vector3 acceleration = (rigidBody.velocity - lastVelocity) / Time.fixedDeltaTime;
//
//		if (acceleration.magnitude > 0.0001f && rigidBody.velocity.magnitude > 0.001f && gameObject.name.Equals (GlobalData.balls [GlobalData.turn].name)) {
//			if (cam)
//				cam.SetActive (true);
//		} else {
//			if (cam)
//				cam.SetActive (false);
//			rigidBody.velocity = Vector3.zero;
//			rigidBody.angularVelocity = Vector3.zero;
//		}
		
//		rb.freezeRotation = true;
//		rb.AddTorque(rb.angularVelocity * -.05f * Time.deltaTime, ForceMode.Impulse);

		// add friction effect to stop the ball's rotation eventually
//		if(rigidBody.angularVelocity.magnitude > SlowDownPerSecond * Time.deltaTime){
//			rigidBody.AddTorque(-rigidBody.angularVelocity.normalized * SlowDownPerSecond, ForceMode.Force);
//		} else {//less than change per frame
//			rigidBody.angularVelocity = Vector3.zero;
//		}

//		lastVelocity = rigidBody.velocity;
	}

	void OnCollisionEnter(Collision other) {
//		Debug.Log ("collided");
//		Debug.Log (other.impulse);

		// add force based on impulse and its mass
//
//		if (other.gameObject.name.Equals("Cube") && !gameObject.name.Equals(GlobalData.balls[GlobalData.turn].name))
//			Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other.gameObject.GetComponent<Collider>());
//		else {
//  			Debug.Log (other.relativeVelocity);
//			Rigidbody rb = other.gameObject.GetComponent<Rigidbody> ();
//			if (rb) {
//				float force = other.impulse.magnitude;
//				if (force == 0.0f)
//					force = 1.0f;
//				ContactPoint cp = other.contacts[0];
//				rigidBody.AddForce (cp.normal * force * rb.mass);
//			}
//			else {
//				rigidBody.AddForce(other.impulse * rigidBody.mass * 30);
//			}
//		}
	}

}
