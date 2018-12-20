using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Tooltip_Parent is used as a GameObject locater so that we know when to stop when we use GetComponentInParent.  We use this to know when to stop searching and when we find this GameObject we can check
	/// and see if it is active or not so we know to close the tooltip as well to prevent a visual bug.  Used on the Quest Tracker and Quest UI.
	/// </summary>
	public class Tooltip_Parent : MonoBehaviour {

	}
}
