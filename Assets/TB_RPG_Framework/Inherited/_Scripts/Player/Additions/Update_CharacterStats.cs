using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	[RequireComponent(typeof(Equipment))]
	[RequireComponent(typeof(Character_Stats))]
	/// <summary>
	/// This script is made for the Player (and anything else you can thing of) such that we need to update stats
	/// for this Character based on its Character Stats, Equipment and (De)Buffs.
	/// </summary>
	public class Update_CharacterStats : MonoBehaviour {

		[SerializeField] private Equipment playerEquipment;
		[SerializeField] private Character_Stats playerStats;

		void Start () {
			// Load this before our update method.
			LoadStatsFromEverything ();
		}

		void Update () {
			// Lets make sure every frame we have updated stats.
			LoadStatsFromEverything ();
		}

		/// <summary>
		/// This method handles assigning your stats when you either load your stats or equip an item as all stats need to be recalculated when something like this happens.
		/// </summary>
		public void LoadStatsFromEverything() {
			// We set our current damage based on our default values and our equipment we are wearing.
			playerStats.SetCurrentDamageFromEquipment (playerEquipment.GetEquipmentDamage ());
			// We set our max health based on our default values and our equipment we are wearing.
			playerStats.SetMaxHealthFromEquipment (playerEquipment.GetEquipmentHealth ());
			// We set our max mana based on our default values and our equipment we are wearing.
			playerStats.SetMaxManaFromEquipment (playerEquipment.GetEquipmentMana ());
			// We set our current movement speed based on our default values and our equipment we are wearing.
			playerStats.SetCurrentMovementSpeedFromEquipment (playerEquipment.GetEquipmentMovementSpeed ());
		}
	}
}
