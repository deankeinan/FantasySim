using UnityEngine;
using UnityEngine.EventSystems;

namespace TrollBridge {

	/// <summary>
	/// Cancel user interfaces that are specified in the Update() and OnPointerDown() methods.  Currently in the demo this appears when we bring up the Item Split UI.
	/// </summary>
	public class Cancel_UserInterfaces : MonoBehaviour, IPointerDownHandler {

		[SerializeField] private UnityEngine.UI.Image image;

		// Here we make checks to see if the UIs we specifically want are up then we activate our transparent Image.
		void Update () {
			//// IF our Item Split UI is active in the Hierarchy.
			//if (Grid_Helper.inventory.GetSplitItemUI ().activeInHierarchy) {
			//	// Display our Image so no click throughs.
			//	image.enabled = true;
			//} else {
			//	image.enabled = false;
			//}
		}

		public void OnPointerDown(PointerEventData data) {
			// So here if we are clicking on something that isnt a UI showing up then we want to cancel anything we see.  
			// Example would be if you had the Item Split UI up and you want a quick exit/cancel out of it you would just click else where that is not on the UI.

			// IF our split inventory UI is up.
			if(Grid_Helper.inventory.GetSplitItemUI ().activeInHierarchy){
				// Lets remove the Item Split UI.
				Grid_Helper.inventory.GetSplitItemUI ().SetActive(false);
			}
		}
	}
}
