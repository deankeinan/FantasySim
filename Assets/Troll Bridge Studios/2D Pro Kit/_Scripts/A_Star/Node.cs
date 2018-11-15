using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Node used with our A Star Pathfinding.
	/// </summary>
	public class Node : IHeapItem<Node> {

		private bool walkable;
		private Vector3 worldPosition;
		private int gridX;
		private int gridY;
		private int movementPenalty;

		private int gCost;
		private int hCost;
		private Node parent;

		private int heapIndex;

		/// <summary>
		/// Initializes a new instance of the Node class.
		/// </summary>
		public Node (bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _penalty) {
			walkable = _walkable;
			worldPosition = _worldPos;
			gridX = _gridX;
			gridY = _gridY;
			movementPenalty = _penalty;
		}

		/// <summary>
		/// Gets the walkable variable.
		/// </summary>
		public bool GetWalkable() {
			return walkable;
		}

		/// <summary>
		/// Gets the world position.
		/// </summary>
		public Vector3 GetWorldPosition() {
			return worldPosition;
		}

		/// <summary>
		/// Gets the G cost.
		/// </summary>
		public int GetGCost () {
			return gCost;
		}

		/// <summary>
		/// Sets the G cost.
		/// </summary>
		public void SetGCost (int _gCost) {
			gCost = _gCost;
		}

		/// <summary>
		/// Gets the H cost.
		/// </summary>
		public int GetHCost () {
			return hCost;
		}

		/// <summary>
		/// Sets the H cost.
		/// </summary>
		public void SetHCost (int _hCost) {
			hCost = _hCost;
		}

		/// <summary>
		/// Gets the parent.
		/// </summary>
		public Node GetParent() {
			return parent;
		}

		/// <summary>
		/// Sets the parent.
		/// </summary>
		public void SetParent(Node _parent) {
			parent = _parent;
		}

		/// <summary>
		/// Gets the grid x.
		/// </summary>
		public int GetGridX () {
			return gridX;
		}

		/// <summary>
		/// Gets the grid y.
		/// </summary>
		public int GetGridY () {
			return gridY;
		}

		/// <summary>
		/// Gets the movement penalty.
		/// </summary>
		public int GetMovementPenalty () {
			return movementPenalty;
		}

		/// <summary>
		/// Sets the movement penalty.
		/// </summary>
		public void SetMovementPenalty (int _movementPenalty) {
			movementPenalty = _movementPenalty;
		}

		/// <summary>
		/// Gets the f cost.
		/// </summary>
		public int fCost {
			get {
				return gCost + hCost;
			}
		}

		/// <summary>
		/// Gets or sets the index of the heap.
		/// </summary>
		public int HeapIndex {
			get {
				return heapIndex;
			}
			set {
				heapIndex = value;
			}
		}

		/// <summary>
		/// Compares costs of nodes.
		/// </summary>
		public int CompareTo(Node nodeToCompare){
			int compare = fCost.CompareTo (nodeToCompare.fCost);
			if(compare == 0){
				compare = hCost.CompareTo (nodeToCompare.hCost);
			}
			return -compare;
		}
	}
}
