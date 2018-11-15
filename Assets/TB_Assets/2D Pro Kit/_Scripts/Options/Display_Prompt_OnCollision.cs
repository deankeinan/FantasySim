using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	[RequireComponent (typeof (Collider2D))]
	/// <summary>
	/// Display prompt on collision and the display location is based on a Camera + Tag relationship as we want 
	/// our prompts to be on a fixed location on the Camera.  Looking for prompts to be displayed above said GameObject
	/// then use "Icon_Display" script.
	/// </summary>
	public class Display_Prompt_OnCollision : MonoBehaviour {

		// The prompt that will be displayed.
		public GameObject displayPrompt;
		// The tag of the prompt location.
		public string displayPromptTag;

		// The prompt location parent.
		private GameObject cameraPromptParent;
		// The prompt GameObject that is created and destroyed.
		private GameObject cameraPromptGO;


		void Start () {
			// Get the prompt parent location based on the tag.  Remember this GameObject we are looking for is a child
			// to our Camera GameObject.
			cameraPromptParent = GameObject.FindGameObjectWithTag (displayPromptTag);
		}

		void OnTriggerEnter2D (Collider2D coll) {
			// IF we do NOT have the player.
			if (!Grid_Helper.helper.IsPlayer (coll.GetComponentInParent <Character_Manager> ())) {
				// We leave.
				return;
			}

			// Create the prompt.  This should make the cameraPromptParent the parent of cameraPromptGO.
			cameraPromptGO = Instantiate (displayPrompt, cameraPromptParent.transform);
			// Set the position to be right where the parent is located.
			cameraPromptGO.transform.position = Vector3.zero;
			// Set the scaling to 1.
			cameraPromptGO.transform.localScale = Vector3.one;

			// Your prompt should now be displayed.
		}

		void OnTriggerExit2D (Collider2D coll) {
			// Exitting means we need to destroy the prompt.
			Destroy (cameraPromptGO);
		}
	}
}
