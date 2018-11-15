using UnityEngine;
using System.Collections;

namespace TrollBridge{

	/// <summary>
	/// Use this script IF you are using 1 camera throughout the entire scene to position the Camera if the scene will not spawn or will have a player in it.  Think Main Menu type of scenes.
	/// </summary>
	public class Set_Camera_Position : MonoBehaviour {

		void Start () {
			Camera.main.transform.position = gameObject.transform.position;
		}
	}
}
