using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Inventory button method that will display or not display our inventory.
	/// </summary>
	public class Inventory_Button : MonoBehaviour {

		public void OpenCloseInventory(){
			// Open or close the inventory.
			Grid_Helper.inventory.OpenCloseInventory();
		}
	}
}
