using UnityEngine;
using UnityEditor;

namespace TrollBridge {

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Visual_Scene_Transition))]
	public class Visual_Scene_Transition_Editor : Editor {

		SerializedProperty fade;
		SerializedProperty slide;
		SerializedProperty twirl;
		SerializedProperty newScene;
		SerializedProperty sceneSpawnLocation;
		SerializedProperty newGame;

		void OnEnable () {
			// Setup the SerializedProperties.
			fade = serializedObject.FindProperty ("fade");
			slide = serializedObject.FindProperty ("slide");
			twirl = serializedObject.FindProperty ("twirl");
			newScene = serializedObject.FindProperty ("newScene");
			sceneSpawnLocation = serializedObject.FindProperty ("sceneSpawnLocation");
			newGame = serializedObject.FindProperty ("newGame");
		}

		public override void OnInspectorGUI() {
			// Set the indentLevel to 0 as default (no indent).
			EditorGUI.indentLevel = 0;
			// Update
			serializedObject.Update();

			// Visual Transition Type Label.
			EditorGUILayout.LabelField(new GUIContent ("Scene Transition", "Types of scene transitions."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Set our property field.
			fade.boolValue = EditorGUILayout.ToggleLeft (new GUIContent ("Fade Transition", "Do you want a fade transition."), !slide.boolValue && !twirl.boolValue);
			// Set our property field.
			slide.boolValue = EditorGUILayout.ToggleLeft (new GUIContent ("Slide Transition", "Do you want a slide transition."), !fade.boolValue && !twirl.boolValue);
			// Set our property field.
			twirl.boolValue = EditorGUILayout.ToggleLeft (new GUIContent ("Twirl Transition", "Do you want a twirl transition."), !slide.boolValue && !fade.boolValue);
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Visual Transition Type Label.
			EditorGUILayout.LabelField(new GUIContent ("New Scene Details", "Fill out the information so that we know WHAT scene to spawn in and WHERE in that scene we need to spawn."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Set our property field.
			EditorGUILayout.PropertyField (newScene, new GUIContent ("Next Scene", "The name of the scene that will be loaded."));
			// Set our property field.
			EditorGUILayout.PropertyField (sceneSpawnLocation, new GUIContent ("Scene Spawn Location", "The location in the new scene that the player will be spawned at.  This number is correlated to the Transform locations on your Scene Manager Script."));
			// Set our property field.
			newGame.boolValue = EditorGUILayout.Toggle (new GUIContent ("New Game", "Boolean used so that if we need to wipe our data we can.\n\n" +
				"When we go to the next scene do we want all of our saved data to be erased?  This goes hand in hand with a New Game button on a main menu screen.\n\n" +
				"(You will only want to call this once at the very start of your game.)"), newGame.boolValue);
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Apply.
			serializedObject.ApplyModifiedProperties();
		}
	}
}
