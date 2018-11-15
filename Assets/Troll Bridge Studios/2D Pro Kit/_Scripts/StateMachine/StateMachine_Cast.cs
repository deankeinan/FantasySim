using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// State machine for a character when they actually cast a skill.
	/// </summary>
	public class StateMachine_Cast : StateMachineBehaviour {

		// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// IF we have made a full cycle through our Cast animation AND we are currently not in a transition.
			if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1 && !animator.IsInTransition (0)) {
				// Set our IsCast to false.
				animator.SetBool ("IsCast", false);
				// The final cast is done so we either are going to cast the skill right after so we have completed our cast.
				animator.SetInteger ("SkillAnimation", 0);
			}
		}

		// OnStateExit is called before OnStateExit is called on any state inside this state machine
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// Since we are leaving our Cast we set our IsCast to false.
			animator.SetBool ("IsCast", false);
			// If something yanks us out of this state such as if get hit by a monster in the middle of this final Cast.
			animator.SetInteger ("SkillAnimation", 0);
		}
	}
}
