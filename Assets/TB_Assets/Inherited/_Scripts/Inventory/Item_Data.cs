using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Item data is our information of what is in our Item_Slot.
	/// </summary>
	public class Item_Data : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
		
		[ReadOnlyAttribute]	public Item item;
		[ReadOnlyAttribute]	public int amount;
		[ReadOnlyAttribute]	public int slotNumber;

		private CanvasGroup canvasGroup;
		private Text amountText;


		void Awake() {
			canvasGroup = GetComponent<CanvasGroup> ();
		}

		void Start () {
			amountText = GetComponentInChildren<Text> ();
		}

		void Update () {
			// IF there is only 1 of this Item,
			// ELSE there are more than 1 of this Item. 
			if (amount == 1) {
				amountText.text = "";
			} else {
				amountText.text = amount.ToString ();
			}
		}

		/// <summary>
		/// Showing our tooltip.
		/// </summary>
		public void OnPointerEnter(PointerEventData data) {
			// Set a variable to let us know we are hovering over an item in our inventory.
			Grid_Helper.inventory.SetInventoryHover (true);
			// IF we have a tooltip.
			if(Grid_Helper.tooltip != null){
				// Show the tooltip.
				Grid_Helper.tooltip.ActivateItemTooltip (item, Grid_Helper.inventory.inventoryPanel);
			}
		}

		/// <summary>
		/// Removing our tooltip.
		/// </summary>
		public void OnPointerExit(PointerEventData data) {
			// Set a variable to let us know we are hovering over an item in our inventory.
			Grid_Helper.inventory.SetInventoryHover (false);
			// IF we have a tooltip.
			if (Grid_Helper.tooltip != null) {
				// Do not show the tooltip.
				Grid_Helper.tooltip.DeactivateItemTooltip ();
			}
		}

		/// <summary>
		/// Split an item.
		/// </summary>
		public void OnPointerDown(PointerEventData data) {
			// Splitting up a stackable Item.
			// IF we are focused on an actual item AND we are holding Shift AND Left OR Right Clicking AND we are not dragging AND the item is a stackable item AND there is more than 1 of the item that is stacked to split items in our inventory.
			if (item != null && (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) && (data.button == PointerEventData.InputButton.Right || data.button == PointerEventData.InputButton.Left)
			     && !data.dragging && item.Stackable && amount > 1) {

				// Item_Split script reference.
				Item_Split itemSplit = Grid_Helper.inventory.GetSplitItemUI ().GetComponent<Item_Split> ();
				// Lets send over the stack amount of this item.
				itemSplit.SetMaxStackAmount (amount);
				// Lets send over the item that is being split.
				itemSplit.SetItemToBeSplit (item);
				// Lets send over the slot number of the item being split.
				itemSplit.SetSlotNumberBeingSplit (slotNumber);
				// Lets send over the Item_Data.
				itemSplit.SetItemData (this);
				// Set it to where the mouse cursor is.
				Grid_Helper.inventory.GetSplitItemUI ().transform.position = Input.mousePosition;
				// Display the tooltip.
				Grid_Helper.inventory.GetSplitItemUI ().SetActive (true);
			}
		}

		/// <summary>
		/// Sell an item to a shop.
		/// </summary>
		public void OnPointerUp(PointerEventData data) {
			// IF we Right Click AND Not hold any of the shift keys AND not dragging so we can USE or EQUIP an item.... So basically we just want to detect JUST a right click
			if (item != null && data.button == PointerEventData.InputButton.Right && !data.dragging && !(Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))) {
				Grid_Helper.inventory.UseItem (item);
			}
		}

		public void OnBeginDrag(PointerEventData data) {
			// IF there is an item.
			if(item != null && data.button == PointerEventData.InputButton.Left) {
				Grid_Helper.inventory.SetCurrentlyDraggedItem (item);
				transform.SetParent (transform.parent.parent);
				transform.position = data.position;
				canvasGroup.blocksRaycasts = false;
			}
		}

		public void OnDrag(PointerEventData data) {
			// IF there is an item.
			if(item != null && data.button == PointerEventData.InputButton.Left) {
				// Set the position where the mouse is.
				transform.position = data.position;
			}
		}

		public void OnEndDrag(PointerEventData data) {
			// IF there is a left click being released.
			if (data.button == PointerEventData.InputButton.Left) {
				// IF we are NOT hovering the mouse over the inventory then drop the item on the ground,
				// ELSE IF we are actually dragging an item.
				if (data.hovered.Count == 0) {
					// Toss the item out of the inventory.
					Grid_Helper.inventory.TossItemOutOfInventory (item.Title, slotNumber, amount);
					// Clear the old slot.
					Grid_Helper.inventory.items [slotNumber] = new Item ();
					// Destroy this gameobject.
					Destroy (gameObject);

				} else if (item != null) {
					// Set this items parent back to the original slot.
					transform.SetParent (Grid_Helper.inventory.slots [slotNumber].transform);
					// Set its local position back to 0 so it can be centered in the slot.
					transform.localPosition = Vector2.zero;
					// Make sure the scale is set properly to 1.
					transform.localScale = Vector2.one;
					// Allow us to block raycast again.
					canvasGroup.blocksRaycasts = true;
				}
				// We are ending our dragging so the currently dragged item is null (nothing).
				Grid_Helper.inventory.SetCurrentlyDraggedItem (null);
			}
		}

		public void OnDrop (PointerEventData data) {
			// IF it is a left click AND 
			// IF we we're actually dragging an item AND 
			// IF whatever we are dragging can be in our inventory (Skill icons to be dragged to inventory = NO.  
			// actionbar icons to be dragged to the inventory = NO).
			if (data.button == PointerEventData.InputButton.Left && 
				Grid_Helper.inventory.GetCurrentlyDraggedItem () != null && 
				Grid_Helper.inventory.GetAllowDragNDrop ()) {

				// Get the Item_Data component from the GameObject that was being dragged and now being dropped via the mouse.
				Item_Data droppedItem = data.pointerDrag.GetComponent<Item_Data> ();

				// IF we are dropping this item on the same item in the inventory SO WE STACK THEM,
				// ELSE we are dropping this item on a different type of item other than itself SO WE SWAP THEM.
				if (droppedItem.item.ID == item.ID) {
					// IF the item is stackable,
					if (item.Stackable) {
						// Now we want to add the amount we are dragging to our item in this slot.
						amount += droppedItem.amount;
						// Clear the old slot from where we originally started dragging.
						Grid_Helper.inventory.items [droppedItem.slotNumber] = new Item ();
						// Destroy the Item Data.
						Destroy (droppedItem.gameObject);
					}
				} else {
					// Placeholder for this Item_Data's slot number.
					int tempSlotNumber = slotNumber;
					// Set the slot number of the item in this current slot to be set to the slot of the dragged item.
					slotNumber = droppedItem.slotNumber;
					// Set the parent of this item to the slot number of the dragged item.
					transform.SetParent (Grid_Helper.inventory.slots [droppedItem.slotNumber].transform);
					// Set the scale to 1.
					transform.localScale = Vector2.one;
					// Set the position of the item to where the dragged item was.
					transform.position = Grid_Helper.inventory.slots [droppedItem.slotNumber].transform.position;

					// Set the dragged items slot number to our tempSlotNumber.
					droppedItem.slotNumber = tempSlotNumber;
					// Set the parent of the dragged item to the transform of this gameobject.
					droppedItem.transform.SetParent (Grid_Helper.inventory.slots [tempSlotNumber].transform);
					// Set the scale to 1.
					droppedItem.transform.localScale = Vector2.one;
					// Set the position of the dragged item to the position of this gameobject.
					droppedItem.transform.position = Grid_Helper.inventory.slots [tempSlotNumber].transform.position;

					// Swap the items in the inventory array.
					Grid_Helper.inventory.items [droppedItem.slotNumber] = droppedItem.item;
					Grid_Helper.inventory.items [slotNumber] = item;

					// Change the text of the Item_Data GameObject.
					gameObject.name = "Item Slot " + slotNumber + " - " + item.Title;
					droppedItem.name = "Item Slot " + droppedItem.slotNumber + " - " + Grid_Helper.inventory.items [droppedItem.slotNumber].Title;

//					Grid_Helper.inventory.PrintInventoryItems ();
				}
			}
		}
	}
}
