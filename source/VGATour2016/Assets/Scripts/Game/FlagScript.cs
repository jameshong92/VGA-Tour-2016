using UnityEngine;
using System.Collections;

public class FlagScript : MonoBehaviour {

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
					Quaternion rotation = Quaternion.LookRotation(crossProduct);
					this.transform.rotation = rotation;
					this.transform.position = currentBall.position - direction.normalized * 17f;
					this.transform.localScale = Vector3.one * 2;
					float distanceToGoal = direction.magnitude / 10.0f;
					TextMesh text = this.transform.GetComponentInChildren<TextMesh> ();
					text.transform.localPosition = Vector3.zero;
					text.fontSize = 60;
					text.anchor = TextAnchor.UpperRight;
					text.color = Color.black;
					text.text = distanceToGoal.ToString("F2") + " m";
					if (distanceToGoal > 2.5f) {
						foreach (Renderer r in rendererComponents)
							r.enabled = true;
					}
				}
			}
		} else {
			foreach (Renderer r in rendererComponents)
				r.enabled = false;
		}
	}

	Transform FindBall() {
		foreach (Transform child in puttingHolder.transform.GetChild(0).transform) {
			if (child.name.Equals(GlobalData.players[GlobalData.turn].name + "Ball")) {
				return child;
			}
		}
		return null;
	}

}
// TODO: CHANGE SIZE OF FLAG AND DISTANCE