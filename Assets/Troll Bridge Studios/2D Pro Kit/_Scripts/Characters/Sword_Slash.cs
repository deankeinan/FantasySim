using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {
	
	public class Sword_Slash : MonoBehaviour {

		[Tooltip ("The starting offset angle to make when this is created.")]
		[SerializeField] private float offsetAngle = 0f;
		[Tooltip ("The rotation angle to make when this is created.")]
		[SerializeField] private int slashAngle = 120;


		private Character_Manager charManager;

		void Awake () {
			// Get our Character Manager in our parent.
			charManager = GetComponentInParent <Character_Manager> ();

			// Set our position, scale and rotation.
			transform.localPosition = Vector3.zero;
			transform.localScale = Vector3.one;
			// Since we have clockwise slashes we grab the starting position.
			transform.localEulerAngles = new Vector3(0f, 0f, charManager.GetSwordRotation (charManager.characterAnimator.GetInteger ("Direction")) + offsetAngle);
		}

		void Start () {
			StartCoroutine (Grid_Helper.helper.RotateSprite (gameObject.transform, charManager.characterAnimator.GetCurrentAnimatorStateInfo (0).length, transform.localEulerAngles.z, transform.localEulerAngles.z - slashAngle));
		}
	}
}
