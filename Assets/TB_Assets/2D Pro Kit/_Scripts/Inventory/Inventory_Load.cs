using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	public class Inventory_Load : MonoBehaviour {

		void Awake () {
			// Load the Inventory.
			Grid_Helper.inventory.LoadInventory ();
		}
	}
}
