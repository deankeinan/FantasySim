using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TrollBridge {

	public class Character_Class : MonoBehaviour {

		[Tooltip ("The class associated with this GameObject.  If you leave this blank then the assumption is that this will be filled by your processes in your game somewhere down the line." +
			"If you manually put a string in here to represent the GameObject's class then we stop right there as the work is over.")]
		public string characterClass;

		void Awake() {
			// IF we did NOT manually give this character a class then we know its a player, otherwise its an enemy we want to give a class to since we dont have to load anything.
			if (String.IsNullOrEmpty (characterClass)) {
				// Load the player stats, if there is any.
				Load ();
			}
		}

		/// <summary>
		/// Save the data.
		/// </summary>
		public void Save () {
			// Create a new Equipment_Data.
			Character_Class_Data data = new Character_Class_Data ();
			// Save the data.
			data.characterClass = characterClass;
			// Turn the Character_Class_Data to Json data.
			string characterClassToJson = JsonUtility.ToJson (data);
			// Save the information.
			PlayerPrefs.SetString ("Character_Class", characterClassToJson);
		}


		/// <summary>
		/// Load the data.
		/// </summary>
		public void Load () {
			// Grab the encrypted Character_Class string.
			string characterClassJson = PlayerPrefs.GetString ("Character_Class");
			// IF there is nothing in this string.
			if (String.IsNullOrEmpty (characterClassJson)) {
				// We need to reach out to our Skill Manager and get our Class.
				//characterClass = Grid_Helper.skillManager.GetCurrentClassChosen ();
				// GTFO of here we done son!
				return;
			}
			// Turn the json data to represent Character_Class_Data.
			Character_Class_Data data = JsonUtility.FromJson<Character_Class_Data> (characterClassJson);
			// Set the class of our character.
			characterClass = data.characterClass;
		}
	}

	[Serializable]
	class Character_Class_Data {	
		public string characterClass;
	}
}
