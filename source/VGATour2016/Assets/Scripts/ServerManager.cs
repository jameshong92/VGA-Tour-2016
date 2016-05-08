using UnityEngine;
using System.Net;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class ServerManager : MonoBehaviour {

	private static int connectionPort = 25001;
	static bool useNat = false;
	public Text input;
	private static string ip = "";
	private static string url = "http://minwookim.co/ip";

	public static string GetIP() {
		Network.InitializeServer(5, connectionPort, useNat);
		string external = new WebClient().DownloadString("http://icanhazip.com"); 
		ip = external;
		return external;
	}

	public void SetKey() {
		string key = input.text;
		Debug.Log (key);
		key = key.ToUpper ();
		if (key.Length == 3 && key.All (char.IsLetter)) {
			WWWForm form = new WWWForm ();
			form.AddField ("message[first_name]", "3D");
			form.AddField ("message[last_name]", key);
			form.AddField ("message[email]", "3D@U.I");
			form.AddField ("message[content]", ip);
			WWW www = new WWW (url, form);
			Debug.Log (www);
			Post (www);
		} else {
			input.text = "Invalid Input!";
		}
	}

	IEnumerator Post(WWW www) {
		Debug.Log ("Start");
		yield return www;
		Debug.Log ("end");
		Debug.Log (www.text);
	}
}