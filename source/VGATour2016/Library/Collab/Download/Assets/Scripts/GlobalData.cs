using System.Collections.Generic;
using UnityEngine;

public class GlobalData {

	public static List<Player> players;
	public static List<GameObject> balls;
	public static GameObject currentClub;
	public enum State { Idle, PuttSet, Putt, BallMove };
	public static State state = State.Idle;

	public static int turn;
	public static int course;
	public static int hole = 2;
	public static int[,] par = {{2, 3, 4, 5}, {4, 5, 0, 0}}; // par[course - 1][hole - 1]

	public static string connectionIP = "127.0.0.1";
	public static int connectionPort = 25001;

}
