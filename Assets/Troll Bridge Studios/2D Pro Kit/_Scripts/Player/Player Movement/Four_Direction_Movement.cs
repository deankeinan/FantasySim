using UnityEngine;
using System.Collections;

namespace TrollBridge {

	[RequireComponent (typeof (Rigidbody2D))]
	/// <summary>
	/// Four direction movement allows us to move only : Up, down, left, right.
	/// </summary>
	public class Four_Direction_Movement : MonoBehaviour {

		// The GameObjects Rigidbody.
		private Rigidbody2D rb;
		// The Character Manager.
		private Character_Manager characterManager;
		// The Character Stats.
		private Character_Stats charStats;
		// Holders for the movements.
		private float moveHorizontal;
		private float moveVertical;
		// For priority of movement press.
		private bool firstKeyPressed;
		private bool horizontalFirst;
		private bool verticalFirst;


		void Awake(){
			// Get the Characters Manager component.
			characterManager = Character_Helper.GetPlayerManager ().GetComponent<Character_Manager> ();
			// Get the characters stats as we use that to potentially alter movement.
			charStats = characterManager.GetComponentInChildren<Character_Stats> ();
			// Get the Rigidbody2D Component.
			rb = GetComponent<Rigidbody2D> ();
			// Set the horizontalFirst to false as nothing is being pressed.
			firstKeyPressed = false;
			horizontalFirst = false;
			verticalFirst = false;
		}

		void Update(){
			// IF we are able to move.
			if(characterManager.canMove){
				// Get a -1, 0 or 1.
				moveHorizontal = Input.GetAxisRaw ("Horizontal");
				moveVertical = Input.GetAxisRaw ("Vertical");
			}
		}

		void FixedUpdate(){
			// IF we are allowed to move.
			// ELSE IF we are not allowed to move unless we are jolted.
			if (characterManager.canMove) {

				// Get the First Key and Second Key press setup when moving.
				KeySetup ();

				Vector2 movement;

				// IF we are moving horizontal first.
				// ELSE IF we are moving vertical first.
				// ELSE we are not even moving.
				if (horizontalFirst) {
					// IF we are moving vertical.
					// ELSE we are moving horizontal.
					if (moveVertical != 0) {
						// Get the inverted directions.
						movement = new Vector2 (0f, moveVertical * characterManager.playerInvertY);
						// Play the animation.
						PlayAnimation (0f, moveVertical);
					} else {
						// Get the inverted directions.
						movement = new Vector2 (moveHorizontal * characterManager.playerInvertX, 0f);
						// Play the animation.
						PlayAnimation (moveHorizontal, 0f);
					}

					// Apply inverted directions with speed.
					movement *= charStats.CurrentMoveSpeed * characterManager.alterSpeed;
					// Apply force.
					rb.AddForce (movement);

				} else if (verticalFirst) {
					// IF we are moving horizontal.
					// ELSE IF we are moving vertical.
					if (moveHorizontal != 0) {
						// Get the inverted directions.
						movement = new Vector2 (moveHorizontal * characterManager.playerInvertX, 0f);
						// Play the animation.
						PlayAnimation (moveHorizontal, 0f);

					} else {
						// Get the inverted directions.
						movement = new Vector2 (0f, moveVertical * characterManager.playerInvertY);
						// Play the animation.
						PlayAnimation (0f, moveVertical);
					}

					// Apply inverted directions with speed.
					movement *= charStats.CurrentMoveSpeed * characterManager.alterSpeed;
					// Apply force.
					rb.AddForce (movement);
				} else {
					PlayAnimation (0f, 0f);
				}
			} else if (!characterManager.currentlyJolted) {
				rb.velocity = Vector2.zero;
			}
		}

		void KeySetup(){
			// IF there is not a key being pressed.
			// ELSE we are pressing a key for movement.
			if(!firstKeyPressed){
				// IF we are moving horizontal.
				// ELSE IF we are moving vertical.
				if(moveHorizontal == -1 || moveHorizontal == 1){
					// First key pressed is moving Horizontal.
					horizontalFirst = true;
					// First key pressed is not Vertical.
					verticalFirst = false;
					// We are now pressing a key down.
					firstKeyPressed = true;
				}else if(moveVertical == -1 || moveVertical == 1){
					// First key pressed is moving Vertical.
					verticalFirst = true;
					// First key pressed is moving Horizontal.
					horizontalFirst = false;
					// We are now pressing a key down.
					firstKeyPressed = true;
				}
			}else{
				// IF we were holding down a key to move Horizontal as the first key pressed.
				if(horizontalFirst){
					// IF we are not moving Horizontal anymore.
					if(moveHorizontal == 0){
						// The first key to be pressed moving Horizontal is not being pressed anymore.
						firstKeyPressed = false;
					}
				}else{
					// IF we are not moving Vertical anymore.
					if(moveVertical == 0){
						// The first key to be pressed moving Vertical is not being pressed anymore.
						firstKeyPressed = false;
					}
				}
			}
		}

		void PlayAnimation(float hor, float vert){
			// IF the user has an animation set and ready to go.
			if(characterManager.characterAnimator != null){
				// IF we have a Four Dirction Animation.
				if(characterManager.fourDirAnim){
					// Play animations.
					Grid_Helper.helper.FourDirectionAnimation(hor * characterManager.playerInvertX, vert * characterManager.playerInvertY, characterManager.characterAnimator);
				}
			}
		}
	}
}
