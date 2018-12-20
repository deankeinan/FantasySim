using UnityEditor;
using UnityEngine;

namespace TrollBridge {

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Cycle_LightIntensity))]
	public class Cycle_LightIntensity_Editor : Editor {

		SerializedProperty lightComponent;
		SerializedProperty twelveAMIntensity, oneAMIntensity, twoAMIntensity, threeAMIntensity, fourAMIntensity, fiveAMIntensity, sixAMIntensity, sevenAMIntensity, eightAMIntensity, nineAMIntensity, tenAMIntensity, elevenAMIntensity;
		SerializedProperty twelvePMIntensity, onePMIntensity, twoPMIntensity, threePMIntensity, fourPMIntensity, fivePMIntensity, sixPMIntensity, sevenPMIntensity, eightPMIntensity, ninePMIntensity, tenPMIntensity, elevenPMIntensity;
		SerializedProperty gtTag;
		SerializedProperty instantChange, lerpThroughHour, lerpAtHour;
		SerializedProperty lerpAtHourSpeed;


		void OnEnable () {
			// Setup the SerializedProperties.
			lightComponent = serializedObject.FindProperty ("lightComponent");

			twelveAMIntensity = serializedObject.FindProperty ("twelveAMIntensity");
			oneAMIntensity = serializedObject.FindProperty ("oneAMIntensity");
			twoAMIntensity = serializedObject.FindProperty ("twoAMIntensity");
			threeAMIntensity = serializedObject.FindProperty ("threeAMIntensity");
			fourAMIntensity = serializedObject.FindProperty ("fourAMIntensity");
			fiveAMIntensity = serializedObject.FindProperty ("fiveAMIntensity");
			sixAMIntensity = serializedObject.FindProperty ("sixAMIntensity");
			sevenAMIntensity = serializedObject.FindProperty ("sevenAMIntensity");
			eightAMIntensity = serializedObject.FindProperty ("eightAMIntensity");
			nineAMIntensity = serializedObject.FindProperty ("nineAMIntensity");
			tenAMIntensity = serializedObject.FindProperty ("tenAMIntensity");
			elevenAMIntensity = serializedObject.FindProperty ("elevenAMIntensity");

			twelvePMIntensity = serializedObject.FindProperty ("twelvePMIntensity");
			onePMIntensity = serializedObject.FindProperty ("onePMIntensity");
			twoPMIntensity = serializedObject.FindProperty ("twoPMIntensity");
			threePMIntensity = serializedObject.FindProperty ("threePMIntensity");
			fourPMIntensity = serializedObject.FindProperty ("fourPMIntensity");
			fivePMIntensity = serializedObject.FindProperty ("fivePMIntensity");
			sixPMIntensity = serializedObject.FindProperty ("sixPMIntensity");
			sevenPMIntensity = serializedObject.FindProperty ("sevenPMIntensity");
			eightPMIntensity = serializedObject.FindProperty ("eightPMIntensity");
			ninePMIntensity = serializedObject.FindProperty ("ninePMIntensity");
			tenPMIntensity = serializedObject.FindProperty ("tenPMIntensity");
			elevenPMIntensity = serializedObject.FindProperty ("elevenPMIntensity");

			gtTag = serializedObject.FindProperty ("gtTag");

			instantChange = serializedObject.FindProperty ("instantChange");
			lerpThroughHour = serializedObject.FindProperty ("lerpThroughHour");
			lerpAtHour = serializedObject.FindProperty ("lerpAtHour");
			lerpAtHourSpeed = serializedObject.FindProperty ("lerpAtHourSpeed");
		}

		public override void OnInspectorGUI()
		{
			// Set the indentLevel to 0 as default (no indent).
			EditorGUI.indentLevel = 0;
			// Update
			serializedObject.Update ();

			// Create the label field.
			EditorGUILayout.LabelField (new GUIContent ("Light Source"), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Create a property field.
			EditorGUILayout.PropertyField (lightComponent, new GUIContent ("Light Component", "The Light component used for our lighting in the scene"));
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Create the label field.
			EditorGUILayout.LabelField (new GUIContent ("Game Timer"), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Create a property field.
			EditorGUILayout.PropertyField (gtTag, new GUIContent ("Game Timer Tag", "The tag of the GameObject that holds our in-game timer script."));
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Create the label field.
			EditorGUILayout.LabelField (new GUIContent ("Lighting Change", "How do we want to change our lighting when it does change."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Create a property field.
			instantChange.boolValue = EditorGUILayout.ToggleLeft (new GUIContent ("Instant Change", "When we hit the next hour do we want to just INSTANTLY change the light intensity with no lerping."), !lerpThroughHour.boolValue && !lerpAtHour.boolValue);
			// Create a property field.
			lerpThroughHour.boolValue = EditorGUILayout.ToggleLeft (new GUIContent ("Lerp Throughout The Hour", "When we want the lighting to gradually change to the next hours lighting."), !instantChange.boolValue && !lerpAtHour.boolValue);
			// Begin a horizontal layout.
			EditorGUILayout.BeginHorizontal ();
			// Create a property field.
			lerpAtHour.boolValue = EditorGUILayout.ToggleLeft (new GUIContent ("Lerp At The Hour", "When we want to change the lighting once we hit the next hour."), !instantChange.boolValue && !lerpThroughHour.boolValue);
			// End the horizontal layout.
			EditorGUILayout.EndHorizontal ();
			// Decrease the indent.
			EditorGUI.indentLevel--;

			if (lerpAtHour.boolValue) {
				// Create the label field.
				EditorGUILayout.LabelField (new GUIContent ("Lerp At The Hour Options"), EditorStyles.boldLabel);
				// Increase the indent.
				EditorGUI.indentLevel++;
				// Set our property fields.
				EditorGUILayout.PropertyField (lerpAtHourSpeed, new GUIContent ("Lerp At The Hour Speed", "The time in seconds it takes to change the light intensity when the next hour is reached."));
				// Decrease the indent.
				EditorGUI.indentLevel--;
			}

			// Create the label field.
			EditorGUILayout.LabelField (new GUIContent ("Time-Light Intensity", "The light intensity based on what the ingame time is."), EditorStyles.boldLabel);
			// Add some space.
			EditorGUILayout.Space ();
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Create the label field.
			EditorGUILayout.LabelField (new GUIContent ("A.M."), EditorStyles.boldLabel);
			// Set our property fields.
			EditorGUILayout.PropertyField (twelveAMIntensity, new GUIContent ("12:00 AM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (oneAMIntensity, new GUIContent ("1:00 AM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (twoAMIntensity, new GUIContent ("2:00 AM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (threeAMIntensity, new GUIContent ("3:00 AM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (fourAMIntensity, new GUIContent ("4:00 AM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (fiveAMIntensity, new GUIContent ("5:00 AM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (sixAMIntensity, new GUIContent ("6:00 AM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (sevenAMIntensity, new GUIContent ("7:00 AM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (eightAMIntensity, new GUIContent ("8:00 AM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (nineAMIntensity, new GUIContent ("9:00 AM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (tenAMIntensity, new GUIContent ("10:00 AM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (elevenAMIntensity, new GUIContent ("11:00 AM", "The light intensity you want for this time."));

			// Add some space.
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			// Create the label field.
			EditorGUILayout.LabelField (new GUIContent ("P.M."), EditorStyles.boldLabel);
			EditorGUILayout.PropertyField (twelvePMIntensity, new GUIContent ("12:00 PM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (onePMIntensity, new GUIContent ("1:00 PM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (twoPMIntensity, new GUIContent ("2:00 PM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (threePMIntensity, new GUIContent ("3:00 PM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (fourPMIntensity, new GUIContent ("4:00 PM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (fivePMIntensity, new GUIContent ("5:00 PM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (sixPMIntensity, new GUIContent ("6:00 PM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (sevenPMIntensity, new GUIContent ("7:00 PM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (eightPMIntensity, new GUIContent ("8:00 PM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (ninePMIntensity, new GUIContent ("9:00 PM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (tenPMIntensity, new GUIContent ("10:00 PM", "The light intensity you want for this time."));
			EditorGUILayout.PropertyField (elevenPMIntensity, new GUIContent ("11:00 PM", "The light intensity you want for this time."));
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Apply.
			serializedObject.ApplyModifiedProperties ();
		}
	}
}