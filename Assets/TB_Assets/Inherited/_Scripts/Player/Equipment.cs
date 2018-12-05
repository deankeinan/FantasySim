using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace TrollBridge {

	/// <summary>
	/// Equipment script that handles what is on our player.
	/// </summary>
	public class Equipment : MonoBehaviour {

		// The stats from the items.
		public Item weapon, armour, bracelet, ring, helmet;
		private int weaponID = -1;
		private int armourID = -1;
		private int ringID = -1;
		private int braceletID = -1;
		private int helmetID = -1;

		private Item_Database database;


		void Awake() {
			database = Grid_Helper.itemDataBase;
			// Load the player stats, if there is any.
			Load ();
		}

		public void RemoveWeapon(){
			weapon = null;
			weaponID = -1;
		}

		public void RemoveArmour(){
			armour = null;
			armourID = -1;
		}

		public void RemoveRing(){
			ring = null;
			ringID = -1;
		}

		public void RemoveBracelet(){
			bracelet = null;
			braceletID = -1;
		}

		public void RemoveHelmet(){
			helmet = null;
			helmetID = -1;
		}

		public Item EquipWeapon (Item item)
		{
			// Set up the potential variable of an Item you have equipped.
			Item oldEquippedItem = null;
			// Set the weapon we are using now as the old equipped weapon.
			oldEquippedItem = weapon;
			// Set the new Item we are equipping as our weapon.
			weapon = item;
			weaponID = weapon.ID;
			// Return the old equipped item.
			return oldEquippedItem;
		}

		public Item EquipArmour (Item item)
		{
			// Set up the potential variable of an Item you have equipped.
			Item oldEquippedItem = null;
			// Set the armour we are using now as the old equipped armour.
			oldEquippedItem = armour;
			// Set the new Item we are equipping as our armour
			armour = item;
			armourID = armour.ID;
			// Return the old equipped item.
			return oldEquippedItem;
		}

		public Item EquipRing (Item item)
		{
			// Set up the potential variable of an Item you have equipped.
			Item oldEquippedItem = null;
			// Set the Item we are using now as the old equipped ring.
			oldEquippedItem = ring;
			// Set the new Item we are equipping as our ring.
			ring = item;
			ringID = ring.ID;
			// Return the old equipped item.
			return oldEquippedItem;
		}

		public Item EquipBracelet (Item item)
		{
			// Set up the potential variable of an Item you have equipped.
			Item oldEquippedItem = null;
			// Set the Item we are using now as the old equipped Bracelet.
			oldEquippedItem = bracelet;
			// Set the new Item we are equipping as our Bracelet.
			bracelet = item;
			braceletID = bracelet.ID;
			// Return the old equipped item.
			return oldEquippedItem;
		}

		public Item EquipHelmet (Item item)
		{
			// Set up the potential variable of an Item you have equipped.
			Item oldEquippedItem = null;
			// Set the Item we are using now as the old equipped Helmet.
			oldEquippedItem = helmet;
			// Set the new Item we are equipping as our Helmet.
			helmet = item;
			helmetID = bracelet.ID;
			// Return the old equipped item.
			return oldEquippedItem;
		}

		public Item GetWeapon(){
			return weapon;
		}

		public Item GetArmour(){
			return armour;
		}

		public Item GetRing(){
			return ring;
		}

		public Item GetBracelet(){
			return bracelet;
		}

		public Item GetHelmet(){
			return helmet;
		}

		public int GetWeaponDamage(){
			// IF there is an actual weapon.
			if(weapon != null){
				// return the weapom damage stat.
				return weapon.Damage;
			}
			// There isn't a weapon so there isn't any damage.
			return 0;
		}

		public int GetWeaponHealth(){
			// IF there is an actual weapon.
			if(weapon != null){
				// return the weapom health stat.
				return weapon.Health;
			}
			// There isn't a weapon so there isn't a health stat.
			return 0;
		}

		public int GetWeaponMana(){
			// IF there is an actual weapon.
			if(weapon != null){
				// return the weapom mana stat.
				return weapon.Mana;
			}
			// There isn't a weapon so there isn't a mana stat.
			return 0;
		}

		public float GetWeaponMoveSpeed(){
			// IF there is an actual weapon.
			if(weapon != null){
				// return the weapom movement speed stat.
				return weapon.MoveSpeed;
			}
			// There isn't a weapon so there isn't a movement speed stat.
			return 0f;
		}

		public int GetArmourDamage(){
			// IF there is an actual armour.
			if(armour != null){
				// return the armour damage stat.
				return armour.Damage;
			}
			// There isn't a armour so there isn't a damage stat.
			return 0;
		}

		public int GetArmourHealth(){
			// IF there is an actual armour.
			if(armour != null){
				// return the armour health stat.
				return armour.Health;
			}
			// There isn't a armour so there isn't a health stat.
			return 0;
		}

		public int GetArmourMana(){
			// IF there is an actual armour.
			if(armour != null){
				// return the armour mana stat.
				return armour.Mana;
			}
			// There isn't a armour so there isn't a mana stat.
			return 0;
		}

		public float GetArmourMoveSpeed(){
			// IF there is an actual armour.
			if(armour != null){
				// return the armour movement speed stat.
				return armour.MoveSpeed;
			}
			// There isn't a armour so there isn't a movement speed stat.
			return 0f;
		}

		public int GetRingDamage(){
			// IF there is an actual ring.
			if(ring != null){
				// return the ring damage stat.
				return ring.Damage;
			}
			// There isn't a ring so there isn't a damage stat.
			return 0;
		}

		public int GetRingHealth(){
			// IF there is an actual ring.
			if(ring != null){
				// return the ring health stat.
				return ring.Health;
			}
			// There isn't a ring so there isn't a health stat.
			return 0;
		}

		public int GetRingMana(){
			// IF there is an actual ring.
			if(ring != null){
				// return the ring mana stat.
				return ring.Mana;
			}
			// There isn't a ring so there isn't a mana stat.
			return 0;
		}

		public float GetRingMoveSpeed(){
			// IF there is an actual ring.
			if(ring != null){
				// return the ring movement speed stat.
				return ring.MoveSpeed;
			}
			// There isn't a ring so there isn't a movement speed stat.
			return 0f;
		}

		public int GetBraceletDamage(){
			// IF there is an actual bracelet.
			if(bracelet != null){
				// return the bracelet damage stat.
				return bracelet.Damage;
			}
			// There isn't a bracelet so there isn't a damage stat.
			return 0;
		}

		public int GetBraceletHealth(){
			// IF there is an actual bracelet.
			if(bracelet != null){
				// return the bracelet health stat.
				return bracelet.Health;
			}
			// There isn't a bracelet so there isn't a health stat.
			return 0;
		}

		public int GetBraceletMana(){
			// IF there is an actual bracelet.
			if(bracelet != null){
				// return the bracelet mana stat.
				return bracelet.Mana;
			}
			// There isn't a bracelet so there isn't a mana stat.
			return 0;
		}

		public float GetBraceletMoveSpeed () {
			// IF there is an actual bracelet.
			if (bracelet != null) {
				// return the bracelet movement speed stat.
				return bracelet.MoveSpeed;
			}
			// There isn't a bracelet so there isn't a movement speed stat.
			return 0f;
		}

		public int GetHelmetDamage(){
			// IF there is an actual Helmet.
			if(helmet != null){
				// return the helmet damage stat.
				return helmet.Damage;
			}
			// There isn't a helmet so there isn't a damage stat.
			return 0;
		}

		public int GetHelmetHealth(){
			// IF there is an actual Helmet.
			if(helmet != null){
				// return the helmet health stat.
				return helmet.Health;
			}
			// There isn't a helmet so there isn't a health stat.
			return 0;
		}

		public int GetHelmetMana(){
			// IF there is an actual Helmet.
			if (helmet != null) {
				// return the Helmet mana stat.
				return helmet.Mana;
			}
			// There isn't a helmet so there isn't a mana stat.
			return 0;
		}

		public float GetHelmetMoveSpeed (){
			// IF there is an actual Helmet.
			if (helmet != null) {
				// return the Helmet movement speed stat.
				return helmet.MoveSpeed;
			}
			// There isn't a helmet so there isn't a movement speed stat.
			return 0f;
		}

		/// <summary>
		/// Gets the equipment damage.
		/// </summary>
		/// <returns>The equipment damage.</returns>
		public float GetEquipmentDamage () {
			return (float)(GetWeaponDamage () + GetArmourDamage () + GetRingDamage () + GetBraceletDamage ());
		}

		/// <summary>
		/// Gets the equipment health.
		/// </summary>
		/// <returns>The equipment health.</returns>
		public float GetEquipmentHealth () {
			return (float)(GetWeaponHealth () + GetArmourHealth () + GetRingHealth () + GetBraceletHealth ());
		}

		/// <summary>
		/// Gets the equipment mana.
		/// </summary>
		/// <returns>The equipment mana.</returns>
		public float GetEquipmentMana () {
			return (float)(GetWeaponMana () + GetArmourMana () + GetRingMana () + GetBraceletMana ());
		}

		/// <summary>
		/// Gets the equipment movement speed.
		/// </summary>
		/// <returns>The equipment movement speed.</returns>
		public float GetEquipmentMovementSpeed () {
			return (float)(GetWeaponMoveSpeed () + GetArmourMoveSpeed () + GetRingMoveSpeed () + GetBraceletMoveSpeed ());
		}

		/// <summary>
		/// Save this instance.
		/// </summary>
		public void Save () {
			// Create a new Equipment_Data.
			Equipment_Data data = new Equipment_Data ();
			// Save the data.
			data.weaponID = weaponID;
			data.armourID = armourID;
			data.ringID = ringID;
			data.braceletID = braceletID;
			data.helmetID = helmetID;
			// Turn the Equipment_Data to Json data.
			string equipmentToJson = JsonUtility.ToJson (data);
			// Save the information.
			PlayerPrefs.SetString ("Equipment", equipmentToJson);
		}

		/// <summary>
		/// Load this instance.
		/// </summary>
		public void Load () {
			// Grab the encrypted Equipment string.
			string equipmentJson = PlayerPrefs.GetString ("Equipment");
			// IF there is nothing in this string.
			if (String.IsNullOrEmpty (equipmentJson)) {
				// GTFO of here we done son!
				return;
			}
			// Turn the json data to represent Equipment_Data.
			Equipment_Data data = JsonUtility.FromJson<Equipment_Data> (equipmentJson);

			// IF a weapon exists.
			if (data.weaponID != -1) {
				// Set the ID of the weapon.
				weaponID = data.weaponID;
				// Load the weapon Item.
				weapon = database.FetchItemByID (weaponID);
			}
			if (data.armourID != -1) {
				// Set the ID of the armour.
				armourID = data.armourID;
				// Load the armour Item.
				armour = database.FetchItemByID (armourID);
			}
			if (data.ringID != -1) {
				// Set the ID of the ring.
				ringID = data.ringID;
				// Load the ring Item.
				ring = database.FetchItemByID (ringID);
			}
			if (data.braceletID != -1) {
				// Set the ID of the bracelet.
				braceletID = data.braceletID;
				// Load the bracelet Item.
				bracelet = database.FetchItemByID (braceletID);
			}
			if (data.helmetID != -1) {
				// Set the ID of the helmet.
				helmetID = data.helmetID;
				// Load the helmet Item.
				helmet = database.FetchItemByID (helmetID);
			}
		}
	}

	[Serializable]
	class Equipment_Data
	{	
		public int weaponID;
		public int armourID;
		public int ringID;
		public int braceletID;
		public int helmetID;
	}
}
