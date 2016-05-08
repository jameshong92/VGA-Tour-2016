using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class HomeControl : MonoBehaviour {

	public GameObject courses;
	public Text singlePlayer;
	public Text multiPlayer;

	public GameObject StartPage;
	public GameObject Single_Multi;
	public GameObject InputName;
	public GameObject InputNames;
	public GameObject Edit_Name;
	public GameObject Courses;
	public GameObject Error_0p;
	public GameObject Error_1p_multi;
	public GameObject Error_noname;
	public GameObject Error_duplicate;
	public GameObject Error_limit;
	public GameObject Error_max;
	public GameObject Error_course;
	public GameObject Message_single;
	public GameObject Message_multi;
	public GameObject ShowIP;
	public GameObject MovingOn;

	private bool single;
	private Color green;
	private Color turquoise;
	private int coursenum;
	private bool connected = false;
	private bool ipShown = false;

	private string ip;

	// Use this for initialization
	void Start () {
		Debug.Log ("start");
		single = true;
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).gameObject.SetActive (false);	
		}
		StartPage.SetActive (true);

		turquoise = transform.GetChild (0).GetChild (1).gameObject.GetComponent<Image> ().color;
		green = transform.GetChild (1).GetChild (3).gameObject.GetComponent<Image> ().color;

		if (GlobalData.yes) {
			Debug.Log (GlobalData.players [0].g_ball);
		}

	//	Screen.orientation = ScreenOrientation.Portrait;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// from Home screen-- tap anywhere to continue to single/multi player
	public void HomeToNext() {
		StartPage.SetActive (false);
		Single_Multi.SetActive (true);
		ip = ServerManager.GetIP ();
	}

	// Single player selected-- add name next
	public void SingleToNext() {
		single = true;
		Single_Multi.SetActive (false);
		InputName.SetActive (true);
	}

	// Multi player selected-- add names next
	public void MultiToNext() {
		single = false;
		Single_Multi.SetActive (false);
		InputNames.SetActive (true);
		InputNames.transform.GetChild(7).GetComponent<Text>().text = 
			GlobalData.players.Count + " player(s) entered";
	}

	// Names added-- select Course next
	public void NameToNext() {
		if (GlobalData.players.Count == 0) {
			Error_0p.SetActive (true);
		} else if (!single && GlobalData.players.Count < 2) {
			Error_1p_multi.SetActive (true);
		} else {
			InputName.SetActive (false);
			InputNames.SetActive (false);
			Courses.SetActive (true);
			Message_single.SetActive (false);
		}
	}

	// Course Selected-- Show IP next
	public void CourseToNext() {
		if (coursenum == 0) {
			Error_course.SetActive (true);
			return;
		}
		Courses.SetActive (false);
		ShowIP.SetActive (true);
		GlobalData.course = coursenum;
		ShowIP.transform.GetChild (5).GetComponent<Text> ().text = ip;
		ipShown = true;
	}

	// Ip shown-- move on
	public void IPToNext() {
		if (connected && ipShown) {
			ShowIP.SetActive (false);
			MovingOn.SetActive (true);
		}
	}

	// Add player to players
	// Called on inputname/ inputnames 
	public void AddPlayer() {
		InputField name;
		if (single) {
			name = InputName.transform.GetChild (4).GetComponent<InputField> ();
			if (GlobalData.players.Count == 0) {
				if (name.text.Length > 0) {
					// check if name is less than 10 AND not duplicate
					if (name.text.Length > 10) {
						Error_limit.SetActive (true);
						return;
					}
					GlobalData.players.Add (new Player (name.text));
					GlobalData.yes = true;
					singlePlayer.text = "Welcome " + name.text + "!";
					Message_single.SetActive (true);
				} else {
					Error_0p.SetActive (true);
				}
			}
			else {
				GlobalData.players [0].name = name.text;
				singlePlayer.text = "Welcome " + name.text + "!";
				Message_single.SetActive (true);
			}
		}
		else {
			name = InputNames.transform.GetChild (4).GetComponent<InputField> ();
			if (name.text.Length > 0) {
				// check if name is less than 10 AND not duplicate
				if (name.text.Length > 10) {
					Error_limit.SetActive (true);
					return;
				}
				if (GlobalData.players.Count == 5) {
					Error_max.SetActive (true);
					return;
				}
				if (GlobalData.players.Exists(Player => Player.name == name.text)) {
					Error_duplicate.SetActive (true);
					return;
				}
				GlobalData.players.Add (new Player (name.text));
				GlobalData.yes = true;
				multiPlayer.text = "Welcome " + name.text + "!";
				Message_multi.SetActive (true);
				name.text = string.Empty;
			}
			else {
				Error_noname.SetActive (true);
			}
		}
	}

	//show screen to edit name, accessible from multiple player input screen
	//inputfield text is set to the last name entered
	public void EditName() {
		InputName.SetActive (false);
		InputNames.SetActive (false);
		Message_multi.SetActive (false);
		InputField input = Edit_Name.transform.GetChild (4).GetComponent<InputField> ();
		input.text = GlobalData.players [GlobalData.players.Count - 1].name;
		Edit_Name.SetActive (true);
	}

	//change name of last entered person to new name
	public void UpdateName() {
		InputField name = Edit_Name.transform.GetChild (4).GetComponent<InputField> ();
		if (name.text.Length > 0) {
			GlobalData.players [GlobalData.players.Count - 1].name = name.text;
			multiPlayer.text = "Changed name to " + name.text;
			name.text = string.Empty;
			Message_multi.SetActive (true);
		}
		else {
			Error_noname.SetActive (true);
		}
	}

	//leave edit name screen, proceed to add next user
	public void addNextPlayer() {
		Message_multi.SetActive (false);
		InputNames.SetActive (true);
		InputNames.transform.GetChild(7).GetComponent<Text>().text =
			GlobalData.players.Count + " player(s) entered";
	}

	// remove error messages--simply go back to the previous screen
	public void RemoveError() {
		Error_0p.SetActive (false);
		Error_1p_multi.SetActive (false);
		Error_noname.SetActive (false);
		Error_duplicate.SetActive (false);
		Error_limit.SetActive (false);
		Error_max.SetActive (false);
		Error_course.SetActive (false);
	}

	// back from names to single_multi select view
	public void BackToSM () {
		InputName.SetActive (false);
		InputNames.SetActive (false);
		Single_Multi.SetActive (true);
		if (GlobalData.players.Count != 0) {
			GlobalData.players.Clear ();
		}
		InputName.transform.GetChild (4).GetComponent<InputField> ().text = string.Empty;
	}

	// back from courses to name enter
	public void BackToName() {
		Courses.SetActive (false);
		if (single) {
			InputName.SetActive (true);
		} else {
			InputNames.SetActive (true);
			InputNames.transform.GetChild (7).GetComponent<Text>().text = 
				GlobalData.players.Count + " player(s) entered";
		}
	}

	// back from IP to showing courses
	public void BackToCourses() {
		ShowIP.SetActive (false);
		Courses.SetActive (true);
	}

	// back from movingOn to show IP
	public void BackToIP() {
		MovingOn.SetActive (false);
		ShowIP.SetActive (true);
	}

	//change color of selected course
	public void HighlightCourse() {
		string course = EventSystem.current.currentSelectedGameObject.name;
		coursenum = int.Parse (course);
		Debug.Log (coursenum);
		for (int i = 0; i < courses.transform.childCount; i++) {
			courses.transform.GetChild (i).GetComponent<Image> ().color = green;
		}
		courses.transform.GetChild(coursenum - 1).GetComponent<Image> ().color = turquoise;
	}

	public void LoadSelectionScene() {

		for (int i = 0; i < GlobalData.players.Count; i++){
			DontDestroyOnLoad (GlobalData.players [i].g_ball);
			DontDestroyOnLoad (GlobalData.players [i].g_club);
		}
		Application.LoadLevel ("Selection");
	}

	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log ("connect");
		connected = true;
		IPToNext ();
	}

}
