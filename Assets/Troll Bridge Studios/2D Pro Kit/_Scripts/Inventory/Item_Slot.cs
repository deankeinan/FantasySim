using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace TrollBridge {

	/// <summary>
	/// Item slot script that is used for the slots of our inventory.
	/// </summary>
	public class Item_Slot : MonoBehaviour, IDropHandler {
		
		[ReadOnlyAttribute]	public int slotNumber;


		public void OnDrop (PointerEventData data) {
			// IF it is a left click AND we we're actually dragging an item AND whatever we are dragging can be in our inventory (Skill icons to be dragged to inventory = NO.  Actionbar icons to be dragged to the inventory = NO).
			if (data.button == PointerEventData.InputButton.Left && Grid_Helper.inventory.GetCurrentlyDraggedItem () != null && Grid_Helper.inventory.GetAllowDragNDrop ()) {
				// Get the Item_Data component from the GameObject that was being dragged and now being dropped via the mouse.
				Item_Data droppedItem = data.pointerDrag.GetComponent<Item_Data> ();

				// IF we are dropping an item on an empty inventory slot,
				// ELSE IF we are dropping an item on a inventory slot that is occupied.
				if (Grid_Helper.inventory.items [slotNumber].ID == -1) {
					// Clear the old slot.
					Grid_Helper.inventory.items [droppedItem.slotNumber] = new Item ();
					// Set the item to the new slot.
					Grid_Helper.inventory.items [slotNumber] = droppedItem.item;
					// Set this slot number to the new slot number.
					droppedItem.slotNumber = slotNumber;
					// Rename to the appropriate slot number based on the slot it was moved to.
					droppedItem.name = "Item Slot " + slotNumber + " - " + Grid_Helper.inventory.items[slotNumber].Title;
				} 
			}
		}
	}
}
