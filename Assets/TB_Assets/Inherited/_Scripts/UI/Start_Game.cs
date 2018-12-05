using UnityEngine;
using UnityEngine.SceneManagement;

namespace TrollBridge {

	/// <summary>
	/// This is the script that starts it all!!  This controls the start of the saved persistent data.  You click "New Game" and all of your saved data gets removed so the game can be played like it was the first run through.
	/// The continue button will not be interactable if there is no saved data as there is nothing to continue!
	/// </summary>
	public class Start_Game : MonoBehaviour {
		
		[Tooltip("The Start scene for when you click New Game.")]
		public string newScene;
		[Tooltip("The location where you want the player to start in the 'New Game Scene Name'.scene")]
		public int newGameStartingPoint;


		public void NewGame(){
			// Remove any saved data.
			Grid_Helper.helper.ClearAllSavedData ();
			// Handle all the middle meaty stuff here.
			Grid_Helper.helper.ReloadManagers ();
			// Load the first scene that starts your game.
			Grid_Helper.helper.ChangeScene(newScene, newGameStartingPoint);
		}
	}
}
