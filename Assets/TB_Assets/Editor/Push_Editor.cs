using UnityEngine;
using UnityEditor;

namespace TrollBridge {

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Push))]
	public class Push_Editor : Editor {

		SerializedProperty topCollider;
		SerializedProperty bottomCollider;
		SerializedProperty leftCollider;
		SerializedProperty rightCollider;
		SerializedProperty parent;
		SerializedProperty tagsThatPushThis;
		SerializedProperty timeToPush;
		SerializedProperty moveSpeed;
		SerializedProperty soundClip;
		SerializedProperty minPitch;
		SerializedProperty maxPitch;

		void OnEnable () 
		{
			// Setup the SerializedProperties.
			topCollider = serializedObject.FindProperty("topCollider");
			bottomCollider = serializedObject.FindProperty("bottomCollider");
			leftCollider = serializedObject.FindProperty("leftCollider");
			rightCollider = serializedObject.FindProperty("rightCollider");
			parent = serializedObject.FindProperty("parent");
			tagsThatPushThis = serializedObject.FindProperty("tagsThatPushThis");
			timeToPush = serializedObject.FindProperty("timeToPush");
			moveSpeed = serializedObject.FindProperty("moveSpeed");
			soundClip = serializedObject.FindProperty("soundClip");
			minPitch = serializedObject.FindProperty("minPitch");
			maxPitch = serializedObject.FindProperty("maxPitch");
		}

		public override void OnInspectorGUI()
		{
			// Set the indentLevel to 0 as default (no indent).
			EditorGUI.indentLevel = 0;
			// Update
			serializedObject.Update();

			// The Edge label.
			EditorGUILayout.LabelField (new GUIContent("Edge", "Select which edge this GameObject represents."), EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			topCollider.boolValue = EditorGUILayout.Toggle (new GUIContent ("Top Collider", "If this is the top part of the collider then mark this true."), !bottomCollider.boolValue && !leftCollider.boolValue && !rightCollider.boolValue);
			bottomCollider.boolValue = EditorGUILayout.Toggle (new GUIContent ("Bottom Collider", "If this is the bottom part of the collider then mark this true."), (!topCollider.boolValue && !leftCollider.boolValue && !rightCollider.boolValue) && (bottomCollider.boolValue && !topCollider.boolValue && !leftCollider.boolValue && !rightCollider.boolValue));
			leftCollider.boolValue = EditorGUILayout.Toggle (new GUIContent ("Left Collider", "If this is the left part of the collider then mark this true."), (!topCollider.boolValue && !bottomCollider.boolValue && !rightCollider.boolValue) && (leftCollider.boolValue && !topCollider.boolValue && !bottomCollider.boolValue && !rightCollider.boolValue));
			rightCollider.boolValue = EditorGUILayout.Toggle (new GUIContent ("Right Collider", "If this is the right part of the collider then mark this true."), (!topCollider.boolValue && !bottomCollider.boolValue && !leftCollider.boolValue) && (rightCollider.boolValue && !topCollider.boolValue && !bottomCollider.boolValue && !leftCollider.boolValue));
			EditorGUI.indentLevel--;

			// The Push Mechanics label.
			EditorGUILayout.LabelField (new GUIContent ("Push Mechanics", "Variables that takes care of the setup and the action of when something is being pushed."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// The parent GameObject.
			EditorGUILayout.PropertyField (parent, new GUIContent ("Parent GameObject", "The parent GameObject (The entity as a whole) which this child GameObject is part of."));
			// The time it takes before the GameObject is pushed.
			EditorGUILayout.PropertyField (timeToPush, new GUIContent ("Time To Push", "The build up time of colliding with this GameObject before it is actually pushed."));
			// The speed of the GameObject when it is pushed.
			EditorGUILayout.PropertyField (moveSpeed, new GUIContent ("Push Speed", "The speed at which when this GameObject is pushed."));
			// Get field for the user to put in the size of the array.
			EditorGUILayout.PropertyField(tagsThatPushThis.FindPropertyRelative("Array.size"), new GUIContent ("Tag Names That Push This", "That GameObject tags that are allowed to interact with this GameObject."));
			// IF there isnt any keys we create a default of 1 since what is the point of this script if you are not dropping anything.
			if(tagsThatPushThis.arraySize == 0)
			{
				// Set the default size of this array.
				tagsThatPushThis.arraySize = 1;
			}
			// Display the array elements.
			for(int i = 0; i < tagsThatPushThis.arraySize; i++)
			{
				// The array of TagsThatPushThis.
				EditorGUILayout.PropertyField(tagsThatPushThis.GetArrayElementAtIndex(i), GUIContent.none, GUILayout.Width (175f));
			}
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// The Push Sounds label.
			EditorGUILayout.LabelField (new GUIContent ("Push Sounds", "The sound and pitch of the sound to be played while the GameObject is being pushed."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Push Sound.
			EditorGUILayout.PropertyField (soundClip, new GUIContent ("Push Sound", "The sound that is played when this GameObject is being pushed."));
			// Min and Max Pitch of Push Sound.
			EditorGUILayout.PropertyField (minPitch, new GUIContent ("Minimum Pitch", "The lowest pitch the Push Sound will be."));
			EditorGUILayout.PropertyField (maxPitch, new GUIContent ("Maximum Pitch", "The highest pitch the Push Sound will be."));
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Apply.
			serializedObject.ApplyModifiedProperties();
		}
	}
}
