using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// This script will wait a time between a min/max timer and makes the flashGameObject inactive for a min/max timer and reactivates it.
	/// </summary>
	public class Timer_GameObject_Activeness : MonoBehaviour {

		// The gameobject you want to flash.
		[SerializeField] private GameObject flashGameObject;
		// The time range in which you want this gameobject to be inactive and reactive.
		[SerializeField] private float minWaitTimer = 2f;
		[SerializeField] private float maxWaitTimer = 5f;
		// The amount of time you want this GameObject to be inactive.
		[SerializeField] private float minInactiveTimer = 0.2f;
		[SerializeField] private float maxInactiveTiemr = 0.2f;
		// Our current timer for countdown.
		private float currentTimer;
		// Boolean to represent if our gameobject is active.
		private bool isFlashActive = true;

		void Start(){
			// Reset the currentTimer.
			currentTimer = Random.Range (minWaitTimer, maxWaitTimer);
		}

		void Update(){
			// The flashGameObject is active.
			if (isFlashActive) {
				// Reduce the time.
				currentTimer -= Time.deltaTime;
				// IF our timer has reached 0.
				if (currentTimer <= 0f) {
					// Lets turn the flash GameObject inactive.
					flashGameObject.SetActive (false);
					// Set isFlashActive off now.
					isFlashActive = false;
					// Wait the time before we leave the flashGameObject inactive.
					StartCoroutine (TurnOff ());
				}
			}
		}

		private IEnumerator TurnOff(){
			// Lets wait for a certain amount of time based on our min and max inactive timers.
			yield return new WaitForSeconds (Random.Range (minInactiveTimer, maxInactiveTiemr));
			// Lets turn the flash Gameobject active.
			flashGameObject.SetActive (true);
			// Set isFlashACtive on now.
			isFlashActive = true;
			// Reset the counter.
			currentTimer = Random.Range (minWaitTimer, maxWaitTimer);
		}
	}
}