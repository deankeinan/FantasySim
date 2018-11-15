using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	[RequireComponent (typeof (Collider2D))]
	/// <summary>
	/// Manual monster spawner is a system that spawns monsters when the Player hits a Key.
	/// </summary>
	public class Manual_Monster_Spawner : MonoBehaviour {
		
		[Tooltip("The key that is pressed that will activate the Monster Spawner(s).")]
		[SerializeField] KeyCode interactionKey;
		[Tooltip("This is where you assign the Monster_Spawners that you want to work with this collision action.")]
		[SerializeField] Monster_Spawner[] mobSpawner;
		[Tooltip("This is used if you want to have some delay before spawning the monsters.")]
		[SerializeField] float delayedTime = 0f;

		private bool isCloseEnough = false;

		void OnCollisionEnter2D (Collision2D coll) {
			// IF the colliding GameObject is the player.
			if (coll.gameObject == Character_Helper.GetPlayer ()) {
				// We are close enough for when we use the KeyCode it will spawn monsters.
				isCloseEnough = true;
			}
		}
		void OnCollisionExit2D (Collision2D coll) {
			// IF the colliding GameObject is the player.
			if (coll.gameObject == Character_Helper.GetPlayer ()) {
				// We are NOT close enough for when we use the KeyCode it will spawn monsters.
				isCloseEnough = false;
			}
		}


		void OnTriggerEnter2D (Collider2D coll) {
			// IF the colliding GameObject is the player.
			if (coll.gameObject == Character_Helper.GetPlayer ()) {
				// We are close enough for when we use the KeyCode it will spawn monsters.
				isCloseEnough = true;
			}
		}
		void OnTriggerStay2D (Collider2D coll) {
			// IF the colliding GameObject is the player.
			if (coll.gameObject == Character_Helper.GetPlayer ()) {
				// We are close enough for when we use the KeyCode it will spawn monsters.
				isCloseEnough = true;
			}
		}
		void OnTriggerExit2D (Collider2D coll) {
			// IF the colliding GameObject is the player.
			if (coll.gameObject == Character_Helper.GetPlayer ()) {
				// We are NOT close enough for when we use the KeyCode it will spawn monsters.
				isCloseEnough = false;
			}
		}

		void Update() {
			// IF we are hitting the interactionKey AND we are close enough to trigger the monster event.
			if (Input.GetKeyDown (interactionKey) && isCloseEnough) {
				// Loop through all of our Monster Spawners.
				for (int i = 0; i < mobSpawner.Length; i++) {
					// Spawn the monsters.
					StartCoroutine (SpawnTimer (i));
				}
			}
		}

		private IEnumerator SpawnTimer (int index) {
			// We wait out our delayed time.
			yield return new WaitForSeconds (delayedTime);
			// spawn the monsters.
			mobSpawner [index].SpawnSetup ();
		}
	}
}
