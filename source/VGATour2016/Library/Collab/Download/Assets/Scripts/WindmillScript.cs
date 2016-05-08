using UnityEngine;
using System.Collections;

public class WindmillScript : MonoBehaviour {

	public float speed = 50f;
	public bool clockwise = true;
	public GameObject windmill;
	public GameObject pivot;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		pivot.transform.RotateAround(pivot.transform.position, Vector3.left, speed);
	}
}
