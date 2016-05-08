using System.Collections.Generic;
using UnityEngine;

public class GlobalData {

	//if players list is not null, yes is true
	//for testing/debug purposes
	public static bool yes = false;
	public static string[] sideViews = {"BirdEye" /* Name of the scene where birdeye view is in */, "Scoreboard",  "BallChange", "ClubChange"};
	public static int currentSideView = 0; 

	public static List<Player> players = new List<Player>();
	public static List<GameObject> balls;
	//	public static GameObject currentClub;
	public enum State { TurnBegin, Idle, PuttSet, Putt, BallMove, HoleTransition };
	public static State state = State.TurnBegin;

	public static int turn;
	public static int course;
	public static int hole = 1;
	public static int[,] par = {{2, 3, 4, 5}, {4, 5, 0, 0}}; // par[course - 1][hole - 1]

	//ADDED BY DIANA
	//the number of unfinished people for hole
	public static int playersLeft;
	//the number of holes in this course
	public static int numHoles;
	//boolean whether holebegin state
	public static bool holeStart = true;
	//boolean goal in--to delay move on to next turn
	public static bool goalin = false;
	//boolean gameover
	public static bool gameover = false;

	public static void NextTurn() {
		if (GlobalData.turn >= 0) { 
			GlobalData.balls [GlobalData.turn].SetActive (false);
		}
		for (int i = 0; i < GlobalData.players.Count; i++) {
			int turn = (GlobalData.turn + i + 1) % GlobalData.players.Count;
			if (!GlobalData.players [turn].isFinished) {
				// activate the ball for the first hit 
				// from the second time, setting to true does not do anything because it is already true
				GlobalData.balls [turn].SetActive (true);
				Debug.Log ("setting ball to active");
				Debug.Log (GlobalData.balls [turn]);
				//				GameObject clubHolder = GameObject.Find ("ClubHolder");
				//				foreach (Transform t in clubHolder.transform) {
				//					Debug.Log ("in foreach!!!!!");
				//					Debug.Log (t.name);
				//					if (t.name.Equals (GlobalData.players [turn].name + "Club")) {
				//						t.GetComponent<Renderer> ().enabled = true;
				//						Debug.Log ("setting club to active");
				//						Debug.Log (t.name);
				//					} else if (t.name.Contains("Club")) {
				//						t.GetComponent<Renderer> ().enabled = false;
				//					}
				//				}
				// change the turn to the new user
				GlobalData.turn = turn;
				GlobalData.players [GlobalData.turn].shotCount += 1;
				return;
			}
		}
		// TODO check if all players are done
	}

	public static void Cleanup() {
		players.Clear ();
		balls.Clear ();
		currentSideView = 0;
		course = hole = 1;
		gameover = goalin = false;
		state = State.TurnBegin;
	}
}
