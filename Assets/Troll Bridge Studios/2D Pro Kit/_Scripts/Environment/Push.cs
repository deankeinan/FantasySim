using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// This script handles an edge of a GameObject in which you want to specifically push this GameObject in a vertical or horizontal manner.  Think of Legend of Zelda when Link would push a boulder, he would push for a second
	/// THEN the boulder would move in the direction that it is being pushed.
	/// </summary>
	[RequireComponent(typeof(EdgeCollider2D))]
	public class Push : MonoBehaviour {

		// Let us know which side this collider is for pushing an object.
		public bool topCollider;
		public bool bottomCollider;
		public bool leftCollider;
		public bool rightCollider;
		// The parent GameObject.
		public GameObject parent;
		// The tags that push the GameObject.
		public string[] tagsThatPushThis;
		// The delay time it takes to push this GameObject.
		public float timeToPush = 1f;
		// The speed at which this gets moved.
		public float moveSpeed = 1f;

		// The sound clip to play when Colliding.
		public AudioClip soundClip;
		// The min and max pitch for when this sound is played.
		public float minPitch = 1;
		public float maxPitch = 1;

		// The parent Transform.
		private Transform parentTransform;
		// The parent GameObject's Rigidbody.
		private Rigidbody2D parentRB;
		// The timer to let us know when to push this.
		private float timer;
		// The sound object that is being played.
		private AudioSource soundSource;


		void OnValidate(){
			// Clamp this so the user has a better idea on the range. Adjust to anything you like.
			moveSpeed = Mathf.Clamp(moveSpeed, 0f, 5f);
			// IF the parent is NULL
			if(parent != null){
				// IF the parent has a Rigidbody2D component
				if(parent.GetComponent<Rigidbody2D>() == null){
					// Add this component to the parent GameObject as it is a requirement for the parent to have a rigidbody for this to work, me (Joey) setting the variables manually is a jump though so we just add the component to let the 
					// developer know that it should be attached to this GameObject, now it is up to YOU (The developer) to figure out what variables need to be set in the RigidBody2D component.
					parent.AddComponent<Rigidbody2D> ();
				}
			}
		}

		void Awake(){
			// Cache the rigidbody for faster reference.
			parentRB = parent.GetComponent<Rigidbody2D>();
			// Cache the transform for faster reference.
			parentTransform = parent.transform;
		}

		void Start(){
			// Set the timer for push delay.
			timer = timeToPush;
		}

		void OnCollisionStay2D(Collision2D coll) {
			// Loop through the size of our tagsThatPushThis array.
			for(int i = 0; i < tagsThatPushThis.Length; i++){
				// IF the colliding GameObjects tag is equal to one of the tags that can push this GameObject.
				if(coll.gameObject.tag == tagsThatPushThis[i]){
					// IF no options were set.
					if(!topCollider && !bottomCollider && !leftCollider && !rightCollider){
						// Let the developer know.
						Debug.Log("Something went wrong, none of the boolean Side Colliders were selected.");
						// GTFO.
						return;
					}

					// Variable for the direction we are going to make the parent gameobject move.
					Vector2 movement;
					// Get the vertical and horizontal input.
					float horAxis = Input.GetAxisRaw ("Horizontal");
					float vertAxis = Input.GetAxisRaw ("Vertical");
					// Depending on which collider is selected depends how the parent GameObject gets pushed.
					if (topCollider && vertAxis == -1f) {
						movement = new Vector2 (0f, -1f);
					} else if (bottomCollider && vertAxis == 1f) {
						movement = new Vector2 (0f, 1f);
					} else if (leftCollider && horAxis == 1f) {
						movement = new Vector2 (1f, 0f);
					} else if (rightCollider && horAxis == -1f) {
						movement = new Vector2 (-1f, 0f);
					} else {
						// Reset the timer.
						timer = timeToPush;
						// Don't do anything.
						return;
					}

					// Countdown the timer.
					timer -= Time.deltaTime;
					// IF we have not pushed this long enough for the timer.
					if(timer >= 0f){
						// GTFO
						return;
					}

					// Play the pushing sound.
					PlayPushSound();
					// Set the velocity of the object.
					parentRB.velocity = movement * moveSpeed;
					// Since we found a match we are done.
					return;
				}
			}
		}

		void OnCollisionExit2D(Collision2D coll){
			// Loop through the size of our tagsThatPushThis array.
			for (int i = 0; i < tagsThatPushThis.Length; i++) {
				// No collision no moverino.
				parentRB.velocity = Vector2.zero;
				// Reset the timer.
				timer = timeToPush;
			}
			// IF a sound is playing.
			if(soundSource != null){
				// Destroy the sound.
				Destroy (soundSource.gameObject);
			}
		}

		/// <summary>
		/// Plays the push sound.
		/// </summary>
		void PlayPushSound(){
			// IF we don't have an active SoundSource.
			if(soundSource == null){
				// Play the soundClip.
				soundSource = Grid_Helper.soundManager.PlaySound(soundClip, parentTransform.position, minPitch, maxPitch);
			}
		}
	}
}
