using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// State machine for any characters that are casting.
	/// </summary>
	public class StateMachine_Casting : StateMachineBehaviour {

		private float skillCastingTime = 0;

		 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// Lets get our casting period.
			//skillCastingTime = Grid_Helper.skillManager.GetCurrentlyUsedSkill ().CastTime;
			// We do not want our player to move while casting.
			Character_Helper.GetPlayerManager ().GetComponent <Character_Manager> ().canMove = false;
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// Reduce our time while we are in here.
			skillCastingTime -= Time.deltaTime;
			// IF we have made it through the whole cast time.
			if (skillCastingTime <= 0) {
				// Set our animation to the next part.  We are not Casting anymore BUT we are now about to Cast.
				animator.SetBool ("IsCasting", false);
				animator.SetBool ("IsCast", true);
			}
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// Leaving this state no matter what results in our IsCasting to be false.
			animator.SetBool ("IsCasting", false);
			// Casting is done so we either are going to cast the skill right after OR we got interrupted which means no more skill.
			animator.SetInteger ("CastType", 0);
			// Since we are no longer casting we can move again.
			Character_Helper.GetPlayerManager ().GetComponent <Character_Manager> ().canMove = true;
		}
	}
}
