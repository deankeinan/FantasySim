using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    /// <summary>
    /// Path request manager that works with our A Star Pathfinding.
    /// </summary>
    public class PathfindingCoroutines : MonoBehaviour
    {

        Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
        PathRequest currentPathRequest;

        static PathfindingCoroutines instance;
        Pathfinding pathFinding;

        bool isProcessingPath;


        void Awake()
        {
            instance = this;
            pathFinding = GetComponent<Pathfinding>();
        }

        public bool IsVectorWalkable(Vector2 target)
        {
            return pathFinding.IsVectorWalkable(target);

        }


        /// <summary>
        /// Requests the path.
        /// </summary>
        public void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback)
        {
            // Lets create a new PathRequest.
            PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
            // Enqueue the PathRequest Queue.
            instance.pathRequestQueue.Enqueue(newRequest);
            // Lets see if we can process the path.
            instance.TryProcessNext();
        }

        /// <summary>
        /// Trys the next process to see if we can start a path.
        /// </summary>
        private void TryProcessNext()
        {
            // IF we are NOT processing a path AND we have 1 or more path requests.
            if (!isProcessingPath && pathRequestQueue.Count > 0)
            {
                // Dequeue a Queue in our pathRequestQueue and make that our currentPathRequest.
                currentPathRequest = pathRequestQueue.Dequeue();
                // Let it known we are processing a path.
                isProcessingPath = true;
                // Start finding our path.
                pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
            }
        }

        /// <summary>
        /// Finished the processing path.
        /// </summary>
        public void FinishedProcessingPath(Vector2[] path, bool success)
        {
            // Make a callback to our PathRequest.
            currentPathRequest.callback(path, success);
            // We are no longer processing a path.
            isProcessingPath = false;
            // Lets see if we can process a path.
            TryProcessNext();
        }

        /// <summary>
        /// Path request struct.
        /// </summary>
        struct PathRequest
        {
            public Vector2 pathStart;
            public Vector2 pathEnd;
            public Action<Vector2[], bool> callback;

            public PathRequest(Vector2 _start, Vector2 _end, Action<Vector2[], bool> _callback)
            {
                pathStart = _start;
                pathEnd = _end;
                callback = _callback;
            }
        }
    }
