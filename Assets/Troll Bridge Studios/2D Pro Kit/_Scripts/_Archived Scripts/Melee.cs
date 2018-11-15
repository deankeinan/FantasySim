using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	public class Melee : Owner {

		[Header ("Damage Information")] 
		// Set a manual damage if this isn't coming from a Entity that has Character_Stats.
		[SerializeField] private float manualDamage = 0f;
		[SerializeField] private CharacterType[] damageTheseTypes;
		[SerializeField] private float joltAmount = 0f;
		[SerializeField] private bool setLayerFromItsCreator = false;


		void Start () {
			// IF you want to set a layer of this melee object based on who created it AND lets make sure we have an owner.
			if (setLayerFromItsCreator && owner != null) {
				// Change the layer of this gameobject.
				gameObject.layer = owner.layer;
			}

			// It would make sense to check and see if "damage" is 0 OR if we have input a manual damage.  
			// IF we did not have something calculate our damage on creation, meaining we did NOT have a Character_Stats
			// to supply our damage numbers so we MUST of set a manual damage for this melee object.
			if (damage == 0 || manualDamage != 0) {
				// Just set the manualDamage as that HAS to have a number that isn't 0 unless there was a screw up on
				// the developers part.
				damage = manualDamage;
			}
		}

		// When we collide with something.
		void OnCollisionEnter2D (Collision2D coll){
			// Send the GameObject we are colliding with.
			MeleeCollision (coll.gameObject);		
		}

		// When we collide with something.
		void OnTriggerEnter2D (Collider2D coll){
			// Send the GameObject we are colliding with.
			MeleeCollision (coll.gameObject);		
		}

		// Pass our colliding GameObject through some tests.
		private void MeleeCollision (GameObject collGameObject) {
			// IF YOU HAVE ANOTHER SCRIPT IN WHICH YOU DO NOT CARE ABOUT USING CHARACTER_MANAGER
			// THEN MAKE SURE TO LOOK FOR THE SCRIPT HERE AND MAKE A CHECK IF IT HAS IT.

			// Grab the Character_Manager Component from the colliding object.
			Character_Manager otherChar = collGameObject.GetComponentInParent<Character_Manager> ();
			// IF we do not have a Character Manager.
			if (otherChar == null) {
				// We leave as we only care about doing damage to Characters.
				return;
			}

			// Start our loop to know if we should damage the GameObject.
			Grid_Helper.helper.DamageCharacterTypeLoop (transform, otherChar, damageTheseTypes, damage, joltAmount);
		}
	}
}
