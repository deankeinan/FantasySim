using UnityEngine;
using System.Collections;

namespace TrollBridge {

	[RequireComponent(typeof(Collider2D))]
	/// <summary>
	/// Slides the camera in a automatic pan manner when our player collides with this.  We set the action that this gameobject will be by checking one of the Edge Directions.
	/// </summary>
	public class Slide_Camera : MonoBehaviour {

		[Header ("Edge Direction")]
		[SerializeField] private bool moveCameraUp;
		[SerializeField] private bool moveCameraDown;
		[SerializeField] private bool moveCameraRight;
		[SerializeField] private bool moveCameraLeft;

		[Header ("Player Move Location")]
		[Tooltip ("The location where you want the player to be moved to when a slide happens.")]
		[SerializeField] private Transform newPosition;
		[Tooltip ("When a slide happens do we want to lock in the X position?")]
		[SerializeField] private bool xPos;
		[Tooltip ("When a slide happens do we want to lock in the Y position?")]
		[SerializeField] private bool yPos;

		[Header ("Next Area")]
		[Tooltip ("The next area we will be sliding to.")]
		public Set_Camera_Boundaries nextCameraBoundaryArea;

		private Camera_Follow_Slide followSlideCamera;

		void OnValidate(){
			// This must be a Trigger.
			GetComponent<Collider2D> ().isTrigger = true;
			// IF the user has both xPos and yPos set to true then we need to fix that as you can only save 1 part.
			if (xPos && yPos) {
				xPos = false;
				yPos = false;
			}
		}

		void Awake () {
			// Make sure we are using the correct Camera script.
			if (Camera.main.GetComponent<Camera_Follow_Slide> () == null) {
				// We do not have the correct camera so let the Developer know.
				Debug.Log ("When using 'Slide_Camera' make sure you are using the 'Follow_Slide_Camera' script on your main camera.");
				// Since we are here... lets pause so this can be fixed.
				Debug.Break ();
				return;
			}
			// Reference the script.
			followSlideCamera = Camera.main.GetComponent<Camera_Follow_Slide> ();
		}

		void OnTriggerEnter2D(Collider2D coll){
			// Go based on tag and if the Follow_Slide_Camera exists.
			if(coll.tag == "Player" && followSlideCamera != null){
				// Let the camera know we are about to start panning.
				followSlideCamera.SetIsPanning (true);

				// Variables for our player position.
				float xSave = newPosition.position.x;
				float ySave = newPosition.position.y;
				// IF we want to save the x position of the player.
				if (xPos) {
					xSave = coll.transform.position.x;
				} 
				// IF we want to save the y position of the player.
				if (yPos) {
					ySave = coll.transform.position.y;
				}
				// IF we want the camera to pan up,
				// ELSE IF we want the camera to pan down,
				// ELSE IF we want the camera to pan right,
				// ELSE IF we want the camera to pan left.
				if (moveCameraUp) {
					followSlideCamera.SetCameraPan (1, xSave, newPosition.position.y);
				} else if (moveCameraDown) {
					followSlideCamera.SetCameraPan (2, xSave, newPosition.position.y);
				} else if (moveCameraLeft) {
					followSlideCamera.SetCameraPan (3, newPosition.position.x, ySave);
				} else if (moveCameraRight) {
					followSlideCamera.SetCameraPan (4, newPosition.position.x, ySave);
				}

				// Set our boundaries to the next area.
				nextCameraBoundaryArea.SetNewBounds ();
			}
		}
	}
}
