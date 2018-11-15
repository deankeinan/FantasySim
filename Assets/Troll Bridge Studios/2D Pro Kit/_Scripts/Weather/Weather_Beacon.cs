using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	public class Weather_Beacon : MonoBehaviour {

		[Header ("Prefab")]
		[SerializeField] private GameObject weatherObject;

		[Header ("Distance")]
		[SerializeField] private float distance;

		[Header ("Amounts")]
		[SerializeField] private int minAmount;
		[SerializeField] private int maxAmount;
		[SerializeField] private int minAmountWhileMoving;
		[SerializeField] private int maxAmountWhileMoving;
		[ReadOnlyAttribute] [SerializeField] private int pooledAmount;
		[ReadOnlyAttribute] [SerializeField] private int pooledAmountWhileMoving;
		// Parent GameObject for our created sprites.
		[SerializeField] private Transform weatherSpritesParent;
		[SerializeField] private Transform weatherSpritesMovingParent;

		[Header ("Emission Boundaries")]
		// Tags to find the Min and Max x on associated with our camera.
		[SerializeField] private string topWeatherEmissionMinTag = "Top Weather Emission Min";
		[SerializeField] private string topWeatherEmissionMaxTag = "Top Weather Emission Max";
		[SerializeField] private bool useBottomEmission = false;
		[SerializeField] private string bottomWeatherEmissionMinTag = "Bottom Weather Emission Min";
		[SerializeField] private string bottomWeatherEmissionMaxTag = "Bottom Weather Emission Max";

		private Character_Manager charaManager;
		private List<GameObject> pooledObjects;
		private List<GameObject> pooledObjectsWhileMoving;

		private Transform topEmissionMinTransform;
		private Transform topEmissionMaxTransform;
		private Transform bottomEmissionMinTransform;
		private Transform bottomEmissionMaxTransform;


		void Start () {
			// Get the player.
			charaManager = Character_Helper.GetPlayerManager ().GetComponent <Character_Manager> ();

			// Create our pool size.
			pooledObjects = ObjectPooler.SharedInstance.CreatePool (maxAmount, weatherObject, weatherSpritesParent.transform, false);
			pooledObjectsWhileMoving = ObjectPooler.SharedInstance.CreatePool (maxAmountWhileMoving, weatherObject, weatherSpritesMovingParent.transform, false);

			// Lets get our min and max emission locations.
			topEmissionMinTransform = GameObject.FindGameObjectWithTag (topWeatherEmissionMinTag).transform;
			topEmissionMaxTransform = GameObject.FindGameObjectWithTag (topWeatherEmissionMaxTag).transform;
			bottomEmissionMinTransform = GameObject.FindGameObjectWithTag (bottomWeatherEmissionMinTag).transform;
			bottomEmissionMaxTransform = GameObject.FindGameObjectWithTag (bottomWeatherEmissionMaxTag).transform;
		}

		void Update () {
			// IF we failed to get the Character Manager script.
			if (charaManager == null) {
				// Get the player.
				charaManager = Character_Helper.GetPlayerManager ().GetComponent <Character_Manager> ();
				// Try again.
				return;
			}

			// IF the distance between the player and the weather beacon is less than our distance AND we do not currently have weather being produced.
			if (Vector2.Distance (charaManager.characterEntity.transform.position, transform.position) <= distance) {
				// Create some weather!
				StartCoroutine (CreateWeather ());
				StartCoroutine (CreateWeatherWhileMoving ());
			} else {
				StopAllCoroutines ();
			}
		}

		private IEnumerator CreateWeather () {
			// This never stops until we leave Beacon area.
			while (true) {
				// Get the distance the player is from the center.
				float playerDist = Vector2.Distance (charaManager.characterEntity.transform.position, transform.position);
				// Get the ratio.
				float ratio = playerDist / distance;
				// Based on how far away we are from the center we gauge our emission amount.
				pooledAmount = Mathf.RoundToInt (Mathf.Lerp (maxAmount, minAmount, ratio));

				// Loop the amount of times we want to have active weather gameobjects.
				for (int i = 0; i < pooledAmount; i++) {
					// IF our gameobject is not active.
					if (!pooledObjects [i].activeInHierarchy) {
						// Set the position of the gameobject.
						pooledObjects [i].transform.position = new Vector2 (Random.Range (topEmissionMinTransform.position.x, topEmissionMaxTransform.position.x), topEmissionMaxTransform.position.y + Random.Range (0f, 0.75f));
						// Set this gameobject active.
						pooledObjects [i].SetActive (true);
					}
					// To the next frame.
					yield return null;
				}
				// To the next frame.
				yield return null;
			}
		}

		private IEnumerator CreateWeatherWhileMoving () {
			// This never stops until we leave Beacon area.
			while (true) {
				// Get the distance the player is from the center.
				float playerDist = Vector2.Distance (charaManager.characterEntity.transform.position, transform.position);
				// Get the ratio.
				float ratio = playerDist / distance;
				// IF the camera is moving up or down then we need to spawn more,
				if (Camera.main.velocity.y != 0f) {
					// We need to create more weather.
					pooledAmountWhileMoving = Mathf.RoundToInt (Mathf.Lerp (maxAmountWhileMoving, minAmountWhileMoving, ratio));
				} else {
					// No need for extra weather.
					pooledAmountWhileMoving = 0;
				}

				// Loop the amount of times we want to have active weather gameobjects.
				for (int i = 0; i < pooledAmountWhileMoving; i++) {
					// IF our gameobject is not active.
					if (!pooledObjectsWhileMoving [i].activeInHierarchy) {
						// IF we want to use the bottom emission and we are moving down,
						// ELSE we want to use the top emission.
						if (useBottomEmission && Camera.main.velocity.y < 0f) {
							// Set the position of the gameobject.
							pooledObjectsWhileMoving [i].transform.position = new Vector2 (Random.Range (bottomEmissionMinTransform.position.x, bottomEmissionMaxTransform.position.x), bottomEmissionMaxTransform.position.y);
						} else {
							// Set the position of the gameobject.
							pooledObjectsWhileMoving [i].transform.position = new Vector2 (Random.Range (topEmissionMinTransform.position.x, topEmissionMaxTransform.position.x), topEmissionMaxTransform.position.y + Random.Range (0f, 0.75f));
						}
						// Set this gameobject active.
						pooledObjectsWhileMoving [i].SetActive (true);
					}
					// To the next frame.
					yield return null;
				}
				// To the next frame.
				yield return null;
			}
		}

		void OnDrawGizmos() {
			// Draw a circular visual to display what area this encompasses.
			Gizmos.DrawWireSphere (transform.position, distance);
		}
	}
}
