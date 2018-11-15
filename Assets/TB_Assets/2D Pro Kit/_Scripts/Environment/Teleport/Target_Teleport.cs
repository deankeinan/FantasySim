using UnityEngine;
using System.Collections;

namespace TrollBridge {

	[RequireComponent (typeof (Collider2D))]
	/// <summary>
	/// Target teleport will teleport a gameobject when it collides with a collider.
	/// </summary>
	public class Target_Teleport : MonoBehaviour {

		// The sound clip to play when teleporting.
		[Tooltip("The sound clip to play when teleporting.")]
		public AudioClip soundClip;

		// The min and max pitch for when this sound is played.
		[Tooltip("The minimum pitch this sound can be played at.")]
		public float minPitch = 1;
		[Tooltip("The maximum pitch this sound can be played at.")]
		public float maxPitch = 1;

		// Teleport effects for starting the destination.
		[Tooltip("Start location - teleport effects.")]
		public GameObject teleportStartEffects;
		// Teleport effects for reaching the destination.
		[Tooltip("End location - teleport effects.")]
		public GameObject teleportEndEffects;

		// The location to be teleported.
		[Tooltip("The location to be teleported.")]
		public Transform newLocation;

		// The GameObjects with these tags can teleport.
		[Tooltip("The GameObjects with these tags can teleport.  If empty then everything will teleport.")]
		public string[] targetTags;


		void OnTriggerEnter2D(Collider2D coll){
			// IF we have an empty array, meaning anything can teleport.
			if (targetTags.Length == 0) {
				// Teleport.
				Teleport (coll);
			} else {
				// Loop through each tag.
				for (int i = 0; i < targetTags.Length; i++) {
					// If we collide with a target tag.
					if (coll.gameObject.tag == targetTags [i]) {
						// Teleport.
						Teleport (coll);
						// Since we found a match we are done.
						return;
					}
				}
			}
		}

		void Teleport(Collider2D coll){
			// IF we have an effect to play at the start location.
			if (teleportStartEffects != null) {
				// Play an effect where the colliding object is.
				Instantiate (teleportStartEffects, coll.transform.position, Quaternion.identity);
			}
			// IF we have an effect to play at the end location.
			if (teleportEndEffects != null) {
				// Play an effect where the colliding object will be teleported to.
				Instantiate (teleportEndEffects, newLocation.transform.position, Quaternion.identity);
			}
			// Play the sound.
			Grid_Helper.soundManager.PlaySound (soundClip, transform.position, minPitch, maxPitch);
			// Teleport the other object. to the new position.
			coll.transform.position = newLocation.position;
		}
	}
}
