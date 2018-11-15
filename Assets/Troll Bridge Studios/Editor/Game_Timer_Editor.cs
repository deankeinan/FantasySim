using UnityEditor;
using UnityEngine;

namespace TrollBridge {

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Game_Timer))]
	public class Game_Timer_Editor : Editor {

		SerializedProperty hourInGameConversion, minuteInGameConversion, secondInGameConversion;
		SerializedProperty currentTimeHour, currentTimeMinute, currentTimeSecond;
		SerializedProperty isAM;

		void OnEnable () {
			// Setup the SerializedProperties.
			hourInGameConversion = serializedObject.FindProperty ("hourInGameConversion");
			minuteInGameConversion = serializedObject.FindProperty ("minuteInGameConversion");
			secondInGameConversion = serializedObject.FindProperty ("secondInGameConversion");
			currentTimeHour = serializedObject.FindProperty ("currentTimeHour");
			currentTimeMinute = serializedObject.FindProperty ("currentTimeMinute");
			currentTimeSecond = serializedObject.FindProperty ("currentTimeSecond");
			isAM = serializedObject.FindProperty ("isAM");
		}

		public override void OnInspectorGUI() {
			// Set the indentLevel to 0 as default (no indent).
			EditorGUI.indentLevel = 0;
			// Update
			serializedObject.Update();

			// Add some vertical space.
			EditorGUILayout.Space ();

			// Create the label field.
			EditorGUILayout.LabelField (new GUIContent("Current Time", "This will display your current time in your game."), EditorStyles.boldLabel);
			// Add some vertical space.
			EditorGUILayout.Space ();
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Begin a horizontal layout.
			EditorGUILayout.BeginHorizontal(GUILayout.Width(10f));
			// Make these not be able to be editted.
			EditorGUI.BeginDisabledGroup (true);
			// Set our float field.
			currentTimeHour.intValue = EditorGUILayout.IntField(currentTimeHour.intValue);
			// Create the label field.
			EditorGUILayout.LabelField (":", GUILayout.Width(25f));
			// Set our float field.
			currentTimeMinute.intValue = EditorGUILayout.IntField(currentTimeMinute.intValue);
			// Create the label field.
			EditorGUILayout.LabelField (":", GUILayout.Width(25f));
			// Set our float field.
			currentTimeSecond.intValue = EditorGUILayout.IntField(currentTimeSecond.intValue);
			// IF its AM or PM.
			if (isAM.boolValue) {
				// Create the label field.
				EditorGUILayout.LabelField ("AM", GUILayout.Width (35f));
			} else {
				// Create the label field.
				EditorGUILayout.LabelField ("PM", GUILayout.Width(35f));
			}
			// End the disabled grp.
			EditorGUI.EndDisabledGroup ();
			// End the horizontal layout.
			EditorGUILayout.EndHorizontal();
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Add some vertical space.
			EditorGUILayout.Space ();

			// Create the label field.
			EditorGUILayout.LabelField (new GUIContent("1 Hour In-Game", "One hour in this game is equal to this time in our real life."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;

			// Begin a horizontal layout.
			EditorGUILayout.BeginHorizontal();

			// Begin a horizontal layout.
			EditorGUILayout.BeginVertical();
			// Begin a horizontal layout.
			EditorGUILayout.BeginHorizontal();
			// Create a label field.
			EditorGUILayout.LabelField(new GUIContent("  Hr"), GUILayout.Width(50f));
			// Create a label field.
			EditorGUILayout.LabelField(new GUIContent(" Min"), GUILayout.Width(50f));
			// Create a label field.
			EditorGUILayout.LabelField(new GUIContent(" Sec"), GUILayout.Width(50f));
			// End the horizontal layout.
			EditorGUILayout.EndHorizontal();

			// Begin a horizontal layout.
			EditorGUILayout.BeginHorizontal();
			// Set our float field.
			hourInGameConversion.intValue = EditorGUILayout.IntField(hourInGameConversion.intValue, GUILayout.Width(50f));
			// Set our float field.
			minuteInGameConversion.intValue = EditorGUILayout.IntField(minuteInGameConversion.intValue, GUILayout.Width(50f));
			// Set our float field.
			secondInGameConversion.intValue = EditorGUILayout.IntField(secondInGameConversion.intValue, GUILayout.Width(50f));
			// End the horizontal layout.
			EditorGUILayout.EndHorizontal();
			// End the horizontal layout.
			EditorGUILayout.EndVertical();

			// End the horizontal layout.
			EditorGUILayout.EndHorizontal();

			// Decrease the indent.
			EditorGUI.indentLevel--;
			// Apply.
			serializedObject.ApplyModifiedProperties();
		}
	}
}
