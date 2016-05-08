/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class VGADefaultTrackableEventHandler : MonoBehaviour,
	ITrackableEventHandler
	{
		#region PRIVATE_MEMBER_VARIABLES

		private TrackableBehaviour mTrackableBehaviour;
		//		public TrackableBehaviour.Status currentStatus;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region UNTIY_MONOBEHAVIOUR_METHODS

		void Start()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			if (mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}
			//			currentStatus = TrackableBehaviour.Status.NOT_FOUND;
		}

		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/// <summary>
		/// Implementation of the ITrackableEventHandler function called when the
		/// tracking state changes.
		/// </summary>
		public void OnTrackableStateChanged(
			TrackableBehaviour.Status previousStatus,
			TrackableBehaviour.Status newStatus)
		{
			if (newStatus == TrackableBehaviour.Status.DETECTED ||
				newStatus == TrackableBehaviour.Status.TRACKED ||
				newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
			{
				OnTrackingFound();
			}
			else
			{
				OnTrackingLost();
			}
			//			currentStatus = newStatus;
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		private void OnTrackingFound()
		{
			// move the rendered objects back to image target
			string objectName = this.gameObject.name + "Temp";
			GameObject temp = GameObject.Find(objectName);
			if (temp) {
				// resetting rotation to 0 0 0 to avoid rb moving
				this.transform.rotation = Quaternion.identity;
				//				Transform[] children = temp.GetComponentsInChildren<Transform> (true);
				foreach (Transform child in temp.transform) {
					child.parent = this.transform;
					child.localPosition = Vector3.zero;
					//					child.rotation = Quaternion.identity;
					//					child.eulerAngles = new Vector3 (0, 90, 0);
					child.eulerAngles = Vector3.zero;
					//					child.localPosition = this.transform.localPosition;
					//					child.localScale = this.transform.localScale;
				}
			}

			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

			// Enable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = true;
			}

			// Enable colliders:
			foreach (Collider component in colliderComponents)
			{
				component.enabled = true;
			}

			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
		}


		private void OnTrackingLost()
		{
			// temporarily move the rendered objects from image target to temporary holder to avoid them falling through
			string objectName = this.gameObject.name + "Temp";
			GameObject temp = GameObject.Find(objectName);
			if (temp && this.transform.gameObject.activeInHierarchy) {
				// resetting rotation to 0 0 0 to avoid rb moving
				temp.transform.rotation = Quaternion.identity;
				//				Transform[] children = this.GetComponentsInChildren<Transform> (true);
				//				for (int i = 1; i < children.Length; i++) {
				foreach (Transform child in this.transform) {
					child.parent = temp.transform;
					child.localPosition = Vector3.zero;
					//					child.rotation = Quaternion.identity;
					child.eulerAngles = Vector3.zero;
					//					child.localScale = temp.transform.localScale;
				}
			}

			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

			// Disable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = false;
			}

			// Disable colliders:
			foreach (Collider component in colliderComponents)
			{
				component.enabled = false;
			}

			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
		}

		#endregion // PRIVATE_METHODS
	}
}
