using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Changes the intensity of a light component based on time when we are inside the collider.
	/// </summary>
	public class Timer_LightIntensity_Collision : MonoBehaviour {

		// Our Lighting system.
		[SerializeField] private Light gameLight;
		[SerializeField] private float originalIntensity;
		[SerializeField] private float flashIntensity;
		[SerializeField] private float minWaitTime = 2f;
		[SerializeField] private float maxWaitTime = 5f;
		[SerializeField] private float minFlashTime = 0.1f;
		[SerializeField] private float maxFlashTime = 0.1f;

		// Used for our min/max flash time.
		private float flashTime;
		// Boolean to let us know which phase we are in.
		private bool isOriginalIntensity = true;

		void Start () {
			// Lets get a random time so we know when to change the intensity.
			flashTime = Random.Range (minWaitTime, maxWaitTime);
		}

		void OnTriggerStay2D (Collider2D coll) {
			// Change our light intensity
			ChangeIntensity ();
		}

		void OnTriggerExit2D () {
			gameLight.intensity = originalIntensity;
		}
		
		private void ChangeIntensity () {
			// Reduce the time.
			flashTime -= Time.deltaTime;
			// IF we are at our original intensity,
			// ELSE we are at our flash intensity.
			if (isOriginalIntensity) {
				// IF our countdown reached 0.
				if (flashTime <= 0f) {
					// Change the intensity.
					gameLight.intensity = flashIntensity;
					// Set our bool to not be on our original intensity.
					isOriginalIntensity = false;
					// Assign our flashTime so that we know when to change from flashIntensity to originalIntensity.
					flashTime = Random.Range (minFlashTime, maxFlashTime);
				}
			} else {
				// IF our countdown reached 0.
				if (flashTime <= 0f) {
					// Change the intensity.
					gameLight.intensity = originalIntensity;
					// Set our bool to be on our original intensity.
					isOriginalIntensity = true;
					// Assign our flashTime so that we know when to change from originalIntensity to flashIntensity.
					flashTime = Random.Range (minWaitTime, maxWaitTime);
				}
			}
		}

		/// <summary>
		/// Gets the is original intensity.
		/// </summary>
		public bool GetIsOriginalIntensity () {
			return isOriginalIntensity;
		}
	}
}
