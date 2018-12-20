using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TrollBridge {

	[CustomPropertyDrawer (typeof (Spawn_System))]
	public class Spawn_System_Drawer : PropertyDrawer {

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
			Rect monster = new Rect (position.x, position.y, 175f, position.height);
			Rect maxMonsterAmount = new Rect (position.x + 190f, position.y, 70f, position.height);
//			Rect location = new Rect (position.x + 280f, position.y, 135f, position.height);
//			Rect timer = new Rect (position.x + 280f, position.y, 70f, position.height);

			// Draw fields - passs GUIContent.none to each so they are drawn without labels.
			EditorGUI.PropertyField (monster, property.FindPropertyRelative ("monster"), GUIContent.none);
			EditorGUI.PropertyField (maxMonsterAmount, property.FindPropertyRelative ("maxMonsterAmount"), GUIContent.none);
//			EditorGUI.PropertyField (location, property.FindPropertyRelative ("location"), GUIContent.none);
//			EditorGUI.PropertyField (timer, property.FindPropertyRelative ("timer"), GUIContent.none);

			// Set indent back to what it was.
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty ();
		}
	}
}
