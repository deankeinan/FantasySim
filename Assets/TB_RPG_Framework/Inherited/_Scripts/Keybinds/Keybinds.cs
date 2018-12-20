using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace TrollBridge {

	public class Keybinds : MonoBehaviour {

		[Header ("Keybinds")]
		[SerializeField] private KeyCode inventoryKeyCode = KeyCode.I;
		[SerializeField] private KeyCode actionKeyCode = KeyCode.T;

		private bool inventoryKeyCodeChange = false;
		private bool actionKeyCodeChange = false;
		private KeyCode chosenKeyCode;


		void Awake () {
			// Lets load our keybinds.
			LoadKeybinds ();
		}
		
		void Update () {
			// IF we want to change our inventory keycode,
			// ELSE IF we want to change our attack keycode,
			// ELSE IF we want to change our action keycode,
			// ELSE IF we want to change our quest tracker keycode,
			// ELSE IF we want to change our skill book keycode,
			// ELSE IF we want to change our alchemy ui keycode,
			// ELSE IF we want to change our blacksmith ui keycode.
			if (inventoryKeyCodeChange) {
				// IF we get a keycode pressed.
				if (KeyCodeChosen ()) {
					// Assign the keycode.
					inventoryKeyCode = chosenKeyCode;
					// Assign our Keybind variables.
					// Set to false.
					inventoryKeyCodeChange = false;
				}
			}  else if (actionKeyCodeChange) {
				// IF we get a keycode pressed.
				if (KeyCodeChosen ()) {
					// Assign the keycode.
					actionKeyCode = chosenKeyCode;
					// Assign our Keybind variables.
					// Set to false.
					actionKeyCodeChange = false;
				}
			} 
		}

		private void AssignKeyCodeChange (KeyCode keyCode, Text keyCodeDisplayString) {
			// Change the string to visually show the user the new key that was selected.
			keyCodeDisplayString.text = keyCode.ToString ().ToUpper ();
			// Lets remove the change keycode text.
		}

		public void ResetToDefaults () {
			// Need to take care of any current actions the user might be on if they happen to want to Reset to Defaults. //

			// Lets remove the change keycode text.
			// Set all to false as everything is about to be set.
			inventoryKeyCodeChange = false;
			actionKeyCodeChange = false;

			// Assign our defaults here.
			inventoryKeyCode = KeyCode.I;
			actionKeyCode = KeyCode.T;
		}

		public void KeybindsDisplay () {

		}

		public void ChangeInventoryKeyCode () {
			// Display to the user they can change by displaying our changeKeyCodeText.
			// Set this to true but make sure we set the others to false.
			inventoryKeyCodeChange = true;
			actionKeyCodeChange = false;
		}


		public void ChangeActionKeyCode () {
			// Display to the user they can change by displaying our changeKeyCodeText.
			// Set this to true but make sure we set the others to false.
			inventoryKeyCodeChange = false;
			actionKeyCodeChange = true;

		}


		public KeyCode GetInventoryKeyCode () {
			return inventoryKeyCode;
		}

		public KeyCode GetActionKeyCode () {
			return actionKeyCode;
		}


		public void SaveKeybinds () {
			// Create a new Saved_Keybinds.
			Saved_Keybinds data = new Saved_Keybinds ();
			// Save the data.
			data.savedInventoryKeyCode = inventoryKeyCode;
			data.savedActionKeyCode = actionKeyCode;
			// Turn the Saved_Keybinds data to Json data.
			string keybindsToJson = JsonUtility.ToJson (data);
			// Save the information.
			PlayerPrefs.SetString ("Keybinds", keybindsToJson);
		}

		public void LoadKeybinds () {
			// Load the json data.
			string keybindsJson = PlayerPrefs.GetString ("Keybinds");
			// IF we dont have saved information.
			if (String.IsNullOrEmpty (keybindsJson)) {
				// Lets reset to our defaults.
				ResetToDefaults();
				// We leave.
				return;
			}

			// Turn the json data to the data to represent Saved_Keybinds.
			Saved_Keybinds data = JsonUtility.FromJson<Saved_Keybinds> (keybindsJson);
			// Load our KeyBinds.
			SetLoadedKeyBinds (data.savedInventoryKeyCode, inventoryKeyCode);
			SetLoadedKeyBinds (data.savedActionKeyCode, actionKeyCode);

		}

		private void SetLoadedKeyBinds (KeyCode savedKC, KeyCode gameActionKC) {
			// IF our saved Inventory Key Code isn't KeyCode.None.
			if (savedKC != KeyCode.None) {
				// Set our KeyCode AND the string visual.
				gameActionKC = savedKC;
			}
		}

		private bool KeyCodeChosen () {
			foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode))) {
				if (Input.GetKeyDown (kcode)) {
					chosenKeyCode = kcode;
					return true;
				}
			}
			return false;
		}
	}

	[Serializable]
	class Saved_Keybinds {
		public KeyCode savedInventoryKeyCode;
		public KeyCode savedActionKeyCode;
	}
}
