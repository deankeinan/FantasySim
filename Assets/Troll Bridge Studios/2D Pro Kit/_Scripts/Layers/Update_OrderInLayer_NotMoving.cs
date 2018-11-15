using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Update sorting layer based on the Y coordinate in our Start Method.
	/// </summary>
	public class Update_OrderInLayer_NotMoving : MonoBehaviour {

		private SpriteRenderer spriteRend;
		private Transform trans;

		void Start(){
			spriteRend = GetComponent<SpriteRenderer> ();
			trans = GetComponent<Transform> ();
			spriteRend.sortingOrder = (int)(trans.position.y * -1000);
		}
	}
}
