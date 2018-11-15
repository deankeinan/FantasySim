using UnityEngine;
using System.Collections;

namespace TrollBridge {

	[RequireComponent(typeof(Collider2D))]
	/// <summary>
	/// Bomb explode script or rather an explode script will represent our area of explosion where we will set a gameobject 'Inactive" if we find out that, that gameobject has the 'Explodable' script to it letting us know to remove it.
	/// </summary>
	public class Bomb_Explode : MonoBehaviour {

		// The built in timer that destroys this gameobject when it explodes.
		private float timer = 0.1f;

		void Start () {
			Destroy (gameObject, timer);
		}

		void OnTriggerEnter2D(Collider2D coll){
			// IF the explosion hits anything that is destroyable by the explosion.
			if (coll.GetComponent<Explodable> () != null) {
				// Deactivate the GameObject.
				Grid_Helper.helper.SetActiveGameObject (coll.gameObject, false);
			}
		}
	}
}
