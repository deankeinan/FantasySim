using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor;

namespace TrollBridge {

	/// <summary>
	/// When we dont want to save when changing scenes.  This is useful if we want to transfer to a Main Menu scene or a similar scene like Main Menu.
	/// </summary>
	public class Scene_Change_Button : MonoBehaviour {

		// boolean for controling our layer.
		[SerializeField] private bool controlLayer = false;
		// The layer that the player will have when going to the next scene.
		[SerializeField] private LayerMask changePlayerLayer;

		/// <summary>
		/// Goes to the next scene without saving any information.
		/// </summary>
		public void NextScene(string nextScene){
			//// Clear list of our states.
			//Grid_Helper.stateManager.ClearList ();
			//// IF we want to control the layer going into the next scene.
			//if (controlLayer) {
			//	// Put our player on this layer.
			//	Character_Helper.GetPlayerManager ().GetComponent <Character_Manager> ().ChangeLayer (changePlayerLayer.value);
			//}
			//// Load the next scene.
			//SceneManager.LoadScene (nextScene);
		}
	}

	/// <summary>
	/// Scene Change Button editor.
	/// </summary>
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Scene_Change_Button))]
	public class Scene_Change_Button_Editor : Editor {

		SerializedProperty controlLayer;
		SerializedProperty changePlayerLayer;

		void OnEnable () {
			// Setup the SerializedProperties.
			controlLayer = serializedObject.FindProperty ("controlLayer");
			changePlayerLayer = serializedObject.FindProperty ("changePlayerLayer");
		}

		public override void OnInspectorGUI() {
			// Set the indentLevel to 0 as default (no indent).
			EditorGUI.indentLevel = 0;
			// Update.
			serializedObject.Update ();
			// Label for the scene change.
			EditorGUILayout.LabelField(new GUIContent ("Control Layer", "When we want to control the layer of the player going into the next scene."), EditorStyles.boldLabel);
			// Display the boolean.
			EditorGUILayout.PropertyField (controlLayer, new GUIContent ("Control Layer", "Do we want to control the layer when going to the next scene?"));
			// IF we selected to control our Layer on the scene change
			if (controlLayer.boolValue) {
				// Display the Layer.
				EditorGUILayout.PropertyField (changePlayerLayer, new GUIContent ("New Layer", "The layer the player will have when going to the next scene."));
			}
			// Apply.
			serializedObject.ApplyModifiedProperties ();
		}
	}
}
