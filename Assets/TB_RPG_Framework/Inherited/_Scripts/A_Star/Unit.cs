using System.Collections;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Unit script which when we do a RequestPath from PathRequestManager this gameobject will move in a A Star pathfinding way to our 'target' location as soon as it can.
	/// </summary>
	public class Unit : MonoBehaviour {

		[Tooltip ("If we want to look for our player.")]
		[SerializeField] private bool targetPlayer = true;
		[Tooltip ("The GameObject we want to follow.  If you have targetPlayer equal to true then this will be disregarded.")]
		[SerializeField] private Transform target;
		[Tooltip ("The speed in which our characters move from waypoint to waypoint.")]
		[SerializeField] private float speed = 5f;
		private Vector2[] path;
		private int targetIndex;
		private Character_Manager characterManager;


		void Start(){
			// Attempt to get this character manager script.
			characterManager = GetComponentInParent <Character_Manager> ();
			// IF we have a target.
			if (target != null) {
				// Find a path.
				PathRequestManager.RequestPath ((Vector2)transform.position, (Vector2)target.position, OnPathFound);
			}
		}

		void Update () {
			// IF we didnt have a target.
			if (target == null) {
				// IF we want the player AND a Player exists.
				if (targetPlayer && Character_Helper.GetPlayer () != null) {
					// Set the player transfer.
					target = Character_Helper.GetPlayer ().transform;
					// Leave.
					return;
				}
			} else {
				// Find a path.
				PathRequestManager.RequestPath ((Vector2)transform.position, (Vector2)target.position, OnPathFound);
			}
		}

		/// <summary>
		/// Raises the path found event.
		/// </summary>
		public void OnPathFound(Vector2[] newPath, bool pathSuccessful){
			// IF we have a successful path found.
			if (pathSuccessful) {
				// Set our new path.
				path = newPath;
				// Reset the index.
				targetIndex = 0;
				// Stop and Start the FollowPath Coroutine.
				StopCoroutine("FollowPath");
				StartCoroutine("FollowPath");
			}
		}

		/// <summary>
		/// Follows the path.
		/// </summary>
		private IEnumerator FollowPath () {
			// IF we have a path.
			if (path.Length > 0) {
				// Get the first spot in the path array.
				Vector2 currentWaypoint = path [0];
				// Constant Loop.
				while (true) {
					// IF we made it to our waypoint.
					if ((Vector2)transform.position == currentWaypoint) {
						// Next index.
						targetIndex++;
						// IF our index is greater or equal to the amount of paths we have.
						if (targetIndex >= path.Length) {
							// Break the loop.
							yield break;
						}
						// Set the next waypoint.
						currentWaypoint = path [targetIndex];
					}
					// Move towards the next waypoint.
					transform.position = Vector2.MoveTowards (transform.position, currentWaypoint, speed * Time.deltaTime);
					// Play the animation.
					PlayAnimation (currentWaypoint.x - transform.position.x, currentWaypoint.y - transform.position.y);
					yield return null;
				}
			}
		}

		/// <summary>
		/// Play the Animation of this GameObject based on if there is a 4 direction or 8 direction animation.
		/// </summary>
		void PlayAnimation(float hor, float vert){
			// IF we have a Character Manager.
			if (characterManager != null) {
				// IF the user has an animation set and ready to go.
				if (characterManager.characterAnimator != null) {
					// IF the character has a Four Direction Animation,
					// ELSE IF the character has a Eight Direction Animation.
					if (characterManager.fourDirAnim) {
						// Play animations.
						Grid_Helper.helper.FourDirectionAnimation (hor * characterManager.playerInvertX, vert * characterManager.playerInvertY, characterManager.characterAnimator);
					} else if (characterManager.eightDirAnim) {
						// Play animation.
						Grid_Helper.helper.EightDirectionAnimation (hor * characterManager.playerInvertX, vert * characterManager.playerInvertY, characterManager.characterAnimator);
					}
				}
			}
		}

		public void OnDrawGizmos() {
			if (path != null) {
				for (int i = targetIndex; i < path.Length; i ++) {
					Gizmos.color = Color.black;
					Gizmos.DrawCube(path[i], Vector3.one/8f);

					if (i == targetIndex) {
						Gizmos.DrawLine(transform.position, path[i]);
					}
					else {
						Gizmos.DrawLine(path[i-1],path[i]);
					}
				}
			}
		}
	}
}
