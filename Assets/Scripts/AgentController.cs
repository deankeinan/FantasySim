using UnityEngine;
using UnityEngine.Events;
using System.Collections;
/// <summary>
    /// Click movement that will move our gameobject to where we clicked based on A Star pathfinding.
    /// </summary>
    public class AgentController : MonoBehaviour
    {

    [SerializeField] private AgentManager agentManager;
        [SerializeField] private float speed = 5f;
        private Vector2[] path;
        private int targetIndex;
        PathfindingCoroutines pathfinder;
        private UnityAction timestepListener;
        private GameObject holder;
        private EventManager events;



    void Start()
        {
            // Attempt to get this character manager script.
            agentManager = GetComponentInParent<AgentManager>();
            pathfinder = GetComponentInParent<PathfindingCoroutines>();
            holder = GameObject.FindWithTag("Holder");
            print(holder.gameObject);
        timestepListener = new UnityAction(TimeStep);
        events = holder.GetComponent<EventManager>();
        events.StartListening("timestep", timestepListener);
       

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TimeStep();
            }
        }

    private void OnEnable()
    {
       
    }

    public void TimeStep(){
        Vector2 newPosition = new Vector2();
        newPosition = GetRandomWalkablePoint();
        pathfinder.RequestPath((Vector2)transform.position, newPosition, OnPathFound);
    }

        public Vector2 GetRandomWalkablePoint(){
            Vector2 newPosition = new Vector2();
            newPosition = Random.insideUnitCircle * 8 + new Vector2(agentManager.transform.position.x, agentManager.transform.position.y);

            while (true)
            {
                if (pathfinder.IsVectorWalkable(newPosition))
                {
                    break;
                }
                Debug.Log("looking for position");
                newPosition = Random.insideUnitCircle * 8 + new Vector2(agentManager.transform.position.x, agentManager.transform.position.y);
            }
            return newPosition;
        }
    
        public void DoItNow()
        {
            // See if we can find ourselves a path after a left click.
            Vector2 newPosition = new Vector2();
            newPosition = Random.insideUnitCircle * 8 + new Vector2(agentManager.transform.position.x, agentManager.transform.position.y);

            while (true)
            {
                if (pathfinder.IsVectorWalkable(newPosition))
                {
                    break;
                }
                Debug.Log("looking for position");
                newPosition = Random.insideUnitCircle * 8 + new Vector2(agentManager.transform.position.x, agentManager.transform.position.y);
            }
            pathfinder.RequestPath((Vector2)transform.position, newPosition, OnPathFound);
        }

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
                // Stop the current FollowPath coroutine.
                StopCoroutine("FollowPath");
                // Start our FollowPath coroutine.
                StartCoroutine("FollowPath");
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
                    if ((Vector2)transform.position == currentWaypoint)
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
                    transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                    // Play the animation.
                    PlayAnimation(currentWaypoint.x - transform.position.x, currentWaypoint.y - transform.position.y);
                    yield return null;
                }
            }
            // Play the animation.
            PlayAnimation(0f, 0f);
            yield return null;
        }

        /// <summary>
        /// Play the Animation of this GameObject based on if there is a 4 direction or 8 direction animation.
        /// </summary>
        void PlayAnimation(float hor, float vert)
        {
            // IF the user has an animation set and ready to go.
            if (agentManager.characterAnimator != null)
            {
            FourDirectionAnimation(hor * agentManager.playerInvertX, vert * agentManager.playerInvertY, agentManager.characterAnimator);
            }
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
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }

    public void SetAnimationsIdle(Animator anim)
    {
        anim.SetBool("IsIdle", true);
        anim.SetBool("IsMoving", false);
    }

    /// <summary>
    /// Assign our animation variables to handle our Moving state.
    /// </summary>
    public void SetAnimationsWalk(Animator anim)
    {
        // We are moving.
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoving", true);
        // We cannot craft while moving.
        anim.SetBool("IsCrafting", false);
    }

    public void FourDirectionAnimation(float moveHorizontal, float moveVertical, Animator anim)
    {
        // IF we are moving we set the animation IsMoving to true,
        // ELSE we are not moving.
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            // Set walk animations.
            SetAnimationsWalk(anim);
        }
        else
        {
            // Set idle animations.
            SetAnimationsIdle(anim);
            // We leave if our player is not moving as we don't want to give access to looking in other directions.
            return;
        }

        // IF we are wanting to go in the positive X direction,
        // ELSE IF we are wanting to move in the negative X direction,
        // ELSE IF we are wanting to move in the negative Y direction,
        // ELSE IF we are wanting to move in the positive Y direction.
        if (moveHorizontal > 0 && Mathf.Abs(moveVertical) <= Mathf.Abs(moveHorizontal))
        {
            anim.SetInteger("Direction", 4);

        }
        else if (moveHorizontal < 0 && Mathf.Abs(moveVertical) <= Mathf.Abs(moveHorizontal))
        {
            anim.SetInteger("Direction", 2);

        }
        else if (moveVertical < 0 && Mathf.Abs(moveVertical) > Mathf.Abs(moveHorizontal))
        {
            anim.SetInteger("Direction", 3);

        }
        else if (moveVertical > 0 && Mathf.Abs(moveVertical) > Mathf.Abs(moveHorizontal))
        {
            anim.SetInteger("Direction", 1);
        }
    }
}
