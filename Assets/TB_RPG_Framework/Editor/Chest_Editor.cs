using UnityEngine;
using UnityEditor;

namespace TrollBridge {

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Chest))]
	public class Chest_Editor : Editor {

		SerializedProperty openChest;
		SerializedProperty closedChest;
		SerializedProperty openChestSound;
		SerializedProperty chestLoot;
		SerializedProperty requireKeyToOpen;
		SerializedProperty sendStraightToInventory;
		SerializedProperty ckc;
		SerializedProperty forcePower;

		void OnEnable ()
		{
			// Setup the SerializedProperties.
			openChest = serializedObject.FindProperty ("openChest");
			closedChest = serializedObject.FindProperty ("closedChest");
			openChestSound = serializedObject.FindProperty ("openChestSound");
			chestLoot = serializedObject.FindProperty ("chestLoot");
			requireKeyToOpen = serializedObject.FindProperty ("requireKeyToOpen");
			sendStraightToInventory = serializedObject.FindProperty ("sendStraightToInventory");
			ckc = serializedObject.FindProperty ("ckc");
			forcePower = serializedObject.FindProperty ("forcePower");
		}

		public override void OnInspectorGUI()
		{

			// Set the indentLevel to 0 as default (no indent).
			EditorGUI.indentLevel = 0;
			// Update
			serializedObject.Update();

			// Add some space.
			EditorGUILayout.Space ();

			// The chest visual label.
			EditorGUILayout.LabelField(new GUIContent("Chest Visual and Sounds", "The open and close look of the chest as well as the sound it makes when opened."), EditorStyles.boldLabel, GUILayout.Width(180f));
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Open chest sprite
			EditorGUILayout.PropertyField (openChest);
			// Close chest sprite.
			EditorGUILayout.PropertyField (closedChest);
			// The sound the chest makes when its opened.
			EditorGUILayout.PropertyField (openChestSound);
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Add some space.
			EditorGUILayout.Space ();

			// Set our array for our chest.
			EditorGUILayout.PropertyField(chestLoot.FindPropertyRelative ("Array.size"), new GUIContent ("Chest Loot", "The loot (GameObjects) that you want to be in this chest."));
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Send straight to inventory property field.
			EditorGUILayout.PropertyField (sendStraightToInventory, new GUIContent ("Send Straight To Inventory", "Do you want the loot in this chest to go straight to your inventory?"));
			// IF we are not sending this straight to the inventory.
			if(!sendStraightToInventory.boolValue){
				EditorGUILayout.PropertyField (forcePower, new GUIContent("Loot launch speed", "The speed at which this loot is tossed from the chest."));
			}
			// IF there isnt any items in our chest.
			if (chestLoot.arraySize == 0) {
				// Set the default size of this array.
				chestLoot.arraySize = 1;
			}
			// Loop the amount of times we have chest loot.
			for (int i = 0; i < chestLoot.arraySize; i++) {
				// Add in the property fields that will be our array spots.
				EditorGUILayout.PropertyField (chestLoot.GetArrayElementAtIndex (i), GUIContent.none);
			}
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Add some space.
			EditorGUILayout.Space ();

			// Set our bool if we need a key to open.
			requireKeyToOpen.boolValue = EditorGUILayout.BeginToggleGroup (new GUIContent("Require Key To Open", "Does this chest require a key to open."), requireKeyToOpen.boolValue);
			// Increase the indent.
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(new GUIContent("Key Name", "The name of the key."), EditorStyles.boldLabel, GUILayout.Width(180f));
			EditorGUILayout.LabelField(new GUIContent("Consume", "Does this key get consumed when used."), EditorStyles.boldLabel, GUILayout.Width(90f));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.PropertyField(ckc, GUIContent.none);
			// Decrease the indent.
			EditorGUI.indentLevel--;
			// End the ToggleGroup.
			EditorGUILayout.EndToggleGroup ();

			// Apply.
			serializedObject.ApplyModifiedProperties();
		}
	}
}
