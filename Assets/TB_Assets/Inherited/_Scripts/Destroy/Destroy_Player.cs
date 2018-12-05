using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Destroy player on scene load if this is enabled.
	/// </summary>
	public class Destroy_Player : MonoBehaviour {

		void Awake() {
			// Get the player managers gameobject
			GameObject player = Character_Helper.GetPlayerManager ();
			// IF we found the player.
			if (player != null) {
				// Destroy the player.
				Destroy (player);
			}
		}
	}
}
