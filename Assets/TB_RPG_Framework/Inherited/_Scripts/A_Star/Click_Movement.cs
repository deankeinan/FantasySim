using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Click movement that will move our gameobject to where we clicked based on A Star pathfinding.
	/// </summary>
	public class Click_Movement : MonoBehaviour {

		[SerializeField] private Character_Manager characterManager;
		[SerializeField] private bool leftClickMove = true;
		[SerializeField] private bool rightClickMove = false;
		[SerializeField] private float speed = 5f;
		private Vector2[] path;
		private int targetIndex;
        PathRequestManager requestManager;

		void Start(){
			// Attempt to get this character manager script.
			characterManager = GetComponentInParent <Character_Manager> ();
            requestManager = GetComponentInParent<PathRequestManager>();

        }

        void Update () {
			if (Input.GetKeyDown(KeyCode.X)) {
                // See if we can find ourselves a path after a left click.
                Vector2 newPosition = new Vector2();
                newPosition = Random.insideUnitCircle * 8 + new Vector2(characterManager.transform.position.x, characterManager.transform.position.y);

                while (true) 
                {
                    if(requestManager.IsVectorWalkable(newPosition)){
                        break;
                    }
                    Debug.Log("looking for position");
                    newPosition = Random.insideUnitCircle * 8 + new Vector2(characterManager.transform.position.x, characterManager.transform.position.y);
                }
                PathRequestManager.RequestPath ((Vector2)transform.position, newPosition, OnPathFound);
			}
		}

        public void DoItNow(){
                // See if we can find ourselves a path after a left click.
                Vector2 newPosition = new Vector2();
                newPosition = Random.insideUnitCircle * 8 + new Vector2(characterManager.transform.position.x, characterManager.transform.position.y);

                while (true)
                {
                    if (requestManager.IsVectorWalkable(newPosition))
                    {
                        break;
                    }
                    Debug.Log("looking for position");
                    newPosition = Random.insideUnitCircle * 8 + new Vector2(characterManager.transform.position.x, characterManager.transform.position.y);
                }
                PathRequestManager.RequestPath((Vector2)transform.position, newPosition, OnPathFound);
        }

		/// <summary>
		/// Raises the path found event.
		/// </summary>
		public void OnPathFound (Vector2[] newPath, bool pathSuccessful) {
			// IF we happened to have a successful path.
			if (pathSuccessful) {
				// Set our new path.
				path = newPath;
				// Start with an index of 0.
				targetIndex = 0;
				// Stop the current FollowPath coroutine.
				StopCoroutine ("FollowPath");
				// Start our FollowPath coroutine.
				StartCoroutine ("FollowPath");
			}
		}

		/// <summary>
		/// Follows the path.
		/// </summary>
		private IEnumerator FollowPath () {
			// IF we have a path.
			if (path.Length != 0) {
				// Get our first location and make that our current waypoint.
				Vector2 currentWaypoint = path [0];

				// WHILE always true.
				while (true) {
					// IF our position reaches our waypoint.
					if ((Vector2)transform.position == currentWaypoint) {
						// Go to our next index.
						targetIndex++;
						// IF our index is greater than or equal to the amount of paths we have.
						if (targetIndex >= path.Length) {
							// GTFO out of this loop as we are done.
							yield break;
						}
						// Next index in our path is now our currentWaypoint.
						currentWaypoint = path [targetIndex];
					}
					// Move towards our currentWaypoint.
					transform.position = Vector2.MoveTowards (transform.position, currentWaypoint, speed * Time.deltaTime);
					// Play the animation.
					PlayAnimation (currentWaypoint.x - transform.position.x, currentWaypoint.y - transform.position.y);
					yield return null;
				}
			}
			// Play the animation.
			PlayAnimation (0f, 0f);
			yield return null;
		}

		/// <summary>
		/// Play the Animation of this GameObject based on if there is a 4 direction or 8 direction animation.
		/// </summary>
		void PlayAnimation(float hor, float vert){
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

		/// <summary>
		/// Draw our grid in the scene view.
		/// </summary>
		public void OnDrawGizmos() {
			if (path != null) {
				for (int i = targetIndex; i < path.Length; i++) {
					Gizmos.color = Color.black;
					Gizmos.DrawCube (path [i], Vector3.one / 8f);

					if (i == targetIndex) {
						Gizmos.DrawLine (transform.position, path [i]);
					} else {
						Gizmos.DrawLine (path [i - 1], path [i]);
					}
				}
			}
		}
	}
}
