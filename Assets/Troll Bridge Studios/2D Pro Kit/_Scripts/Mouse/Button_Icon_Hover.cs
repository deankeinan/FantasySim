using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Button icon hover script that changes the mouse icon when it enters its section.
	/// </summary>
	public class Button_Icon_Hover : MonoBehaviour {

		[SerializeField] private Texture2D cursor;


		public void OnMouseHoverEnter () {
			// Change our cursor to look different.
			Cursor.SetCursor (cursor, new Vector2 (cursor.width / 2, cursor.height / 2), CursorMode.Auto);
		}

		public void OnMouseHoverExit () {
			// Change our cursor back to default/
			Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		}

		//public void OnMouseHoverShop () {
		//	// IF we are hovering over this WHILE we have a shop UI open.
		//	//if (Grid_Helper.shopData.isShopping) {
		//	//	// Change our cursor to look different.
		//	//	Cursor.SetCursor (cursor, new Vector2 (cursor.width / 2, cursor.height / 2), CursorMode.Auto);
		//	//}
		//}
	}
}
