using UnityEngine;
using System.Collections;

namespace TrollBridge {

	[RequireComponent (typeof (Collider2D))]
	/// <summary>
	/// Scene change when we enter the collision of this GameObject.
	/// </summary>
	public class Scene_Change : MonoBehaviour {

		// The tags that trigger the scene change.
		[Tooltip("The tags that trigger the scene change.")]
		[SerializeField] private string[] targetTags;
		// The scene that will be loaded.
		[Tooltip("The name of the scene that will be loaded.")]
		[SerializeField] private string newScene;
		// The location in the new scene that the player will be spawned at.  This number is correlated to the
		// Transform locations on your Camera.
		[Tooltip("The location in the new scene that the player will be spawned at.  This number is correlated to the " +
		         "Transform locations on your Scene Manager Script.")]
		[SerializeField] private int sceneSpawnLocation = 0;


		void OnTriggerEnter2D(Collider2D coll){
			// Loop through the array.
			for (int i = 0; i < targetTags.Length; i++) {
				// IF this Tag is touched by the Tag(s) you consided in your targetTags.
				if (coll.gameObject.tag == targetTags [i]) {
					// Load the next scene.
					Grid_Helper.helper.ChangeScene (newScene, sceneSpawnLocation);
					// We done.
					break;
				}
			}
		}
	}
}
