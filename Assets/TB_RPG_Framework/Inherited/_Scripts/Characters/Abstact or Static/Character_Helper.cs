using UnityEngine;
using System.Collections.Generic;
using System;

namespace TrollBridge {

	/// <summary>
	/// Character_Helper allows us to do quick and easy searches based on the methods here.
	/// </summary>
	public static class Character_Helper {

		// List of Characters.
		static List<Character> characters = new List<Character>();

		// Register the character in our List.
		public static void Register(Character character){
			if(!characters.Contains(character)){
				characters.Add (character);
			}
		}

		// Unregister the character in our List.
		public static void Unregister(Character character){
			if(characters.Contains(character)){
				characters.Remove (character);
			}
		}

		// Get all the Characters in our List.
		public static List<Character> GetAllCharacters (){
			return characters;
		}

		// Get a certain type of characters in our List.
		public static List<Character> GetCharactersByType(List<Character> characterList, CharacterType characterType){
			// Clear the list.
			characterList.Clear ();
			// Loop through the amount of Characters
			for(int i = 0; i < characters.Count; i++){
				// IF we have a matching type.
				if(characters[i].characterType == characterType){
					// Add it to the list.
					characterList.Add (characters[i]);
				}
			}
			// Return the list.
			return characterList;
		}

		// Get the characters in our List by tag.
		public static List<Character> GetCharactersByTag(List<Character> items, string tagName){
			// Clear the items list.
			items.Clear ();
			// Loop based on how many characters we have in the scene.
			for (int i = 0; i < characters.Count; i++) {
				// IF the tags match.
				if (characters [i].gameObject.tag == tagName) {
					// Add this character to our return list.
					items.Add (characters [i]);
				}
			}
			// Return the list of characters.
			return items;
		}

		// Get the Player's Character Manager GameObject of the Player.
		public static GameObject GetPlayerManager () {
			// Loop through the amount of Characters we have.
			for (int i = 0; i < characters.Count; i++) {
				// IF we have found the Character Manager of the player based on their CharacterTypes.
				if (characters [i].characterType == CharacterType.Hero) {
					// Return our player gameobject that has the tag Player Manager.
					return characters [i].gameObject;
				}
			}
			return null;
		}

		// Get the Player GameObject (Use this if you only plan on having 1 Hero)
		public static GameObject GetPlayer (){
			// Loop through the amount of Characters we have.
			for (int i = 0; i < characters.Count; i++) {
				// IF we have found the Character Manager of the player based on their CharacterTypes.
				if (characters [i].characterType == CharacterType.Hero) {
					// Return the public prefab in Character_Manager called Character Entity which is our Player GameObject.
					return characters [i].GetComponent <Character_Manager> ().characterEntity;
				}
			}
			return null;
		}

		// Get the closest character based on distance supplied and the type.
		public static GameObject GetClosestCharacterType(Transform characterEntityTransform, CharacterType characterType, float aggroDistance){
			// Lets get our distance variables.
			float distance = 0f;
			float shortestDistance = 0f;
			GameObject shortestDistanceGameObject = null;

			// Loop the amount of characters we have.
			for (int i = 0; i < characters.Count; i++) {
				// Get the distance between the 2 GameObjects.
				distance = Vector2.Distance (characterEntityTransform.GetComponentInParent<Character> ().characterEntity.transform.position, characters [i].GetComponentInParent<Character> ().characterEntity.transform.position);
				// IF our distance between 2 entities is shorter than our aggro distance,
				// AND we have a matching character type that we want to follow.
				if (distance < aggroDistance && characters [i].characterType == characterType) {
					// IF we do not have a shortest distance GameObject yet OR 
					// these 2 GameObjects distances are shorter than our current shortest distance,
					if (shortestDistanceGameObject == null || distance < shortestDistance) {
						// First GameObject we check will ALWAYS be a winner until something else comes and beats it.
						shortestDistanceGameObject = characters [i].gameObject;
						// Assign our shortestDistance since we are using the shortestDistanceGameObject.
						shortestDistance = distance;
					}
				}
			}
			return shortestDistanceGameObject;
		}
	}
}
