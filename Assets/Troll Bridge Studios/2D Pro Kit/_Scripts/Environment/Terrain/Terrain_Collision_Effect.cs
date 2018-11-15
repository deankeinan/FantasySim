using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TrollBridge {

	[RequireComponent (typeof (Effector2D))]
	/// <summary>
	/// Terrain collision effect will create terrainEffects when we enter the collider and stopped when we exit the collider.
	/// </summary>
	public class Terrain_Collision_Effect: MonoBehaviour {

		// The animation to be played while colliding.
		[Tooltip("The effect to be played while colliding.")]
		public GameObject terrainEffect;

		
		void OnTriggerEnter2D(Collider2D coll){
			// Loop through the children of this gameobject.
			foreach(Transform child in coll.transform){
				// IF the child gameobject has a tag of 'feet'?
				if(child.gameObject.tag == "Feet"){
					// Spawn the animation at the feet.
					GameObject anim = Instantiate(terrainEffect, child.gameObject.transform.position, Quaternion.identity) as GameObject;
					// Set the animation as a child.
					Grid_Helper.helper.SetParentTransform(child.gameObject.transform, anim);
				}
			}
		}

		void OnTriggerExit2D(Collider2D coll){
			// Loop through the children of this gameobject.
			foreach(Transform child in coll.transform){
				// IF the child gameobject has a tag of 'feet'?
				if(child.gameObject.tag == "Feet"){
					// Destroy all child gameobjects.
					Grid_Helper.helper.DestroyGameObjectsByParent(child.gameObject);
				}
			}
		}
	}
}
