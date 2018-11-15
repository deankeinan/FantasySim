using UnityEngine;
using UnityEditor;

namespace TrollBridge {

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Dialogue_Setup))]
	public class Dialogue_Setup_Editor : Editor {

		SerializedProperty entityName;
		SerializedProperty uniqueName;
		SerializedProperty dialogueTreeNames;
		SerializedProperty startingDialogue;
		SerializedProperty typeText;
		SerializedProperty typeSound;

		void OnEnable () {
			// Setup the SerializedProperties.
			entityName = serializedObject.FindProperty ("entityName");
			uniqueName = serializedObject.FindProperty ("uniqueName");
			dialogueTreeNames = serializedObject.FindProperty ("dialogueTreeNames");
			startingDialogue = serializedObject.FindProperty ("startingDialogue");
			typeText = serializedObject.FindProperty ("typeText");
			typeSound = serializedObject.FindProperty ("typeSound");
		}

		public override void OnInspectorGUI() {

			// Set the indentLevel to 0 as default (no indent).
			EditorGUI.indentLevel = 0;
			// Update
			serializedObject.Update ();
			// Add some space.
			EditorGUILayout.Space ();

			// The Name Label.
			EditorGUILayout.LabelField (new GUIContent ("Name Label", "Here is where you will give your he/she/it a name to display in the Dialogue and for referencing."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// The name of this thing.
			EditorGUILayout.PropertyField (entityName, new GUIContent ("Entity Name", "The name of this object you want to display during the dialogue."));
			// The unique identifier name you want this thing to have.
			EditorGUILayout.PropertyField (uniqueName, new GUIContent ("Unique Name", "The unique identifier for this object so that if we want to dictate another dialouge we would reference this."));
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Add some space.
			EditorGUILayout.Space ();

			// The Dialogue Tree Label.
			EditorGUILayout.LabelField (new GUIContent ("Dialogue Trees", "Here is where you will give your he/she/it a name to display in the Dialogue and for referencing."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Set our array for our chest.
			EditorGUILayout.PropertyField (dialogueTreeNames.FindPropertyRelative ("Array.size"), new GUIContent ("Dialogue Tree", "The JSON name of the dialogue tree you want to have associated with this object."));
			// IF there isnt any items in our chest.
			if (dialogueTreeNames.arraySize == 0) {
				// Set the default size of this array.
				dialogueTreeNames.arraySize = 1;
			}
			// Loop the amount of times we have chest loot.
			for (int i = 0; i < dialogueTreeNames.arraySize; i++) {
				// Add in the property fields that will be our array spots.
				EditorGUILayout.PropertyField (dialogueTreeNames.GetArrayElementAtIndex (i), GUIContent.none);
			}
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Add some space.
			EditorGUILayout.Space ();

			// The advanced options text label.
			EditorGUILayout.LabelField (new GUIContent ("Text", "Here is where you can choose how you want the text to be displayed in the dialogue."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// The starting dialogue string..
			EditorGUILayout.PropertyField (startingDialogue, new GUIContent ("Starting Dialogue", "String that will be placed for buttons in your UI for this Dialogue_Setup."));
			// Type text.
			EditorGUILayout.PropertyField (typeText, new GUIContent ("Typed Text", "This will make sure your text is typed out."));
			// IF we want to have typed out text.
			if (typeText.boolValue) {
				// The typed text sound.
				EditorGUILayout.PropertyField (typeSound, new GUIContent ("Typed Text Sound", "This sound will be played whenever a character is typed out in our Typed Text."));
			}
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Add some space.
			EditorGUILayout.Space ();

			// The Quest Dialogue Label.
			EditorGUILayout.LabelField (new GUIContent ("Quest Dialogue", "Dialogue to be dealt with anything quest based."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Apply.
			serializedObject.ApplyModifiedProperties ();
		}
	}
}
