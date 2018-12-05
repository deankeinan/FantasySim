using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Scene zoom default sets our zoom to a size when this is enabled.
	/// </summary>
	public class Scene_Zoom_Default : MonoBehaviour {

		[Tooltip("The orthographic size you want for the Camera when this scene is initially loaded.")]
		public float initialOrthographicSize;

		void Awake () {
			// Set the camera orthographicSize.
			Camera.main.orthographicSize = initialOrthographicSize;	
		}
	}
}
