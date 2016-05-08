using UnityEngine;
using System.Collections.Generic;

public class ScoreController : MonoBehaviour {
	public static List<Player> players;

	// Use this for initialization
	void Start () {
//		players = new List<Player>();
//		Player player1 = new Player ("James");
//		player1.score = -2;
//		Player player2 = new Player ("Ephraim");
//		player2.score = -3;
//		Player player3 = new Player ("Harry");
//		player3.score = -3;
//		Player player4 = new Player ("Diana");
//		player4.score = 0;
//		Player player5 = new Player ("Min Woo");
//		player5.score = +2;
//
//		players.Add(player1);
//		players.Add(player2);
//		players.Add(player3);
//		players.Add(player4);
//		players.Add(player5);


		players.Sort((x, y) => x.score.CompareTo(y.score));
		Debug.Log (players[0].score);

		for (int i = 0; i < players.Count; i++) {
			int score = i + 1;
			if (i > 0 && players [i].score == players [i - 1].score) {
				score = i;
			}
			GetComponentsInChildren<TextMesh> () [i].text = string.Format (" {0,-7}{1,-12}{2,3}", score, players [i].name, players [i].score.ToString ("+#;-#;0"));
		}
	}
	
	// Update is called once per frame
	void Update () {
		// if hole changed

	}
}
