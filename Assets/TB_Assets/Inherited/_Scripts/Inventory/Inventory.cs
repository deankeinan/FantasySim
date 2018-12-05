using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace TrollBridge {

	/// <summary>
	/// Inventory system.
	/// </summary>
	public class Inventory : MonoBehaviour {

		[Header ("Amount of Slots")]
		[Tooltip("The amount of default inventory slots. You will want this number to be the LOWEST number of slots the player can have.")]
		public int defaultSlotAmount = 10;

		[Header ("Panels")]
		[Tooltip("Optional, The GameObject in which you want to be shown when an item split happens.")]
		[SerializeField] private GameObject splitItemPanel;
		[Tooltip("The panel that holds all of the inventory container elements.")]
		public GameObject inventoryPanel;
		[Tooltip("The panel that holds the container for the inventory slots.")]
		public GameObject slotPanel;

		[Header ("Prefabs")]
		[Tooltip("The GameObject that represents each individual inventory slot.")]
		public GameObject inventorySlot;
		[Tooltip("The GameObject that represents the Item that is in each Inventory Slot.")]
		public GameObject inventoryItem;

		[Header ("Throw Away Power")]
		[Tooltip("The force power that is used to toss items out of the inventory.")]
		public float forcePower = 400f;
		[Tooltip("Radius that handles the distance an object is created away from its creater (The player in this case.)")]
		public float radius = 0.7f;

		[Header ("Audio")]
		[Tooltip("The sound that is played when this script is enabled.")]
		[SerializeField] private AudioClip openInventorySound;
		[Tooltip("The sound that is played when this script is disabled.")]
		[SerializeField] private AudioClip closeInventorySound;

		// THIS IS COMMENTED OUT DUE TO HAVING A KEYBINDS SCRIPT (Keybinds.cs).  IF YOU CHOOSE TO NOT USE THE KEYBINDS AND WANT TO BE ABLE TO SET KEYCODES HERE, THEN UNCOMMENT THIS OUT.
//		[Tooltip("The Key that is used to open or close your inventory.")]
//		public KeyCode inventoryKey;

		public List<Item> items = new List<Item> ();
		[HideInInspector] public List<GameObject> slots = new List<GameObject>();
		private Item currentlyDraggedItem;

		// Saved variable to let us know if we had any extensions
		private int extraInventorySlots = 0;
		// Variable to let us know if we should not permit any Drag N Drop events in our inventory.
		private bool allowDragNDrop = true;

		// Our boolean to let us know if we are hovering over an iventory item.
		private bool isInventoryHovering = false;


		void Update() {
			// IF the inventory key is pushed AND our Canvas is active (meaning we only care about opening the inventory when the player is active) AND our options window is NOT active (open).
			if (Input.GetKeyDown (Grid_Helper.kBinds.GetInventoryKeyCode()) && Grid_Helper.setup.UICanvas.activeInHierarchy && !Grid_Helper.optionManager.optionsPanel.activeInHierarchy) {
				// Open or Close the inventory.
				OpenCloseInventory ();
			}

			// IF YOU DON'T WANT TO USE THE KEYBINDS SCRIPT THEN COMMENT OUT WHAT IS ABOVE THIS AND UNCOMMENT WHAT IS BELOW HERE.  SINCE WE HAVE A KEYBINDS SCRIPT IT MAKES NO SENSE TO USE THE BELOW CODE.

//			// IF the inventory key is pushed AND our Canvas is active (meaning we only care about opening the inventory when the player is active) AND our keybinds window is NOT active (open).
//			if (Input.GetKeyDown (inventoryKey) && Grid.setup.UICanvas.activeInHierarchy && !Grid.kBinds.GetKeyBindsWindow().activeInHierarchy) {
//				// Open or Close the inventory.
//				OpenCloseInventory ();
//			}
//			// IF we have a keycode that has gotten changed from our keybinds.
//			if (inventoryKey != Grid.kBinds.GetInventoryKeyCode ()) {
//				// Lets make sure we are always setting our KeyCode to open and close the inventory based on our Keybinds script.
//				inventoryKey = Grid.kBinds.GetInventoryKeyCode ();
//			}
		}

		public void SetInventoryHover (bool isHover) {
			isInventoryHovering = isHover;
		}

		/// <summary>
		/// Uses an item from the inventory.
		/// </summary>
		public void UseItem (Item usedItem) {
			// IF we have a Consumable,
			// ELSE IF we have Equipment.
			if (usedItem.Type == "Consumable") {
				// Use our consumable.
				UseConsumable (usedItem);
			} else if (usedItem.Type == "Weapon" || usedItem.Type == "Armour" || usedItem.Type == "Ring" || usedItem.Type == "Bracelet" || usedItem.Type == "Helmet") {
				// Use our Equipment.
				UseEquipment (usedItem);
			}
		}

		/// <summary>
		/// Uses the consumable.  Since our type is a consumable we will use it and reduce it by 1 but if its amount is 0 after usage we remove it from the inventory.
		/// </summary>
		public void UseConsumable (Item usedItem) {
			// Get the Character_Manager component.
			Character_Manager character = Character_Helper.GetPlayerManager ().GetComponent<Character_Manager> ();

			// Loop the amount of inventory spaces we have.
			for (int i = 0; i < defaultSlotAmount + extraInventorySlots; i++) {
				// IF the IDs match.
				if (items [i].ID == usedItem.ID) {
					// Play the Pickup Sound.
					Grid_Helper.soundManager.PlaySound (usedItem.UsedSound);
					// Add "usage" attributes associated with this item.  At this current time if you are looking at the demo we only add HP.
					character.GetComponentInChildren<Character_Stats> ().AddHealth ((float)usedItem.RestoreHP);

					// Remove the item from our inventory.
					RemoveItemFromInventory (usedItem.ID, 1);
					// Do not show the tooltip.
					Grid_Helper.tooltip.DeactivateItemTooltip ();
					// Leave this loop because we can only use 1 item at a time so we are done.
					return;
				}
			}
		}

		/// <summary>
		/// Equips/Swaps the equipment.  This will check the types of equipment to send that Item and its stat to the player.
		/// </summary>
		public void UseEquipment (Item usedItem) {
			// Get the Character_Manager component.
			Character_Manager character = Character_Helper.GetPlayerManager ().GetComponent<Character_Manager> ();

			// Get the Equipment component.
			Equipment equipment = character.GetComponentInChildren<Equipment> ();
			// Create a temp variable for our item that is swapped out by whatever we are currently equipping.
			Item swappedItem = null;
			// IF we have a weapon item,
			// ELSE IF we have a armour item,
			// ELSE IF we have a ring item,
			// ELSE IF we have a bracelet item,
			// ELSE we dont have an item to equip and we made an error somewhere as we should of not been in here.  One of the IF statements SHOULD work.
			if (usedItem.Type == "Weapon") {
				// Set the new weapon to the player while returning the old weapon (if the player wielded one).
				swappedItem = equipment.EquipWeapon (usedItem);
			} else if (usedItem.Type == "Armour") {
				// Set the new Armour to the player while returning the old Armour (if the player wearing one).
				swappedItem = equipment.EquipArmour (usedItem);
			} else if (usedItem.Type == "Ring") {
				// Set the new Ring to the player while returning the old Ring (if the player wearing one).
				swappedItem = equipment.EquipRing (usedItem);
			} else if (usedItem.Type == "Bracelet") {
				// Set the new Bracelet to the player while returning the old Bracelet (if the player wearing one).
				swappedItem = equipment.EquipBracelet (usedItem);
			} else {
				// Nothing we can equip so lets leave.
				Debug.Log ("We entered the UseEquipment method but didnt equip anything.  Something isnt right with the labeling of the Types.");
				return;
			}
				
			// Loop the amount of times we have inventory spaces.
			for (int i = 0; i < defaultSlotAmount + extraInventorySlots; i++) {
				// IF the IDs match.
				if (items [i].ID == usedItem.ID) {

					// IF we have an item being swapped out.
					if (swappedItem != null) {
						// Add the swapped out equipped item to the inventory.
						AddItem (swappedItem.ID, 1);
					}
					// Play the Pickup Sound.
					Grid_Helper.soundManager.PlaySound (usedItem.UsedSound);
					// Do not show the tooltip.
					Grid_Helper.tooltip.DeactivateItemTooltip ();
					// Clear the inventory slot.
					ClearSlotInInventory (i, slots [i].GetComponentInChildren <Item_Data> ().gameObject);
					// Since we found a match lets get out of this loop.
					break;
				}
			}
		}

		/// <summary>
		/// Is this item currently in our inventory.
		/// </summary>
		public bool IsItemInTheInventory (int id) {
			// IF the item list contains the ID we are passing through.
			if (items.Contains (Grid_Helper.itemDataBase.FetchItemByID (id))) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Adds the stackable item in the inventory.
		/// </summary>
		public void AddItem (int id, int amount) {
			// Get the item based on the id.
			Item itemToAdd = Grid_Helper.itemDataBase.FetchItemByID (id);

			// IF our items list contains itemToAdd AND this item is stackable.
			if (items.Contains (itemToAdd) && itemToAdd.Stackable) {
				// Add stackable item.
				AddStackableItemInInventory (id, amount);
				// We are done lets leave.
				return;
			}

			// Add the item to the inventory.
			AddItemToInventory (itemToAdd, amount);
		}

		/// <summary>
		/// Adds the stackable item in the inventory.
		/// </summary>
		public void AddStackableItemInInventory (int _id, int _amount) {
			// Loop though the items list.
			for (int i = 0; i < items.Count; i++) {
				// IF we already have this item in our inventory.
				if (items [i].ID == _id) {
					// Get the Item_Data component.
					Item_Data data = slots [i].GetComponentInChildren<Item_Data> ();
					// Add the amount we picked up to what we already have.
					data.amount += _amount;
					// GTFO.
					return;
				}
			}
		}

		/// <summary>
		/// Adds the item to inventory.
		/// </summary>
		public void AddItemToInventory (Item itemToAdd, int _amount) {
			// How ever many times we need to add this item.
			for (int j = 0; j < _amount; j++) {
				// Loop through all the items.
				for (int i = 0; i < items.Count; i++) {
					// IF this item slot is empty.
					if (items [i].ID == -1) {
						// Set the item we have been given to this spot in the items list.
						items [i] = itemToAdd;
						// Create the GameObject.
						GameObject itemObj = Instantiate (inventoryItem);
						// Get the Item_Data Component.
						Item_Data idComp = itemObj.GetComponent<Item_Data> ();
						// Set the item in the idComp.
						idComp.item = itemToAdd;
						// Set the slot number of this item.
						idComp.slotNumber = i;
						// Set the transform of itemObj.
						itemObj.transform.SetParent (slots [i].transform);
						itemObj.transform.localScale = Vector2.one;
						itemObj.transform.localPosition = Vector2.zero;
						// Adjust the sprite and color of the Image.
						itemObj.GetComponent<UnityEngine.UI.Image> ().sprite = itemToAdd.SpriteImage;
						itemObj.GetComponent<UnityEngine.UI.Image> ().color = new Color (itemToAdd.R, itemToAdd.G, itemToAdd.B, itemToAdd.A);
						// Change the name of the itemObj GameObject to the name of the item.
						itemObj.name = "Item Slot " + i + " - " + itemToAdd.Title;
						// IF this item is stackable.
						if (itemToAdd.Stackable) {
							// Set the amount for the idComp.
							idComp.amount = _amount;
							return;
						}
						// If we come here then we have an item that isn't stackable so we just set the amount to 1.
						idComp.amount = 1;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Process where we remove an item from the inventory based on the itemID and the itemAmountToRemove.
		/// </summary>
		public void RemoveItemFromInventory (int itemID, int itemAmountToRemove) {
			// Tally up the amount we have in our inventory.
			int itemTotalAmount = TotalAmountOfItemInInventory (itemID);
			// IF we are trying to remove more than what we have OR we are removing 0.
			if (itemAmountToRemove > itemTotalAmount || itemAmountToRemove == 0) {
				// We leave as we did something wrong.  You should never remove something you do not have.
				Debug.Log ("Removing more than what you have or removing 0!  Check itemAmountToRemove to find out why this number is larger than what is in our inventory.");
				return;
			}
				
			// Remove the item in a inventory slot.
			RemoveInventorySlotItem (itemID, itemAmountToRemove);
		}

		/// <summary>
		/// Removes the inventory from the inventory based on the itemAmountToRemove.
		/// </summary>
		private void RemoveInventorySlotItem (int itemID, int itemAmountToRemove) {
			// Loop the amount of slots we have.
			for (int i = 0; i < slots.Count; i++) {
				// IF this item has the same ID as our itemID.
				if (items [i].ID == itemID) {
					// Get the Item_Data component.
					Item_Data data = slots [i].GetComponentInChildren<Item_Data> ();
					// IF itemAmountToRemove is equal to the amount of the item we have in our inventory,
					// ELSE IF itemAmountToRemove is greater than the amount of the item we have in our inventory,
					// ELSE itemAmountToRemove is less than the amount of the item we have in our inventory.
					if (itemAmountToRemove == data.amount) {
						// Clear the inventory slot.
						ClearSlotInInventory (i, data.gameObject);
						// itemAmountToRemove is 0 so we leave.
						return;
					} else if (itemAmountToRemove > data.amount) {
						// Clear the inventory slot.
						ClearSlotInInventory (i, data.gameObject);
						// Deduct the amount.
						itemAmountToRemove -= data.amount;
					} else {
						// Remove the remaining amount.
						data.amount -= itemAmountToRemove;
						// itemAmountToRemove is 0 so we leave.
						return;
					}
				}
			}
		}

		/// <summary>
		/// Returns the amount of an item in the inventory.
		/// </summary>
		public int TotalAmountOfItemInInventory (int itemID) {
			// New Counter.
			int counter = 0;
			// Loop through all the item slots.
			for (int i = 0; i < items.Count; i++) {
				// IF this item slot contains the same item id as itemID.
				if (items[i].ID == itemID) {
					// Get the Item_Data component.
					Item_Data data = slots [i].GetComponentInChildren<Item_Data> ();
					// IF there isnt a Item_Data component.
					if (data == null) {
						// Go to the next iteration.
						continue;
					}
					// Add to our counter.
					counter += data.amount;
				}
			}
			// Return what we have tallied up.
			return counter;
		}

		/// <summary>
		/// Returns the amount of free spots in your inventory.
		/// </summary>
		public int GetFreeSpots() {
			// New counter.
			int count = 0;
			// Loop through all the items.
			for (int i = 0; i < items.Count; i++) {
				// IF this item slot is empty.
				if (items [i].ID == -1) {
					// Increase the counter.
					count++;
				}
			}
			// Return the counter.
			return count;
		}

		/// <summary>
		/// Gets the split item UI.
		/// </summary>
		public GameObject GetSplitItemUI () {
			return splitItemPanel;
		}

		/// <summary>
		/// Gets the currently dragged item.
		/// </summary>
		public Item GetCurrentlyDraggedItem () {
			return currentlyDraggedItem;
		}

		/// <summary>
		/// Sets the currently dragged item.
		/// </summary>
		public void SetCurrentlyDraggedItem (Item newDraggedItem) {
			currentlyDraggedItem = newDraggedItem;
		}

		/// <summary>
		/// Determines whether this instance can drag N drop based on our "allowDragNDrop" variable.
		/// </summary>
		public bool GetAllowDragNDrop () {
			return allowDragNDrop;
		}

		/// <summary>
		/// Sets the allow drag N drop variable.
		/// </summary>
		public void SetAllowDragNDrop (bool newDragNDrop) {
			allowDragNDrop = newDragNDrop;
		}

		/// <summary>
		/// Opens and close inventory.
		/// </summary>
		public void OpenCloseInventory() {
			// IF we are opening the inventory,
			// ELSE we are closing the inventory.
			if (!inventoryPanel.activeInHierarchy) {
				Grid_Helper.soundManager.PlaySound (openInventorySound);
			} else {
				// IF we closed our inventory while hovering over an item.
				if (isInventoryHovering) {
					// Change our cursor back to default/
					Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
				}
				Grid_Helper.soundManager.PlaySound (closeInventorySound);
			}
			// Turn the inventory panel to the opposite activeness.
			inventoryPanel.SetActive (!inventoryPanel.activeInHierarchy);
		}

		/// <summary>
		/// Tosses the item out of inventory.
		/// </summary>
		public void TossItemOutOfInventory (string itemTitle, int slotNumber, int amount) {
			// Spawn the item from it being thrown out of the inventory.
			GameObject goItem = Grid_Helper.helper.SpawnObject (Grid_Helper.setup.GetGameObjectPrefab (itemTitle), Character_Helper.GetPlayer ().transform.position, Quaternion.identity, Character_Helper.GetPlayer (), radius);
			// Store the amount that was tossed out.
			goItem.GetComponent<Item_GameObject> ().amount = amount;
			// Launch the item in a random direction.
			Grid_Helper.helper.LaunchAwayFromPosition (goItem, Character_Helper.GetPlayer ().transform.position, forcePower);
		}
			
		/// <summary>
		/// This will create the amount of GameObject slots.
		/// </summary>
		private void CreateSlots(){
			// Loop the amount of slots.
			for(int i = 0; i < defaultSlotAmount + extraInventorySlots; i++) {
				// Add a -1 Item.
				items.Add (new Item());
				// Create the Slot.
				slots.Add (Instantiate(inventorySlot));
				// Assign the Slot a slot number.
				slots [i].GetComponent<Item_Slot> ().slotNumber = i;
				// Set the Slot parent to the Slot Panel.
				slots [i].transform.SetParent (slotPanel.transform);
				// Set the scaling to 1.
				slots [i].transform.localScale = Vector2.one;
			}
		}

		/// <summary>
		/// The amount of inventory slots to add.  The parameter 'amount' will alter the extraInventorySlots variable but it wont set it to the amount number.  
		/// Example if you have 5 extra inventory slots and you use this method with the parameter of 3, you will then have 8 extra inventory slots.
		/// </summary>
		public void AddExtraSlots(int amount){
			// Loop the amount of times we want to add a slot.
			for(int i = 0; i < amount; i++){
				// Add a -1 Item.
				items.Add (new Item());
				// Create the Slot.
				slots.Add (Instantiate(inventorySlot));
				// Assign the Slot a slot number by taking the default and adding how many extra we have.
				slots [defaultSlotAmount + extraInventorySlots].GetComponent<Item_Slot> ().slotNumber = defaultSlotAmount + extraInventorySlots;
				// Set the Slot parent to the Slot Panel.
				slots [defaultSlotAmount + extraInventorySlots].transform.SetParent (slotPanel.transform);
				// Set the scaling to 1.
				slots [defaultSlotAmount + extraInventorySlots].transform.localScale = Vector2.one;
				// Increase the extraInventorySlots.
				extraInventorySlots++;
			}
		}

		/// <summary>
		/// Loads the inventory.
		/// </summary>
		public void LoadInventory(){
			// So lets remove all the stuff in the inventory and make it a fresh start.
			DestroyInventory();
			// Time to load it up.
			Load();
		}

		/// <summary>
		/// Destroys the inventory.
		/// </summary>
		private void DestroyInventory(){
			// Destroy each slot.
			Grid_Helper.helper.DestroyGameObjectsByParent (slotPanel);
			// Make our slot and inventory Lists resort to their default.
			items = new List<Item> ();
			slots = new List<GameObject> ();
		}

		/// <summary>
		/// Clears the inventory.
		/// </summary>
		public void ClearInventory(){
			// loop through the amount of slots.
			for(int i = 0; i < slots.Count; i++){
				// Destroy each child in the slots.
				Grid_Helper.helper.DestroyGameObjectsByParent (slots[i]);
				// Set each slot to an empty item.
				items[i] = new Item();
			}
		}

		/// <summary>
		/// Clears a slot in the inventory list where "index" is located.
		/// </summary>
		public void ClearSlotInInventory (int index, GameObject slotChild) {
			// Clear the old slot.
			items [index] = new Item ();
			// Destroy this item from our inventory.
			Destroy (slotChild);
		}

		/// <summary>
		/// Prints the inventory items.
		/// </summary>
		public void PrintInventoryItems(){
			// Loop and print each title in the item.
			for(int i = 0; i < slots.Count; i++){
				Debug.Log ("Item " + i + " = " + items[i].Title);
			}
		}

		/// <summary>
		/// Save this instance.
		/// </summary>
		public void Save () {
			// Create a new Saved_Inventory.
			Saved_Inventory data = new Saved_Inventory ();
			// Save the data.
			for (int i = 0; i < defaultSlotAmount + extraInventorySlots; i++) {
				// Add the slot id.
				data.slotID.Add (items [i].ID);
				// IF in the items List we have an ID that isnt -1 so that means we have an actual item.
				// ELSE we have no item.
				if (items [i].ID != -1) {
					// Store the amount of this item.
					data.slotAmounts.Add (slots [i].GetComponentInChildren<Item_Data> ().amount);
				} else {
					// There isn't an item here so add 0.
					data.slotAmounts.Add (0);
				}
			}
			// Save the extra inventory slots.
			data.extraInventorySlots = extraInventorySlots;
			// Turn the Saved_Inventory data to Json data.
			string inventoryToJson = JsonHelper.ToJson (data);
			// Save the information.
			PlayerPrefs.SetString ("Inventory", inventoryToJson);
		}

		/// <summary>
		/// Load this instance.
		/// </summary>
		private void Load () {
			// Grab the information on what is in the inventory.
			string inventoryJson = PlayerPrefs.GetString ("Inventory");
			// IF there is nothing in this string.
			if (String.IsNullOrEmpty (inventoryJson)) {
				// Create the slots on a default level.
				CreateSlots ();
				// GTFO of here we done son!
				return;
			}
			// Turn the json data to the data to represent Saved_Inventory.
			Saved_Inventory data = JsonHelper.FromJson<Saved_Inventory> (inventoryJson);
			// Set our extra inventory slots before we create our inventory.
			extraInventorySlots = data.extraInventorySlots;
			// Create the default slots.
			CreateSlots ();

			// Loop through however many inventory slots we have.
			for (int i = 0; i < defaultSlotAmount + extraInventorySlots; i++) {
				// Fetch the item based on the saved ID for this slot.
				Item fetchedItem = Grid_Helper.itemDataBase.FetchItemByID (data.slotID [i]);
				// IF we have an actual item.
				if (fetchedItem != null) {
					// Assign the item to the slot.
					items [i] = fetchedItem;
					// Create the item.
					GameObject itemObj = Instantiate (inventoryItem);
					// Get the Item_Data component.
					Item_Data iData = itemObj.GetComponentInChildren<Item_Data> ();
					// Set the Item_Data item and slot number.
					iData.item = fetchedItem;
					iData.slotNumber = i;
					// Set the transform.
					itemObj.transform.SetParent (slots [i].transform);
					itemObj.transform.localScale = Vector2.one;
					itemObj.transform.localPosition = Vector2.zero;
					// Set the itemObj sprite, sprite color and name of the image.
					itemObj.GetComponent<UnityEngine.UI.Image> ().sprite = fetchedItem.SpriteImage;
					itemObj.GetComponent<UnityEngine.UI.Image> ().color = new Color (fetchedItem.R, fetchedItem.G, fetchedItem.B, fetchedItem.A);
					itemObj.name = "Item Slot " + i + " - " + fetchedItem.Title;

					// Set the Item_Data amount to what we had saved.
					iData.amount = data.slotAmounts [i];
					// Set the text to display how much we have.
					iData.GetComponentInChildren<Text> ().text = iData.amount.ToString ();
				} 
			}
		}
	}

	[Serializable]
	class Saved_Inventory {
		public List<int> slotID = new List<int>();
		public List<int> slotAmounts = new List<int>();
		public int extraInventorySlots = 0;
	}
}
