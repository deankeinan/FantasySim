using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Move this gameobject from start location to the end location withing the moveTime.
	/// </summary>
	public class Move_GameObject : MonoBehaviour {

		[Tooltip("The start location of this gameobject.")]
		public Transform StartLocation;
		[Tooltip("The end location of this gameobject.")]
		public Transform EndLocation;
		[Tooltip("The speed at which this GameObject will move from its Start Location to its End Location.")]
		public float moveTime;

		/// <summary>
		/// Moves this gameobject from the start to end.
		/// </summary>
		public void MoveStartToEnd(){
			StartCoroutine (Grid_Helper.helper.SmoothStepGameObject (transform, StartLocation, EndLocation, moveTime));
		}

		/// <summary>
		/// Moves this gameobject back to the start.
		/// </summary>
		public void MoveToStart(){
			// Move back to the start.
			transform.position = StartLocation.position;
		}
	}
}
