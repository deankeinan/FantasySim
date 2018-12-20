using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	public class StateMachine_Visuals : StateMachineBehaviour {

		[Tooltip ("GameObject you want to create when we enter this State.")]
		[SerializeField] private GameObject[] stateEnterVisual;
		[Tooltip ("GameObject you want to create when we exit this State.")]
		[SerializeField] private GameObject[] stateExitVisual;

		// OnStateEnter is called before OnStateEnter is called on any state inside this state machine.
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// IF we have a enter visual for this state.
			if (stateEnterVisual.Length != 0) {
				// Loop the amount of times we have enter visuals.
				for (int i = 0; i < stateEnterVisual.Length; i++) {
					// Spawn the destroy effects.
					Grid_Helper.helper.SpawnObject(stateEnterVisual[i], animator.transform.position, Quaternion.identity, animator.gameObject);
				}
			}
		}

		// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine.
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// IF we have made a full cycle through our animation AND we are currently not in a transition.
			if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1 && !animator.IsInTransition (0)) {
				// IF we have a exit visual for this state.
				if (stateExitVisual.Length != 0) {
					// Loop the amount of times we have exit visuals.
					for (int i = 0; i < stateExitVisual.Length; i++) {
						// Create the visual for finishing the animation meaning we want to display something at the end of 
						// each animation.
						Grid_Helper.helper.SpawnObject(stateExitVisual[i], animator.transform.position, Quaternion.identity, animator.gameObject);
					}
				}
			}
		}
	}
}
