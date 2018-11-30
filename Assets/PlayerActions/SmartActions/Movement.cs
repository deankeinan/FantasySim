using UnityEngine;
using System.Collections;
namespace TrollBridge
{

    /// <summary>
    /// Click movement that will move our gameobject to where we clicked based on A Star pathfinding.
    /// </summary>
    public class No_Click_Movement
    {

        [SerializeField] private Character mover;
        [SerializeField] private float speed = 5f;
        private Vector2[] path;
        private int targetIndex;


        /// <summary>
        /// Raises the path found event.
        /// </summary>
        public void OnPathFound(Vector2[] newPath, bool pathSuccessful)
        {
            // IF we happened to have a successful path.
            if (pathSuccessful)
            {
                // Set our new path.
                path = newPath;
                // Start with an index of 0.
                targetIndex = 0;
                FollowPath();
            }
        }

        /// <summary>
        /// Follows the path.
        /// </summary>
        private IEnumerator FollowPath()
        {
            // IF we have a path.
            if (path.Length != 0)
            {
                // Get our first location and make that our current waypoint.
                Vector2 currentWaypoint = path[0];

                // WHILE always true.
                while (true)
                {
                    // IF our position reaches our waypoint.
                    if ((Vector2)mover.transform.position == currentWaypoint)
                    {
                        // Go to our next index.
                        targetIndex++;
                        // IF our index is greater than or equal to the amount of paths we have.
                        if (targetIndex >= path.Length)
                        {
                            // GTFO out of this loop as we are done.
                            yield break;
                        }
                        // Next index in our path is now our currentWaypoint.
                        currentWaypoint = path[targetIndex];
                    }
                    // Move towards our currentWaypoint.
                    mover.transform.position = Vector2.MoveTowards(mover.transform.position, currentWaypoint, speed * Time.deltaTime);
                    yield return null;
                }
            }

            yield return null;
        }

        /// <summary>
        /// Draw our grid in the scene view.
        /// </summary>
        public void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one / 8f);

                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(mover.transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }
    }
}
