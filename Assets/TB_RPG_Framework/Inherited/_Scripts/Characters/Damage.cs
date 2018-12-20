using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	[RequireComponent(typeof(Collider2D))]
	/// <summary>
	/// Damage script will damage gameobjects that have a Character_Manager and Character_Stats associated to it.
	/// </summary>
	public class Damage : Owner {

		[Header ("Damage Information")] 
		// Set a manual damage if this isn't coming from a Entity that has Character_Stats.
		[Tooltip ("If no Character Stats script exists then we will want to set our own damage here.")] 
		[SerializeField] private float manualDamage = 0f;
		[Tooltip ("These types will be the ones damaged.")] 
		[SerializeField] private CharacterType[] damageTheseTypes;
		[Tooltip ("The amount we will jolt when something is damaged.")] 
		[SerializeField] private float joltAmount = 0f;
		[Tooltip ("When this gameobject is created do we want its layer to be of the same of its creator?")]
		[SerializeField] private bool setLayerFromItsCreator = false;
		// Any successful collision that does damage will make the "collisionSound".
		[Tooltip ("The sound we will make when this gameobject applies damage.")]
		[SerializeField] private AudioClip collisionSound;


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
				// Just set the damage as that HAS to have a number that isn't 0 unless there was a screw up on
				// the developers part.
				damage = manualDamage;
			}
		}

		void OnCollisionEnter2D (Collision2D coll){
			CollideDamage (coll.gameObject);
		}

		void OnCollisionStay2D (Collision2D coll){
			CollideDamage (coll.gameObject);
		}

		void OnTriggerEnter2D (Collider2D coll){
			CollideDamage (coll.gameObject);
		}

		void OnTriggerStay2D (Collider2D coll) {
			CollideDamage (coll.gameObject);
		}


		private void CollideDamage(GameObject coll) {
			// IF YOU HAVE ANOTHER SCRIPT IN WHICH YOU DO NOT CARE ABOUT USING CHARACTER_MANAGER
			// THEN MAKE SURE TO LOOK FOR THE SCRIPT HERE AND MAKE A CHECK IF IT HAS IT.

			// Grab the Character_Manager Component from the colliding object.
			Character_Manager otherChar = coll.GetComponentInParent<Character_Manager> ();
			// IF there is NOT a Character_Manager component.
			if (otherChar == null) {
				// We return because we only care about damaging a gameobject that has the Character_Manager 
				// Component.
				return;
			}
			// IF the colliding GameObject's Character_Manager characterEntity is NOT the same as the 
			// GameObject we are colliding with.
			if (!otherChar.characterEntity.Equals (coll.gameObject)) {
				// We leave as we are damaging something that isn't a piece of the Entity.
				// Example would be 2 people clashing swords, Yes both swords are part of the Entity and 
				// children of that entity BUT it ISN'T the Entity so we do not care.
				return;
			}

			// Since we have a Character_Manager lets get the Character_Stats script.
			Character_Stats charStats = otherChar.GetComponentInChildren<Character_Stats> ();
			// IF there is not a script to hold the stats of this Character.
			if (charStats == null) {
				// Then there is nothing we can damage we are attacking a Character but this character is 
				// statless so its like a town friendly NPC.
				return;
			}

			// Play a sound.
			Grid_Helper.soundManager.PlaySound (collisionSound);
			// Lets go into our loop and see if we can damage anything.
			Grid_Helper.helper.DamageCharacterTypeLoop (transform, otherChar, damageTheseTypes, damage, joltAmount);
		}
	}
}
