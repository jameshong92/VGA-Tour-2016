using UnityEngine;
using System.Collections.Generic;

public class Test : MonoBehaviour {

	public void ButtonClicked() 
	{
		if (GlobalData.players == null) {
			return;
		}
		for (int i = 0; i < GlobalData.players.Count; i++) {
			Debug.Log (GlobalData.players[i].name);
		}
	}
}
