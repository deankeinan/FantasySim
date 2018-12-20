using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// State machine that deals with projectiles created from the character.
	/// </summary>
	public class StateMachine_CharacterSkillProjectile : StateMachineBehaviour {

		[SerializeField] private GameObject projectile;


	    // OnStateEnter is called before any state inside this state machine.
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// Reference the Character Manager.
			Character_Manager charManager = animator.GetComponentInParent <Character_Manager> ();
			// Lets create our projectile based on the direction of the player.
			GameObject skill = Instantiate (projectile, charManager.GetProjectileLocation (animator.GetInteger ("Direction")), Quaternion.identity);

			// Get the currenly used skill.
			//Skill skillUsed = Grid_Helper.skillManager.GetCurrentlyUsedSkill ();
			// Set the damage of this skill.
			//skill.GetComponent <Owner> ().damage = Grid_Helper.skillManager.CalculateSkillDamage (skillUsed, charManager.characterStats);
			// Set the owner of the skill.
			skill.GetComponent <Owner> ().owner = animator.gameObject;
		}
	}
}
