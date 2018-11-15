using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	[RequireComponent (typeof (Collider2D))]
	/// <summary>
	/// Section spawn will create monsters when the player enters a section.
	/// </summary>
	public class Section_Spawn : MonoBehaviour {

		[Tooltip("Drag and Drop for this variable the Monster_Spawner's associated with this section.")]
		[SerializeField] private Monster_Spawner[] mobSpawner;
		[Tooltip("If you want the monsters to be destroyed when you leave this section.")]
		[SerializeField] private bool destroyOnExit;

		private Camera_Follow_Slide cfs;
		private bool timeToSpawn;

		void Start(){
			// Find out which camera is being used.
			GameObject camGO = Camera.main.gameObject;
			// IF we have a follow slide camera,
			if (camGO.GetComponent<Camera_Follow_Slide> () != null) {
				cfs = camGO.GetComponent<Camera_Follow_Slide> ();
			}
		}

		void Update(){
			// IF we are ready to spawn.
			if (timeToSpawn) {
				// IF we have a camera follow slide,
				if (cfs != null) {
					// IF the camera is not panning.
					if (!cfs.GetIsPanning ()) {
						// We have entered the section and the camera has stopped panning.
						SpawnSectionSpawn (true);
						// Reset.
						timeToSpawn = false;
					}
				}
			}
		}

		void OnTriggerEnter2D(Collider2D coll){
			// IF this is the player.
			if (coll.gameObject == Character_Helper.GetPlayer ()) {
				// Set to true for spawning.
				timeToSpawn = true;
			}
		}

		void OnTriggerExit2D(Collider2D coll){
			// IF this is the player.
			if (coll.gameObject == Character_Helper.GetPlayer ()) {
				// IF we want to destroy the spawned gameobject when we leave this section.
				if (destroyOnExit) {
					// Loop through all of our Monster_Spawners.
					for (int i = 0; i < mobSpawner.Length; i++) {
						// Destroy all the monsters.
						mobSpawner [i].DestroyAllMonsters ();
					}
					// We leave.
					return;
				}
				// Loop through all of our Monster_Spawners.
				for (int i = 0; i < mobSpawner.Length; i++) {
					// Deactivate the monsters left in this area.
					mobSpawner [i].SetMonstersActiveness (false);
				}
			}
		}

		public void SpawnSectionSpawn(bool activeness){
			// Loop through all of our Monster_Spawners.
			for (int i = 0; i < mobSpawner.Length; i++) {
				// IF we have a mob spawner in this section.
				if (mobSpawner [i] != null) {
					// We want to Activate the monsters that were inactive.
					mobSpawner [i].SetMonstersActiveness (activeness);
					// We are now calling the Monster Spawner to spawn the mobs.
					mobSpawner [i].SpawnSetup ();
				}
			}
		}
	}
}