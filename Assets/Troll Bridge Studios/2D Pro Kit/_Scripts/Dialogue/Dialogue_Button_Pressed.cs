using System.Collections;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Dialogue Button Pressed script is placed on buttons that will start up the Dialogue_Setup script.
	/// </summary>
	public class Dialogue_Button_Pressed : MonoBehaviour {

		private Dialogue_Setup dialogueSetup;

		public void SetDialogueSetup (Dialogue_Setup newDialogueSetup) {
			dialogueSetup = newDialogueSetup;
		}

		public void DialogueButtonPressed () {
			// Remove the Multi Dialogue Window.
			//Grid_Helper.multipleInteractionsData.DisplayMultipleInteractionsUI (false);
			// Set the closest Dialogue.
			Grid_Helper.dialogueData.SetClosestDialogue (dialogueSetup);
			// Construct the dialogue with the closest GameObject with a Dialogue_Setup.
			Grid_Helper.dialogueData.Dialogue ();
		}
	}
}
