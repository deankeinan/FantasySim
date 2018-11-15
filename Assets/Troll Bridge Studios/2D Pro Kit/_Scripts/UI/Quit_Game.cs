using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Quits the game with Application.Quit().
	/// </summary>
	public class Quit_Game : MonoBehaviour {

		public void QuitGame(){
			// Quit the game.
			Application.Quit();
		}
	}
}
