using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AddPlayerOnClick : MonoBehaviour {

	public GameObject playersInput;
	public Button addPlayer;

	public void AddPlayer() 
	{
		InputField lastPlayer = playersInput.transform.GetChild (playersInput.transform.childCount - 1).GetComponent<InputField> ();
		InputField newPlayer = Instantiate(lastPlayer);
		newPlayer.text = "";
		newPlayer.transform.parent = playersInput.transform;
		newPlayer.transform.localScale = lastPlayer.transform.localScale;
		newPlayer.transform.localPosition = lastPlayer.transform.localPosition;
		newPlayer.transform.localPosition += Vector3.down * 40;
		addPlayer.transform.localPosition = newPlayer.transform.localPosition + Vector3.down * 40;
		Debug.Log(lastPlayer.text);
	}
}
