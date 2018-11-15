using UnityEngine;
using System.Collections;

namespace TrollBridge {

	[RequireComponent(typeof(Light))]
	public class Cycle_LightIntensity : MonoBehaviour {

		// Our Light Component.
		[SerializeField] private Light lightComponent;

		// Set our range for our floats.
		[Range(0f, 1f)]
		[SerializeField] private float twelveAMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float oneAMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float twoAMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float threeAMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float fourAMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float fiveAMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float sixAMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float sevenAMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float eightAMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float nineAMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float tenAMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float elevenAMIntensity;
		// Set our range for our floats.
		[Range(0f, 1f)]
		[SerializeField] private float twelvePMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float onePMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float twoPMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float threePMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float fourPMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float fivePMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float sixPMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float sevenPMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float eightPMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float ninePMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float tenPMIntensity;
		[Range(0f, 1f)]
		[SerializeField] private float elevenPMIntensity;

		// The tag of our GameObject with our Game Timer on it.
		[SerializeField] private string gtTag;

		// Options for changing the light intensity.
		[SerializeField] private bool instantChange;
		[SerializeField] private bool lerpThroughHour;
		[SerializeField] private bool lerpAtHour;
		[SerializeField] private float lerpAtHourSpeed;

		// Our bool to let us know when to begin.
		private bool startCycle = false;
		// Our bool to let us know if we are currently lerping.
		private bool isLerping = false;

		// Array for our Light Intensity.
		private float[] lightIntensityAM = new float[12];
		private float[] lightIntensityPM = new float[12];

		// Our current hour.
		private int nextHour = -1;

		// Our timer script, or if you have your own timer script then replace this here.
		private Game_Timer gt;


		void OnValidate(){
			// Refresh our light intensity when we alter anything in this script.
			RefreshIntensity();
		}

		void Awake(){
			// Grab the Game Timer.
			gt = GameObject.FindGameObjectWithTag (gtTag).GetComponent<Game_Timer> ();
			// Start our cycle.
			BeginCycle ();
		}

		void Start() {
			// IF we have not begun our hour tracking yet.
			if (nextHour == -1) {
				nextHour = gt.GetNextHour ();
			}
			// IF its an instant change or we are lerping at the hour, both start off the scene the same,
			// ELSE IF we lerp throughout the entire hour so we start our lerping at the get go with setting our lighting based on how much time is left in the hour.
			if (instantChange || lerpAtHour) {
				// IF we are in AM,
				// ELSE we arein PM.
				if (gt.GetMeridien ()) {
					// Set the AM lighting.
					lightComponent.intensity = lightIntensityAM [gt.GetHour () - 1];
				} else {
					// Set the PM lighting.
					lightComponent.intensity = lightIntensityPM [gt.GetHour () - 1];
				}
			} else if (lerpThroughHour) {
				// IF we are in AM,
				// ELSE we arein PM.
				if (gt.GetMeridien ()) {
					// IF our meridien changes next hour,
					// ELSE our meridien doesn't change next hour.
					if (gt.IsMeridienChangingNextHour ()) {
						// Lets lerp our lighting.
						StartCoroutine (LerpLight (lightIntensityAM[gt.GetHour() - 1], lightIntensityPM [GetNextHourIntensityIndex (gt.GetHour ())], (float)gt.GetTotalSeconds (), GetRatioToNextHour (gt.GetMinute (), gt.GetSecond ())));
					} else {
						// Lets lerp our lighting.
						StartCoroutine (LerpLight (lightIntensityPM[gt.GetHour() - 1], lightIntensityAM [GetNextHourIntensityIndex (gt.GetHour ())], (float)gt.GetTotalSeconds (), GetRatioToNextHour (gt.GetMinute (), gt.GetSecond ())));
					}
				} else {
					// IF our meridien changes next hour,
					// ELSE our meridien doesn't change next hour.
					if (gt.IsMeridienChangingNextHour ()) {
						// Lets lerp our lighting.
						StartCoroutine (LerpLight (lightIntensityPM[gt.GetHour() - 1], lightIntensityAM [GetNextHourIntensityIndex (gt.GetHour ())], (float)gt.GetTotalSeconds (), GetRatioToNextHour (gt.GetMinute (), gt.GetSecond ())));
					} else {
						// Lets lerp our lighting.
						StartCoroutine (LerpLight (lightIntensityAM[gt.GetHour() - 1], lightIntensityPM [GetNextHourIntensityIndex (gt.GetHour ())], (float)gt.GetTotalSeconds (), GetRatioToNextHour (gt.GetMinute (), gt.GetSecond ())));
					}
				}
			}
		}
		
