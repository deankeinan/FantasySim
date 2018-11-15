using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

namespace TrollBridge {

	/// <summary>
	/// Character stats is our stats of our character.
	/// </summary>
	public class Character_Stats : MonoBehaviour {

		// The characters base damage.
		public float DefaultDamage = 0f;
		// The characters base movement speed.
		public float DefaultMoveSpeed = 1f;
		// The characters base health.
		public float DefaultHealth = 3f;
		public float DefaultMaxHealth = 5f;
		// The characters base mana.
		public float DefaultMana = 20f;
		public float DefaultMaxMana = 20f;


		[ReadOnlyAttribute]
		public float CurrentDamage;
		[ReadOnlyAttribute]
		public float BonusDamage;

		[ReadOnlyAttribute]
		public float CurrentMoveSpeed;
		[ReadOnlyAttribute]
		public float BonusMoveSpeed;

		[ReadOnlyAttribute]
		public float MaxHealth;
		[ReadOnlyAttribute]
		public float CurrentHealth;
		[ReadOnlyAttribute]
		public float BonusHealth;

		[ReadOnlyAttribute]
		public float MaxMana;
		[ReadOnlyAttribute]
		public float CurrentMana;
		[ReadOnlyAttribute]
		public float BonusMana;

		// Used for our Player_Manager referencing.
		private Character_Manager characterManager;


		void Awake () {
			characterManager = GetComponentInParent<Character_Manager> ();
			Load ();
		}

		/// <summary>
		/// Increase the max health.
		/// </summary>
		public void IncreaseMaxHealth(float amount)	{
			// We add to our bonus variable to keep track of how much bonus we have.
			BonusHealth += amount;
			// Since our max is growing, so does our current with it.
			CurrentHealth += amount;
		}

		/// <summary>
		/// Increase the max mana.
		/// </summary>
		public void IncreaseMaxMana(float amount) {
			// We add to our bonus variable to keep track of how much bonus we have.
			BonusMana += amount;
			// Since our max is growing, so does our current with it.
			CurrentMana += amount;
		}

		/// <summary>
		/// Increase the base damage.
		/// </summary>
		public void IncreaseBaseDamage(float amount) {
			// We add to our bonus variable to keep track of how much bonus we have.
			BonusDamage += amount;
		}

		/// <summary>
		/// Increase the base movement speed.
		/// </summary>
		public void IncreaseBaseMoveSpeed(float amount) {
			// We add to our bonus variable to keep track of how much bonus we have.
			BonusMoveSpeed += amount;
		}



		/// <summary>
		/// Add to our current health pool.  If we happen to go above our max health we set our current health
		/// at max health.
		/// </summary>
		public void AddHealth (float amount) {
			// IF we go above our max health.
			if (CurrentHealth + amount >= MaxHealth) {
				// Set our current health to our max health.
				CurrentHealth = MaxHealth;
			} else {
				// Just add to our current health pool.
				CurrentHealth += amount;
			}
		}

		/// <summary>
		/// Subtract from our current health pool.  If we happen to go below 0 we just set our current heaht to 0.
		/// </summary>
		public void SubtractHealth (float amount) {
			// IF this damaging attack reduces our HP to 0 or below which kills us,
			// ELSE this damaging attack didnt kill us so we just reduce the HP.
			if (CurrentHealth - amount <= 0f) {
				// We set our current health to 0.
				CurrentHealth = 0;
			} else {
				// Reduce our health.
				CurrentHealth -= amount;
			}
		}



		/// <summary>
		/// Get the default health.
		/// </summary>
		public float GetDefaultHealth(){
			return DefaultHealth;
		}

		/// <summary>
		/// Get the default max health.
		/// </summary>
		public float GetDefaultMaxHealth(){
			return DefaultMaxHealth;
		}

		/// <summary>
		/// Get the current.
		/// </summary>
		public float GetCurrentHealth(){
			return CurrentHealth;
		}

