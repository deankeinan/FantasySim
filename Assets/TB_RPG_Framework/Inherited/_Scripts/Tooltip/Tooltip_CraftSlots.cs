using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TrollBridge {

	/// <summary>
	/// Tooltip_CraftSlots is a tooltip display driven script that is focused on displaying Item information that are shown in UI's.  The main focus of this script deals with crafting in the demo.
	/// </summary>
	public class Tooltip_CraftSlots : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

		private Item item;
		private GameObject parentGameObject;

		/// <summary>
		/// Sets the item and its index.
		/// </summary>
		public void SetItem (Item newItem, GameObject newParentGameObject) {
			item = newItem;
			parentGameObject = newParentGameObject;
		}

		/// <summary>
		/// Showing our tooltip.
		/// </summary>
		public void OnPointerEnter(PointerEventData data) {
			// IF we have a tooltip.
			if(Grid_Helper.tooltip != null) {
				// Show the tooltip.
				Grid_Helper.tooltip.ActivateItemTooltip (item, parentGameObject);
			}
		}

		/// <summary>
		/// Removing our tooltip.
		/// </summary>
		public void OnPointerExit(PointerEventData data) {
			// IF we have a tooltip.
			if(Grid_Helper.tooltip != null){
				// Do not show the tooltip.
				Grid_Helper.tooltip.DeactivateItemTooltip ();
			}
		}
	}
}
