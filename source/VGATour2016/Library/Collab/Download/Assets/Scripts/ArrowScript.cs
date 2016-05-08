using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {

	public GameObject puttingHolder;
	private Renderer r;
	private Renderer[] rendererComponents;

	// Use this for initialization
	void Start () {
		rendererComponents = GetComponentsInChildren<Renderer>(true);

//		r = this.transform.GetChild (0).GetComponent<Renderer> ();
		foreach (Renderer r in rendererComponents)
			r.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		if (GlobalData.state == GlobalData.State.PuttSet || GlobalData.state == GlobalData.State.Putt) {
			Transform goal;
			Transform currentBall = FindBall();

			foreach (Transform t in puttingHolder.transform.GetChild(0)) {
				if (t.name.Equals ("Goal")) {
					goal = t.gameObject.transform;
					Vector3 direction = currentBall.position - goal.position;

					Vector3 crossProduct = Vector3.Cross(direction, Vector3.up);
//					Debug.Log (direction);
					Quaternion rotation = Quaternion.LookRotation(crossProduct);
					this.transform.rotation = rotation;
					this.transform.position = currentBall.position - direction.normalized * 2f;
					TextMesh text = this.transform.GetComponentInChildren<TextMesh> ();
					text.text = (direction.magnitude / 10.0f).ToString("F2") + " m";
					foreach (Renderer r in rendererComponents)
						r.enabled = true;
				}
			}
		} else {
			foreach (Renderer r in rendererComponents)
				r.enabled = false;
		}
	}

	Transform FindBall() {
		foreach (Transform child in puttingHolder.transform.GetChild(0).transform) {
			if (child.CompareTag (GlobalData.players [GlobalData.turn].ball)) {
				return child;
			}
		}
		return null;
	}

}
