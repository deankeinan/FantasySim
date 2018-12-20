using UnityEngine;
using System.Collections;

namespace TrollBridge{

	public class Attack_Timer : MonoBehaviour {

		// The gameobject representation of our weapon.
		[SerializeField] private GameObject weapon;
		[SerializeField] private Animator characterAnimator;

		public void DoWeShowWeapon(){
			weapon.SetActive (true);
		}

//		public void AttackTimer(){
//			characterAnimator.SetBool ("IsAttacking", false);
//			Character_Manager.GetPlayerManager ().GetComponent<Player_Manager> ().ChangeCanMove (true);
//			weapon.SetActive (false);
//		}
	}
}
