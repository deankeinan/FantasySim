using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Item pickup timer makes an item not able to be picked up after a certain time allowing us to see the item that was just created so we as a player know we obtained something.
	/// </summary>
	public class Item_Pickup_Timer : MonoBehaviour {

		[Tooltip("This is the wait time (in seconds) for when this GameObject can be picked up.")]
		public float waitTime = 0.5f;
		// The Item GameObject script.
		private Item_GameObject ig;


		void Awake () {
			// Grab the Item_GameObject script that is on this GameObject
			ig = GetComponent<Item_GameObject> ();
			// Set the GameObject to not be able to be picked up.
			ig.canPickUp = false;
			// Since this just spawned we make it non pickupable for a short time so the player can tell wtf this item actually is lol before grabbing it.
			StartCoroutine (TimeTillPickUp());
		}

		/// <summary>
		/// Start a timer for how long it takes for this item to be picked up.
		/// </summary>
		/// <returns>The till pick up.</returns>
		private IEnumerator TimeTillPickUp()
		{	
			// Wait a certain amount of time
			yield return new WaitForSeconds(waitTime);
			ig.canPickUp = true;
		}
	}
}
