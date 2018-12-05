using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	[RequireComponent(typeof(Collider2D))]
	/// <summary>
	/// Monster Spawner that will create monsters when we enter this GameObjects Collider2D.
	/// </summary>
	public class Collision_Monster_Spawner : MonoBehaviour 
	{

		[Tooltip("Do you want to have this script run more than once after it spawns?\n\n" +
			"Example would be if I push a bookcase on top of a button that triggers monsters to spawn, I then kill the monsters and move the same bookcase off the button and back on the button.\n\n" +
			"This variable handles if you want this to work multiple times or a 1 time action.")]
		[SerializeField] private bool multipleSpawns;

		[Tooltip("If you want to have spawning be based on GameObject Tags colliding then drag and drop the GameObject that you want to spawn mobs when a collision happens for this variable.")]
		[SerializeField] private string[] tagsThatSpawn;

		[Tooltip("If you want to have spawning be based on GameObjects colliding then drag and drop the GameObject that you want to spawn mobs when a collision happens for this variable.")]
		[SerializeField] private GameObject[] collisionsThatSpawn;

		[Tooltip("This is where you assign the Monster_Spawners that you want to work with this collision action.")]
		[SerializeField] private Monster_Spawner[] mobSpawner;

		[Tooltip("This is used if you want to have some delay before spawning the monsters.")]
		[SerializeField] private float delayedTime = 0f;


		void OnCollisionEnter2D (Collision2D coll) {
			SpawnBasedOnCollision (coll.gameObject);
		}

		void OnTriggerEnter2D (Collider2D coll) {
			SpawnBasedOnCollision (coll.gameObject);
		}

		private void SpawnBasedOnCollision(GameObject collidingGameObject) {
			// Loop through our array of GameObjects and see if this matches our current colliding GameObject.
			for (int i = 0; i < collisionsThatSpawn.Length; i++) {
				// IF we have a match.
				if (collisionsThatSpawn [i] == collidingGameObject) {
					// Loop through our Monster Spawner array and spawn the mobs from them since we have a match.
					for (int j = 0; j < mobSpawner.Length; j++) {
						// Spawn the monsters.
						StartCoroutine (SpawnTimer (j));
					}
					// We are done so lets leave.
					return;
				}
			}

			// Loop through our array of GameObject tags.
			for (int i = 0; i < tagsThatSpawn.Length; i++) {
				// IF we have a tag match.
				if (tagsThatSpawn [i] == collidingGameObject.tag) {
					// Loop through our Monster Spawner array and spawn the mobs from them since we have a match.
					for (int j = 0; j < mobSpawner.Length; j++) {
						// Spawn the monsters.
						StartCoroutine (SpawnTimer (j));
					}
					// We are done so lets leave.
					return;
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
