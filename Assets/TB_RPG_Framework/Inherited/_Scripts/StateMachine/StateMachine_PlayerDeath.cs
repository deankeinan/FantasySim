using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	public class StateMachine_PlayerDeath : StateMachineBehaviour {

		// OnStateEnter is called before OnStateEnter is called on any state inside this state machine
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// Easier referencing.
			Character_Manager charManager = animator.gameObject.GetComponentInParent <Character_Manager> ();
			// Display health as 0.
			charManager.characterStats.CurrentHealth = 0f;
			// Remove the collider to prevent any more damage being taken to cause errors.
			charManager.characterCollider.enabled = false;
			// Make the player not be able to control the character while dead.
			charManager.canMove = false;
		}
	}
}
