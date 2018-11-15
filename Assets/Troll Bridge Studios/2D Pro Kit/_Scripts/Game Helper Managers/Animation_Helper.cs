using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Animation helper that holds all methods that deal with altering animation variables.
	/// </summary>
	public class Animation_Helper : MonoBehaviour {

		/// <summary>
		/// Assign our animation variables to handle our Idle state.
		/// </summary>
		public void SetAnimationsIdle (Animator anim) {
			anim.SetBool ("IsIdle", true);
			anim.SetBool ("IsMoving", false);
		}

		/// <summary>
		/// Assign our animation variables to handle our Moving state.
		/// </summary>
		public void SetAnimationsWalk (Animator anim) {
			// We are moving.
			anim.SetBool ("IsIdle", false);
			anim.SetBool ("IsMoving", true);
			// We cannot craft while moving.
			anim.SetBool ("IsCrafting", false);
		}

		/// <summary>
		/// Assign our animation variables based on being able to attack standing idle.
		/// </summary>
		public void SetAnimationsAttack (Animator anim) {
			anim.SetBool ("IsIdle", true);
			anim.SetBool ("IsMoving", false);
			anim.SetBool ("IsAttacking", true);
			anim.SetInteger ("SkillAnimation", 0);
			anim.SetBool ("IsCasting", false);
			anim.SetBool ("IsCast", false);
			// We cannot craft while attacking.
			anim.SetBool ("IsCrafting", false);
		}

		/// <summary>
		/// Sets the animations based on the skill we use.
		/// </summary>
		public void SetAnimationsSkill (Animator anim, int skill, float castTime) {
			anim.SetBool ("IsAttacking", false);
			anim.SetInteger ("SkillAnimation", skill);
			// We cannot craft while casting a skill.
			anim.SetBool ("IsCrafting", false);

			// IF our cast time equals 0.  AKA - This is an Instant Cast Skill,
			// ELSE our cast time does not equal 0.  AKA - This is not an Instant Cast,
			if (castTime == 0f) {
				// This IS an instant cast so NO casting.
				anim.SetBool ("IsCasting", false);
				anim.SetBool ("IsCast", true);
			} else {
				// This is NOT an instant cast so we WILL be casting.
				anim.SetBool ("IsCasting", true);
				anim.SetBool ("IsCast", false);
				// Since we are casting we are currently idle.
				anim.SetBool ("IsIdle", true);
				// Since we are idle we are not moving.
				anim.SetBool ("IsMoving", false);
			}
		}

		/// <summary>
		/// Sets the animations based on the character being hit.
		/// </summary>
		public void SetAnimationsHit (Animator anim, float hitAnimationTime) {
			// Since we are being hit we need to make sure our other animation variables are properly set.
			anim.SetBool ("IsIdle", false);
			anim.SetBool ("IsMoving", false);
			anim.SetBool ("IsAttacking", false);
			anim.SetBool ("IsCasting", false);
			anim.SetBool ("IsCast", false);
			anim.SetInteger ("SkillAnimation", 0);
			anim.SetBool ("IsHit", true);
			// We cannot craft when being attacked.
			anim.SetBool ("IsCrafting", false);
			// Set our Hit Animations.
			StartCoroutine (HitAnimation (anim, hitAnimationTime));
		}

		private IEnumerator HitAnimation (Animator anim, float hitAnimationTime) {
			// Pause for our how long we want to be in this animation.
			yield return new WaitForSeconds (hitAnimationTime);
			// IF we destroy this GO before it ends it animation.
			if (anim != null) {
				// We go back to not being hit.
				anim.SetBool ("IsHit", false);
			}
		}

		/// <summary>
		/// Sets the animations to dead when the character dies.
		/// </summary>
		public void SetAnimationsDead (Animator anim) {
			// Since we are dying we need to set IsDead to true and all the other actions to false.
			anim.SetBool ("IsAttacking", false);
			anim.SetBool ("IsCasting", false);
			anim.SetBool ("IsCast", false);
			anim.SetInteger ("SkillAnimation", 0);
			// We cannot craft when we are dead.
			anim.SetBool ("IsCrafting", false);
			anim.SetBool ("IsDead", true);
		}

		/// <summary>
		/// Sets the animation to crafting when the character begins a craft.
		/// </summary>
		public void SetAnimationCraft (Animator anim) {
			// Since we are crafting we need to set IsCrafting to true and all the other actions to false besides IsIdle.
			anim.SetBool ("IsIdle", true);
			anim.SetBool ("IsMoving", false);
			anim.SetBool ("IsAttacking", false);
			anim.SetBool ("IsCasting", false);
			anim.SetBool ("IsCast", false);
			anim.SetInteger ("SkillAnimation", 0);
			anim.SetBool ("IsHit", false);
			// We are crafting.
			anim.SetBool ("IsCrafting", true);
		}

		/// <summary>
		/// Sets the animation of a finished craft.
		/// </summary>
		public void SetAnimationFinishCraft (Animator anim) {
			// We are done crafting so we set our IsCrafting to false.
			anim.SetBool ("IsCrafting", false);
		}
	}
}
