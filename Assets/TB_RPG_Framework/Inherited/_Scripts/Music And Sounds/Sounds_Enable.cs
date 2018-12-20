using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Play a sound when this GameObject is enabled.
	/// </summary>
	public class Sounds_Enable : MonoBehaviour {

		[Tooltip("The sound that is played when this script is enabled.")]
		[SerializeField] private AudioClip enableSound;

		void OnEnable () {
			// Play the OnEnable sounds.
			Grid_Helper.soundManager.PlaySound (enableSound);
		}
	}
}
