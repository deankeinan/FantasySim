using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace TrollBridge {

	/// <summary>
	/// This script sets up the locations that your player can potentially spawn at.  
	/// </summary>
	public class Scene_Locations : MonoBehaviour {

		// The list of areas that you can spawn via scene change.
		[Tooltip("The list of Transform locations that you can spawn at when transferring to this scene.")]
		public Transform[] sceneSpawnLocations;

		void Awake () {
			// Spawn the player.
			SpawnPlayer();
		}

		/// <summary>
		/// Spawns the player.
		/// </summary>
		private void SpawnPlayer(){
			// IF we have a player to spawn.
			if (Grid_Helper.setup.player != null) {
				// IF the player is already spawned,
				// ELSE the player is not spawned and needs to be and will be positioned accordingly.
				if (Character_Helper.GetPlayer () != null) {
					// Move this player to the new location.
					Character_Helper.GetPlayer ().transform.position = sceneSpawnLocations [Grid_Helper.setup.GetSceneSpawnLocation()].position;
				} else {
					// Create this player at the appropriate location.
					Instantiate (Grid_Helper.setup.player, sceneSpawnLocations [Grid_Helper.setup.GetSceneSpawnLocation()].position, Quaternion.identity);
				}
			}
		}
	}
}
