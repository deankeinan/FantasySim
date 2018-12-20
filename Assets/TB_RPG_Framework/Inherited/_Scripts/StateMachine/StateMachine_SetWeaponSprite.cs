using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	public class StateMachine_SetWeaponSprite : StateMachineBehaviour {

		// Purpose of this script is to just set the GameObject's Sprite Renderer to our currently equipped weapon sprite.
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			// Referencing to make code easier to read.
			Character_Manager charManager = animator.GetComponentInParent <Character_Manager> ();
			Equipment charEquipment = charManager.characterEquipment;
			SpriteRenderer weaponRenderer = charManager.meleeWeapon.GetComponent <SpriteRenderer> ();
			Item weapon = charEquipment.GetWeapon ();
			// Let's set the weapon sprite to look like our currently equipped weapon.
			weaponRenderer.sprite = weapon.SpriteImage;
			weaponRenderer.color = new Color (weapon.R, weapon.B, weapon.G, weapon.A);
		}
	}
}
