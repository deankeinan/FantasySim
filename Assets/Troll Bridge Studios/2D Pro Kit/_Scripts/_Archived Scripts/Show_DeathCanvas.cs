using UnityEngine;
using System.Collections;

namespace TrollBridge {

	public class Show_DeathCanvas : MonoBehaviour {

		public void ShowDeathCanvas () {
			// Start a coroutine that moves the Death Panel from 1 point to another point.
			Grid_Helper.endPanel.MoveStartToEnd();
		}
	}
}
