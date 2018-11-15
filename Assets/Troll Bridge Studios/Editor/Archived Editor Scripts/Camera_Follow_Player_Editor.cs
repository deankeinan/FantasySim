using UnityEngine;
using System.Collections;
using UnityEditor;

namespace TrollBridge {

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Camera_Follow_Player))]
	public class Camera_Follow_Player_Editor : Editor {

		SerializedProperty leftUITag;
		SerializedProperty rightUITag;
		SerializedProperty topUITag;
		SerializedProperty bottomUITag;

		void OnEnable(){
			leftUITag = serializedObject.FindProperty ("leftUITag");
			rightUITag = serializedObject.FindProperty ("rightUITag");
			topUITag = serializedObject.FindProperty ("topUITag");
			bottomUITag = serializedObject.FindProperty ("bottomUITag");
		}

		public override void OnInspectorGUI(){
			// Set the indentLevel to 0 as default (no indent).
			EditorGUI.indentLevel = 0;
			// Update
			serializedObject.Update();

			// Resolution label.
			EditorGUILayout.LabelField("UI Tags", EditorStyles.boldLabel);
			// Increase the Indent the lines.
			EditorGUI.indentLevel++;
			// The UI Offset Option.
			EditorGUILayout.PropertyField(leftUITag);
			EditorGUILayout.PropertyField(rightUITag);
			EditorGUILayout.PropertyField(topUITag);
			EditorGUILayout.PropertyField(bottomUITag);
			// Decrease the Indent of the lines.
			EditorGUI.indentLevel--;

			// Apply
			serializedObject.ApplyModifiedProperties();
		}
	}
}
