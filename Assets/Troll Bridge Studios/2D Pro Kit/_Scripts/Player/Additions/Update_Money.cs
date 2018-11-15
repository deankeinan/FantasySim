using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TrollBridge {

	[RequireComponent(typeof(Text))]
	/// <summary>
	/// Update money script will look to our Money script and update whatever the currencyToUpdate is for display amount.
	/// </summary>
	public class Update_Money : MonoBehaviour {

		public string currencyToUpdate;

		private GameObject playerGO;
		private Money currency;
		private Text moneyText;

		void Start () {
			moneyText = GetComponent<Text> ();
		}

		void Update () {
			// IF there isn't a player manager gameobject active on the scene.
			if (playerGO == null) {
				// Get the Player Manager GameObject.
				playerGO = Character_Helper.GetPlayerManager ();
				return;
			}
			// IF there isn't a Money component set yet.
			if(currency == null){
				// Get the Money script that is on the player GameObject.
				currency = playerGO.GetComponentInChildren<Money> ();
				return;
			}
			// Update the text to this currency.
			moneyText.text = "x " + currency.GetCurrency(currencyToUpdate).ToString();
		}
	}
}
