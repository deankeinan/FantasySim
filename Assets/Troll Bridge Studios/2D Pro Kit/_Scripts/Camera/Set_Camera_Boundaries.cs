using UnityEngine;
using System.Collections;

namespace TrollBridge{

	/// <summary>
	/// Set camera boundaries allows us to setup an area for our camera to pan in.
	/// </summary>
	public class Set_Camera_Boundaries : MonoBehaviour {

		// Size of our boundary.
		[SerializeField] private Vector2 boundarySize;

		// Boundaries.
		private float leftBoundary, rightBoundary, topBoundary, bottomBoundary;
		// Slide when we touch an edge giving us sections in our map.
		private Camera_Follow_Slide cameraFollowSlide;


		void Awake () {
			// Lets get our boundaries.
			leftBoundary = gameObject.transform.position.x - boundarySize.x / 2;
			rightBoundary = gameObject.transform.position.x + boundarySize.x / 2;
			topBoundary = gameObject.transform.position.y + boundarySize.y / 2;
			bottomBoundary = gameObject.transform.position.y - boundarySize.y / 2;

			// IF we are using the Follow_Slide_Camera.
			if (Camera.main.GetComponent<Camera_Follow_Slide> () != null) {
				// Get the Camera Follow Slide component.
				cameraFollowSlide = Camera.main.GetComponent<Camera_Follow_Slide> ();
				// We got what we need lets leave.
				return;
			}
			// We do not have the correct camera so let the Developer know.
			Debug.Log ("Could not find any of the script this communicates with.");
		}

		void Start () {
			// Need to check if the playeris inside the bounds.
			if (IsPlayerInBounds (Character_Helper.GetPlayer ().transform.position)) {
				// IF the Follow_Slide_Camera exists.
				if (cameraFollowSlide != null) {
					// Set the new boundaries.
					SetNewBounds ();
					return;
				}
			}
		}

		void OnDrawGizmos() {
			Gizmos.DrawWireCube (transform.position, new Vector3 (boundarySize.x, boundarySize.y, 1f));
		}

		/// <summary>
		/// Sets the new bounds.
		/// </summary>
		public void SetNewBounds () {
			// Set the new boundaries.
			cameraFollowSlide.SetCameraBounds (bottomBoundary, topBoundary, leftBoundary, rightBoundary);
		}

		/// <summary>
		/// Determines whether the player is inside the bounds.
		/// </summary>
		private bool IsPlayerInBounds (Vector3 playerLocation) {
			// IF the x AND y position is inside the boundary.
			if (leftBoundary < playerLocation.x && rightBoundary > playerLocation.x &&
			    topBoundary > playerLocation.y && bottomBoundary < playerLocation.y) {
				// return true.
				return true;
			} else {
				return false;
			}
		}
	}
}
