using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TrollBridge {

	[RequireComponent(typeof(Text))]
	/// <summary>
	/// Update bombs will display the amount of a certain type of bombs we have based on 'bombToUpdate'.
	/// </summary>
	public class Update_Bombs : MonoBehaviour {

		public string bombToUpdate;

		private GameObject playerGO;
		private Bombs dropBombs;
		private Text bombText;

		void Start () {
			bombText = GetComponent<Text> ();
		}
		
		void Update () {
			// IF there isn't a player gameobject active on the scene.
			if (playerGO == null) {
				// Get the Player GameObject.
				playerGO = Character_Helper.GetPlayerManager ();
				return;
			}
			// IF there isn't a Bomb component set.
			if(dropBombs == null){
				// Get the Bomb script that is a child to the Player GameObject.
				dropBombs = playerGO.GetComponentInChildren<Bombs> ();
				return;
			}
			// Set the Text component.
			bombText.text = "x " + dropBombs.GetBombs(bombToUpdate).ToString();
		}
	}
}
