using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	[RequireComponent (typeof (Rigidbody2D))]
	[RequireComponent (typeof (SpriteRenderer))]
	public class Weather_SpriteAttributes : MonoBehaviour {

		// Created sprite attributes.
		[Header ("Lifespan")]
		[SerializeField] private float spriteLifeSpanMin = 10f;
		[SerializeField] private float spriteLifeSpanMax = 10f;

		[Header ("Speed")]
		[SerializeField] private Vector2 spriteVelocityMin;
		[SerializeField] private Vector2 spriteVelocityMax;

		[Header ("Rotation")]
		[SerializeField] private Vector3 spriteRotateMinAngle;
		[SerializeField] private Vector3 spriteRotateMaxAngle;
		[SerializeField] private Vector3 spriteRotateSpeedMin;
		[SerializeField] private Vector3 spriteRotateSpeedMax;

		[Header ("Alpha")]
		[SerializeField] private bool spriteAlphaShrink = false;
		[SerializeField] private float spriteAlphaShrinkAmount = 0f;
		[SerializeField] private float spriteAlphaMin = 0.8f;
		[SerializeField] private float spriteAlphaMax = 1f;

		[Header ("Scale")]
		[SerializeField] private bool spriteScaleShrink = false;
		[SerializeField] private float spriteScaleShrinkAmount = 0f;
		[SerializeField] private float spriteScaleMin = 0.8f;
		[SerializeField] private float spriteScaleMax = 1f;

		private bool moveDown = true;
		private SpriteRenderer spriteRenderer;

		void Awake () {
			spriteRenderer = GetComponent <SpriteRenderer> ();
		}

		// Use this for initialization
		void OnEnable () {
			// Set our lifespan.
			StartCoroutine (SetLifeSpan (Random.Range (spriteLifeSpanMin, spriteLifeSpanMax)));

			// IF we are moving down,
			// ELSE we are moving up.
			if (moveDown) {
				// Set our velocity.
				GetComponent <Rigidbody2D> ().velocity = new Vector2 (Random.Range (spriteVelocityMin.x, spriteVelocityMax.x), Random.Range (spriteVelocityMin.y, spriteVelocityMax.y));
			} else {
				// Set our velocity.
				GetComponent <Rigidbody2D> ().velocity = new Vector2 (Random.Range (spriteVelocityMin.x, spriteVelocityMax.x), Random.Range (-spriteVelocityMin.y, -spriteVelocityMax.y));

			}
			// Set our rotation speed.
			transform.eulerAngles = new Vector3 (Random.Range (spriteRotateMinAngle.x, spriteRotateMaxAngle.x), Random.Range (spriteRotateMinAngle.y, spriteRotateMaxAngle.y), Random.Range (spriteRotateMinAngle.z, spriteRotateMaxAngle.z));
			// Set the alpha.
			spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Random.Range (spriteAlphaMin, spriteAlphaMax));
			// Set the scale.
			float newScale = Random.Range (spriteScaleMin, spriteScaleMax);
			transform.localScale = new Vector2 (newScale, newScale);
		}

		void Update () {
			// IF we want to keep shrinking the alpha.
			if (spriteAlphaShrink) {
				// Reduce the alpha.
				spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - spriteAlphaShrinkAmount);
				// IF our alpha is 0 meaning we cannot see it.
				if (spriteRenderer.color.a <= 0) {
					// We cannot be seen so set it inactive.
					gameObject.SetActive (false);
				}
			}
			// IF we want to keep shrinking the scale.
			if (spriteScaleShrink) {
				// Reduce the scale.
				transform.localScale = new Vector2 (transform.localScale.x - spriteScaleShrinkAmount, transform.localScale.y - spriteScaleShrinkAmount);
				// IF we have no scale in our X OR Y.
				if (transform.localScale.x <= 0 || transform.localScale.y <= 0) {
					// We cannot be seen so set it inactive.
					gameObject.SetActive (false);
				}
			}
			// Set our rotation speed.
			transform.eulerAngles += new Vector3 (Random.Range (spriteRotateSpeedMin.x, spriteRotateSpeedMax.x), Random.Range (spriteRotateSpeedMin.y, spriteRotateSpeedMax.y), Random.Range (spriteRotateSpeedMin.z, spriteRotateSpeedMax.z));
		}

		private IEnumerator SetLifeSpan (float lifeSpan) {
			// Wait the lifespan time before we set this inactive.
			yield return new WaitForSeconds(lifeSpan);
			// Set this gameobject inactive.
			gameObject.SetActive (false);
		}

		public void MovingDown () {
			moveDown = true;
		}

		public void MovingUp () {
			moveDown = false;
		}
	}
}
