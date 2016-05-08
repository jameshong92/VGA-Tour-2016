using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

public class ClientManager : MonoBehaviour {
	private string connectionIP = "";
	private int connectionPort = 25001;
	public GameObject message;
	public GameObject inputField;
	public GameObject button;
	private bool attempted;
	private string url = "http://minwookim.co/ip?key=";

	public void ConnectToServer() {
		if (Network.peerType == NetworkPeerType.Disconnected) {
			connectionIP = inputField.GetComponentInChildren<Text> ().text;
			if (connectionIP.Replace (".", "").All (char.IsDigit) && connectionIP.Length > 0) {
				Network.Connect (connectionIP, connectionPort);
				attempted = true;
			} else if (Regex.IsMatch (connectionIP.ToUpper (), @"^[A-Z]+$")) {
				string get_url = url + connectionIP.ToUpper();
				WWW www = new WWW (get_url);
				StartCoroutine (Get (www));
			} else {
				message.GetComponent<Text> ().text = "Invalid IP Address or Key.\nTry Again!";
			}
		}
	}

	IEnumerator Get(WWW www) {
		yield return www;
		connectionIP = www.text;
		Network.Connect (connectionIP, connectionPort);
		Debug.Log (connectionIP);
		attempted = true;
	}
		
	void OnConnectedToServer() {
		Debug.Log ("Client Connected");
		message.GetComponent<Text> ().text = "Connection Successful";
		inputField.SetActive (false);
		button.SetActive (false);
		attempted = false;
		InputManager.lastActionTime = Time.time;
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		message.GetComponent<Text>().text = "Connection Closed.\nReconnect.";
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