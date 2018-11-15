using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrollBridge {

	/// <summary>
	/// UI_Update is a script that will display text by moving it from the start location to the mid location and then to the end location.
	/// </summary>
	public class UI_Update : MonoBehaviour {

		[Header ("Texts")]
		public Text title;
		public Text subtitle;

		[Header ("Times")]
		public float moveTime = 0.5f;
		public float displayTime = 2f;

		[Header ("Locations")]
		public Transform titleStartLocation;
		public Transform titleMidLocation;
		public Transform titleEndLocation;
		public Transform subtitleStartLocation;
		public Transform subtitleMidLocation;
		public Transform subtitleEndLocation;


		void Start () {
			title.gameObject.transform.position = titleStartLocation.position;
			subtitle.gameObject.transform.position = subtitleStartLocation.position;
		}

		public void StartInformationCoroutineDisplay (string newTitle, string newSubtitle) {

			// Need to make sure we do not already have this display being shown so we need to stop all coroutines and start everything back
			// to StartLocation.
			StopAllCoroutines ();

			// Reset.
			title.transform.position = titleStartLocation.position;
			subtitle.transform.position = subtitleStartLocation.position;

			// Start our Coroutine.
			StartCoroutine (UpdateInformationDisplay (newTitle, newSubtitle));
		}

		public IEnumerator UpdateInformationDisplay (string newTitle, string newSubtitle) {
			title.text = newTitle;
			subtitle.text = newSubtitle;

			// Now we Lerp title and subtitle to the proper locations.
			StartCoroutine (Grid_Helper.helper.SmoothStepGameObject (title.gameObject.transform, titleStartLocation, titleMidLocation, moveTime));
			StartCoroutine (Grid_Helper.helper.SmoothStepGameObject (subtitle.gameObject.transform, subtitleStartLocation, subtitleMidLocation, moveTime));
			// Wait the time it takes to move.
			yield return new WaitForSeconds (displayTime + moveTime);
			// We now move the text's to their end location.
			StartCoroutine (Grid_Helper.helper.SmoothStepGameObject (title.gameObject.transform, titleMidLocation, titleEndLocation, moveTime));
			StartCoroutine (Grid_Helper.helper.SmoothStepGameObject (subtitle.gameObject.transform, subtitleMidLocation, subtitleEndLocation, moveTime));
		}
	}
}
