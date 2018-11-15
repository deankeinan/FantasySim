using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	public class StateMachine_Sounds : StateMachineBehaviour {

		[Tooltip ("The AudioClip to play when we enter this State.")]
		[SerializeField] private AudioClip stateEnterSound;
		[Tooltip ("The AudioClip to play when we exit this State.")]
		[SerializeField] private AudioClip stateExitSound;
		[SerializeField] private float minimumPitch = 1.0f;
		[SerializeField] private float maximumPitch = 1.0f;

		private bool weDone = false;


		// OnStateEnter is called before OnStateEnter is called on any state inside this state machine
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// Play a sound when we enter this State.
			Grid_Helper.soundManager.PlaySound (stateEnterSound, animator.gameObject.transform.position, minimumPitch, maximumPitch);
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// IF we have made a full cycle through our Cast animation AND we are currently not in a transition.
			if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1 && !animator.IsInTransition (0)) {
				// Play a sound when we exit this State.
				Grid_Helper.soundManager.PlaySound (stateExitSound, animator.gameObject.transform.position, minimumPitch, maximumPitch);
				// Set a boolean to let us know to NOT play the OnStateExit.
				weDone = true;
			}
		}

		// OnStateExit is called before OnStateExit is called on any state inside this state machine
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// IF we are done.
			if (weDone) {
				// We leave.
				return;
			}

			// Play a sound when we exit this State.
			Grid_Helper.soundManager.PlaySound (stateExitSound, animator.gameObject.transform.position, minimumPitch, maximumPitch);
		}
	}
}
