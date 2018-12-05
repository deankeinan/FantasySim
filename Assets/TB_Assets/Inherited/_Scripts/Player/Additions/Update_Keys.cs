using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TrollBridge {

	[RequireComponent(typeof(Text))]
	/// <summary>
	/// Update keys will display the amount of keys you have specified by keyToUpdate.  This works with the Key script.
	/// </summary>
	public class Update_Keys : MonoBehaviour {

		public string keyToUpdate;

		private GameObject playerGO;
		private Key key;
		private Text keyText;

		void Start () {
			keyText = GetComponent<Text> ();
		}

		void Update () {
			// IF there isn't a player manager gameobject active on the scene.
			if (playerGO == null) {
				// Get the Player Manager GameObject.
				playerGO = Character_Helper.GetPlayerManager ();
				return;
			}
			// IF there isn't a Key component set yet.
			if(key == null){
				// Get the Key script that is on the player GameObject.
				key = playerGO.GetComponentInChildren<Key> ();
				return;
			}
			// Set the Text component.
			keyText.text = "x " + key.GetKeys(keyToUpdate).ToString();
		}
	}
}