		void Update () {
			// IF we are ready to start our cycle.
			if (startCycle) {
				// IF we have instant change lightning,
				// ELSE IF we have lerp through the hour lighting AND we are currently not lerping,
				// ELSE IF we have lerp at the hour for lighthing
				if (instantChange) {
					// IF our current hour matches the next hour letting us know we have reached the next hour.
					if (gt.GetHour () == nextHour) {
						// IF we are in AM,
						// ELSE we arein PM.
						if (gt.GetMeridien ()) {
							// Set the AM lighting.
							lightComponent.intensity = lightIntensityAM [gt.GetHour () - 1];
						} else {
							// Set the PM lighting.
							lightComponent.intensity = lightIntensityPM [gt.GetHour () - 1];
						}
						// Since we have found a match lets update our next hour.
						nextHour = gt.GetNextHour ();
					}
				} else if (lerpThroughHour && !isLerping) {
					// IF we are in AM,
					// ELSE we arein PM.
					if (gt.GetMeridien ()) {
						// IF our meridien changes next hour,
						// ELSE our meridien doesn't change next hour.
						if (gt.IsMeridienChangingNextHour ()) {
							// Lets lerp our lighting.
							StartCoroutine (LerpLight (lightIntensityPM[gt.GetHour() - 1], lightIntensityPM [GetNextHourIntensityIndex (gt.GetHour ())], (float)gt.GetTotalSeconds (), GetRatioToNextHour (gt.GetMinute (), gt.GetSecond ())));
						} else {
							// Lets lerp our lighting.
							StartCoroutine (LerpLight (lightIntensityAM[gt.GetHour() - 1], lightIntensityAM [GetNextHourIntensityIndex (gt.GetHour ())], (float)gt.GetTotalSeconds (), GetRatioToNextHour (gt.GetMinute (), gt.GetSecond ())));
						}
					} else {
						// IF our meridien changes next hour,
						// ELSE our meridien doesn't change next hour.
						if (gt.IsMeridienChangingNextHour ()) {
							// Lets lerp our lighting.
							StartCoroutine (LerpLight (lightIntensityAM[gt.GetHour() - 1], lightIntensityAM [GetNextHourIntensityIndex (gt.GetHour ())], (float)gt.GetTotalSeconds (), GetRatioToNextHour (gt.GetMinute (), gt.GetSecond ())));
						} else {
							// Lets lerp our lighting.
							StartCoroutine (LerpLight (lightIntensityPM[gt.GetHour() - 1], lightIntensityPM [GetNextHourIntensityIndex (gt.GetHour ())], (float)gt.GetTotalSeconds (), GetRatioToNextHour (gt.GetMinute (), gt.GetSecond ())));
						}
					}
				} else if (lerpAtHour) {
					// IF our current hour matches the next hour letting us know we have reached the next hour.
					if(gt.GetHour() == nextHour) {
						// IF we are in AM,
						// ELSE we arein PM.
						if (gt.GetMeridien ()) {
							// IF our meridien changes next hour,
							// ELSE our meridien doesn't change next hour.
							if (gt.IsMeridienChangingNextHour ()) {
								// Lets lerp our lighting.
								StartCoroutine (LerpLight (lightComponent.intensity, lightIntensityPM [gt.GetHour() - 1], lerpAtHourSpeed, 0f));
							} else {
								// Lets lerp our lighting.
								StartCoroutine (LerpLight (lightComponent.intensity, lightIntensityAM [gt.GetHour() - 1], lerpAtHourSpeed, 0f));
							}
						} else {
							// IF our meridien changes next hour,
							// ELSE our meridien doesn't change next hour.
							if (gt.IsMeridienChangingNextHour ()) {
								// Lets lerp our lighting.
								StartCoroutine (LerpLight (lightComponent.intensity, lightIntensityAM [gt.GetHour() - 1], lerpAtHourSpeed, 0f));
							} else {
								// Lets lerp our lighting.
								StartCoroutine (LerpLight (lightComponent.intensity, lightIntensityPM [gt.GetHour() - 1], lerpAtHourSpeed, 0f));
							}
						}
						// Since we have found a match lets update our next hour.
						nextHour = gt.GetNextHour ();
					}
				}
			}
		}

