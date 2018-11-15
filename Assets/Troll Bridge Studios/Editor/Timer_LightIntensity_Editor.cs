using UnityEngine;
using UnityEditor;

namespace TrollBridge {

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Timer_LightIntensity))]
	public class Timer_LightIntensity_Editor : Editor {

		SerializedProperty originalIntensity;
		SerializedProperty flashIntensity;

		SerializedProperty minWaitTime;
		SerializedProperty maxWaitTime;
		SerializedProperty minFlashTime;
		SerializedProperty maxFlashTime;


		void OnEnable () {
			// Setup the SerializedProperties.
			originalIntensity = serializedObject.FindProperty("originalIntensity");
			flashIntensity = serializedObject.FindProperty("flashIntensity");

			minWaitTime = serializedObject.FindProperty("minWaitTime");
			maxWaitTime = serializedObject.FindProperty("maxWaitTime");
			minFlashTime = serializedObject.FindProperty("minFlashTime");
			maxFlashTime = serializedObject.FindProperty("maxFlashTime");
		}

		public override void OnInspectorGUI() {
			// Set the indentLevel to 0 as default (no indent).
			EditorGUI.indentLevel = 0;
			// Update.
			serializedObject.Update();

			// Add some vertical space.
			EditorGUILayout.Space ();

			// Create the label field.
			EditorGUILayout.LabelField (new GUIContent ("Light Intensity"), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Create a property field.
			EditorGUILayout.PropertyField (originalIntensity, new GUIContent ("Original Light Intensity", "The original intensity that will linger."));
			// Create a property field.
			EditorGUILayout.PropertyField (flashIntensity, new GUIContent ("Flash Light Intensity", "The new intensity that you want the light to be changed to."));
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Create the label field.
			EditorGUILayout.LabelField (new GUIContent ("Wait Timers", "The Minimum and Maximum time we wait before we change to our flashIntensity."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Create a property field.
			EditorGUILayout.PropertyField (minWaitTime, new GUIContent ("Minimum Wait Time", "The minimum time before we change the Intensity to flashIntensity."));
			// Create a property field.
			EditorGUILayout.PropertyField (maxWaitTime, new GUIContent ("Maximum Wait Time", "The maximum time before we change the Intensity to flashIntensity."));
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Create the label field.
			EditorGUILayout.LabelField (new GUIContent ("Flash Timers", "The Minimum and Maximum time we wait before we change to our originalIntensity."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Create a property field.
			EditorGUILayout.PropertyField (minFlashTime, new GUIContent ("Minimum Flash Time", "The Minimum and Maximum time we wait before we change to our originalIntensity."));
			// Create a property field.
			EditorGUILayout.PropertyField (maxFlashTime, new GUIContent ("Maximum Flash Time", "The Minimum and Maximum time we wait before we change to our originalIntensity."));
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Decrease the indent.
			EditorGUI.indentLevel--;
			// Apply.
			serializedObject.ApplyModifiedProperties();
		}
	}
}
