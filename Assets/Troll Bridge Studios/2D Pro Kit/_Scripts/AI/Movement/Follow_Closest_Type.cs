using UnityEngine;
using System.Collections.Generic;

namespace TrollBridge {

	/// <summary>
	/// Follow closest type follows a gameobject if its "CharacterType" is 'TypeToFollow'.
	/// </summary>
	public class Follow_Closest_Type : MonoBehaviour {

		[Tooltip("The CharacterType to follow")]
		public CharacterType TypeToFollow;
		[Tooltip("How far the mob can see before aggroing on the CharacterType")]
		public float AggroDistance = 5f;
		[Tooltip("The distance you want this GameObject to be away from who/whatever it is following.")]
		public float gapDistance = 0f;

		private Character_Manager characterManager;
		private Character_Stats charStats;
		private List<Character> listCharacter = new List<Character>();
		private Transform _transform;

		void Start () {
			// Get the Character Manager.
			characterManager = GetComponent<Character_Manager> ();
			// Get the Character Stats component.
			charStats = characterManager.GetComponentInChildren<Character_Stats> ();
			// Get the Transform componenet.
			_transform = characterManager.GetComponent<Transform> ();
		}

		void Update () {
			// IF this character is able to move.
			if (characterManager.canMove) {
				// Get the list of all the characters.
				listCharacter = Character_Helper.GetCharactersByType (listCharacter, TypeToFollow);
				// IF the List of CharacterTypes is greater than 0.
				if (listCharacter.Count > 0) {
					// Get the closest GameObject with the CharacterType chosen and save it to _character.
					GameObject _character = Character_Helper.GetClosestCharacterType (characterManager.characterEntity.transform, TypeToFollow, AggroDistance);
					// IF the closest gameobject is not null AND IF both GameObjects distance apart from each other is greater than the Gap Distance.
					if (_character != null && Vector3.Distance (_transform.position, _character.transform.position) >= gapDistance) {
						// Move the actual character of this gameobject closer to _character gameobject.
						characterManager.characterEntity.transform.position = 
							Vector2.MoveTowards (characterManager.characterEntity.transform.position, _character.GetComponent<Character> ().characterEntity.transform.position, Time.deltaTime * charStats.CurrentMoveSpeed);
					}
				}
			}
		}
	}
}
