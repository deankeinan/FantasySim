using UnityEngine;
using UnityEditor;

namespace TrollBridge {

	[CustomPropertyDrawer (typeof (Bomb_Types))]
	public class Bomb_Type_Drawer : PropertyDrawer {
		
		// Draw the property inside the given rect
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			// Using BeginProperty / EndProperty on the parent property means that
			// prefab override logic works on the entire property.
			EditorGUI.BeginProperty (position, label, property);

			// Draw label
			position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);

			// Don't make child fields be indented.
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			// Calculate rects.
			Rect bombName = new Rect (position.x, position.y, 175f, position.height);
			Rect bombAmount = new Rect (position.x + 190f, position.y, 50f, position.height);
			Rect bombSound = new Rect (position.x + 260f, position.y, 120f, position.height);

			// Draw fields - passs GUIContent.none to each so they are drawn without labels.
			EditorGUI.PropertyField (bombName, property.FindPropertyRelative ("bombObject"), GUIContent.none);
			EditorGUI.PropertyField (bombAmount, property.FindPropertyRelative ("bombAmount"), GUIContent.none);
			EditorGUI.PropertyField (bombSound, property.FindPropertyRelative ("bombSound"), GUIContent.none);

			// Set indent back to what it was.
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty ();
		}
	}
}
