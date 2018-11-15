using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	public class StateMachine_AnimationEndDestroy : StateMachineBehaviour {

		// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine.
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// IF we have made a full cycle through our animation AND we are currently not in a transition.
			if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1 && !animator.IsInTransition (0)) {
				// Send this GameObject over to see what should get destroyed.
				Grid_Helper.helper.DestroyCharacterManagerGameObject (animator.gameObject);
			}
		}
	}
}
