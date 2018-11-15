using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	[RequireComponent (typeof (Animator))]
	public class Animation_SpeedTime : MonoBehaviour {

		[Header ("Time Intervals")]
		[Tooltip ("The shortest time interval for whatever our speed is.")]
		[SerializeField] private float timeIntervalMin;
		[Tooltip ("The longest time interval for whatever our speed is.")]
		[SerializeField] private float timeIntervalMax;

		[Header ("Speed Intervals")]
		[Tooltip ("The minimum speed range for our animation speed.")]
		[SerializeField] private float speedMin;
		[Tooltip ("The maximum speed range for our animation speed.")]
		[SerializeField] private float speedMax;

		[Header ("Wait Time Between Intervals")]
		[Tooltip ("The time before the next random number to be picked for the animation speed.")]
		[SerializeField] private float waitTime;

		[Header ("Animator Component")]
		[SerializeField] private Animator objectAnimator;


		void Start () {
			// Start the Coroutine.
			StartCoroutine (StartSpeedTimeChange ());
		}

		private IEnumerator StartSpeedTimeChange () {
			// Never stop.
			while (true) {
				// Get a random time from our range.
				float randomTimeInterval = Random.Range (timeIntervalMin, timeIntervalMax);
				// Get a random speed from our range.
				float randomSpeedInterval = Random.Range (speedMin, speedMax);
				// Get our speed the Animator.
				float startSpeed = objectAnimator.speed;

				// Loop the time of our randomTimeInterval.
				for (float x = 0.0f; x < 1.0f; x += Time.deltaTime / randomTimeInterval) {
					// Lerp the speed to our randomSpeedInterval.
					objectAnimator.speed = Mathf.Lerp (startSpeed, randomSpeedInterval, x);
					// Go to next frame.
					yield return null;
				}
				// Set the speed to its final destination.
				objectAnimator.speed = randomSpeedInterval;

				// Lets take a break here and let the current speed run its course for 'waitTime' seconds.
				yield return new WaitForSeconds(waitTime);

				// Go to next frame.
				yield return null;
			}
		}
	}
}
