using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	public class StateMachine_ShowDeathCanvas : StateMachineBehaviour {

		private bool isShown = false;

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// IF we have made a full cycle through our animation AND we are currently not in a transition AND our death panel is not being shown.
			if (animator.GetCurrentAnimatorStateInfo (layerIndex).normalizedTime >= 1 && !animator.IsInTransition (layerIndex) && !isShown) {
				// Start a coroutine that moves the Death Panel from 1 point to another point.
				Grid_Helper.endPanel.MoveStartToEnd();
				// Our death canvas is being shown.
				isShown = true;
			}
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// Our death canvas is NOT being show as we are leaving this State.
			isShown = false;
		}
	}
}
