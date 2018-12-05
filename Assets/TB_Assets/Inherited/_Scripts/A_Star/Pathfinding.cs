using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

namespace TrollBridge {

	/// <summary>
	/// Pathfinding script that works with our A Star Pathfinding.
	/// </summary>
	public class Pathfinding : MonoBehaviour {

		PathRequestManager requestManager;
		AStar_Grid aStarGrid;


		void Awake () {
			requestManager = GetComponent<PathRequestManager> ();
			aStarGrid = GetComponent<AStar_Grid> ();
		}

		/// <summary>
		/// Check and make sure we are moving somewhere we can.
		/// </summary>
		public bool Requestable (Vector2 startPos, Vector2 targetPos) {
			// Get our Nodes based on our Vector3 positions.
			Node startNode = aStarGrid.NodeFromWorldPoint (startPos);
			Node targetNode = aStarGrid.NodeFromWorldPoint (targetPos);
			// IF the startNode and targetNode are the same.
			if (startNode.Equals (targetNode)) {
				// This is not a requestable spot.
				return false;
			}

			// Met all the requirements.
			return true;
		}

		/// <summary>
		/// Starts the find path coroutine.
		/// </summary>
		public void StartFindPath(Vector2 startPos, Vector2 targetPos){
			StartCoroutine (FindPath(startPos, targetPos));
		}

		/// <summary>
		/// Finds the path.
		/// </summary>
		private IEnumerator FindPath (Vector3 startPos, Vector3 targetPos) {
//			Stopwatch sw = new Stopwatch ();
//			sw.Start ();

			Vector2[] wayPoints = new Vector2[0];
			bool pathSuccess = false;
			// Get our Nodes based on our Vector3 positions.
			Node startNode = aStarGrid.NodeFromWorldPoint (startPos);
			Node targetNode = aStarGrid.NodeFromWorldPoint (targetPos);

			// IF our target node is walkable.
			if (targetNode.GetWalkable ()) {
				// Create a Heap of Nodes with the max size being our 2D grid for A Star movement.
				// All start off being openNodes.
				Heap<Node> openSet = new Heap<Node> (aStarGrid.MaxSize);
				// Create an empty HashSet of Nodes for our Nodes that are closed.
				HashSet<Node> closedSet = new HashSet<Node> ();
				// Add our Starting Node to begin our pathing.
				openSet.Add (startNode);

				// WHILE we have Nodes in our Heap.
				while (openSet.Count > 0) {
					// Pop the first Node off and that is our current Node.
					Node currentNode = openSet.RemoveFirst ();
					// We know about this Node now so we move it to our closedSet.
					closedSet.Add (currentNode);

					// IF the Node we are on is the targetNode (our destination).
					if (currentNode == targetNode) {
//						sw.Stop ();
//						print ("Path found : " + sw.ElapsedMilliseconds + " ms");
						// Our pathing has been successful
						pathSuccess = true;
						// Break the loop.
						break;
					}

					// Loop to each neighbour in our A Star Pathfinding Grid.
					foreach (Node neighbour in aStarGrid.GetNeighbours(currentNode)) {
						// IF the Node is NOT Walkable OR the Node is in our closedSet.
						if (!neighbour.GetWalkable () || closedSet.Contains (neighbour)) {
							// We go to the next next.
							continue;
						}

						// Get our movement cost if we were to move to this neighbour.
						int newMovementCostToNeighbour = currentNode.GetGCost () + GetDistance (currentNode, neighbour) + neighbour.GetMovementPenalty();
						// IF the new movement cost to move to our neighbour is less than our previous GCost OR 
						// IF the openSet does NOT contain our neighbour meaning we have not visited it yet
						if (newMovementCostToNeighbour < neighbour.GetGCost () || !openSet.Contains (neighbour)) {
							// Set our GCost
							neighbour.SetGCost (newMovementCostToNeighbour);
							// Set our HCost
							neighbour.SetHCost (GetDistance (neighbour, targetNode));
							// Set the parent of this node.
							neighbour.SetParent (currentNode);

							// IF this neighbour Node happened to be a node that is NOT in our openSet,
							// ELSE this neighbour Node that IS in our openSet.
							if (!openSet.Contains (neighbour)) {
								// We add this neighbour node to our openSet.
								openSet.Add (neighbour);
							} else {
								// It is in our openSet so lets update the Node.
								openSet.UpdateItem (neighbour);
							}
						} 
					}
				}
			}

			// IF we happened to find a path to our targetNode.
			if (pathSuccess) {
				// Retrace our path and set those points as our wayPoints.
				wayPoints = RetracePath (startNode, targetNode);
			}
			// Finalize the path processing.
			requestManager.FinishedProcessingPath (wayPoints, pathSuccess);
			yield return null;
		}

		/// <summary>
		/// Retraces the path we have found.
		/// </summary>
		private Vector2[] RetracePath (Node startNode, Node endNode) {
			// Create a new list of Nodes.
			List<Node> path = new List<Node> ();
			// Our current Node is our end node since we are retracing.
			Node currentNode = endNode;

			// WHILE our currentNode is not the startNode (meaning we are done.).
			while (currentNode != startNode) {
				// Add the currentNode to our path.
				path.Add (currentNode);
				// The currentNode's parent is now the currentNode.
				currentNode = currentNode.GetParent ();
			}
			// Simplify the path.
			Vector2[] wayPoints = SimplifyPath (path);
			// Reverse the array of waypoints.
			Array.Reverse (wayPoints);
			// Reverse the List of Nodes for our path.
			path.Reverse ();
			// Set the aStarGrid's path so that we can visually see the path in our Scene View.
			aStarGrid.path = path;
			// Return our waypoints.
			return wayPoints;
		}

		/// <summary>
		/// Simplifies the path.
		/// </summary>
		Vector2[] SimplifyPath(List<Node> path) {
			// New list of Vector2.
			List<Vector2> wayPoints = new List<Vector2> ();
			// Set our directionOld to 0,0,0.
			Vector2 directionOld = Vector2.zero;

			// LOOP the amount of times we have paths.
			for(int i = 1; i < path.Count; i++){
				// Get our new vector direction based on our current and old path.
				Vector2 directionNew = new Vector2(path[i-1].GetGridX() - path[i].GetGridX(), path[i-1].GetGridY() - path[i].GetGridY());
				// IF our new direction does not equal the old direction.
				if(directionNew != directionOld){
					// We add a waypoint.
					wayPoints.Add (path [i-1].GetWorldPosition ());
				}
				// The new direction is our old direction now.
				directionOld = directionNew;
			}
			// Return our waypoints.
			return wayPoints.ToArray ();
		}

		/// <summary>
		/// Gets the distance between 2 nodes.
		/// </summary>
		private int GetDistance (Node nodeA, Node nodeB) {
			// Get the x distance.
			int distX = Mathf.Abs (nodeA.GetGridX() - nodeB.GetGridX());
			// Get the y distance.
			int distY = Mathf.Abs (nodeA.GetGridY() - nodeB.GetGridY());

			// IF our x distance is greater than our y distance,
			// ELSE our x distance is less than our y distance.
			if (distX > distY) {
				return 14 * distY + 10 * (distX - distY);
			} else {
				return 14 * distX + 10 * (distY - distX);
			}
		}
	}
}
