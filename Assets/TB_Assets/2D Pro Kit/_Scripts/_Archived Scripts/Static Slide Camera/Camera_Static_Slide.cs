using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace TrollBridge {

	/// <summary>
	/// This script has been archived but if you like you can use it however you want.  
	/// The script Camera_Follow_Slide can do what this script does and more.
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class Camera_Static_Slide : MonoBehaviour {
		
		[Tooltip("The tag names of your UI that correspond to which side they are on.  Only strings that are input will be searched.")]
		public string leftUITag, rightUITag, topUITag, bottomUITag;
		// The amount to slide the player over when a camera pans.
		[Tooltip("The amount to slide the player over on the X axis (Horizontally) when a camera pans.")]
		public float horizontalSlideAmount = 0.4f;
		[Tooltip("The amount to slide the player over on the Y axis (Vertically) when a camera pans.")]
		public float verticalSlideAmount = 0.25f;
		// Make a public speed for the Camera Panning, with a default speed of 30f.
		[Tooltip("The camera panning speed.")]
		public float cameraSlideSpeed = 30f;

		// Cache the Transform.
		private Transform _transform;
		// The Camera.
		private Camera _camera;
		// The dimensions of the camera.
		private float cameraWidth = 0f;
		private float cameraHeight = 0f;

		// The direction and how far we move over on a camera pan.
		private Vector3 destination;

		// Used for if the camera is panning or not.
		private bool isPanning = false;
		// Used to store the player GameObject.
		private GameObject playerGO;
		// Used to store the player GameObjects Transform.
		private Transform playerTransform;

		// Border float holders for the camera boundaries.
		private float bb, lb;

		// The offset if the UI is at the top.
		private float moveUpOffset, moveDownOffset, moveLeftOffset, moveRightOffset;

		// UI Lengths.
		private float topUILength;
		private float botUILength;
		private float leftUILength;
		private float rightUILength;
		// The inner visual screen that the player interacts in.
		private float innerWidth, innerHeight;

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
			// Call what normally would happen if this camera was running its code for the first time.
			SetIsPanning (false);

			// Set Initial Settings.
			SetLengthsAndOffsets();
			// Get the player GameObject.
			playerGO = Character_Helper.GetPlayer();
			// IF the player does not exists... yet.
			if(playerGO == null){
				return;
			}
			// Cache the transform.
			playerTransform = playerGO.transform;
			// Initially position in the bottom left tile of one of your slide areas so we can get a proper start position and smooth slide transitions when going to another area (not another scene but just the same scene but the area next to it).
			PositionCamera();
			// Set the camera up where it is supposed to start since we have found the player.
			AlignCamera();
		}

		void Awake () {
			// Get the Transform.
			_transform = gameObject.transform;
			// Grab the Camera Component.
			_camera = Camera.main;
			// Set the UI offsets.
			topUILength = GUI_Helper.GetUILength (topUITag, false) / 100;
			botUILength = GUI_Helper.GetUILength (bottomUITag, false) / 100f;
			leftUILength = GUI_Helper.GetUILength (leftUITag, true) / 100f;
			rightUILength = GUI_Helper.GetUILength (rightUITag, true) / 100f;
		}

		void Start(){
			// Get the player GameObject.
			playerGO = Character_Helper.GetPlayer();
			// IF the player GameObject is null.
			if(playerGO == null){
				// IF after searching and no player at the Start, then we can assume this scene doesnt have a player in it and we will not control the positioning of this Camera from here.
				return;
			}
			// Grab the transform.
			playerTransform = playerGO.transform;
			// Set Initial Settings.
			SetLengthsAndOffsets();
			// Initially position in the bottom left tile of one of your slide areas so we can get a proper start position and smooth slide transitions when going to another area (not another scene but just the same scene but the area next to it).
			PositionCamera();
			// Set the camera up where it is supposed to start since we have found the player.
			AlignCamera();
		}

		void Update () {
			// IF the player GameObject is null.
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
			// IF we are in a cutscene.
			if(isCutScene){
				// Do nothing with the camera .
				return;
			}
			// Detect IF we are at the edges of the Camera to Pan.
			CameraEdgeDetection();
		}

		void LateUpdate(){
			// IF we dont have a player then we do not move the camera at all through this script.
			if(playerGO == null){
				// GTFO.
				return;
			}
			// IF we are in a cutscene.
			if(isCutScene){
				// Do nothing with the camera .
				return;
			}
			// Move the Camera.
			CameraMove();
		}

		void CameraMove(){
			// IF we are currently panning.
			if(isPanning){
				// Move the Camera.
				_transform.position = Vector3.MoveTowards(_transform.position, destination, cameraSlideSpeed * Time.deltaTime);
				// IF we reached our destination!!!
				if(_transform.position == destination){
					// We are not panning anymore.
					SetIsPanning (false);
					// We can now move the character.
					ChangePlayerMovement(true);
				}
			}
		}

		void CameraEdgeDetection(){
			// IF we are not panning.
			if(!isPanning && !isCutScene) {
				// Grab the Vector3 viewport of Camera to the Player. We are looking for range 0 -> 1.
				Vector3 screenPos = _camera.WorldToViewportPoint (playerTransform.position);
				// IF moving to the LEFT,
				// ELSE IF moving to the RIGHT,
				// ELSE IF moving to the TOP,
				// ELSE IF moving to the BOTTOM.
				if (screenPos.x - moveLeftOffset < 0f) {
					// Move Left.
					SetCameraPanning (-horizontalSlideAmount, 0f, new Vector3(_transform.position.x - innerWidth, _transform.position.y, _transform.position.z));
				}else if (screenPos.x + moveRightOffset > 1f) {
					// Move Right.
					SetCameraPanning (horizontalSlideAmount, 0f, new Vector3(_transform.position.x + innerWidth, _transform.position.y, _transform.position.z));
				}else if (screenPos.y + moveUpOffset > 1) {
					// Move Up.
					SetCameraPanning (0f, verticalSlideAmount, new Vector3(_transform.position.x, _transform.position.y + innerHeight, _transform.position.z));
				}else if (screenPos.y - moveDownOffset < 0f) {
					// Move Down.
					SetCameraPanning (0f, -verticalSlideAmount, new Vector3(_transform.position.x, _transform.position.y - innerHeight, _transform.position.z));
				}
			}
		}

		void SetCameraPanning(float horSlide, float vertSlide, Vector3 newDestination){
			// Set the bool to true since we are panning.
			SetIsPanning (true);
			// The new Vector3 location to move to.
			destination = newDestination;
			// As the camera is moving we need to move the Player a little bit over.
			playerTransform.Translate(new Vector2(horSlide, vertSlide));
			// Assign the player movement variable to false so the player doesn't move during transition.
			ChangePlayerMovement(false);
		}

		void ChangePlayerMovement(bool isMovable){
			// Change the player variable for movement purposes.
			Character_Helper.GetPlayerManager().GetComponent<Character_Manager>().canMove = isMovable;
		}

		void PositionCamera(){
			// Variable to get us our borders for setting up the slide properly.
			GameObject botBorder, leftBorder;
			// IF we have a bottom left border piece as one,
			// ELSE we have a bottom and a left border piece that are different.
			if (GameObject.FindGameObjectWithTag ("BottomLeftBorder") != null) {
				botBorder = GameObject.FindGameObjectWithTag ("BottomLeftBorder");
				leftBorder = botBorder;
			} else if (GameObject.FindGameObjectWithTag ("BottomBorder") != null && GameObject.FindGameObjectWithTag ("LeftBorder")) {
				botBorder = GameObject.FindGameObjectWithTag ("BottomBorder");
				leftBorder = GameObject.FindGameObjectWithTag ("LeftBorder");
			} else {
				// There are no indicators that we are setting up a border for our slide, so we must be on a scene where we do not care about them and will position the camera elsewhere manually.
				return;
			}
			// We need to get the position of the left and bottom tile.
			SetBottomLeftBounds(botBorder, leftBorder);
			// Now that we have what we need lets set the position.
			_transform.position = new Vector3(lb + (cameraWidth / 2f) - leftUILength, bb + (cameraHeight / 2f) - botUILength, _transform.position.z);
		}

		void SetBottomLeftBounds(GameObject _bottomCameraBorder, GameObject _leftCameraBorder){
			// Grabbing the collider2d and the renderer if they exists for future checks.
			Collider2D _BColl = _bottomCameraBorder.GetComponent<Collider2D>();
			Renderer _BRend = _bottomCameraBorder.GetComponent<Renderer>();
			// Grabbing the collider2d and the renderer if they exists for future checks.
			Collider2D _LColl = _leftCameraBorder.GetComponent<Collider2D>();
			Renderer _LRend = _leftCameraBorder.GetComponent<Renderer>();

			// let's check, 
			// IF the bounding border is an invisible Collider object, Border. (Collider && !Renderer),
			// ELSE IF the bounding border is a visable Sprite || Collider Sprite, Ground Tile || Wall Tile.,
			// ELSE then the user has just made a point where they do not want the camera to go past -- Just a plain GameObject.

			// Bottom Collider check.
			if((_BColl != null && _BRend == null)){
				bb = _BColl.bounds.max.y;
			}else if((_BColl != null && _BRend != null) || (_BColl == null && _BRend != null)){
				bb = _BRend.bounds.min.y;
			}else {
				bb = _bottomCameraBorder.transform.position.y;
			}

			// Left Collider check.
			if((_LColl != null && _LRend == null)){
				lb = _LColl.bounds.max.x;
			}else if((_LColl != null && _LRend != null) || (_LColl == null && _LRend != null)){
				lb = _LRend.bounds.min.x;
			}else {
				lb = _leftCameraBorder.transform.position.x;
			}
		}

		void AlignCamera(){
			// Grab the Vector3 viewport of Camera to the Player.
			Vector3 screenPos = _camera.WorldToViewportPoint (playerTransform.position);

			// WHILE our character is to the right of the camera view and not behind our right UI.
			while (screenPos.x + moveRightOffset > 1f) {
				// Move this camera over by our innerWidth.
				_transform.position = new Vector3 (_transform.position.x + innerWidth, _transform.position.y, _transform.position.z);
				// Lets reset our viewport point from where the player is.
				screenPos = _camera.WorldToViewportPoint (playerTransform.position);
			}

			// WHILE our character is to the left of the camera view and not behindo ur left UI.
			while (screenPos.x - moveLeftOffset < 0f) {
				// Move this camera over by our innerWidth.
				_transform.position = new Vector3 (_transform.position.x - innerWidth, _transform.position.y, _transform.position.z);
				// Lets reset our viewport point from where the player is.
				screenPos = _camera.WorldToViewportPoint (playerTransform.position);
			}

			// WHILE our character is above the cameras view and not behind our top UI.
			while (screenPos.y + moveUpOffset > 1f) {
				// Move this camera over by our innerHeight.
				_transform.position = new Vector3 (_transform.position.x, _transform.position.y + innerHeight, _transform.position.z);
				// Lets reset our viewport point from where the player is.
				screenPos = _camera.WorldToViewportPoint (playerTransform.position);
			}

			// WHILE our character is below the cameras view and not behind our bottom UI.
			while(screenPos.y - moveDownOffset < 0f) {
				// Move this camera over by our innerHeight.
				_transform.position = new Vector3 (_transform.position.x, _transform.position.y - innerHeight, _transform.position.z);
				// Lets reset our viewport point from where the player is.
				screenPos = _camera.WorldToViewportPoint (playerTransform.position);
			}
		}

		void SetLengthsAndOffsets () {
			// The Camera's width and height.
			cameraHeight = _camera.orthographicSize*2;
			cameraWidth = _camera.aspect * cameraHeight;
			// The actual height and width that the player moves around in.
			innerHeight = cameraHeight - (topUILength + botUILength);
			innerWidth = cameraWidth - (leftUILength + rightUILength);
			moveUpOffset = topUILength / cameraHeight;
			moveDownOffset = botUILength / cameraHeight;
			moveLeftOffset = leftUILength / cameraHeight;
			moveRightOffset = rightUILength / cameraHeight;
		}

		public void ChangeCutScene(bool cutScene){
			isCutScene = cutScene;
		}

		public bool GetIsPanning(){
			return isPanning;
		}

		public void SetIsPanning(bool newIsPanning){
			isPanning = newIsPanning;
		}
	}
}
