using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace TrollBridge {

	[RequireComponent(typeof(Camera))]
	/// <summary>
	/// This is a camera that is built for following the player and allowing for boundaries to be set so that we can can "Slide" to the next area in our scene.
	/// </summary>
	public class Camera_Follow_Slide : MonoBehaviour {

		[Tooltip("The tag names of your UI that correspond to which side they are on.  Only strings that are input will be searched.")]
		public string leftUITag, rightUITag, topUITag, bottomUITag;
//		// The amount to move the Player when a camera pan happens.
//		[Tooltip("The amount to slide the player over on the X axis (Horizontally) when a camera pans.")]
//		public float horizontalSlideAmount = 0.2f;
//		[Tooltip("The amount to slide the player over on the Y axis (Vertically) when a camera pans.")]
//		public float verticalSlideAmount = 0.1f;
		// Make a public speed for the Camera Panning, with a default speed of 30f.
		[Tooltip("The camera panning speed.")]
		public float cameraSlideSpeed = 30f;

		// Used to store the player GameObject.
		private GameObject playerGO;
		private Transform playerTransform;

		// Slide Camera variables
		private int direction;
		// The location where the camera will be moving.
		private Vector3 destination;

		// This gameobjects Transform.
		private Transform _transform;
		// The Main Camera.
		private Camera _camera;
		// The dimensions of the camera.
		private float cameraWidth = 0f;
		private float cameraHeight = 0f;

		// Used for Camera bounds.
		private float leftBound, finalLeftBound;
		private float rightBound, finalRightBound;
		private float bottomBound, finalBottomBound;
		private float topBound, finalTopBound;

		// Border float holders for the camera boundaries.
		private float bb, tb, lb, rb;

		// The cameras x and y.
		private float camX;
		private float camY;

		// The booleans to let us know what the camera is doing.
		private bool isPanning = false;
		private bool isBounds = false;

		// The offset if the UI is at the top.
		private float moveUpOffset, moveDownOffset, moveLeftOffset, moveRightOffset;

		// The UI Lengths.
		private float topUILength;
		private float botUILength;
		private float leftUILength;
		private float rightUILength;
		// The inner visual screen that the player interacts in.
		private float innerWidth, innerHeight;
		// The bool for a cut-scene.
		private bool isCutScene = false;


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
			// IF the player does not exists... yet.
			if(playerGO == null){
				return;
			}
			// Cache the transform.
			playerTransform = playerGO.transform;
		}

		void Awake () {
			// Set the transform.
			_transform = gameObject.transform;
            _transform.position.Set(_transform.position.x, _transform.position.y, _transform.position.z + 50);
			// Grab the main Camera Component.
			_camera = Camera.main;
			// The Camera's width and height.
			cameraHeight = _camera.orthographicSize*2;
			cameraWidth = _camera.aspect * cameraHeight;

			// Set the UI offsets.
			topUILength = (GUI_Helper.GetUILengthRatio (topUITag, false, Camera.main) / Screen.height) * cameraHeight;
			botUILength = (GUI_Helper.GetUILengthRatio (bottomUITag, false, Camera.main) / Screen.height) * cameraHeight;
			leftUILength = (GUI_Helper.GetUILengthRatio (leftUITag, true, Camera.main) / Screen.width) * cameraWidth;
			rightUILength = (GUI_Helper.GetUILengthRatio (rightUITag, true, Camera.main) / Screen.width) * cameraWidth;

			// The actual height and width that the player moves around in.
			innerHeight = cameraHeight - (topUILength + botUILength);
			innerWidth = cameraWidth - (leftUILength + rightUILength);
		}

		void Start(){


		}

		/// <summary>
		/// We check to see whenever the player is spawned in the scene and then we act.
		/// </summary>
		void Update () {
			// IF the player gameobject is null.
			if (playerGO == null) {
				// Get the player GameObject.
				playerGO = Character_Helper.GetPlayer ();
				return;
			}
			// IF the players reference to its transform is null.
			if (playerTransform == null) {
				// Grab the transform.
				playerTransform = playerGO.transform;
			}

			// IF we have not set our bounds yet.
			if (!isBounds) {
				// Leave as we have no boundaries so we do not know wtf to do in terms of any restriction on how this camera mechanic is supposed to work
				return;
			}
			// IF we are currently in a cutscene.
			if (isCutScene) {
				// We are in a cutscene so we do not care about how this camera operates as in a cutscene we are manually controlling the camera.
				return;
			}
			// In-case we have any sort of zoom feature along with boundaries we def need to refresh our cameras width and height.
			RefreshBounds ();
			// We passed all of our prequisites so lets clamp values between the bounds.
			camX = Mathf.Clamp (playerTransform.position.x + (rightUILength - leftUILength) / 2f, finalLeftBound - leftUILength, finalRightBound - rightUILength);
			camY = Mathf.Clamp (playerTransform.position.y + (topUILength - botUILength) / 2f, finalBottomBound - botUILength, finalTopBound + topUILength);
		}

		void LateUpdate(){
			// IF the bounds has not been set.
			if (!isBounds) {
				// We want a follow and slide camera... well you need to set your bounds first.
				return;
			}
			// IF the Player is currently in a cut-scene.
			if (isCutScene) {
				// Do not control as in a cut-scene we want to manually control the camera.
				return;
			}
			// IF the camera is panning,
			if (isPanning) {
				// Move the Camera.
				CameraMove ();
				// Done.
				return;
			}
			// IF there isnt a Cut-Scene and the Camera isn't panning, then we are following the player.
			_transform.position = new Vector3 (camX, camY, _transform.position.z);
		}

		void CameraMove(){
			// Move the Camera.
			_transform.position = Vector3.MoveTowards (_transform.position, destination, cameraSlideSpeed * Time.deltaTime);
			// IF we reached our destination!!!
			if (_transform.position == destination) {
				// We are not panning anymore.
				SetIsPanning (false);
				// Change the player variable for movement purposes.
				ChangePlayerMovement (true);
			}
		}

		void ChangePlayerMovement(bool isMovable){
			// Change the player variable for movement purposes.
			Character_Helper.GetPlayerManager().GetComponent<Character_Manager> ().canMove = isMovable;
		}

		/// <summary>
		/// Control which direction we will be panning based on our integer "dir".
		/// </summary>
		public void SetCameraPan(int dir, float xPos, float yPos){
			// Assign the direction to Pan the Camera.
			direction = dir;
			switch (direction) {
			// Move UP
			case 1:
				destination = new Vector3 (_transform.position.x, _transform.position.y + innerHeight, _transform.position.z + 50);
				// As the camera will be panning lets move our player in the direction desired to it can collide with the new bounds area to be set.
				break;
			// Move DOWN
			case 2:
				destination = new Vector3 (_transform.position.x, _transform.position.y - innerHeight, _transform.position.z);
				// As the camera will be panning lets move our player in the direction desired to it can collide with the new bounds area to be set.
				break;
			// Move LEFT
			case 3:
				destination = new Vector3 (_transform.position.x - innerWidth, _transform.position.y, _transform.position.z);
				// As the camera will be panning lets move our player in the direction desired to it can collide with the new bounds area to be set.
				break;
			// Move RIGHT
			case 4:
				destination = new Vector3 (_transform.position.x + innerWidth, _transform.position.y, _transform.position.z);
				// As the camera will be panning lets move our player in the direction desired to it can collide with the new bounds area to be set.
				break;
			}
			// Since we have our camera destination lets place our player at the next spot.
			playerTransform.position = new Vector2 (xPos, yPos);
			// Change the player variable for movement purposes.
			ChangePlayerMovement (false);
		}

		/// <summary>
		/// Refreshs the bounds.
		/// </summary>
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

		/// <summary>
		/// Sets the camera bounds based on the parameters.
		/// </summary>
		public void SetCameraBounds(float _bottomCameraBorder, float _topCameraBorder, float _leftCameraBorder, float _rightCameraBorder) {
			// Store the locations of the bounds.
			bottomBound = _bottomCameraBorder;
			topBound = _topCameraBorder;
			leftBound = _leftCameraBorder;
			rightBound = _rightCameraBorder;
			// We now have our bounds.
			isBounds = true;
		}

		/// <summary>
		/// Changes the cut scene.
		/// </summary>
		public void ChangeCutScene(bool cutScene){
			isCutScene = cutScene;
		}

		/// <summary>
		/// Is the camera panning?
		/// </summary>
		public bool GetIsPanning(){
			return isPanning;
		}

		/// <summary>
		/// Sets our camera panning boolean.
		/// </summary>
		public void SetIsPanning(bool newIsPanning){
			isPanning = newIsPanning;
		}
	}
}
