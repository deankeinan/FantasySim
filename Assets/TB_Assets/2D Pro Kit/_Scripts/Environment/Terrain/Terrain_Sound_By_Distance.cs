using UnityEngine;
using System.Collections;

namespace TrollBridge {

	[RequireComponent (typeof (Collider2D))]
	/// <summary>
	/// Terrain sound by distance will play an AudioClip based on a certain amount of distance we have traveled.
	/// </summary>
	public class Terrain_Sound_By_Distance : MonoBehaviour {

		// The sound clip to play when Colliding.
		public AudioClip soundClip;
		// The min and max pitch for when this sound is played.
		public float minPitch = 1;
		public float maxPitch = 1;

		// The distance before playing another sound
		public float distance = 1;
		// The distance variable that holds the current distance.
		private float currDistance;

		// Previous position.
		private Vector2 prev;
		// Current position.
		private Vector2 curr;


		void OnTriggerEnter2D(Collider2D coll){
			// IF there is a sound to play.
			if(soundClip != null && coll.gameObject.tag == "Player"){
				// Set the start distance.
				curr = coll.transform.position;
			}
		}

		void OnTriggerStay2D(Collider2D coll){
			// IF we are colliding with a Player tag.
			if(coll.gameObject.tag == "Player"){
				// The new is the old.
				prev = curr;
				// Get the current distance.
				curr = coll.transform.position;
				// Compare distances.
				currDistance += Vector2.Distance(curr, prev);

				// IF we have traveled the required amount.
				if(currDistance > distance){
					// Play the sound.
					AudioSource soundSource = Grid_Helper.soundManager.PlaySound(soundClip, coll.transform.position, minPitch, maxPitch);
					// IF we have the sound muted or no soundClip was provided.
					if (soundSource == null) {
						// we leave.
						return;
					}
					// Set the parent of this gameobject.
					Grid_Helper.helper.SetParentTransform(coll.gameObject.transform, soundSource.gameObject);
					// Reset currDistance.
					currDistance = 0;
				}
			}
		}
	}
}
