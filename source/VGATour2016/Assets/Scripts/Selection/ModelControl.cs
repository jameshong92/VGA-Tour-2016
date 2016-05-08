using UnityEngine;
using System.Collections;

public class ModelControl : MonoBehaviour {

	public Behaviour halo;
//	public GameObject text;

	private Vector3 pos;

	// Use this for initialization
	void Start () {
		halo.enabled = false;
//		text.SetActive (false);
	}

	// Update is called once per frame
	void Update () {

	}

	void selectObj() {
		pos = transform.position;
//		text.transform.position = pos + Vector3.up;
		halo.enabled = true;
//		text.SetActive (true);

	}

	void deselectObj() {
		halo.enabled = false;
//		text.SetActive (false);
	}

	void reset() {
		transform.position = new Vector3 (0, 0, 0);
		Debug.Log ("hi");
	}

}
