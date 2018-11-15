using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Object Pooler, this will pool objects so that we can manage our memory better.
	/// </summary>
	public class ObjectPooler : MonoBehaviour {

		public static ObjectPooler SharedInstance;


		void Awake() {
			SharedInstance = this;
		}

		/// <summary>
		/// Sets up the pool.
		/// </summary>
		public GameObject SetUpForPool (GameObject objectToPool, Transform parent, bool activeness) {
			// Create a pooled gameobject.
			GameObject obj = (GameObject)Instantiate (objectToPool, parent);
			// Set it inactive.
			obj.SetActive (activeness);
			// Return this GameObject.
			return obj;
		}


		/// <summary>
		/// Creates the pool.
		/// </summary>
		public List<GameObject> CreatePool (int amountToPool, GameObject objectToPool, Transform parent, bool activeness) {
			// Create a List for our Pooled GameObjects.
			List<GameObject> pooledObjects = new List<GameObject>();
			// Loop the amount based on the size of our currentAmountToPool.
			for (int i = 0; i < amountToPool; i++) {
				// Add to our pooled objects.
				pooledObjects.Add (SetUpForPool (objectToPool, parent, activeness));
			}
			// Return the List of pooled GameObjects.
			return pooledObjects;
		}


		/// <summary>
		/// This method will loop through a list of gameobjects and return the first one that is inactive.
		/// otherwise it will return null.
		/// </summary>
		public GameObject GetPooledObject (List<GameObject> pooledObjects, int pooledAmount) {
			// Loop the amount of pooled objects we have.
			for (int i = 0; i < pooledAmount; i++) {
				// IF this pooled object is NOT active in our hierarchy.
				if (!pooledObjects [i].activeInHierarchy) {
					// Return the gameobject.
					return pooledObjects [i];
				}
			}
			// Pool too small so return null.
			return null;
		}
	}
}
