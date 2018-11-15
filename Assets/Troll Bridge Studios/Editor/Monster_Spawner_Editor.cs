using UnityEditor;
using UnityEngine;

namespace TrollBridge {

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Monster_Spawner))]
	public class Monster_Spawner_Editor : Editor {

		SerializedProperty oneTimeSpawn;
		SerializedProperty spawner;
		SerializedProperty location;
		SerializedProperty spawnAllAtStart;
		SerializedProperty rangeRespawn;
		SerializedProperty alphaRange;
		SerializedProperty omegaRange;
		SerializedProperty spawnHolder;
		SerializedProperty isTimeBased;
		SerializedProperty timer;
		SerializedProperty setLayerFromSpawner;

		void OnEnable () {
			// Setup the SerializedProperties.
			oneTimeSpawn = serializedObject.FindProperty ("oneTimeSpawn");
			spawner = serializedObject.FindProperty ("spawner");
			location = serializedObject.FindProperty ("location");
			spawnAllAtStart = serializedObject.FindProperty ("spawnAllAtStart");
			rangeRespawn = serializedObject.FindProperty ("rangeRespawn");
			alphaRange = serializedObject.FindProperty ("alphaRange");
			omegaRange = serializedObject.FindProperty ("omegaRange");
			spawnHolder = serializedObject.FindProperty ("spawnHolder");
			isTimeBased = serializedObject.FindProperty ("isTimeBased");
			timer = serializedObject.FindProperty ("timer");
			setLayerFromSpawner = serializedObject.FindProperty ("setLayerFromSpawner");
		}

		public override void OnInspectorGUI(){

			// Set the indentLevel to 0 as default (no indent).
			EditorGUI.indentLevel = 0;
			// Update
			serializedObject.Update();

			EditorGUILayout.Space ();

			// Label.
			EditorGUILayout.LabelField (new GUIContent("What to Spawn", "The Monster (GameObject) to spawn."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Start a horizontal layout.
			EditorGUILayout.BeginHorizontal();
			// Label.
			EditorGUILayout.LabelField(new GUIContent("Monster", "The Monster (GameObjects) you want to spawn."), EditorStyles.boldLabel, GUILayout.Width(180f));
			// Label.
			EditorGUILayout.LabelField(new GUIContent("Max Amount", "The maximum amount of Monsters (GameObjects) this can spawn."), EditorStyles.boldLabel, GUILayout.Width(105f));
			// End the horizontal layout.
			EditorGUILayout.EndHorizontal ();
			// Create the Property Fields.
			EditorGUILayout.PropertyField(spawner, GUIContent.none);
			// Decrease the indent.
			EditorGUI.indentLevel--;

			EditorGUILayout.Space ();

			// Label.
			EditorGUILayout.LabelField (new GUIContent("Where to Spawn", "This is where you setup where you want your Monster (GameObjects) to spawn"), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Label.
			EditorGUILayout.LabelField(new GUIContent("Spawn Locations", "Where do we want to spawn our GameObjects.\n\n" +
				"If you have specific spots you want to spawn then have a GameObject where all of its children are the specific locations you want and then drag and drop the parent GameObject for this variable.\n\n" +
				"If you want spawning to be random within a certain area then drag and drop a GameObject which has a Collider2D attached to it (BoxCollider2D, CircleCollider2D, PolygonCollider2D, etc) for this variable.  \n\n" +
				"You can even have the Collider2D be attached to this GameObject"), EditorStyles.boldLabel);
			// Create the Property Fields.
			EditorGUILayout.PropertyField(location, GUIContent.none);
			// Label.
			EditorGUILayout.LabelField (new GUIContent("Spawn Holder", "Place the GameObject you wish to be the parent of the monsters spawned."), EditorStyles.boldLabel);
			// Create the Property Fields.
			EditorGUILayout.PropertyField(spawnHolder, GUIContent.none);
			// Decrease the indent.
			EditorGUI.indentLevel--;

			EditorGUILayout.Space ();

			// Label.
			EditorGUILayout.LabelField (new GUIContent("Spawn Restrictions", "The have restrictions to how the Monster (GameObjects) are spawned."), EditorStyles.boldLabel);
			// Increase the indent.
			EditorGUI.indentLevel++;
			// Create the property fields.
			setLayerFromSpawner.boolValue = EditorGUILayout.ToggleLeft (new GUIContent("Set Layer From Spawner", "When a monster is spawned do we set its layer from the GameObject that has the Monster Spawner script.  If set to false then the layer of the spawned mob will be based on its prefab."), setLayerFromSpawner.boolValue);

			// Create the property fields.
			oneTimeSpawn.boolValue = EditorGUILayout.ToggleLeft (new GUIContent("One Time Spawn", "If you want to have this monster spawner only work one time per scene load."), oneTimeSpawn.boolValue);
			// Create the property fields.
			spawnAllAtStart.boolValue = EditorGUILayout.ToggleLeft (new GUIContent("Spawn All At Start", "When the scene is loaded do you want the amount of Monsters equal to your max amount to be initially spawned."), spawnAllAtStart.boolValue);

			// Start a horizontal layout.
			EditorGUILayout.BeginHorizontal ();
			// Begin a Toggle Group.
			isTimeBased.boolValue = EditorGUILayout.BeginToggleGroup (new GUIContent ("Is Time Based", "Do we want to spawn mobs based on a timer?\n\n" +
				"If you choose to uncheck this than odds are you are going to manually spawn them with another script such as flipping a switch or moving a bookcase on top of a tile."), isTimeBased.boolValue);
			// Create the property fields.
			EditorGUILayout.PropertyField (timer, GUIContent.none);
			// End the Toggle Group.
			EditorGUILayout.EndToggleGroup ();
			// End the horizontal layout.
			EditorGUILayout.EndHorizontal ();

			// Start a horizontal layout.
			EditorGUILayout.BeginHorizontal ();
			// Begin a Toggle Group.
			rangeRespawn.boolValue = EditorGUILayout.BeginToggleGroup (new GUIContent ("Range Respawn", "If you would like a certain amount of monsters to spawn when a respawn happens.\n\n" +
				"This is used for when a respawn happens you do not want the max amount to be flat out spawned.\n\n" +
				"Instead you can go for shorter respawn timers and have 1-3 spawn instead of 'max amount' always being spawned"), rangeRespawn.boolValue);
			// Start a horizontal layout.
			EditorGUILayout.BeginHorizontal ();
			// Create the property fields.
			EditorGUILayout.PropertyField (alphaRange, GUIContent.none);
			EditorGUILayout.PropertyField (omegaRange, GUIContent.none);
			// End the horizontal layout.
			EditorGUILayout.EndHorizontal ();
			// End the Toggle Group.
			EditorGUILayout.EndToggleGroup ();
			// End the horizontal layout.
			EditorGUILayout.EndHorizontal ();
			// Decrease the indent.
			EditorGUI.indentLevel--;

			// Apply.
			serializedObject.ApplyModifiedProperties();
		}
	}
}