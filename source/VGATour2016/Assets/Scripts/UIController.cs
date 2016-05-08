using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GameObject HoleView;
	public GameObject TurnView;
	public GameObject State_idle;
	public GameObject State_set;
	public GameObject State_putt;
	public GameObject Help;
	public GameObject scoreboard;
	public GameObject birdseyeView;
	public GameObject gameOverView;
	public GameObject Messages;


//	public GameObject travel;
	public GameObject sideView;

	private Text holeText;
	private Text turnText;
	private Text Winner;

	private GameObject Errors;
	private bool Errordisplayed;
	private GlobalData.State prevState = GlobalData.State.Idle;
	private int prevSideView;


	// Use this for initialization
	void Start () {
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).gameObject.SetActive (false);
		} 
//		GlobalData.players.Add (new Player ("Diana"));
//		GlobalData.turn = 1;
//		GlobalData.course = 2;
//		GlobalData.players [0].score = 2;
//		GlobalData.players [0].shotCount = 5;
		Debug.Log (GlobalData.state);
		Errors = Help.transform.GetChild (3).gameObject;
		prevSideView = -1;
		Errordisplayed = false;

		scoreboard.SetActive (false);
		TurnOffAllUI (birdseyeView);
		hideMessage ();
		UpdateUI ();
	}
	
	// Update is called once per frame
	void Update () {

		if (!GlobalData.holeStart && prevState == GlobalData.State.TurnBegin) {
			UpdateUI();
		}

		if (!GlobalData.goalin && prevState == GlobalData.State.HoleTransition) {
			UpdateUI ();
		}
//
//		if (prevState != GlobalData.state) {
//			prevState = GlobalData.state;
//			UpdateUI ();
//		}

		UpdateUI ();

		if (sideView.transform.childCount > 0) {
			if (prevSideView != GlobalData.currentSideView) {
				prevSideView = GlobalData.currentSideView;
				UpdateBirdsEye ();
			}
		}
		if (sideView.transform.childCount == 0) {
			TurnOffAllUI (birdseyeView);
		}

	}

	public void UpdateUI() {

		if (GlobalData.state == GlobalData.State.Idle) {
			TurnOffAllUI (transform.gameObject);
			SetPlayerStatus (State_idle.transform.GetChild (2).gameObject);
			State_idle.SetActive (true);
			return;
		}
		if (GlobalData.state == GlobalData.State.Putt) {
			TurnOffAllUI (transform.gameObject);
			SetPlayerStatus (State_putt.transform.GetChild (2).gameObject);
			State_putt.SetActive (true);
			return;
		}
		if (GlobalData.state == GlobalData.State.PuttSet) {
			TurnOffAllUI (transform.gameObject);
			SetPlayerStatus (State_set.transform.GetChild (2).gameObject);
			State_set.SetActive (true);
			return;
		}
		if (GlobalData.state == GlobalData.State.TurnBegin) {
			//ADDED BY DIANA
			scoreboard.SetActive (false);
			if (GlobalData.holeStart) {
				displayHoleView ();
			} else {
				TurnOffAllUI (transform.gameObject);
				TurnOffAllUI (Messages);
				TurnView.transform.GetChild (2).GetComponent<Text> ().text = GlobalData.players [GlobalData.turn].name
				+ "'s Turn\nShot #" + (GlobalData.players [GlobalData.turn].shotCount + 1);
				TurnView.SetActive (true);
				return;
			}
		}

		if (GlobalData.state == GlobalData.State.BallMove && !GlobalData.goalin) {
			Debug.Log ("ballmove");
			TurnOffAllUI (transform.gameObject);
			if (!Errordisplayed)
				displayMessage ("ballMoving");
		}

		if (GlobalData.state == GlobalData.State.HoleTransition) {
			if (!GlobalData.goalin && !GlobalData.gameover) {
				TurnOffAllUI (transform.gameObject);
				scoreboard.SetActive (true);
			}
			if (GlobalData.gameover) {
				gameOverView.SetActive (true);
			}
		}

	}

	public void UpdateBirdsEye() {
		TurnOffAllUI (birdseyeView);
		birdseyeView.transform.GetChild (GlobalData.currentSideView).gameObject.SetActive (true);
	}

	void TurnOffAllUI(GameObject g) {
		for (int i = 0; i < g.transform.childCount; i++) {
			g.transform.GetChild (i).gameObject.SetActive (false);
		}
	}

	void SetPlayerStatus(GameObject playerStatus) {
		// 0-name 1-score 2-hole 3-par 4-shot
		Player player = GlobalData.players[GlobalData.turn];
		playerStatus.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = player.name;
		playerStatus.transform.GetChild (1).GetChild (0).GetComponent<Text> ().text = player.score.ToString ();
		playerStatus.transform.GetChild (2).GetChild (0).GetComponent<Text> ().text = "Hole\n" + GlobalData.hole.ToString ();
		playerStatus.transform.GetChild (3).GetChild (0).GetComponent<Text> ().text = "Par\n"
		+ GlobalData.par [GlobalData.course - 1, GlobalData.hole - 1].ToString ();
		playerStatus.transform.GetChild (4).GetChild (0).GetComponent<Text> ().text = "Shot\n" + (player.shotCount+1).ToString ();
			
	}

	void displayHoleView() {
		TurnOffAllUI (transform.gameObject);
		HoleView.transform.GetChild (2).GetComponent<Text> ().text = "Hole #" + GlobalData.hole.ToString () + "\nPar " +
		GlobalData.par [GlobalData.course - 1, GlobalData.hole - 1].ToString ();
		HoleView.SetActive (true);
	}

	public void displayhelp() {
		Help.SetActive (true); Errors.SetActive (true);
		if (GlobalData.state == GlobalData.State.Idle) {
			Help.transform.GetChild (0).gameObject.SetActive (true);
		}

		if (GlobalData.state == GlobalData.State.PuttSet) {
			Help.transform.GetChild (1).gameObject.SetActive (true);
		}

		if (GlobalData.state == GlobalData.State.Putt) {
			Help.transform.GetChild (2).gameObject.SetActive (true);
		}
	}

	public void hidehelp() {
		for (int i = 0; i < Help.transform.childCount; i++){
			Help.transform.GetChild(i).gameObject.SetActive (false);
		}
	}

	public void displayError(string whichError) {
		Errordisplayed = true;
		Debug.Log ("error");
		Help.SetActive (true);
		Errors.SetActive (true);
		// if state is idle -- i.e. person tried to puttset too far away
		if (whichError == "TooFarFromBall") {
			Errors.transform.GetChild (0).gameObject.SetActive (true);
			Invoke ("hideError", 2);
		}

		// if ball out of bounds
		if (whichError == "OutOfBounds") {
			Errors.transform.GetChild (1).gameObject.SetActive (true);
			Invoke ("hideError", 3);
		}

	}

	public void hideError() {
		Errordisplayed = false;
		for (int i = 0; i < Errors.transform.childCount; i++)
			Errors.transform.GetChild (i).gameObject.SetActive (false);
	}

	public void displayMessage(string whichMessage) {
		Messages.SetActive (true);

		Debug.Log ("message");
		if (whichMessage == "ballChanged") {
			Messages.transform.GetChild (0).gameObject.SetActive (true);
			Invoke ("hideMessage", 2);
		}

		if (whichMessage == "clubChanged") {
			Messages.transform.GetChild (1).gameObject.SetActive (true);
			Invoke ("hideMessage", 2);
		}

		if (whichMessage == "ballMoving") {
			Messages.transform.GetChild (2).gameObject.SetActive (true);
//			Invoke ("hideMessage", 2);
		}

		if (whichMessage == "goalIN") {
			Messages.transform.GetChild (3).gameObject.SetActive (true);
			Invoke ("hideMessage", 3);
		}

	}

	public void hideMessage() {
		for (int i = 0; i < Messages.transform.childCount; i++) 
			Messages.transform.GetChild(i).gameObject.SetActive(false);
//		GlobalData.goalin = false;
	}

	public void GameOver() {
		GlobalData.gameover = true;
		GlobalData.players.Sort((x, y) => x.score.CompareTo(y.score));
		Winner.text = "The Winner is " + GlobalData.players [0].name + "!!";
		gameOverView.SetActive (true);
	}
}
