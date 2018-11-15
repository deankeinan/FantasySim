using UnityEngine;
using System.Collections;

namespace TrollBridge {

	[RequireComponent(typeof(Camera))]
	/// <summary>
	/// Mouse camera zoom allows zooming with the middle mouse button.
	/// </summary>
	public class Mouse_CameraZoom : MonoBehaviour {

		// The time frame of the actual zooming.
		public float zoomTime = 0.1f;
		// The amount to zoom in and out by with each roll of the middle mouse button or any button of your choosing.
		public float zoomAmount = 0.2f;
		// The min number you want your Orthographic size to be to simulate zooming in.
		public float minZoom = 3.6f;
		// The max number you want your Orthographic size to be to simulate zooming out.
		public float maxZoom = 4.4f;

		// Camera reference.
		private Camera cam;
		// The variable that will be if we are zooming in or out.
		private float axisNum = 0f;

		private float destinationOrthoSize;
		private float referenceSize;
		private bool isRunning = false;

		void Awake () {
			// Get the camera component.
			cam = GetComponent<Camera> ();
			// Get the OrthographicSize and we hold that as our initial reference.
			referenceSize = cam.orthographicSize;
		}

		void Update () {
			// Store the input variable.  We multiply by 10 as the actual increments by default are 0.1f, if you wish to alter these yourself make sure to check out the Edit -> Project Settings -> Input -> Mouse ScrollWheel.
			axisNum = Input.GetAxisRaw ("Mouse ScrollWheel") * 10f;
			// IF the input is not the middle mouse wheel OR we have a coroutine already running OR we dont have a player in the scene. 
			// (Normally all mouses these days have a middle button so just code for that, if someone really doesnt have a middle mouse button then this isnt on you, it's on them at this point in time with technology.  
			// We stop coding for ancient devices after a while or the most simple scripts would be loaded with a lot of crap we dont need to deal with which destroys time and time is the most valuable asset we have.)
			if (axisNum == 0f || isRunning || Character_Helper.GetPlayer() == null) {
				// No zoom so we do nothing.
				return;
			}
			// Get the amount to move based on the axisNum and the zoomAmount;
			float amountToMove = axisNum * zoomAmount;
			// Lets get our destination of where we want to move.
			destinationOrthoSize = referenceSize - amountToMove;
			// Now lets check if we are in the boundaries.
			// IF our destinationOrthoSize is bigger than our maxZoom we want,
			// ELSE IF our destinationOrthoSize is less than our minZoom we want,
			if (destinationOrthoSize >= maxZoom) {
				// Set the destinationOrthoSize to be our maxZoom.
				destinationOrthoSize = maxZoom;
			} else if (destinationOrthoSize <= minZoom) {
				// Set the destinationOrthoSize to be our minZoom.
				destinationOrthoSize = minZoom;
			}
		}

		void LateUpdate () {
			// IF the input is not the middle mouse wheel OR there is a zoom already happening OR there isnt a player in the scene.
			if (axisNum == 0 || isRunning || Character_Helper.GetPlayer() == null) {
				// Do nothing as we didn't zoom.
				return;
			}
			// We need to stop all of our current zooming as we have a new destination for our Orthographic Size.
			StopAllCoroutines ();
			// Start the coroutine which zooms.
			StartCoroutine (ZoomTime (cam.orthographicSize, destinationOrthoSize, zoomTime));
		}

		private IEnumerator ZoomTime (float start, float end, float time) {
			isRunning = true;
			// Loop in a SmoothStep manner to get a smooth movement.
			for (float x = 0.0f; x < 1.0f; x += Time.deltaTime / time) {
				// Smooth Step the movement.
				cam.orthographicSize = Mathf.SmoothStep (start, end, x);
				// Need this or we get a cluster inside Ienumerator loops.
				yield return null;
			}
			isRunning = false;
			cam.orthographicSize = end;
			referenceSize = end;
		}
	}
}
