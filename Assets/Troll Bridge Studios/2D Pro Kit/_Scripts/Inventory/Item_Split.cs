using UnityEngine;
using UnityEngine.UI;

namespace TrollBridge {

	/// <summary>
	/// Item split system allows us to split up Items in our game.
	/// </summary>
	public class Item_Split : MonoBehaviour {

		[SerializeField] private Text splitAmountText;

		private Item item;
		private Item_Data itemData;
		private int slotNumberBeingSplit;
		private int maxStackAmount;
		private int currentStackAmount;

		void OnEnable () {
			// Set our current Stack Amount to 1.
			currentStackAmount = 1;
			// Lets make sure that when this is enabled it will display 1.
			splitAmountText.text = currentStackAmount.ToString ();
		}

		void Update (){
			splitAmountText.text = currentStackAmount.ToString ();
		}

		public void SetItemToBeSplit (Item _item) {
			item = _item;
		}

		public void SetMaxStackAmount (int _maxStackAmount) {
			maxStackAmount = _maxStackAmount;
		}

		public void SetSlotNumberBeingSplit (int _slotNumberBeingSplit) {
			slotNumberBeingSplit = _slotNumberBeingSplit;
		}

		public void SetItemData (Item_Data _itemData) {
			itemData = _itemData;
		}

		public void IncreaseStackSplit () {
			// IF we are increasing and have not reached the max yet.
			if(currentStackAmount + 1 < maxStackAmount){
				// Increase our current stack amount.
				currentStackAmount++;
			}
		}

		public void DecreaseStackSplit () {
			// IF our current Stack Amount is greater than 1.
			if(currentStackAmount > 1){
				// Decrease our current stack amount.
				currentStackAmount--;
			}
		}

		/// <summary>
		/// Splits the item.  In the default case of this kit we have 2 options, The inventory is full or the inventory has a free space.
		/// Since the default of the kit is drag N drop and not solo Click somewhere else to place an item we either fill in the spot 
		/// where its empty with the split item or if the inventory is full we just toss it out of the inventory.
		/// </summary>
		public void SplitItem () {
			// IF there is a free spot in the inventory,
			// ELSE there is not a free spot in the inventory.
			if (Grid_Helper.inventory.GetFreeSpots () > 0) {
				// Add the item to the inventory with not stacking.
				Grid_Helper.inventory.AddItemToInventory (item, currentStackAmount);
			} else {
				// Toss the item out of the inventory.
				Grid_Helper.inventory.TossItemOutOfInventory (item.Title, slotNumberBeingSplit, currentStackAmount);
			}
			// Reduce the amount of the stack by the currentStackAmount.
			itemData.amount -= currentStackAmount;
			// Remove the Split window.
			SetActiveness (false);
		}

		/// <summary>
		/// Sets this script active or inactive.
		/// </summary>
		public void SetActiveness (bool isActive) {
			gameObject.SetActive (isActive);
		}
	}
}
