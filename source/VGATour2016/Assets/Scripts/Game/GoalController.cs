using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour {

	public AudioSource cheer;
	public AudioSource unimpressed;
	public AudioSource cup;
	public GameObject puttingHolder;

	private GameObject uicontrol;

//	private bool goalIn;
	float goalInTime;

	// Use this for initialization
	void Start () {
		Debug.Log ("goal initialized");
		uicontrol = GameObject.Find ("UI_game view");
		GlobalData.goalin = false;
	}

	// Update is called once per frame
	void Update () {
		if (GlobalData.goalin && Time.time - goalInTime > 3f) {
			GlobalData.players[GlobalData.turn].g_ball.GetComponent<BallScript> ().StateToTurnBegin ();
			GlobalData.goalin  = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (!cup.isPlaying && !cheer.isPlaying) {
			GameObject objectInHole = other.transform.gameObject;
			foreach (Player p in GlobalData.players) {
				if (p.g_ball.name.Equals (objectInHole.name)) {
					if (GlobalData.goalin == false) {
						goalInTime = Time.time;
						GlobalData.goalin = true;
					}
					p.isFinished = true;

					GlobalData.playersLeft--;
					Debug.Log ("player done, left: " + GlobalData.playersLeft);
					uicontrol.GetComponent<UIController> ().hideMessage ();
					uicontrol.GetComponent<UIController> ().displayMessage ("goalIN");
					cup.Play ();
					cheer.PlayDelayed (1.8f);
					p.g_ball.GetComponent<Rigidbody> ().Sleep ();

					if (GlobalData.playersLeft == 0) {
						calculateScore ();
						GlobalData.turn = 0;
						GlobalData.hole++;
						GlobalData.holeStart = true;
						GlobalData.state = GlobalData.State.HoleTransition;
					} else {

					}
//					p.g_ball.SetActive (false);
				}
			}
		}
	}

	//ADDED BY DIANA
	void calculateScore() {
		int par = GlobalData.par [GlobalData.course - 1, GlobalData.hole - 1];
		foreach (Player p in GlobalData.players) {
			p.score += (p.shotCount - par + 1);
			p.isFinished = false;
		}
	}
}
