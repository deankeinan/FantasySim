using System.Collections;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Interaction allows us to focus on "interaction" based actions with a GameObject.
	/// </summary>
	public class Interaction : MonoBehaviour {
		
		void Update () {
			// IF we pressed the Interaction Key.
			if (Input.GetKeyDown (Grid_Helper.kBinds.GetActionKeyCode ())) {
                print("recieved");
				if (Grid_Helper.dialogueData.IsWithinDialogueInteraction ()) {
					// Find the closest Dialogue_Setup.
					Grid_Helper.dialogueData.FindClosestDialogue (Character_Helper.GetPlayer ());
					// Construct the dialogue with the closest GameObject with an Dialogue_Setup.
					Grid_Helper.dialogueData.Dialogue ();
					// This is what we checked for so we are done.
					return;
				}

			}
		}
	}
}
