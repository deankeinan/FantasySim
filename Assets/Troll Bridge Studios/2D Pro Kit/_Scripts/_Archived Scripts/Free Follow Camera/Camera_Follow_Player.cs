using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace TrollBridge {

	/// <summary>
	/// This script has been archived but if you like you can use it however you want.  
	/// The script Camera_Follow_Slide can do what this script does and more.
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class Camera_Follow_Player : MonoBehaviour {

		[Tooltip("The tag names of your UI that correspond to which side they are on.  Only strings that are input will be searched.")]
		public string leftUITag, rightUITag, topUITag, bottomUITag;

		// Used to store the player GameObject.
		private GameObject playerGO;
		private Transform playerTransform;

		// The gameobjects Transform.
		private Transform _transform;

		// The Camera and min and max bounds.
		private Camera _camera;
		private float boundMinX;
		private float boundMaxX;
		private float boundMinY;
		private float boundMaxY;

		// Used for Camera bounds.
		private float leftBound, finalLeftBound;
		private float rightBound, finalRightBound;
		private float bottomBound, finalBottomBound;
		private float topBound, finalTopBound;

		// The cameras x and y.
		private float camX;
		private float camY;

		// Border float holders for the camera boundaries.
		private float bb, tb, lb, rb;

		// The lengths of the UIs.
		private float topUILength;
		private float botUILength;
		private float leftUILength;
		private float rightUILength;

		// The bool for a cut-scene.
		private bool isCutScene = false;
		// The booleans to let us know we are in bounds.
		private bool isBounds = false;


		void OnEnable(){
			// Add a delegate.
			SceneManager.sceneLoaded += OnLoadedLevel;
		}

		void OnDisable(){
			// Remove the delegate.
			SceneManager.sceneLoaded -= OnLoadedLevel;
		}

		// Incase you want to have a 1 use camera we need to set defaults on a scene change and set up the boundary variables before we track the player.
		void OnLoadedLevel(Scene scene, LoadSceneMode mode){
			// Set defaults.
			isBounds = false;
			// Get the player GameObject.
			playerGO = Character_Helper.GetPlayer();
			// IF we have a player to get when changing the scene.
			if(playerGO == null){
				return;
			}
			// Grab the transform of the player.
			playerTransform = playerGO.transform;
		}

		void Awake () {
			// Set the transform.
			_transform = gameObject.transform;
			// Grab the main Camera Component.
			_camera = Camera.main;
			// Set the UI offsets.
			topUILength = GUI_Helper.GetUILength (topUITag, false) / 100;
			botUILength = GUI_Helper.GetUILength (bottomUITag, false) / 100f;
			leftUILength = GUI_Helper.GetUILength (leftUITag, true) / 100f;
			rightUILength = GUI_Helper.GetUILength (rightUITag, true) / 100f;
		}

		void Start() {
			// Get the player GameObject.
			playerGO = Character_Helper.GetPlayer();
			// IF the player GameObject is null.
			if(playerGO == null){
				// IF after searching and no player at the Start, then we can assume this scene doesnt have a player in it and we will not control the positioning of this Camera from here.
				return;
			}
			// Grab the transform.
			playerTransform = playerGO.transform;
		}

		/// <summary>
		/// We check to see whenever the player is spawned in the scene and then we act.
		/// </summary>
		void Update () {
			// IF the player gameobject is null.
			if(playerGO == null){
				// Get the player GameObject.
				playerGO = Character_Helper.GetPlayer();
				return;
			}
			// IF the players reference to its transform is null.
			if(playerTransform == null){
				// Grab the transform.
				playerTransform = playerGO.transform;
			}
			// IF the bounds have been set and we are not in a cutscene.
			if(isBounds && !isCutScene){
				// Incase we have any sort of zoom feature along with boundaries we def need to refresh our cameras width and height.
				RefreshBounds ();
				// Clamp values between the bounds.
				camX = Mathf.Clamp(playerTransform.position.x + (rightUILength - leftUILength) / 2f, finalLeftBound - leftUILength, finalRightBound + rightUILength);
				camY = Mathf.Clamp(playerTransform.position.y + (topUILength - botUILength) / 2f, finalBottomBound - botUILength, finalTopBound + topUILength);
			}
		}

		void LateUpdate () {
			// IF we are in the middle of a cutscene.
			if (isCutScene) {
				// Do no camera control from here in the Late Update.
				return;
			}
			// IF the bounds have been set.
			if (isBounds) {
				// IF there isnt a Cut-Scene and the Camera isn't panning, then we are following the player.
				_transform.position = new Vector3 (camX, camY, _transform.position.z);
				return;
			}

			// IF the player GameObject exists.
			if (playerTransform != null) {
				// Follow the player with no boundaries.
				_transform.position = new Vector3 (
					playerTransform.position.x + (rightUILength - leftUILength) / 2f, 
					playerTransform.position.y + (topUILength - botUILength) / 2f, 
					_transform.position.z);
			}
		}

		private void RefreshBounds () {
			// Get the camera ratios.
			float camVertExtent = _camera.orthographicSize;
			float camHorzExtent = _camera.aspect * camVertExtent;
			// Set the new bounds.
			finalBottomBound = bottomBound + camVertExtent;
			finalTopBound = topBound - camVertExtent;
			finalLeftBound = leftBound + camHorzExtent;
			finalRightBound = rightBound - camHorzExtent;
		}

		public void SetCameraBounds(float _bottomCameraBorder, float _topCameraBorder, float _leftCameraBorder, float _rightCameraBorder) {
			// Store the locations of the bounds.
			bottomBound = _bottomCameraBorder;
			topBound = _topCameraBorder;
			leftBound = _leftCameraBorder;
			rightBound = _rightCameraBorder;
			// We now have our bounds.
			isBounds = true;
		}

		public void ChangeCutScene (bool cutScene) {
			isCutScene = cutScene;
		}
	}
}
