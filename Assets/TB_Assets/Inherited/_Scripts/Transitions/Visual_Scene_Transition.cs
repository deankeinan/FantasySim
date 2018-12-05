using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace TrollBridge {

	/// <summary>
	/// Visual scene transition script is used to decide how we will use our transition.  Pick between any of the transitions listed and when the method "StartSceneVisualTransition ()" is called we will do the visual transition.
	/// </summary>
	public class Visual_Scene_Transition : MonoBehaviour {

		[Tooltip ("If you want a fade transition.")]
		[SerializeField] private bool fade;
		[Tooltip ("If you want a slide transition.")]
		[SerializeField] private bool slide;
		[Tooltip ("If you want a twirl transition.")]
		[SerializeField] private bool twirl;
		[Tooltip ("The name of the scene that will be loaded.")]
		[SerializeField] private string newScene;
		[Tooltip ("The location in the new scene that the player will be spawned at.  This number is correlated to the Transform locations on your Scene Manager Script.")]
		[SerializeField] private int sceneSpawnLocation = 0;
		[Tooltip ("Boolean used so that if we need to wipe our data we can.  When we go to the next scene we want all of our saved data to be erased. This goes hand in hand with a New Game button on a main menu screen.")]
		[SerializeField] private bool newGame = false;


		void OnTriggerEnter2D(Collider2D coll){
			// IF the colliding GameObject is NOT the player.
			if (coll.gameObject != Character_Helper.GetPlayer ()) {
				// We leave.
				return;
			}
			// Lets change our scene.
			StartSceneVisualTransition ();
		}


		/// <summary>
		/// Begin the visual scene transition.
		/// </summary>
		public void StartSceneVisualTransition () {
			// IF we starting a new game.
			if (newGame) {
				// Clear all of our saved data.
				Grid_Helper.helper.ClearAllSavedData ();
				// Handle all the middle meaty stuff here.
				Grid_Helper.helper.ReloadManagers ();
			}

			// IF we want to have a fade,
			// ELSE IF we want to have a slide,
			// ELSE IF we want to have a twirl,
			if (fade) {
				// Start the coroutine for our fading.
				StartCoroutine (Grid_Helper.visualTransition.FadeCycle (newScene, sceneSpawnLocation));
			} else if (slide) {
				// Start the coroutine for our sliding.
				StartCoroutine (Grid_Helper.visualTransition.SlideCycle (newScene, sceneSpawnLocation));
			} else if (twirl) {
				// Start the coroutine for our twirl.
				StartCoroutine (Grid_Helper.visualTransition.TwirlCycle (newScene, sceneSpawnLocation));
			}
		}
	}
}
