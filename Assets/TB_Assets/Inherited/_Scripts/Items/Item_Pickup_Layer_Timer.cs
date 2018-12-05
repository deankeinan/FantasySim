using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Item pickup timer makes an item not able to be picked up after a certain time allowing us to see the item that was just created so we as a player know we obtained something.
	/// </summary>
	public class Item_Pickup_Layer_Timer : MonoBehaviour {

		[Tooltip("This is the wait time (in seconds) for when this GameObject can be picked up.")]
		public float waitTime = 0.5f;
		// The Layer for when we want this gameobject to not be interactable.
		public LayerMask noCollideLayer;
		// The Layer for when we want this gameobject to be interactable.
		public LayerMask collideLayer;


		void Awake () {
			// When this item is created we are going to put it on a layer that can't interact with the player but not go through walls.  So we have a layer just for this.
			gameObject.layer = (int)Mathf.Log (noCollideLayer.value, 2);
			// Since this just spawned we make it non pickupable for a short time so the player can tell wtf this item actually is lol before grabbing it.
			StartCoroutine (TimeTillPickUp());
		}

		/// <summary>
		/// Start a timer for how long it takes for this item to be picked up.
		/// </summary>
		/// <returns>The till pick up.</returns>
		private IEnumerator TimeTillPickUp() {	
			// Wait a certain amount of time
			yield return new WaitForSeconds(waitTime);
			// Change to the final layer to be interacted with.
			gameObject.layer = (int)Mathf.Log (collideLayer.value, 2);
		}
	}
}
