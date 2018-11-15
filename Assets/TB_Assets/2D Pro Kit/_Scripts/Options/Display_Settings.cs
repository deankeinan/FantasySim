using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Script used so that we have a method to communicate with our Options_Manager to display our Options Window.
	/// </summary>
	public class Display_Settings : MonoBehaviour {

		/// <summary>
		/// Method that will display or remove the options UI.
		/// </summary>
		public void DisplaySettings(){
			Grid_Helper.optionManager.OptionsDisplay ();
		}
	}
}
