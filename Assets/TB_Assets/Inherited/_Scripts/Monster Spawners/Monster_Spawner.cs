using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Monster spawner system.
	/// </summary>
	public class Monster_Spawner : MonoBehaviour {

		[SerializeField] bool oneTimeSpawn;
		[SerializeField] bool rangeRespawn;
		[SerializeField] bool spawnAllAtStart;
		[SerializeField] Spawn_System spawner;
		[SerializeField] GameObject location;
		[SerializeField] int alphaRange = 0;
		[SerializeField] int omegaRange  = 0;
		[SerializeField] Transform spawnHolder;
		[SerializeField] bool isTimeBased = true;
		[SerializeField] float timer = 0f;
		[SerializeField] bool setLayerFromSpawner = false;

		private bool isSpawned = false;
		private bool isSpecific = false;
		private Collider2D coll;
		private float currentTimer;

		private List<GameObject> mobSpawned = new List<GameObject> ();
		private List<Transform> spawnPoints = new List<Transform> ();

		void Start() {
			// Lets set our timer up.
			currentTimer = timer;

			// Get the Collider.
			coll = location.GetComponent<Collider2D> ();
			// IF we don't have a Collider2D.
			if (coll == null) {
				// We have specific locations with the locations being the child of the 'location' GameObject.
				isSpecific = true;
				// Loop and store the specific locations.
				for (int i = 0; i < location.transform.childCount; i++) {
					// Store the transform.
					spawnPoints.Add (location.transform.GetChild (i));
				}
			}

			// Do we want all of our mobs to spawn at the get go.
			if (spawnAllAtStart) {
				// We need to spawn our GameObjects when everything starts.
				Spawn (spawner.monster, spawner.maxMonsterAmount);
			}
		}

		void Update() {
			// IF we have spawning based on a timer.
			if (isTimeBased) {
				// Countdown our current time.
				currentTimer -= Time.deltaTime;
				// IF our currentTimer is less than or equal to 0, meaning it's time to spawn.
				if (currentTimer <= 0f) {
					// Go into our logic here to decide how to spawn our monsters.
					SpawnSetup ();
					// Reset the countdown.
					currentTimer = timer;
				}
			}
		}

		/// <summary>
		/// Time to spawn some mobs.
		/// </summary>
		private void Spawn(GameObject monster, int amountToSpawn) {
			// IF we already spawned.
			if (isSpawned) {
				// isSpawned is true so we only want this script to work 1 time for spawning.
				return;
			}

			// IF there is a mob to spawn AND we are spawning 1 more or higher.
			if (monster != null && amountToSpawn > 0) {
				// Lets spawn it based on the monsterAmount.
				for (int j = 0; j <= amountToSpawn; j++) {
					// IF the count of the amount of monsters spawned equals the max amount that we want to be spawned.
					if (mobSpawned.Count >= spawner.maxMonsterAmount) {
						// IF we have a one time spawn restriction.
						if (oneTimeSpawn) {
							isSpawned = true;
						}
						// We have the max spawned so lets leave.
						return;
					}
					// Variable for our location to spawn.
					Vector3 locationToSpawn;
					// IF we have specific locations to spawn,
					// ELSE we have an area to spawn in.
					if (isSpecific) {
						// Find a spot within our predefined locations to spawn
						locationToSpawn = spawnPoints [j].position;
					} else {
						// Find a spot to spawn a monster.
						locationToSpawn = PointInCollider ();
					}

					// The GameObject for our Monster.
					GameObject monsterSpawned;
					// IF we want to set the layer of the spawned monster the same as the spawner.
					if (setLayerFromSpawner) {
						// Actually spawn the monster.
						monsterSpawned = Grid_Helper.helper.SpawnObject (monster, locationToSpawn, Quaternion.identity, gameObject);
					} else {
						// Actually spawn the monster.
						monsterSpawned = Grid_Helper.helper.SpawnObject (monster, locationToSpawn, Quaternion.identity, monster);
					}
					// IF we have a place to store the spawned monsters for organization purposes.
					if (spawnHolder != null) {
						// Set the parent transform for organization purposes.
						Grid_Helper.helper.SetParentTransform (spawnHolder.transform, monsterSpawned);
					}
					// Add to the list.
					mobSpawned.Add (monsterSpawned);
				}
			}
		}

		private Vector3 PointInCollider () {
			// Get the variables that will allow us to figure our the boundaries.
			Bounds bounds = coll.bounds;
			Vector3 center = bounds.center;

			float x = 0;
			float y = 0;
			// DO a random point WHILE we are not finding a point that overlaps with our collider.
			do {
				x = UnityEngine.Random.Range (center.x - bounds.extents.x, center.x + bounds.extents.x);
				y = UnityEngine.Random.Range (center.y - bounds.extents.y, center.y + bounds.extents.y);
			} while (!coll.OverlapPoint (new Vector2 (x, y)));

			return new Vector3 (x, y, 0f);
		}

		private void CleanListForNulls() {
			// IF there are monsters that have been destroyed.
			if (mobSpawned.Contains (null)) {
				// Loop the amount of times we have mobs spawned before.
				for (int i = mobSpawned.Count - 1; i > -1; i--) {
					// IF this spot is null in the list.
					if (mobSpawned [i] == null) {
						// Remove this spot in the list.
						mobSpawned.RemoveAt (i);
					}
				}
			}
		}

		public void SpawnSetup() {
			// Clean the list.
			CleanListForNulls ();
			// IF we don't have spawn restrictions,
			// ELSE we have spawn restrictions.
			if (!rangeRespawn) {
				// Spawn the monsters.
				Spawn (spawner.monster, spawner.maxMonsterAmount);
			} else {
				// Pick a random amount from our alpha and omega range.
				int respawnNumber = UnityEngine.Random.Range (alphaRange, omegaRange);
				// Spawn the monsters.
				Spawn (spawner.monster, respawnNumber);
			}
		}

		public void SetMonstersActiveness(bool activeness){
			// Clean the list of any nulls.
			CleanListForNulls ();
			// Loop through the amount of remaining monsters.
			for (int i = 0; i < mobSpawned.Count; i++) {
				// Set the activeness of the gameobjects.
				mobSpawned [i].SetActive (activeness);
			}
		}

		public void DestroyAllMonsters(){
			// Loop through the amount of remaining monsters.
			for (int i = 0; i < mobSpawned.Count; i++) {
				// Set the activeness of the gameobjects.
				Destroy (mobSpawned [i]);
			}
			// Clear the list.
			mobSpawned.Clear ();
		}
	}

	[System.Serializable]
	public class Spawn_System 
	{
		public GameObject monster;
		public int maxMonsterAmount;
//		public float timer;
	}
}
