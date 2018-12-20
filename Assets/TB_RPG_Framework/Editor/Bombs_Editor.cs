using UnityEngine;
using UnityEditor;
using System.Collections;

namespace TrollBridge {

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Bombs))]
	public class Bombs_Editor : Editor {

		SerializedProperty typesOfBombs;
		SerializedProperty bombDistance;
		SerializedProperty bombKey;
		SerializedProperty playerAnimator;
		SerializedProperty playerBody;

		void OnEnable () {
			// Setup the SerializedProperties.
			typesOfBombs = serializedObject.FindProperty ("typesOfBombs");
			bombDistance = serializedObject.FindProperty ("bombDistance");
			bombKey = serializedObject.FindProperty ("bombKey");
			playerAnimator = serializedObject.FindProperty ("playerAnimator");
			playerBody = serializedObject.FindProperty ("playerBody");
		}

		public override void OnInspectorGUI() {
			// Set the indentLevel to 0 as default (no indent).
			EditorGUI.indentLevel = 0;
			// Update
			serializedObject.Update();

			// Setup label.
			EditorGUILayout.LabelField("Bomb Setup", EditorStyles.boldLabel);
			// Increment the indent.
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField (bombDistance);
			EditorGUILayout.PropertyField (bombKey);
			EditorGUILayout.PropertyField (playerAnimator);
			EditorGUILayout.PropertyField (playerBody);
			// Decrement the indent.
			EditorGUI.indentLevel--;

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			// Setup label.
			EditorGUILayout.LabelField("Types of Bombs", EditorStyles.boldLabel);
			// Increment the indent.
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(typesOfBombs.FindPropertyRelative("Array.size"));
			// IF there isnt any bombs we create a default of 1 since what is the point of this script if you dont have these items in your game.
			if(typesOfBombs.arraySize == 0){
				// Set the default size of this array.
				typesOfBombs.arraySize = 1;
			}
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(new GUIContent("Name", "The name of the bomb."), EditorStyles.boldLabel, GUILayout.Width(185f));
			EditorGUILayout.LabelField(new GUIContent("Amount", "The amount of bombs."), EditorStyles.boldLabel, GUILayout.Width(90f));
			EditorGUILayout.LabelField(new GUIContent("Sound", "The sound when this type of bomb is placed."), EditorStyles.boldLabel, GUILayout.Width(90f));
			EditorGUILayout.EndHorizontal();
			for(int i = 0; i < typesOfBombs.arraySize; i++)
			{
				EditorGUILayout.PropertyField(typesOfBombs.GetArrayElementAtIndex(i), GUIContent.none);
			}
			// Decrement the indent.
			EditorGUI.indentLevel--;

			// Apply.
			serializedObject.ApplyModifiedProperties();
		}
	}
}
