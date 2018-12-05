using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Game timer start begins our time for our game.
	/// </summary>
	public class Game_Timer_Start : MonoBehaviour {

		void Awake () {
			GameObject.FindGameObjectWithTag ("Game Timer").GetComponent<Game_Timer> ().SetCanTime(true);
		}
	}
}
