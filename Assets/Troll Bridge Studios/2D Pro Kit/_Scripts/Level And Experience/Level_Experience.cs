using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace TrollBridge {

	/// <summary>
	/// Level experience system that keeps track of your experience and your level.
	/// </summary>
	public class Level_Experience : MonoBehaviour {
		
		[SerializeField] private int currentLevel = 1;
		[SerializeField] private int currentExperience = 0;

		[SerializeField] private int[] playerLevelExperience;


		void OnValidate () {
			// Loop through the player level experience.
			for (int i = 0; i < playerLevelExperience.Length; i++) {
				// IF our current exp is less than a level experience.
				if (currentExperience < playerLevelExperience[i]) {
					// We set our current level
					currentLevel = i+1;
					// We are done.
					return;
				}
			}
			// Set our max level.
			currentLevel = playerLevelExperience.Length + 1;
		}

		/// <summary>
		/// Gets the current level.
		/// </summary>
		public int GetCurrentLevel () {
			return currentLevel;
		}

		/// <summary>
		/// Gets the current experience.
		/// </summary>
		public int GetCurrentExperience () {
			return currentExperience;
		}

		/// <summary>
		/// Increases the experience.
		/// </summary>
		public void IncreaseExperience (int expAmount) {
			// Add to our experience.
			currentExperience += expAmount;

			// Loop through the player level experience.
			for (int i = 0; i < playerLevelExperience.Length; i++) {
				// IF our current exp is less than a level experience.
				if (currentExperience < playerLevelExperience[i]) {
					// We set our current level
					currentLevel = i+1;
					// We are done.
					return;
				}
			}
			// Set our max level.
			currentLevel = playerLevelExperience.Length + 1;
		}

		/// <summary>
		/// Gets the next exp to next level.
		/// </summary>
		public int GetExpToNextLevel () {
			// IF we are max level.
			if (currentLevel > playerLevelExperience.Length) {
				// No exp to lvl again as we are max level.
				return 0;
			}
			// Return the left over exp to the next level.
			return playerLevelExperience [currentLevel - 1] - currentExperience;
		}
	}




	/// <summary>
	/// Level experience editor.
	/// </summary>
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Level_Experience))]
	public class Level_Experience_Editor : Editor {

		SerializedProperty currentLevel;
		SerializedProperty playerLevelExperience;
		SerializedProperty currentExperience;

		void OnEnable () {
			// Setup the SerializedProperties.
			playerLevelExperience = serializedObject.FindProperty ("playerLevelExperience");
			currentLevel = serializedObject.FindProperty ("currentLevel");
			currentExperience = serializedObject.FindProperty ("currentExperience");
		}

		public override void OnInspectorGUI(){

			// Set the indentLevel to 0 as default (no indent).
			EditorGUI.indentLevel = 0;
			// Update
			serializedObject.Update ();

			// Set our property fields for our level and experience.
			EditorGUILayout.PropertyField (currentLevel);
			EditorGUILayout.PropertyField (currentExperience);

			// Get the array.
			EditorGUILayout.PropertyField (playerLevelExperience.FindPropertyRelative ("Array.size"));
			// IF there isnt any levels we create a default of 1 since what is the point of this script if you are not going to have levels.
			if (playerLevelExperience.arraySize == 0) {
				// Set the default size of this array.
				playerLevelExperience.arraySize = 1;
			}

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField (new GUIContent ("Current Level", "The current level."), EditorStyles.boldLabel, GUILayout.Width (120f));
			EditorGUILayout.LabelField (new GUIContent ("Next Level", "The next level."), EditorStyles.boldLabel, GUILayout.Width (120f));
			EditorGUILayout.LabelField (new GUIContent ("Total Experience", "The total amount of experience needed to the next level."), EditorStyles.boldLabel, GUILayout.Width (120f));
			EditorGUILayout.EndHorizontal ();

			for (int i = 0; i < playerLevelExperience.arraySize; i++) {
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("", EditorStyles.boldLabel, GUILayout.Width (35f));
				EditorGUILayout.LabelField ((i + 1).ToString (), EditorStyles.boldLabel, GUILayout.Width (110f));
				EditorGUILayout.LabelField ((i + 2).ToString (), EditorStyles.boldLabel, GUILayout.Width (95f));
				EditorGUILayout.PropertyField (playerLevelExperience.GetArrayElementAtIndex (i), GUIContent.none);
				EditorGUILayout.EndHorizontal ();
			}

			// Apply.
			serializedObject.ApplyModifiedProperties ();
		}
	}
}
