using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Update sorting layer based on the Y coordinate in our Update Method.
	/// </summary>
	public class Update_OrderInLayer_Moving : MonoBehaviour {

		[SerializeField] private int internalSort = 0;

		private SpriteRenderer spriteRend;
		private Transform trans;

		void Start(){
			spriteRend = GetComponent<SpriteRenderer> ();
			trans = GetComponent<Transform> ();
			spriteRend.sortingOrder = (int)(trans.position.y * -1000) + internalSort;
		}

		void Update(){
			spriteRend.sortingOrder = (int)(trans.position.y * -1000) + internalSort;
		}
	}
}
