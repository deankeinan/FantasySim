using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrollBridge {

	public class Inactive_EmptyText : MonoBehaviour {

		[Tooltip ("Text components in which we check to be empty.")]
		[SerializeField] private Text[] textsToBeChecked;
		
		void Update () {
			// Loop the amount of texts that we want to check.
			for (int i = 0; i < textsToBeChecked.Length; i++) {
				// IF the text is null or empty.
				if (string.IsNullOrEmpty (textsToBeChecked [i].text)) {
					// Set it inactive.
					textsToBeChecked [i].gameObject.SetActive (false);
				} else {
					// Set it active.
					textsToBeChecked [i].gameObject.SetActive (true);
				}
			}
		}
	}
}
