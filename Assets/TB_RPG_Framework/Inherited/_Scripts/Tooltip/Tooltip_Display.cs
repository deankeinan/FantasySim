using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TrollBridge {

	/// <summary>
	/// Tooltip display when we hover our mouse over this GameObject on our action bars.
	/// </summary>
	public class Tooltip_Display : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

		// When there is a item in the action bar this will NOT be null.
		private Item itemBar;

		[Tooltip ("This is a boolean to not only let us know we are dealing with a preset bar but one that represents our current equipped weapon.")]
		[SerializeField] private bool weaponOnly = false;
		[Tooltip ("This is a boolean to not only let us know we are dealing with a preset bar but one that represents our current equipped armour.")]
		[SerializeField] private bool armourOnly = false;
		[Tooltip ("This is a boolean to not only let us know we are dealing with a preset bar but one that represents our current equipped helmet.")]
		[SerializeField] private bool helmetOnly = false;

		/// <summary>
		/// Showing our tooltip.
		/// </summary>
		public void OnPointerEnter(PointerEventData data) {
        

			//// DEALING WITH A PRESET BAR ////

			// IF we are dealing with a weapon,
			// ELSE IF we are dealing with armour,
			// ELSE IF if you want to add more.
			if (weaponOnly) {
				// Get the equipment that is on our player.
				Equipment charEquipment = Character_Helper.GetPlayerManager ().GetComponentInChildren <Character_Manager> ().characterEquipment;
				// IF we don't have a weapon.
				if (charEquipment.GetWeapon () == null) {
					// We leave.
					return;
				}
				// Since we have a weapon we can now display our tooltip.
				Grid_Helper.tooltip.ActivateItemTooltip (charEquipment.GetWeapon (), transform.parent.gameObject);
				// We leave.
				return;

			} else if (armourOnly) {
				// Get the equipment that is on our player.
				Equipment charEquipment = Character_Helper.GetPlayerManager ().GetComponentInChildren <Character_Manager> ().characterEquipment;
				// IF we don't have armour.
				if (charEquipment.GetArmour () == null) {
					// We leave.
					return;
				}
				// Since we have armour we can now display our tooltip.
				Grid_Helper.tooltip.ActivateItemTooltip (charEquipment.GetArmour (), transform.parent.gameObject);
				// We leave.
				return;

			} else if (helmetOnly) {
				// Get the equipment that is on our player.
				Equipment charEquipment = Character_Helper.GetPlayerManager ().GetComponentInChildren <Character_Manager> ().characterEquipment;
				// IF we don't have helmet.
				if (charEquipment.GetHelmet () == null) {
					// We leave.
					return;
				}
				// Since we have a helmet we can now display our tooltip.
				Grid_Helper.tooltip.ActivateItemTooltip (charEquipment.GetHelmet (), transform.parent.gameObject);
				// We leave.
				return;
			}
			//// END OF DEALING WITH A PRESET BAR ////


			// IF we have a tooltip.
			if (Grid_Helper.tooltip != null) {
				// IF there is an item in this action bar,
				// ELSE IF there is a skill in this action bar.
				if (itemBar != null) {
					// Show the tooltip.
					Grid_Helper.tooltip.ActivateItemTooltip (itemBar, transform.parent.gameObject);
				} 
			}
		}

		/// <summary>
		/// Removing our tooltip.
		/// </summary>
		public void OnPointerExit(PointerEventData data) {
			// IF we have a tooltip.
			if(Grid_Helper.tooltip != null) {
				// Do not show the tooltip.
				Grid_Helper.tooltip.DeactivateItemTooltip ();
			}
		}



		/// <summary>
		/// Sets the item for this action bar.
		/// </summary>
		public void SetItem (Item newItem) {
			itemBar = newItem;
		}
	}
}