		/// <summary>
		/// Get the current max health.
		/// </summary>
		public float GetMaxHealth(){
			return MaxHealth;
		}

		/// <summary>
		/// Get the bonus health.
		/// </summary>
		public float GetBonusHealth(){
			return BonusHealth;
		}

		/// <summary>
		/// Set the max health.
		/// </summary>
		public void SetMaxHealth(float newMaxHealth){
			MaxHealth = newMaxHealth;
		}

		/// <summary>
		/// Gets the default bonus max health.
		/// </summary>
		/// <returns>The default bonus max health.</returns>
		public float GetDefaultBonusMaxHealth () {
			return DefaultMaxHealth + BonusHealth;
		}




		/// <summary>
		/// Get the default mana.
		/// </summary>
		public float GetDefaultMana(){
			return DefaultMana;
		}

		/// <summary>
		/// Get the default max mana.
		/// </summary>
		public float GetDefaultMaxMana(){
			return DefaultMaxMana;
		}

		/// <summary>
		/// Get the current mana.
		/// </summary>
		public float GetCurrentMana(){
			return CurrentMana;
		}

		/// <summary>
		/// Get the max mana.
		/// </summary>
		public float GetMaxMana(){
			return MaxMana;
		}

		/// <summary>
		/// Get the bonus mana.
		/// </summary>
		public float GetBonusMana(){
			return BonusMana;
		}

		/// <summary>
		/// Set the max mana.
		/// </summary>
		public void SetMaxMana(float newMaxMana){
			MaxMana = newMaxMana;
		}

		/// <summary>
		/// Gets the default bonus max mana.
		/// </summary>
		/// <returns>The default bonus max mana.</returns>
		public float GetDefaultBonusMaxMana () {
			return DefaultMaxMana + BonusMana;
		}




		/// <summary>
		/// Get the default damage.
		/// </summary>
		public float GetDefaultDamage(){
			return DefaultDamage;
		}

		/// <summary>
		/// Get the current damage.
		/// </summary>
		public float GetCurrentDamage(){
			return CurrentDamage;
		}

		/// <summary>
		/// Get the bonus damage.
		/// </summary>
		public float GetBonusDamage(){
			return BonusDamage;
		}

		/// <summary>
		/// Set the current damage.
		/// </summary>
		public void SetCurrentDamage(float newCurrentDamage){
			CurrentDamage = newCurrentDamage;
		}

		/// <summary>
		/// Gets the default bonus damage.
		/// </summary>
		/// <returns>The default bonus damage.</returns>
		public float GetDefaultBonusDamage () {
			return DefaultDamage + BonusDamage;
		}




		/// <summary>
		/// Get the default movement speed.
		/// </summary>
		public float GetDefaultMoveSpeed(){
			return DefaultMoveSpeed;
		}

		/// <summary>
		/// Get the current movement speed.
		/// </summary>
		public float GetCurrentMoveSpeed(){
			return CurrentMoveSpeed;
		}

		/// <summary>
		/// Get the bonus movement speed.
		/// </summary>
		public float GetBonusMoveSpeed(){
			return BonusMoveSpeed;
		}

		/// <summary>
		/// Set the current movementspeed.
		/// </summary>
		public void SetCurrentMoveSpeed(float newMoveSpeed){
			CurrentMoveSpeed = newMoveSpeed;
		}


		public float GetDefaultBonusMovementSpeed () {
			return DefaultMoveSpeed + BonusMoveSpeed;
		}



		/// <summary>
		/// Sets the current damage from equipment.  This is where you will want to setup your algorithm.
		/// </summary>
		public void SetCurrentDamageFromEquipment (float equipmentDamage) {
			// Set our current damage based on our Default Damage + Bonus Damage + Equipment Damage.
			CurrentDamage = GetDefaultBonusDamage () + equipmentDamage;
		}

		/// <summary>
		/// Sets the current max health from equipment.  This is where you will want to setup your algorithm.
		/// </summary>
		/// <param name="equipmentHealth">Equipment health.</param>
		public void SetMaxHealthFromEquipment (float equipmentHealth) {
			MaxHealth = GetDefaultBonusMaxHealth () + equipmentHealth;
		}

