using UnityEngine;

namespace TrollBridge {

	[RequireComponent (typeof (Rigidbody2D))]
	/// <summary>
	/// Eight direction movement allows us to move : up, down, left, right, up-right, up-left, down-right and down-left.
	/// </summary>
	public class Eight_Direction_Movement : MonoBehaviour {

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


		void Awake () {
			// Get the Character Manager component.
			characterManager = Character_Helper.GetPlayerManager ().GetComponent<Character_Manager> ();
			// Get the characters stats as we use that to potentially alter movement.
			charStats = characterManager.GetComponentInChildren<Character_Stats> ();
			// Get the Rigidbody2D Component.
			rb = GetComponent<Rigidbody2D> ();
		}

		void Update(){
			// IF we are allowed to move.
			if (characterManager.canMove) {
				// Get a -1, 0 or 1.
				moveHorizontal = Input.GetAxisRaw ("Horizontal");
				moveVertical = Input.GetAxisRaw ("Vertical");
			}
			// Get Vector2 direction.
			movement = new Vector2 (moveHorizontal * characterManager.playerInvertX, moveVertical * characterManager.playerInvertY);
			// Apply direction with speed and alterspeed.
			movement *= charStats.CurrentMoveSpeed * characterManager.alterSpeed;
		}

		void LateUpdate () {
			// IF we are not above our movement threshhold.
			if (Mathf.Abs (movement.x) < 0.001) {
				movement.x = 0;
			}

			if (Mathf.Abs (movement.y) < 0.001) {
				movement.y = 0;
			}
			// Make sure we truncate our position.
			gameObject.transform.position = new Vector2 (Mathf.Round(gameObject.transform.position.x * 100f) / 100f, Mathf.Round(gameObject.transform.position.y * 100f) / 100f);

		}

		void FixedUpdate() {
			// IF we are able to move.
			// ELSE IF we are not able to be jolted.
			if (characterManager.canMove) {
				// IF the character has an animation set.
				PlayAnimation (moveHorizontal, moveVertical);
				// Apply the force for movement.
				rb.AddForce (movement);
			} else if (!characterManager.currentlyJolted) {
				// No movement.
				rb.velocity = Vector2.zero;
			}
		}

		/// <summary>
		/// Play the Animation of this GameObject based on if there is a 4 direction or 8 direction animation.
		/// </summary>
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
