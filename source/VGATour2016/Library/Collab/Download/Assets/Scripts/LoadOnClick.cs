using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadOnClick : MonoBehaviour {

	public GameObject playersInput;

	public void LoadScene(int level) 
	{
		GlobalData.players = new List<Player>();
		for (int i = 1; i < playersInput.transform.childCount; i++) {
			InputField playerInput = playersInput.transform.GetChild (i).GetComponent<InputField>();
			if (playerInput.text.Length > 0) {
				GlobalData.players.Add (new Player(playerInput.text));
			}
		}
		Application.LoadLevel(level);	
	}
}
