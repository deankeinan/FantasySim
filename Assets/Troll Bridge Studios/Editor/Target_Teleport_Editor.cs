using UnityEngine;
using System.Collections;
using UnityEditor;

namespace TrollBridge {

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Target_Teleport))]
	public class Target_Teleport_Editor : Editor{

		SerializedProperty targetTags;

		SerializedProperty soundClip;
		SerializedProperty minPitch;
		SerializedProperty maxPitch;

		SerializedProperty teleportStartEffects;
		SerializedProperty teleportEndEffects;

		SerializedProperty newLocation;

		void OnEnable(){
			targetTags = serializedObject.FindProperty ("targetTags");

			soundClip = serializedObject.FindProperty ("soundClip");
			minPitch = serializedObject.FindProperty ("minPitch");
			maxPitch = serializedObject.FindProperty ("maxPitch");

			teleportStartEffects = serializedObject.FindProperty ("teleportStartEffects");
			teleportEndEffects = serializedObject.FindProperty ("teleportEndEffects");

			newLocation = serializedObject.FindProperty ("newLocation");
		}

		public override void OnInspectorGUI(){
			// Set the indentLevel to 0 as default (no indent).
			EditorGUI.indentLevel = 0;
			// Update
			serializedObject.Update();


			// Tag Label.
			EditorGUILayout.LabelField("Tags", EditorStyles.boldLabel);
			// Increase the Indent the lines.
			EditorGUI.indentLevel++;
//			// Set our array for our chest.
//			EditorGUILayout.PropertyField(targetTags.FindPropertyRelative ("Array.size"), new GUIContent ("Tag Name", "The GameObjects with these tags can teleport.  IF this array length is 0 then there are no restrictions on teleporting and "));
			// Array for the Target Tags.
			EditorGUILayout.PropertyField(targetTags, new GUIContent("Tag Name", "The GameObjects with these tags can teleport.  IF this array length is 0 then there are no restrictions on teleporting and "), true);
			// Decrease the Indent.
			EditorGUI.indentLevel--;


			// Movement Label.
			EditorGUILayout.LabelField("Teleport", EditorStyles.boldLabel);
			// Increase the Indent.
			EditorGUI.indentLevel++;
			// The new location to be teleported to.
			EditorGUILayout.PropertyField(newLocation, new GUIContent("Teleport Location", "The location to be teleported."));
			// Decrease the Indent.
			EditorGUI.indentLevel--;


			// Sound Label.
			EditorGUILayout.LabelField("Sound", EditorStyles.boldLabel);
			// Increase the Indent.
			EditorGUI.indentLevel++;
			// The audio clip.
			EditorGUILayout.PropertyField(soundClip, new GUIContent("Sound Clip", "The sound clip to play when teleporting."));
			// The minimum pitch.
			EditorGUILayout.PropertyField(minPitch, new GUIContent("Minimum Pitch", "The minimum pitch this sound can be played at.  A random number between the minPitch and a maxPitch will be chosen."));
			// The maximum pitch.
			EditorGUILayout.PropertyField(maxPitch, new GUIContent("Maximum Pitch", "The maximum pitch this sound can be played at.  A random number between the minPitch and a maxPitch will be chosen."));
			// Decrease the Indent.
			EditorGUI.indentLevel--;


			// Effects Label.
			EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);
			// Increase the Indent.
			EditorGUI.indentLevel++;
			// The start animation.
			EditorGUILayout.PropertyField(teleportStartEffects, new GUIContent("Start Effects", "Start location teleport effects."));
			// The end animation.
			EditorGUILayout.PropertyField(teleportEndEffects, new GUIContent("End Effects", "End location teleport effects."));
			// Decrease the Indent.
			EditorGUI.indentLevel--;


			// Apply
			serializedObject.ApplyModifiedProperties();
		}
	}
}
