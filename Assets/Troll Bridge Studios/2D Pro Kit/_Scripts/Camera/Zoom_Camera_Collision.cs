using UnityEngine;
using System.Collections;

namespace TrollBridge {

	[RequireComponent (typeof (Collider2D))]
	/// <summary>
	/// Sets the Camera size (Zoom) in a Lerp manner when collision happens.  So when the player hits this collider we will move the camera close or further away to display a zoom effect.
	/// </summary>
	public class Zoom_Camera_Collision : MonoBehaviour {

		[Tooltip ("The orthographic size you want hte camera to be when a collision happens.")]
		public float cameraOrthographicSize = 0f;
		[Tooltip ("The time it takes to zoom to the orthographic size.")]
		public float zoomTime = 1f;


		void OnTriggerEnter2D (Collider2D coll) {
			// IF the colliding objects tag is the player.
			if (coll.tag == "Player") {
				// IF the cameras orthographic size is the same as our cameraOrthographicSize.
				if(Camera.main.orthographicSize == cameraOrthographicSize){
					// Do nothing.
					return;
				}

				// Stop all coroutines
				StopAllCoroutines ();
				// Start our zooming!!!!
				StartCoroutine (AdjustCameraOrthographicSize ());
			}
		}

		private IEnumerator AdjustCameraOrthographicSize () {
			// Grab our current orthographic size.
			float orthoSize = Camera.main.orthographicSize;
			// Loop with the zoomTime duration.
			for (float x = 0.0f; x < 1.0f; x += Time.deltaTime / zoomTime) {
				// Lerp to the desired cameraOrthographicSize.
				Camera.main.orthographicSize = Mathf.Lerp (orthoSize, cameraOrthographicSize, x);
				// TO THE NEXT FRAME!!!
				yield return null;
			}
			// When we are done with the loop we will need to set the numbers to their goal.
			Camera.main.orthographicSize = cameraOrthographicSize;
		}
	}
}