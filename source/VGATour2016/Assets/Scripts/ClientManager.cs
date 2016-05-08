using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour {
	private string connectionIP = "";
	private int connectionPort = 25001;
	public GameObject message;
	public GameObject inputField;
	public GameObject button;
	private bool attempted;

	public void ConnectToServer() {
		if (Network.peerType == NetworkPeerType.Disconnected) {
			connectionIP = inputField.GetComponentInChildren<Text> ().text;
			Debug.Log (connectionIP);
			Network.Connect (connectionIP, connectionPort);
			attempted = true;
		}
	}

	void OnConnectedToServer() {
		Debug.Log ("Client Connected");
		message.GetComponent<Text> ().text = "Connection Successful.";
		inputField.SetActive (false);
		button.SetActive (false);
		attempted = false;
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		message.GetComponent<Text>().text = "Connection Closed.\nPlay Again!";
		button.GetComponentInChildren<Text> ().text = "Connect to Server.";
		inputField.SetActive (true);
		button.SetActive (true);
	}

	void OnFailedToConnect(NetworkConnectionError error) {
		Handheld.Vibrate ();
		Handheld.Vibrate ();
		message.GetComponent<Text>().text = "Connection Failed.\nTry Again!";
		attempted = false;
	}

	void Update() {
		if (attempted) {
			message.GetComponent<Text>().text = "Now Connecting...";
		}
	}
}