using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// Dont destroy on scene a new load.
	/// </summary>
	public class Dont_Destroy_On_Scene_Load : MonoBehaviour {

		void Awake () {
			DontDestroyOnLoad(gameObject);
		}
	}
}
