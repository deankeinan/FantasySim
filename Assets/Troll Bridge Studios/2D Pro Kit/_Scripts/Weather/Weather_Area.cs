using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	[RequireComponent (typeof (Collider2D))]
	public class Weather_Area : MonoBehaviour {

		[Header ("Prefab")]
		[SerializeField] private GameObject objectToPool;

		[Header ("Amount")]
		[SerializeField] private int amountToPool;
		[ReadOnlyAttribute] [SerializeField] private int currentlyUsed;
		// When entering and leaving this Weather_Area the time it takes to blend in or out the weather.
		[SerializeField] private float blendTime = 3f;
		// Parent GameObject for our created sprites.
		[SerializeField] private Transform weatherSpritesParent;

		[Header ("Emission Boundaries")]
		// Tags to find the Min and Max x on associated with our camera.
		[SerializeField] private string topWeatherEmissionMinTag = "Top Weather Emission Min";
		[SerializeField] private string topWeatherEmissionMaxTag = "Top Weather Emission Max";
		[SerializeField] private bool useBottomEmission = false;
		[SerializeField] private string bottomWeatherEmissionMinTag = "Bottom Weather Emission Min";
		[SerializeField] private string bottomWeatherEmissionMaxTag = "Bottom Weather Emission Max";

		private List<GameObject> pooledObjects;
		private float currentTimeBlend = 0.0f;
		private bool turnOn = false;

		private Transform topEmissionMinTransform;
		private Transform topEmissionMaxTransform;
		private Transform bottomEmissionMinTransform;
		private Transform bottomEmissionMaxTransform;


		void Start () {
			// Create our pool size.
			pooledObjects = ObjectPooler.SharedInstance.CreatePool (amountToPool, objectToPool, weatherSpritesParent.transform, false);

			// Lets get our min and max emission locations.
			topEmissionMinTransform = GameObject.FindGameObjectWithTag (topWeatherEmissionMinTag).transform;
			topEmissionMaxTransform = GameObject.FindGameObjectWithTag (topWeatherEmissionMaxTag).transform;
			bottomEmissionMinTransform = GameObject.FindGameObjectWithTag (bottomWeatherEmissionMinTag).transform;
			bottomEmissionMaxTransform = GameObject.FindGameObjectWithTag (bottomWeatherEmissionMaxTag).transform;
		}

		void OnTriggerEnter2D (Collider2D coll) {
			// IF we do NOT have the player.
			if (!Grid_Helper.helper.IsPlayer (coll.GetComponentInParent <Character_Manager> ())) {
				// We leave.
				return;
			}

			// Turn the weather on.
			turnOn = true;
			// Stop all Coroutines.
			StopAllCoroutines ();
			// Slowly increase to our amount.
			StartCoroutine (IncreasePool ());
		}

		void OnTriggerExit2D (Collider2D coll) {
			// IF we do NOT have the player.
			if (!Grid_Helper.helper.IsPlayer (coll.GetComponentInParent <Character_Manager> ())) {
				// We leave.
				return;
			}

			// Stop all Coroutines.
			StopAllCoroutines ();
			// Slowly reduce the emission.
			StartCoroutine (DecreasePool ());
		}

		void Update () {
			// IF we are turned on.
			if (turnOn) {
				// Lets show some weather.
				EmitWeather ();
			}
		}

		private IEnumerator IncreasePool () {
			// Loop the time amount of our blendTime.
			for (float x = currentTimeBlend; x < 1.0f; x += Time.deltaTime / blendTime) {
				// Save our time incase we leave in the middle of a blend.
				currentTimeBlend = x;
				// The amount we want to be active.
				currentlyUsed = Mathf.RoundToInt (Mathf.Lerp (0, amountToPool, x));
				// Next frame.
				yield return null;
			}
			// Set everything to the end.
			currentlyUsed = amountToPool;
			// Reset the blend time.
			currentTimeBlend = 1.0f;
		}

		private IEnumerator DecreasePool () {
			// Loop the time amount of our blendTime.
			for (float x = currentTimeBlend; x > 0.0f; x -= Time.deltaTime / blendTime) {
				// Save our time incase we leave in the middle of a blend.
				currentTimeBlend = x;
				// The amount we want to be active.
				currentlyUsed = Mathf.RoundToInt (Mathf.Lerp (0, amountToPool, x));
				// Next frame.
				yield return null;
			}
			// Set everything to the start.
			currentlyUsed = 0;
			// Reset the blend time.
			currentTimeBlend = 0.0f;
			// Turn the weather on.
			turnOn = false;
		}

		private void EmitWeather () {
			// Loop the amount of times we want to have active weather gameobjects.
			for (int i = 0; i < currentlyUsed; i++) {
				// IF our gameobject is not active.
				if (!pooledObjects [i].activeInHierarchy) {
					// IF the camera is moving down/south AND we want to use the bottom emission,
					// ELSE we want to use the top emission.
					if (Camera.main.velocity.y < 0f && useBottomEmission) {
						// Set the direction this needs to move.
						pooledObjects [i].GetComponent <Weather_SpriteAttributes> ().MovingUp ();
						// Set the position of the gameobject.
						pooledObjects [i].transform.position = new Vector2 (Random.Range (bottomEmissionMinTransform.position.x, bottomEmissionMaxTransform.position.x), bottomEmissionMaxTransform.position.y);
					} else {
						// Set the direction this needs to move.
						pooledObjects [i].GetComponent <Weather_SpriteAttributes> ().MovingDown ();
						// Set the position of the gameobject.
						pooledObjects [i].transform.position = new Vector2 (Random.Range (topEmissionMinTransform.position.x, topEmissionMaxTransform.position.x), topEmissionMaxTransform.position.y + Random.Range (0f, 0.75f));
					}
					// Set this gameobject active.
					pooledObjects [i].SetActive (true);
				}
			}
		}
	}
}
	