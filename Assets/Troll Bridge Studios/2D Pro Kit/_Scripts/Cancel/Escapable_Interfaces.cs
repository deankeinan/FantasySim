using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Escapable interfaces allows for a 1 button clear all for removing specific UIs as a quick clear in your game.  Example is inventory and crafting are up at the same time and an enemy comes out of nowhere and is attacking you; rather than
	/// exit out of both you can push 1 key to clear both and fight the enemy attacking you.
	/// </summary>
	public class Escapable_Interfaces : MonoBehaviour {

		[Tooltip ("They key to press to remove all the User Interfaces variable.")]
		[SerializeField] private KeyCode escapableKey;
		[Tooltip ("The gameobjects to be removed when the escapable key is pressed.")]
		[SerializeField] private GameObject[] userInterfaces;

		private List<GameObject> userInterfaceList;


		void Start () {
			userInterfaceList = userInterfaces.ToList ();
		}

		void Update () {
			// IF we hit the key that will remove the UIs.
			if (Input.GetKeyDown (escapableKey)) {
				// Set all the UIs inactive.
				SetNotActive ();
			}
		}

		public bool IsAnyInterfaceActive () {
			// Loop the amount of times we have interfaces we want to set inactive.
			for (int i = 0; i < userInterfaces.Length; i++) {
				// IF this user interface is active.
				if (userInterfaces[i].activeInHierarchy) {
					return true;
				}
			}
			return false;
		}

		public void SetNotActive () {

			// THIS IS WHERE YOU SET PRIORITY OF WHAT PANELS/UI/INTERFACES YOU WANT TO CANCEL IN ORDER WHEN YOU HIT ESCAPE.

			// IF we are closing our dialogue panel,
			// ELSE IF we are closing our multiple interaction panel,
			// ELSE IF we are closing our shop panel,
			// ELSE IF we are closing our quest panel,
			// ELSE IF we are closing our learn crafts panel,
			// ELSE IF we are closing our item split panel,
			// ELSE IF we are closing our inventory panel,
			// ELSE IF we are closing our options panel,
			// ELSE IF we are closing our quest tracker,
			// ELSE IF we are closing our skill book,
			// ELSE Active panel but no code to make it inactive.
			if (userInterfaceList.Contains (Grid_Helper.dialogueData.GetDialoguePanel ()) && Grid_Helper.dialogueData.GetDialoguePanel ().activeInHierarchy) {
				Grid_Helper.dialogueData.DisplayDialoguePanel (false);
				return;

			} else if (userInterfaceList.Contains (Grid_Helper.inventory.GetSplitItemUI ()) && Grid_Helper.inventory.GetSplitItemUI ().activeInHierarchy) {
				Grid_Helper.inventory.GetSplitItemUI ().GetComponent <Item_Split> ().SetActiveness (false);
				return;

			} else if (userInterfaceList.Contains (Grid_Helper.inventory.inventoryPanel) && Grid_Helper.inventory.inventoryPanel.activeInHierarchy) {
				Grid_Helper.inventory.OpenCloseInventory ();
				return;

			} else if (userInterfaceList.Contains (Grid_Helper.optionManager.optionsPanel) && Grid_Helper.optionManager.optionsPanel.activeInHierarchy) {
				Grid_Helper.optionManager.OptionsDisplay (false);
                return;
			}
		}
	}
}