		/// <summary>
		/// Sets the max mana from equipment.  This is where you will want to setup your algorithm.
		/// </summary>
		/// <param name="equipmentMana">Equipment mana.</param>
		public void SetMaxManaFromEquipment (float equipmentMana) {
			MaxMana = GetDefaultBonusMaxMana () + equipmentMana;
		}

		/// <summary>
		/// Sets the current movement speed from equipment.  This is where you will want to setup your algorithm
		/// </summary>
		/// <param name="equipmentMovementSpeed">Equipment movement speed.</param>
		public void SetCurrentMovementSpeedFromEquipment (float equipmentMovementSpeed) {
			CurrentMoveSpeed = GetDefaultBonusMovementSpeed () + equipmentMovementSpeed;
		}






		/// <summary>
		/// Save our Character Stats.
		/// </summary>
		public void Save() {
			// Create a new Player_Data.
			Character_Data data = new Character_Data ();
			// Save the data.
			data.currentHealth = CurrentHealth;
			data.currentMana = CurrentMana;
			// Save the bonus information.
			data.bonusHealth = BonusHealth;
			data.bonusMana = BonusMana;
			data.bonusDamage = BonusDamage;
			data.bonusMoveSpeed = BonusMoveSpeed;

			// Turn the Character_Stats data into Json data.
			string charStatsToJson = JsonUtility.ToJson (data);
			// IF we are saving the Player/Hero,
			// ELSE we are saving an NPC.
			if (characterManager.characterType == CharacterType.Hero) {
				// Save the information.
				PlayerPrefs.SetString ("Player", charStatsToJson);
			} else {
				// Save the information. (scene name / gameobject name).  Care when using this as you want unique names of your monsters if you choose to have them saved.
				PlayerPrefs.SetString (SceneManager.GetActiveScene().name +"/"+ gameObject.name, charStatsToJson);
			}
		}

		private void Load()
		{
			string charStatsJson;
			// IF we are loading the Player/Hero,
			// ELSE we are loading an NPC.
			if (characterManager.characterType == CharacterType.Hero) {
				// Load the information.
				charStatsJson = PlayerPrefs.GetString ("Player");
				// IF there is nothing in this string.
				if (String.IsNullOrEmpty (charStatsJson)) {
					// Load the default value of the stats.
					CurrentHealth = DefaultHealth;
					CurrentMana = DefaultMana;
					// GTFO of here we done son!
					return;
				}
			} else {
				// Load the information. (scene name / gameobject name).
				charStatsJson = PlayerPrefs.GetString (SceneManager.GetActiveScene().name +"/"+ gameObject.name);
				// IF there is nothing in this string.
				if (String.IsNullOrEmpty (charStatsJson)) {
					// Load the default value of the stats.
					CurrentDamage = DefaultDamage;
					CurrentHealth =  DefaultHealth;
					MaxHealth = DefaultMaxHealth;
					CurrentMana =  DefaultMana;
					MaxMana = DefaultMaxMana;
					CurrentMoveSpeed = DefaultMoveSpeed;
					// GTFO of here we done son!
					return;
				}
			}
			// Turn the json data to represent Equipment_Data.
			Character_Data data = JsonUtility.FromJson<Character_Data> (charStatsJson);
			// Load the player stats.
			CurrentHealth = data.currentHealth;
			CurrentMana = data.currentMana;
			BonusDamage = data.bonusDamage;
			BonusMoveSpeed = data.bonusMoveSpeed;
			BonusHealth = data.bonusHealth;
			BonusMana = data.bonusMana;
		}
	}


	[Serializable]
	class Character_Data
	{	
		public float currentHealth;
		public float currentMana;

		public float bonusHealth;
		public float bonusMana;
		public float bonusDamage;
		public float bonusMoveSpeed;
	}
}

