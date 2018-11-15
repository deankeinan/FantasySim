using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	[RequireComponent (typeof (Pathfinding))]
	[RequireComponent (typeof (PathRequestManager))]
	/// <summary>
	/// A star grid system for pathfinding.
	/// </summary>
	public class AStar_Grid : MonoBehaviour {

		[Header ("Scene View")]
		[Tooltip ("Display the grid in our Scene View?")]
		[SerializeField] private bool displayGridGizmos = true;

		[Header ("Grid Size")]
		[Tooltip ("How big our Nodes are in our A Star Grid.")]
		[SerializeField] private float nodeRadius = 0.1f;
		[Tooltip ("Set the size of our A Star Grid for pathfinding.")]
		[SerializeField] private Vector2 gridWorldSize;

		[Header ("Regions")]
		[Tooltip ("Set layers that are not walkable.")]
		[SerializeField] private LayerMask unwalkableMask;
		[Tooltip ("Layer regions that we can set weights to for more 'Smarter' pathfinding.")]
		[SerializeField] private TerrainType[] walkableRegions;

		[Header ("Obstacle Penalty")]
		[Tooltip ("A movement penalty that will make areas around obstacles hold higher weight so that we don't always nut hug walls when pathfinding.")]
		[SerializeField] private int obstacleProximityPenalty = 10;

		private LayerMask walkableMask;
		private Dictionary<int, int> walkableRegionDictionary = new Dictionary<int, int>();

		Node[,] aStarGrid;

		private float nodeDiameter;
		private int gridSizeX, gridSizeY;

		int penaltyMin = int.MaxValue;
		int penaltyMax = int.MinValue;


		void Awake () {
			// Get our Node diameter.
			nodeDiameter = nodeRadius*2;
			// X and Y size of our A Star Grid.
			gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
			gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

			// Loop based on how many layers we want to be weighted.
			foreach (TerrainType region in walkableRegions) {
				// Set the layer mask.
				walkableMask += region.terrainMask;
				// Set the weight penalty.
				walkableRegionDictionary.Add ((int)Mathf.Log (region.terrainMask.value, 2), region.terrainPenalty);
			}
			// Create our grid.
			CreateGrid ();
		}

		/// <summary>
		/// Gets the max size of our grid.
		/// </summary>
		public int MaxSize{
			get {
				return gridSizeX * gridSizeY;
			}
		}

		/// <summary>
		/// Gets the neighbour nodes.
		/// </summary>
		public List<Node> GetNeighbours(Node node) {
			// Create a new light of Nodes.
			List<Node> neighbours = new List<Node>();
			// Loop based on how many ways we can go on the X axis (Left, Stand Still, Right).
			for(int x = -1; x <= 1; x++) {
				// Loop based on how many ways we can go on the Y axis (Up, Stand Still, Down).
				for(int y = -1; y <= 1; y++) {
					// IF we are on the NODE we are currently standing on.
					if(x == 0 && y == 0) {
						// NEEEEEEEEEEEEEEEEEEEEEEEEXT!
						continue;
					}
					// combine checkX and checkY so that we are looking at 1 of our 8 neighbours.
					int checkX = node.GetGridX () + x;
					int checkY = node.GetGridY () + y;
					// IF we are still in the A Star Grid (Check that fail here means we are outside of the A Star Grid was was setup.).
					if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
						// Add this Node as a neighbour.
						neighbours.Add (aStarGrid[checkX, checkY]);
					}
				}
			}
			// Return our neighbour Nodes.
			return neighbours;
		}

		/// <summary>
		/// Get a nodes from a world point.
		/// </summary>
		public Node NodeFromWorldPoint (Vector2 worldPosition) {
			// Get the node we are closest to.
			float percentX = (worldPosition.x - transform.position.x) / gridWorldSize.x + 0.5f - (nodeRadius / gridWorldSize.x);
			float percentY = (worldPosition.y - transform.position.y) / gridWorldSize.y + 0.5f - (nodeRadius / gridWorldSize.y);
			// Clamp the percents.
			percentX = Mathf.Clamp01 (percentX);
			percentY = Mathf.Clamp01 (percentY);
			// Round to our nearest Node.
			int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
			int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
			// Return the Node we are closest to.
			return aStarGrid [x, y];
		}

		/// <summary>
		/// Creates the A Star Grid.
		/// </summary>
		private void CreateGrid() {
			// Create a new Node with the size based on our gridSizeX and gridSizeY.
			aStarGrid = new Node[gridSizeX, gridSizeY];
			// Get our most bottom left Node for reference.
			Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;

			// Loop the amount of times we have Nodes on our x axis.
			for (int x = 0; x < gridSizeX; x++) {
				// Loop the amount of times we have Nodes on our y axis.
				for (int y = 0; y < gridSizeY; y++) {
					// Find a location in our A Star Grid for a Node.
					Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
					// Is this walkable (based on what we put in the inspector for unwalkableMask).
					bool walkable = !(Physics2D.OverlapCircle (worldPoint, nodeRadius, unwalkableMask));
					// our default movementPenalty.
					int movementPenalty = 0;
					// Create a Ray.
					Ray ray = new Ray ((Vector3)worldPoint + Vector3.forward * 50, Vector3.back);
					// RaycastHit variable for if we hit something with our Ray/Raycast.
					RaycastHit hit;
					// IF we hit something.
					if (Physics.Raycast (ray, out hit, 100, walkableMask)) {
						// Set our penalty to our layer.
						walkableRegionDictionary.TryGetValue (hit.collider.gameObject.layer, out movementPenalty);
					}
					// IF this is not walkable.
					if(!walkable){
						// Add our penalty around Nodes we cannot move on.
						movementPenalty += obstacleProximityPenalty;
					}
					// Create a new node at x,y.
					aStarGrid [x, y] = new Node (walkable, worldPoint, x, y, movementPenalty);
				}
			}
			// Display blur for our penalties.
			BlurPenaltyMap (3);
		}

		/// <summary>
		/// Blurs the penalty Nodes in our map.
		/// </summary>
		private void BlurPenaltyMap (int blurSize) {
			// The size of the blur and how far we blur.
			int kernelSize = blurSize * 2 + 1;
			int kernelExtents = (kernelSize - 1) / 2;
			// Create a 2D array with the sizes of how many nodes are in our X and Y.
			// Also these 2 variables are the penalties for passing this Node Horizontally.
			int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
			int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

			// Loop the amount of times we have Y Nodes.
			for (int y = 0; y < gridSizeY; y++) {
				// Loop the amount of times from -kernelExtents to kernelExtents.
				for (int x = -kernelExtents; x <= kernelExtents; x++) {
					// sampleX is Nodes next to us on the X axis.
					int sampleX = Mathf.Clamp (x, 0, kernelExtents);
					// Add the penalty to this Node in our penaltiesHorizontalPass array for when we want to move across this node horizontally.
					penaltiesHorizontalPass [0, y] += aStarGrid [sampleX, y].GetMovementPenalty ();
				}

				// Loop the amount of times we have Nodes in our X axis.
				for(int x = 1; x < gridSizeX; x++){
					// Based on our kernelExtents we will be removing an Index.
					int removeIndex = Mathf.Clamp (x - kernelExtents - 1, 0, gridSizeX);
					int addIndex = Mathf.Clamp (x + kernelExtents, 0, gridSizeX - 1);
					// Finalize the penaltiesHorizontalPass for the Node.
					penaltiesHorizontalPass [x, y] = penaltiesHorizontalPass [x - 1, y] - aStarGrid [removeIndex, y].GetMovementPenalty () + aStarGrid [addIndex, y].GetMovementPenalty ();
				}
			}

			// Loop the amount of times we have X Nodes.
			for (int x = 0; x < gridSizeX; x++) {
				// Loop the amount of times from -kernelExtents to kernelExtents.
				for (int y = -kernelExtents; y <= kernelExtents; y++) {
					// Create a 2D array with the sizes of how many nodes are in our X and Y.
					// Also these 2 variables are the penalties for passing this Node Vertically.
					int sampleY = Mathf.Clamp (y, 0, kernelExtents);
					// Add the penalty to this Node in our penaltiesVerticalPass array for when we want to move across this node vertically.
					penaltiesVerticalPass [x, 0] += penaltiesHorizontalPass [x, sampleY];
				}

				int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, 0] / (kernelSize * kernelSize));
				aStarGrid [x, 0].SetMovementPenalty (blurredPenalty);

				// Loop the amount of times we have Y Nodes.
				for (int y = 1; y < gridSizeY; y++) {
					// Based on our kernelExtents we will be removing an Index.
					int removeIndex = Mathf.Clamp (y - kernelExtents - 1, 0, gridSizeY);
					int addIndex = Mathf.Clamp (y + kernelExtents, 0, gridSizeY - 1);
					// Finalize the penaltiesHorizontalPass for the Node.
					penaltiesVerticalPass [x, y] = penaltiesVerticalPass [x, y - 1] - penaltiesHorizontalPass [x, removeIndex] + penaltiesHorizontalPass [x, addIndex];
					// The penalty number for this blurred Node.
					blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, y] / (kernelSize * kernelSize));
					// Assign the penalty number.
					aStarGrid [x, y].SetMovementPenalty (blurredPenalty);

					// IF our blurredPenalty number is bigger than the max penalty number we set.
					if (blurredPenalty > penaltyMax) {
						penaltyMax = blurredPenalty;
					}
					// IF our blurredPenalty number is less than our min penalty number we set.
					if (blurredPenalty < penaltyMin) {
						penaltyMin = blurredPenalty;
					}
				}
			}
		}

		public List<Node> path;
		void OnDrawGizmos() {
			Gizmos.DrawWireCube (transform.position, new Vector3 (gridWorldSize.x, gridWorldSize.y, 1f));
			if (aStarGrid != null && displayGridGizmos) {
				foreach (Node n in aStarGrid) {
					Gizmos.color = Color.Lerp (Color.white, Color.black, Mathf.InverseLerp (penaltyMin, penaltyMax, n.GetMovementPenalty ()));
					Gizmos.color = (n.GetWalkable ()) ? Gizmos.color : Color.red;
					if(path != null){
						if(path.Contains(n)){
							Gizmos.color = Color.black;
						}
					}
					Gizmos.DrawCube (n.GetWorldPosition (), Vector3.one * (nodeDiameter - .1f));
				}
			}
		}

		[System.Serializable]
		public class TerrainType {
			public LayerMask terrainMask;
			public int terrainPenalty;
		}
	}
}
