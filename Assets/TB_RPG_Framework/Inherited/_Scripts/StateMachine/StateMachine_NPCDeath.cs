using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// State machine NPC death.  In this statemachine when a non player character dies we will go through a list of 
	/// mechanics to get a proper death.  Death in NPCs gives Loot, Quest Oriented Credit and the Destruction of itself.
	/// </summary>
	public class StateMachine_NPCDeath : StateMachineBehaviour {

//		// When this gameobject is literally dead, how long do we leave it dead before we destroy it.
//		[SerializeField] private float timeTillDestroy = 0.5f;
		// Boolean to let us know to not enter this state again after it dies.
		private bool isDead = false;


		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// IF we are dead.
			if (isDead) {
				// Leave.
				return;
			}
			// We are dead.
			isDead = true;

			// IF there is a Loot component attached to this GameObject.
			if (animator.GetComponentInChildren<Loot>() != null) {
				// Drop loot.
				animator.GetComponentInChildren<Loot> ().DeathDrop();
			}
			// IF there is a Experience_NPCDeath component attached to this GameObject.
			if (animator.GetComponentInChildren<Experience_NPCDeath>() != null) {
				// Give the player experience points.
				animator.GetComponentInChildren<Experience_NPCDeath> ().GivePlayerExperience ();
			}

			// Get the Character_Manager.
			Character_Manager charManager = animator.GetComponentInParent <Character_Manager> ();
			// Make sure to stop this GameObject from moving.
			charManager.canMove = false;
			// Get all Collider2D's associated with this GameObject.
			Collider2D[] allColliders = charManager.GetComponentsInChildren <Collider2D> ();
			// Loop and set all the Collider2D's INACTIVE.
			for (int i = 0; i < allColliders.Length; i++) {
				// Set the Collider2D's INACTIVE.
				allColliders [i].enabled = false;
			}
			// Send off the kill of this GameObject to our Quest System.
			//Grid_Helper.questManager.QuestMobKill (animator.GetComponentInParent <Character_Manager>().characterEntity.name);
			// Destroy this gameobject.
			Destroy(charManager.gameObject);
//			Destroy(charManager.gameObject, timeTillDestroy);
		}
	}
}
