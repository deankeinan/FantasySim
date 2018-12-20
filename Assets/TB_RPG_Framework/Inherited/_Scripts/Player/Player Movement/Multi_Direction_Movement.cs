using UnityEngine;
using System.Collections;

namespace TrollBridge {

	[RequireComponent (typeof (Rigidbody2D))]
	/// <summary>
	/// Multi direction movement are based on a slow build up the longer you hold a key down or move in a certain direction.  So there are no restrictions to the direction you want to move.
	/// </summary>
	public class Multi_Direction_Movement : MonoBehaviour {

		// Vector direction we are moving.
		private Vector2 movement;
		// The GameObjects Rigidbody.
		private Rigidbody2D rb;
		// The Character Manager.
		private Character_Manager characterManager;
		// The Character Stats.
		private Character_Stats charStats;
		// Holders for the movements.
		private float moveHorizontal;
		private float moveVertical;


		void Awake(){
			// Get the Characters Manager component.
			characterManager = Character_Helper.GetPlayerManager ().GetComponent<Character_Manager> ();
			// Get the characters stats as we use that to potentially alter movement.
			charStats = characterManager.GetComponentInChildren<Character_Stats> ();
			// Get the Rigidbody2D Component.
			rb = GetComponent<Rigidbody2D> ();
		}

		void Update() {
			// IF we are allowed to move.
			if (characterManager.canMove) {
				// Get a -1, 0 or 1.
				moveHorizontal = Input.GetAxis ("Horizontal");
				moveVertical = Input.GetAxis ("Vertical");
			}
		}
		
		void FixedUpdate() {
			// IF we are able to move.
			// ELSE IF we cannot move.
			if(characterManager.canMove){
				// Get Vector2 direction.
				movement = new Vector2(moveHorizontal * characterManager.playerInvertX, moveVertical * characterManager.playerInvertY);
				// Apply direction with speed, alterspeed and if we have the ability to even move.
				movement *= charStats.CurrentMoveSpeed * characterManager.alterSpeed;
				// IF the user has an animation set and ready to go.
				PlayAnimation(moveHorizontal, moveVertical);
				// Apply force.
				rb.AddForce(movement);
			}else if(!characterManager.currentlyJolted){
				rb.velocity = Vector2.zero;
			}
		}

		void PlayAnimation(float hor, float vert){
			// IF the user has an animation set and ready to go.
			if (characterManager.characterAnimator != null) {
				// IF the character has a Four Direction Animation,
				// ELSE IF the character has a Eight Direction Animation.
				if (characterManager.fourDirAnim) {
					// Play animations.
					Grid_Helper.helper.FourDirectionAnimation (hor * characterManager.playerInvertX, vert * characterManager.playerInvertY, characterManager.characterAnimator);
				} else if (characterManager.eightDirAnim) {
					// Play animation.
					Grid_Helper.helper.EightDirectionAnimation (hor * characterManager.playerInvertX, vert * characterManager.playerInvertY, characterManager.characterAnimator);
				}
			}
		}			
	}
}
