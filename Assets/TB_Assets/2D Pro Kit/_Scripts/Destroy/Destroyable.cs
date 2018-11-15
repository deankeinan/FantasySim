using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// This script is used mainly for environmental GameObjects in which we want to be destroyable by other Items but not neccessarily destroyable by a bomb (we already have a script for that).  We want things to be destroyed by swords, shovels, axes, etc.  
	/// This script will take care of that and if you add anything else make sure you add to this script.  Since we use Items.Json to fill our Item Database we will pull information from Item_Database to let us know if we can take the actions we need to.
	/// </summary>
	public class Destroyable : MonoBehaviour {

		// Optional - Only worry about the integrity of this variable IF you are using the State_Transform Component attached to this GameObject to save the State.
		public bool isPlaced = false;
		// The sub types that interacts with this GameObject.
		public string[] subTypesThatDestroy;
		// How many times this GameObject gets hit from the "subTypeThatDestroys" until it goes bye bye.
		public int numberOfHits;
		// The display to spawn when the GameObject is destroyed.
		public GameObject afterDestroyEffects;
		// The locations of the afterDestroyEffects.
		public Transform[] afterDestroyEffectLocations;
		// The display to spawn when the GameObject is hit (not destroyed).
		public GameObject hitEffects;
		// The locations of the hitEffects.
		public Transform[] hitEffectLocations;


		void OnTriggerEnter2D (Collider2D coll) {
			// Whatever is colliding with us, let us check for the player.
			// IF the colliding GameObject has a Character Manager.
			if(coll.GetComponentInParent<Character_Manager> () == null){
				// We leave as the only thing that can destroy environmental things will be the player.
				return;
			}
			// IF our Character is a Hero/Player.
			if (coll.GetComponentInParent<Character_Manager> ().characterType != CharacterType.Hero) {
				// We leave as the only things we want to destroy the environment is the player.
			}
			// A Character exists so lets get the main parent incase we need to traverse downward to find other scripts/components.
			Character_Manager characterManager = coll.GetComponentInParent<Character_Manager> ();
			// So now that we have a Player_Manager lets scope down the children and find the Equipment script.
			Equipment equipment = characterManager.GetComponentInChildren<Equipment> ();
			// Lets get the current weapon's subType we are wielding.
			string subType = equipment.GetWeapon ().SubType;

			// Loop the amount of times we have subTypes.
			for (int k = 0; k < subTypesThatDestroy.Length; k++) {
				// IF we have a matching subTypeThatDestroy and subType.
				if (subTypesThatDestroy [k] == subType) {
					// Reduce the numberOfHits (really this is just a simulation of HP in a nutshell but not really health per say).
					// IF you didnt want to do just flat amount of hits you can always set up a higher variable for numberOfHits and apply the damage the player has in their Character_Stats to apply damage here.  
					// Normally though these type of destroyables are # of flat hits then they go bye bye.  You can also do flat amounts and base the damage done to these destroyables based on the "Rarity" or "Damage" of the item,
					// so like : 
					// int rarity = GetIntBasedOnRarity(equipment.GetWeapon().Rarity) // There isnt a method GetIntBasedOnRarity() but you get the idea that Common = 1, Rare = 2, Epic = 3, etc.
					// numberOfHits -= rarity;
					// or
					// numberOfHits -= equipment.GetWeapon().Damage;  // Remember we labeled the SubType so if you wanted to set damage to this item and not have it effect other types we can.

					// Reduce the number of hits.
					numberOfHits -= 1;
					// IF numberOfHits is less than or equal to zero.
					// ELSE we still have numberOfHits left.
					if (numberOfHits <= 0) {
						// Loop the amount of times we have locations to spawn our destroy effects.
						for (int i = 0; i < afterDestroyEffectLocations.Length; i++) {
							// Create our Destroy Effect at one of our locations specified.
							Instantiate (afterDestroyEffects, afterDestroyEffectLocations [i].transform.position, Quaternion.identity);
						}
						// Check and see if there is a State_Handler script.
						Grid_Helper.helper.CheckState (isPlaced, gameObject);
					} else {
						// We are not getting destroyed but since we are taking "damage" we need to display to the player that something is happening to this GameObject.  
						// If not the player can swing a sword/shovel/axe on a bush/dirt hole/tree and with no indicator that player will probably not try it again which can result in bad experience for the player.
						// Now, there are MANY ways to handle this, you can run a coroutine and alter the colors to show contact that something is happening OR you can spawn like "poof" leaves/rubble/wood chippings/clouds.
						// What we do in the demo is we take the "poof" effect and leave the variable hitEffects for this as I have created sprites for this purpose.  IF you are lacking sprites to display this effect I would recommend using the color effect.

						// Loop the amount of times we have locations to spawn our effects.
						for (int i = 0; i < hitEffectLocations.Length; i++) {
							// Create our Effect at one of our locations specified.
							Instantiate (hitEffects, hitEffectLocations [i].transform.position, Quaternion.identity);
						}
					}
					// We found a match for our SubType so it cannot match with anything else.
					return;
				}
			}
		}
	}
}
