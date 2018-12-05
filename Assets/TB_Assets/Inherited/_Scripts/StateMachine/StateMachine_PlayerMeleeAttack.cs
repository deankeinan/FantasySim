using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	public class StateMachine_PlayerMeleeAttack : StateMachineBehaviour {

		[SerializeField] private int sortOrderGap = 10;
		private SpriteRenderer playerRenderer;
		private SpriteRenderer weaponRenderer;
		private GameObject weaponGameObject;


		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// Reference the Character Manager.
			Character_Manager charManager = animator.GetComponentInParent <Character_Manager> ();
			// Get our player Renderer.
			playerRenderer = charManager.characterEntity.GetComponent <SpriteRenderer> ();
			// Get our weapon.
			Item meleeWeapon = Character_Helper.GetPlayerManager ().GetComponentInChildren <Equipment> ().GetWeapon ();


			// Now we need to know which way we are facing to position the weapon.
			weaponGameObject = Instantiate (Grid_Helper.setup.GetGameObjectPrefab (meleeWeapon.WeaponSlug), charManager.GetMeleeLocation (animator.GetInteger ("Direction")));
			// Get our weapon Renderer.
			weaponRenderer = weaponGameObject.GetComponent <SpriteRenderer> ();
			// Set the layer of the character entity.
			weaponRenderer.sortingLayerName = playerRenderer.sortingLayerName;
			// Set the owner to this weapon.
			weaponGameObject.GetComponent <Owner> ().owner = animator.gameObject;
			// Set the damage to this weapon by putting the Item's damage as the parameter.
			weaponGameObject.GetComponent <Owner> ().damage = charManager.GetComponentInChildren <Character_Stats> ().GetCurrentDamage ();


//			// IF this is NOT a full body sprite and we are going into the children to find a body reference.
//			if (playerRenderer == null) {
//				// Cache the Transform of the player.
//				Transform charTransform = charManager.characterEntity.transform;
//				// Loop the amount of children we have.
//				for (int x = 0; x < charTransform.childCount; x++) {
//					// IF the tag of the child gameobject is Body
//					if (charTransform.GetChild (x).tag == "Body") {
//						// Get the Sprite Renderer for the body reference.
//						playerRenderer = charTransform.GetChild (x).GetComponent <SpriteRenderer> ();
//						// Leave.
//						break;
//					}
//				}
//			}

			// Set the order layer it needs to be in.
			// IF the weapon should be displayed behind of the player,
			// ELSE the weapon should be displayed infront the player.
			if (animator.GetInteger ("Direction") == 1) {
				// Set the weapon behind the player.
				weaponRenderer.sortingOrder = playerRenderer.sortingOrder - sortOrderGap;
			} else {
				// Set the weapon infront of the player.
				weaponRenderer.sortingOrder = playerRenderer.sortingOrder + sortOrderGap;
			}
			// Play the attack sound (if there is one).
			Grid_Helper.soundManager.PlaySound (meleeWeapon.AttackSound);
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// IF we have made a full cycle through our animation AND we are currently not in a transition,
			// ELSE we are midway in the cycle so we care about sorting order.
			if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1 && !animator.IsInTransition (0)) {
				// Destroy our weapon.
				Destroy (weaponGameObject);
				// Set our IsAttacking to false.
				animator.SetBool ("IsAttacking", false);
				// Make it so that our player can move.
				Character_Helper.GetPlayerManager ().GetComponent <Character_Manager> ().canMove = true;
			} else {
//				// IF the weapon should be displayed behind of the player,
//				// ELSE the weapon should be displayed infront the player.
//				if (animator.GetInteger ("Direction") == 1) {
//					// Set the weapon behind the player.
//					weaponRenderer.sortingOrder = playerRenderer.sortingOrder - sortOrderGap;
//				} else {
//					// Set the weapon infront of the player.
//					weaponRenderer.sortingOrder = playerRenderer.sortingOrder + sortOrderGap;
//				}
			}
		}

		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// Destroy our weapon.
			Destroy (weaponGameObject);
			// We are not longer attacking in our animation since we are leaving this state.
			animator.SetBool ("IsAttacking", false);
			// Make it so that our player can move.
			Character_Helper.GetPlayerManager ().GetComponent <Character_Manager> ().canMove = true;
		}
	}
}