		private IEnumerator LerpLight(float startIntensity, float endIntensity, float totalSeconds, float startRatio) {
			// We are currently lerping.
			isLerping = true;
			// Loop in a SmoothStep manner to get a smooth fade.
			for (float x = startRatio; x < 1.0f; x += Time.deltaTime / totalSeconds) {
				// Change the light intensity.
				lightComponent.intensity =  Mathf.SmoothStep(startIntensity, endIntensity, x);
				// Need this or all hell/darkness breaks loose and Unity crashes, just like Supernatural Season 11.
				yield return null;
			}
			// Make sure we get to the endIntensity incase the loop didnt make it to 100%.
			lightComponent.intensity = endIntensity;
			// We are no longer lerping.
			isLerping = false;
		}

		private void RefreshIntensity(){
			// Store our AM time light intensity.
			lightIntensityAM[0] = oneAMIntensity;
			lightIntensityAM[1] = twoAMIntensity;
			lightIntensityAM[2] = threeAMIntensity;
			lightIntensityAM[3] = fourAMIntensity;
			lightIntensityAM[4] = fiveAMIntensity;
			lightIntensityAM[5] = sixAMIntensity;
			lightIntensityAM[6] = sevenAMIntensity;
			lightIntensityAM[7] = eightAMIntensity;
			lightIntensityAM[8] = nineAMIntensity;
			lightIntensityAM[9] = tenAMIntensity;
			lightIntensityAM[10] = elevenAMIntensity;
			lightIntensityAM[11] = twelveAMIntensity;
			// Store our PM time light intensity.
			lightIntensityPM[0] = onePMIntensity;
			lightIntensityPM[1] = twoPMIntensity;
			lightIntensityPM[2] = threePMIntensity;
			lightIntensityPM[3] = fourPMIntensity;
			lightIntensityPM[4] = fivePMIntensity;
			lightIntensityPM[5] = sixPMIntensity;
			lightIntensityPM[6] = sevenPMIntensity;
			lightIntensityPM[7] = eightPMIntensity;
			lightIntensityPM[8] = ninePMIntensity;
			lightIntensityPM[9] = tenPMIntensity;
			lightIntensityPM[10] = elevenPMIntensity;
			lightIntensityPM[11] = twelvePMIntensity;
		}

		/// <summary>
		/// Gets the next hour intensity index.
		/// </summary>
		private int GetNextHourIntensityIndex(int currentHour){
			// IF our current hour is 12,
			// ELSE our current hour is 1-11.
			if (currentHour == 12) {
				// We need to head to the start of the array for our light intensity.
				return 0;
			} else {
				// Get our spot in the array for our light intensity.
				return currentHour;
			}
		}

		/// <summary>
		/// Gets the ratio to next hour.
		/// </summary>
		private float GetRatioToNextHour(int minute, int seconds){
			return ((minute * 60) + seconds) / 3600f;
		}

		/// <summary>
		/// Begins the cycle.
		/// </summary>
		public void BeginCycle(){
			startCycle = true;
		}

		/// <summary>
		/// Stops the cycle.
		/// </summary>
		public void StopCycle(){
			startCycle = false;
		}
	}
}
