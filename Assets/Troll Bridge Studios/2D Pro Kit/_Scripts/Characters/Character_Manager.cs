using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	public class Character_Manager : Character {

		[Header ("Misc")]
		// The time it takes to use your main attack again right after you use it.
		public float MainAttackCooldown = 0.2f;
		// Can this character be jolted (knockbacked) when taking damage;
		public bool canBeJolted;
		// Are we currently being jolted.
		public bool currentlyJolted = false;
		// Options for player interactions.
		public float hitAnimationTime = 0.2f;

		// The List of Area Dialogues currently inside.
		public List<Area_Dialogue> ListOfAreaDialogues = new List<Area_Dialogue>();


		void Awake () {
			// The transform of the Character Entity for referencing.
			characterTransform = characterEntity.GetComponent<Transform> ();
			// Get the Character Collider2D.
			characterCollider = characterEntity.GetComponent<Collider2D> ();
			// Get the Animator Component.
			characterAnimator = characterEntity.GetComponent<Animator> ();
			// Get the Rigidbody2D Component.
			characterRigidBody2D = characterEntity.GetComponent<Rigidbody2D> ();
			// Get the Money/Currency Script.
			characterCurrency = GetComponentInChildren<Money> ();
			// Get the Equipment Script.
			characterEquipment = GetComponentInChildren<Equipment> ();
			// Get the Character Stats script.
			characterStats = GetComponentInChildren<Character_Stats> ();
			// Get the Character Class.
			characterClass = GetComponentInChildren<Character_Class> ();
		}

		void Start () {
			// IF there is a animator on the Character.
			if (characterAnimator != null) {
				// IF the Animator is a Two Direction Animation,
				// ELSE IF the Animator is a Four Direction Animation,
				// ELSE IF the Animator is a Eight Direction Animation.
				if (characterAnimator.GetLayerName (0) == "Two Base") {
					// Set to true that we have a Two Direction Animation.
					twoDirAnim = true;
					// Set true that we have a Four Direction Animation.
					fourDirAnim = false;
					// Set false that we have a Eight Direction Animation.
					eightDirAnim = false;
				} else if (characterAnimator.GetLayerName (0) == "Four Base") {
					// Set to false that we have a Two Direction Animation.
					twoDirAnim = false;
					// Set true that we have a Four Direction Animation.
					fourDirAnim = true;
					// Set false that we have a Eight Direction Animation.
					eightDirAnim = false;
				} else if (characterAnimator.GetLayerName (0) == "Eight Base") {
					// Set to false that we have a Two Direction Animation.
					twoDirAnim = false;
					// Set false since we dont have a Four Direction Animation.
					fourDirAnim = false;
					// Set true since we dont have a Eight Direction Animation.
					eightDirAnim = true;
				}
			}
			// Load the latest Layer, Sorting Layer and Sorting Layer Order.
			LoadLayers ();
		}

		void Update () {
			// Find out how much the character has moved.
			characterAmountMoved = (Vector2) characterTransform.position - characterPrevLocation;

			// IF we have a Hero character type (The Player),
			// ELSE we have a Non Hero character type (NPC's).
			if (characterType == CharacterType.Hero) {
				// Updates to check for with our Hero.
				HeroUpdate ();
			}else {
				// Updates to check for with a NPC.
				NPCUpdate (characterAmountMoved.x, characterAmountMoved.y);
			}

			// Set where we were.
			characterPrevLocation = characterTransform.position;
		}


		/// <summary>
		/// The character is a hero so we will take all actions for our hero.
		/// </summary>
		private void HeroUpdate () {
			
		}

		/// <summary>
		/// The character is a NPC so we will take all actions for our NPC.
		/// </summary>
		private void NPCUpdate (float amountMovedX, float amountMovedY) {
			// IF the character has an animation set and ready to go.
			if (characterAnimator != null) {
				// IF there is an Action Key Dialogue currently running,
				// ELSE IF the character can move.
				if (isInteracting) {
					// Handle the direction the NPC is looking.
					NPCLookDirection ();
				} else if (canMove) {
					// Play the animation.
					PlayAnimation (amountMovedX, amountMovedY);
				}
			}
		}

		/// <summary>
		/// Method to decide which way this NPC should be facing for our animation purposes.
		/// </summary>
		private void NPCLookDirection () {
			// Store the focused objects Transform.
			Transform focTransform = interactionFocusTarget.transform;
			// IF we have a Two Direction Animation for this gameobject,
			// ELSE IF we have a Four Direction Animation for this gameobject,
			// ELSE IF we have a Eight Direction Animation for this gameobject.
			if (twoDirAnim) {
				// Make this gameobjects animation face in the direction desired.
				Grid_Helper.helper.TwoDirectionAnimation (focTransform.position.x - characterTransform.position.x, 
					focTransform.position.y - characterTransform.position.y, characterAnimator);
			} else if (fourDirAnim) {
				// Make this gameobjects animation face in the direction desired.
				Grid_Helper.helper.FourDirectionAnimation (focTransform.position.x - characterTransform.position.x, 
					focTransform.position.y - characterTransform.position.y, characterAnimator);
			} else if (eightDirAnim) {
				// Make this gameobjets animation face in the direction desired.
				Grid_Helper.helper.EightDirectionAnimation (focTransform.position.x - characterTransform.position.x, 
					focTransform.position.y - characterTransform.position.y, characterAnimator);
			} else {
				Debug.Log ("Something went wrong when trying to figure out which animation you have.");
			}
		}

		/// <summary>
		/// Method that sets our animation variables based on our 2 parameters "hor" and "vert".
		/// </summary>
		private void PlayAnimation (float hor, float vert) {
			// IF the NPC has a Two Direction Animation,
			// ELSE IF the NPC has a Four Direction Animation,
			// ELSE IF the NPC has a Eight Direction Animation.
			if (twoDirAnim) {
				// Play animations.
				Grid_Helper.helper.TwoDirectionAnimation (hor, vert, characterAnimator);
			}else if (fourDirAnim) {
				// Play animations.
				Grid_Helper.helper.FourDirectionAnimation (hor, vert, characterAnimator);
			} else if (eightDirAnim) {
				// Play animation.
				Grid_Helper.helper.EightDirectionAnimation (hor, vert, characterAnimator);
			} else {
				Debug.Log ("Something went wrong when trying to figure out which animation you have.");
			}
		}

		/// <summary>
		/// When the character takes damage we are either Dead or Alive and dealing with the actions.
		/// </summary>
		public void TakeDamage (float damage, Transform damagingTransform, float joltAmount) {
			// Need to check and see if we can even take damage.  We do not want anything taking damage during a jolt phase.
			if (currentlyJolted) {
				// We leave.
				return;
			}

			// Remove HP.
			characterStats.SubtractHealth (damage);
			// IF we are dead.
			if (characterStats.CurrentHealth <= 0f) {
				// We DIEDEDED!!! NOOOOOOOO....
				Death ();
			} else {
				// We are hit.
				Hit (damagingTransform, joltAmount);
			}
		}

		/// <summary>
		/// Everything you want to happen when the character dies.
		/// </summary>
		private void Death () {
			// Set our animation variables.
			Grid_Helper.animHelper.SetAnimationsDead (characterAnimator);
		}

		/// <summary>
		/// Everything you want to happen when the Character takes damage but doesn't die.
		/// </summary>
		private void Hit (Transform otherTransform, float joltAmount) {
			// Set our animation variables.
			Grid_Helper.animHelper.SetAnimationsHit (characterAnimator, hitAnimationTime);

			// IF the character that we collided with can be knocked back.
			if (canBeJolted) {
				// Knock GameObject back.
				Knockback (otherTransform, joltAmount);
				// Make the Hero not be able to control the character while being knockedback.
				StartCoroutine (NoCharacterControl());
			}
		}

		/// <summary>
		/// Method that handles our knockbacks.  characterEntity will be the entity that will be getting the knockback.
		/// </summary>
		public void Knockback (Transform otherTransform, float joltAmount) {
			// Get the relative position.
			Vector2 relativePos = characterEntity.transform.position - otherTransform.position;
			// Get the rigidbody2D
			Rigidbody2D charRigid = characterEntity.GetComponent<Rigidbody2D> ();
			// Stop the colliding objects velocity.
			charRigid.velocity = Vector3.zero;
			// Apply knockback.
			charRigid.AddForce (relativePos.normalized * joltAmount, ForceMode2D.Impulse);
		}

		/// <summary>
		/// Method that handles everything we want when we want the character to not be in control.  Based on our hitAnimationTime
		/// dictates how long we will not be in control for when we call this method.
		/// </summary>
		private IEnumerator NoCharacterControl() {
			// Make the player not be able to control the character while the knockback is happening.
			canMove = false;
			// We are currently being knockbacked.
			currentlyJolted = true;
			// Wait for 'HitAnimationTime' before being able to control the character again.
			yield return new WaitForSeconds (hitAnimationTime);
			// Stop the knockback.
			characterEntity.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			// We can now move the character.
			canMove = true;
			// We are not being jolted anymore.
			currentlyJolted = false;
		}
			
		/// <summary>
		/// Returns the position based on the direction our Character is facing for us to use for our skill or any other purpose.
		/// </summary>
		public Vector3 GetProjectileLocation (int direction) {
			// IF we are facing North/Up,
			// ELSE IF we are facing West/Left,
			// ELSE IF we are facing South/Down,
			// ELSE IF we are facing East/Right
			if (direction == 1) {
				return projectileLocationNorth.position;
			} else if (direction == 2) {
				return projectileLocationWest.position;
			} else if (direction == 3) {
				return projectileLocationSouth.position;
			} else {
				return projectileLocationEast.position;
			}
		}

		/// <summary>
		/// Returns the melee location Transform.  We use this to for when we create our weapons we place them as a child
		/// of one of these GameObjects returned.
		/// </summary>
		public Transform GetMeleeLocation (int direction) {
			// IF we are facing North/Up,
			// ELSE IF we are facing West/Left,
			// ELSE IF we are facing South/Down,
			// ELSE IF we are facing East/Right
			if (direction == 1) {
				return meleeLocationNorth.transform;
			} else if (direction == 2) {
				return meleeLocationWest.transform;
			} else if (direction == 3) {
				return meleeLocationSouth.transform;
			} else {
				return meleeLocationEast.transform;
			}
		}

		/// <summary>
		/// Returns the rotation of our Sword type weapon.  We have Sword type weapons do a clockwise slash of 120 degrees.
		/// This will work properly with a 4 direction animation as this needs to be catered if you are adding dialgonal 
		/// animations.
		/// </summary>
		public float GetSwordRotation (int direction) {
			// IF we are facing North/Up,
			// ELSE IF we are facing West/Left,
			// ELSE IF we are facing South/Down,
			// ELSE IF we are facing East/Right
			if (direction == 1) {
				return 150f;
			} else if (direction == 2) {
				return 240f;
			} else if (direction == 3) {
				return 330f;
			} else {
				return 60f;
			}
		}

		/// <summary>
		/// Gets the straight angle of looking up, down, left and right.  Basically you are looking up then you are looking
		/// in the 90 degree direction as Right is 0, Up is 90, Left is 180 and Down is 270.
		/// </summary>
		public float GetStraightSwordRotation (int direction) {
			// IF we are facing North/Up,
			// ELSE IF we are facing West/Left,
			// ELSE IF we are facing South/Down,
			// ELSE IF we are facing East/Right
			if (direction == 1) {
				return 90f;
			} else if (direction == 2) {
				return 180f;
			} else if (direction == 3) {
				return 270f;
			} else {
				return 0f;
			}
		}

		/// <summary>
		/// Changes the layer of the player.
		/// </summary>
		public void ChangeLayer (int layerMaskValue) {
			// Change the Collision Layer of the colliding GameObject.
			characterEntity.layer = (int)Mathf.Log (layerMaskValue, 2);
			// Loop through all the children.
			for (int i = 0; i < characterEntity.transform.childCount; i++) {
				// Get the child of this gameobject.
				GameObject child = characterEntity.transform.GetChild (i).gameObject;
				// Change the Collision Layer of the colliding GameObject.
				child.layer = (int)Mathf.Log (layerMaskValue, 2);
			}
		}

		public void ChangeSortingLayer (string sortLayer, int sortLayerOrder) {
			// Change the Sort Layer and Order Number.
			GetComponent<SpriteRenderer> ().sortingLayerName = sortLayer;
			GetComponent<SpriteRenderer> ().sortingOrder = sortLayerOrder;
		}

		/// <summary>
		/// Save the player stats.
		/// </summary>
		public void SavePlayer(){
			// Save the Inventory.
			Grid_Helper.inventory.Save ();
			// Save the Class Attributes.
			characterClass.Save ();
			// Save the Character Stats.
			characterStats.Save ();
			// Save the Equipment.
			characterEquipment.Save ();
			// Save the types of Currencies/Money.
			GetComponentInChildren<Money> ().Save ();
			// Save the Keys.
			GetComponentInChildren<Key> ().Save ();
			// Save the Bombs.
			GetComponentInChildren<Bombs> ().SaveBombs ();

			// Save the Layer, Sorting Layer and Sorting Layer Order.
			int playerMask = gameObject.layer;
			string playerLayerName = characterEntity.GetComponent<SpriteRenderer> ().sortingLayerName;
			int playeLayerOrder = characterEntity.GetComponent<SpriteRenderer> ().sortingOrder;
			// Save this information.
			PlayerPrefs.SetInt ("playerLayer", playerMask);
			PlayerPrefs.SetString ("playerLayerName", playerLayerName);
			PlayerPrefs.SetInt ("playeLayerOrder", playeLayerOrder);
		}

		public void LoadLayers () {
			// IF this isn't our hero.
			if (characterType != CharacterType.Hero) {
				// We leave.
				return;
			}

			// IF this is the first time here.
			if (string.IsNullOrEmpty (PlayerPrefs.GetString ("playerLayerName"))) {
				// Done.
				return;
			}
			// Easier referencing.
			int playerLayer = PlayerPrefs.GetInt ("playerLayer");
			string sortingLayerName = PlayerPrefs.GetString ("playerLayerName");
			int sortingLayerOrder = PlayerPrefs.GetInt ("playeLayerOrder");

			// Load this information on our character entity and its children.
			characterEntity.layer = playerLayer;
			characterEntity.GetComponent<SpriteRenderer> ().sortingLayerName = sortingLayerName;
			characterEntity.GetComponent<SpriteRenderer> ().sortingOrder = sortingLayerOrder;

			// Set the Weapon layer.
			meleeWeapon.GetComponent<SpriteRenderer> ().sortingLayerName = sortingLayerName;

			// For the kids!
			for (int i = 0; i < characterEntity.transform.childCount; i++) {
				// IF there is a sprite renderer
				if (characterEntity.transform.GetChild (i).GetComponent<SpriteRenderer> () == null) {
					// Skip to the next iteration.
					continue;
				}
				// Set the layers.
				characterEntity.transform.GetChild (i).gameObject.layer = playerLayer;
				characterEntity.transform.GetChild (i).GetComponent<SpriteRenderer> ().sortingLayerName = sortingLayerName;
				characterEntity.transform.GetChild (i).GetComponent<SpriteRenderer> ().sortingOrder = sortingLayerOrder;
			}
		}
	}
}
