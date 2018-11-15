using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace TrollBridge {

	/// <summary>
	/// Changes the scene as soon as we load the scene.  This is used in the demo for the very first scene in which we load all the important scripts then we move into the actual game.
	/// If you want to start a new game with this then make sure to check newGame = true.
	/// </summary>
	public class Scene_Change_Instant : MonoBehaviour {
		
		[Tooltip("The scene name we want to change to as soon as this current scene is done loading everything in its Awake and Start.")]
		[SerializeField] private string sceneName;
		[Tooltip("Are we starting a new game where the player is made in the next scene?")]
		[SerializeField] private bool newGame = true;


		void Start () {
			// IF we are instantly going into our scene with our player.
			if (newGame) {
				// Load a new game.
				NewGame ();
			} else {
				// Go to the next scene.
				SceneManager.LoadScene (sceneName);
			}
		}

		public void NewGame () {
			// Remove any saved data.
			Grid_Helper.helper.ClearAllSavedData ();
			// Handle all the middle meaty stuff here.
			Grid_Helper.helper.ReloadManagers ();
			// Load the first scene that starts your game.
			Grid_Helper.helper.ChangeScene(sceneName, 0);
		}
	}
}
