using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Tooltip system that we use for displaying certain types of tooltips.
	/// </summary>
	public class Tooltip : MonoBehaviour {

		// The tooltip gameobject that will have text on it.
		public GameObject itemTooltip;

		// A placeholder for our parent GameObject of the UIs that display and create the Tooltip.
		private GameObject parentTooltipGameObject;


		void Start () {
			// Make sure all tooltips are off when it starts.
			DeactivateItemTooltip ();
		}

		void Update () {
			// IF we do not have a parentTooltipGameObject.
			if (parentTooltipGameObject == null) {
				// Remove any tooltips.
				DeactivateItemTooltip ();
				// Leave.
				return;
			}

			// IF we do not have the parent of the UI that created and displayed our tooltip active 
			// then we can assume that window was closed so we need to close the tooltip as well.
			if (!parentTooltipGameObject.activeInHierarchy) {
				// Remove any tooltips.
				DeactivateItemTooltip ();
				// We no longer have a parent GameObject that is active.
				parentTooltipGameObject = null;
				// Leave.
				return;
			}
			// IF the Item tooltip is currently active,
			// ELSE the Skill tooltip is currently active.
			if (itemTooltip.activeInHierarchy) {
				// Move the tooltip to where the mouse is.
				itemTooltip.transform.position = Input.mousePosition;
			} 
		}

		/// <summary>
		/// Activates the tooltip with Item information.
		/// </summary>
		public void ActivateItemTooltip (Item _item, GameObject parentGameObject) {
			// Build the item tooltip string.
			ConstructItemTooltip (_item);
			// Set the parentTooltipGameObject.
			parentTooltipGameObject = parentGameObject;
			// Set the tooltip active.
			itemTooltip.SetActive (true);
		}
		/// <summary>
		/// Deactivates the item tooltip.
		/// </summary>
		public void DeactivateItemTooltip () {
			// Set the tooltip inactive (off).
			itemTooltip.SetActive (false);
			// No parent tooltip.
			parentTooltipGameObject = null;
		}


		/// <summary>
		/// This is where you construct your tooltip to display all the stats of your items.  You can alter text color from the example you see below. Be sure to look at what else Unity allows you to do.
		/// </summary>
		private void ConstructItemTooltip (Item item) {
			// Create an empty string.
			string data = "";
			// IF we have a Common rarity Item,
			// ELSE IF we have a Rare rarity Item,
			// ELSE IF we have a Legendary rarity Item.
			if (item.Rarity == "Common") {
				data = "<color=#FFFFFFFF><b>" + item.Title + "</b></color>\n\n";
			} else if (item.Rarity == "Rare") {
				data = "<color=#00FFFFFF><b>" + item.Title + "</b></color>\n\n";
			} else if (item.Rarity == "Legendary") {
				data = "<color=#FFB800FF><b>" + item.Title + "</b></color>\n\n";
			}
			// Check to see if there are any stats on this item.
			if (item.Damage != 0) {
				string stat = "<color=#FFFFFFFF>Damage : +" + item.Damage + "</color>\n";
				data = string.Concat (data, stat);
			}
			if (item.Armour != 0) {
				string stat = "<color=#FFFFFFFF>Armour : +" + item.Armour + "</color>\n";
				data = string.Concat (data, stat);
			}
			if (item.MagicArmour != 0) {
				string stat = "<color=#FFFFFFFF>Magic Armour : +" + item.MagicArmour + "</color>\n";
				data = string.Concat (data, stat);
			}
			if (item.MoveSpeed != 0) {
				string stat = "<color=#FFFFFFFF>Movement Speed : +" + item.MoveSpeed + "</color>\n";
				data = string.Concat (data, stat);
			}
			if (item.Health != 0) {
				string stat = "<color=#FFFFFFFF>Health : +" + item.Health + "</color>\n";
				data = string.Concat (data, stat);
			}
			if (item.Mana != 0) {
				string stat = "<color=#FFFFFFFF>Mana : +" + item.Mana + "</color>\n";
				data = string.Concat (data, stat);
			}
			// Display the item description.
			data = string.Concat (data, "\n" + item.Description);
			itemTooltip.transform.GetChild (0).GetComponent<Text> ().text = data;
		}
	}
}
