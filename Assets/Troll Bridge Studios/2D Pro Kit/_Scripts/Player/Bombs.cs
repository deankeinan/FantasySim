using UnityEngine;
using System.Collections;
using System;

namespace TrollBridge {

	/// <summary>
	/// Bombs script to hold all of our different type of bombs.
	/// </summary>
	public class Bombs : MonoBehaviour {

		// The types of bombs.
		[Tooltip ("All the types of bombs in your game.")]
		public Bomb_Types[] typesOfBombs;

		[Tooltip ("The distance from this GameObjects pivot you want the bomb to be dropped.  The bomb will be placed in the direction the Player is facing (Based on the Direction number in the animation.)")]
		public float bombDistance;
		[Tooltip ("The key you can set to activate placing a bomb.")]
		public KeyCode bombKey;
		[Tooltip ("The player Animator.  This is needed so that we can find out which way the player is facing so we know which direction to drop a bomb and based on bombDistance will dictate how far the bomb is dropped.")]
		public Animator playerAnimator;
		[Tooltip ("The GameObject that represents our dropper.  Ideally a GameObject that has a Sprite Renderer.")]
		public GameObject playerBody;

		// This lets us know which bomb to place.  This is used to point to a location in our array to know which bomb to drop.
		private int bombChoice = 0;

		void Awake () {
			LoadBombs ();
		}

		void Update(){
			// IF we pressed the button to drop a bomb.
			if (Input.GetKeyDown (bombKey)) {
				// IF we have any bombs to place.  We go into our Bomb_Currency then look through the array to our choice of current bomb 'bombChoice' to let us know which bomb to drop.
				if (typesOfBombs[bombChoice].bombAmount > 0) {
					float x = 0;
					float y = 0;
					// Get the direction the player is facing.
					int dir = playerAnimator.GetInteger ("Direction");
					// IF we are looking up,
					// ELSE IF we are looking left,
					// ELSE IF we are looking down,
					// ELSE IF we are looking right.
					if (dir == 1) {
						y = bombDistance;
					} else if (dir == 2) {
						x = -bombDistance;
					} else if (dir == 3) {
						y = -bombDistance;
					} else if (dir == 4) {
						x = bombDistance;
					}
					// Place a bomb.  Add the Bomb Distance based on the direction the player is looking.
					Grid_Helper.helper.SpawnObject (typesOfBombs[bombChoice].bombObject, playerBody.transform.position + new Vector3 (x, y, 0f), Quaternion.identity, playerAnimator.gameObject);
					// Subtract a Bomb.
					SubtractBombs (typesOfBombs [bombChoice].bombObject.name, 1);
				}
			}
		}

		/// <summary>
		/// Saves the bombs.
		/// </summary>
		public void SaveBombs() {
			// Setup the data to be saved.
			string[] bombNames = new string[typesOfBombs.Length];
			int[] bombAmount = new int[typesOfBombs.Length];
			// Loop the amount of times we have bombs.
			for (int i = 0; i < typesOfBombs.Length; i++) {
				bombNames [i] = typesOfBombs [i].bombObject.name;
				bombAmount [i] = typesOfBombs [i].bombAmount;
			}

			// Create a new Bomb_Data for our information to be saved.
			Bomb_Data data = new Bomb_Data ();
			data.bombName = bombNames;
			data.bombAmount = bombAmount;

			// Create a new data structure for our saving.
			Bomb_Choice choiceData = new Bomb_Choice ();
			// Save the data.
			choiceData.bombChoice = bombChoice;

			// Turn the data into Json data.
			string bombToJson = JsonUtility.ToJson (data);
			string bombChoiceToJson = JsonUtility.ToJson (choiceData);
			// Save this information.
			PlayerPrefs.SetString ("Bombs", bombToJson);
			PlayerPrefs.SetString ("BombsChoice", bombChoiceToJson);
		}

		/// <summary>
		/// Loads the bombs.
		/// </summary>
		private void LoadBombs() {
			// Get the saved information.
			string bombJson = PlayerPrefs.GetString ("Bombs");
			string bombChoiceJson = PlayerPrefs.GetString ("BombsChoice");
			// IF we have a null or empty string.
			if (String.IsNullOrEmpty (bombJson)) {
				// We do nothing.
				return;
			}

			// Cast the Json data to our Bomb information.
			Bomb_Data data = JsonUtility.FromJson<Bomb_Data> (bombJson);
			Bomb_Choice choiceData = JsonUtility.FromJson<Bomb_Choice> (bombChoiceJson);

			// Since we are loading we need to do a compare with the name and associate the amount that way, as when you are developing there are times you forget things when you were planning everything out, so this will take care of 
			// implementations you shall add in the future without giving any errors instead of flat out just matching the amounts with the array index integer.

			// We only want to fully loop the length of what was saved.
			for (int i = 0; i < data.bombName.Length; i++) {
				// Lets grab the name of our entry to compare to the main list that is in the inspector.
				string name = data.bombName [i];
				for (int j = 0; j < typesOfBombs.Length; j++) {
					// IF the name in our saved data matches the name of the GameObject for the Bombs.cs script in in your Inspector.
					if (name == typesOfBombs [j].bombObject.name) {
						// We set the amount.
						typesOfBombs [j].bombAmount = data.bombAmount [i];
					}
				}
			}

			// Set the bomb we currently have.
			bombChoice = choiceData.bombChoice;
		}

		/// <summary>
		/// Get the amount of bombs based on our bombChoice.
		/// </summary>
		public int GetBombs(string bombName) {
			// Loop through all the keys.
			for (int i = 0; i < typesOfBombs.Length; i++) {
				// IF we find the key we are looking for.
				if (typesOfBombs [i].bombObject.name == bombName) {
					// Return the amount of bombs.
					return typesOfBombs [i].bombAmount;
				}
			}
			return 0;
		}

		/// <summary>
		/// Add Bombs. 
		/// Whatever bombChoice is will be the bomb that will have the amount added to it.
		/// </summary>
		public void AddBombs(string bombName, int amount) {
			// Loop the amount of times we have type of bombs.
			for (int i = 0; i < typesOfBombs.Length; i++) {
				// IF the bomb names match.
				if (typesOfBombs [i].bombObject.name == bombName) {
					// Add the amount.
					typesOfBombs[i].bombAmount += amount;
				}
			}
		}

		/// <summary>
		/// Revmove Bombs. 
		/// Whatever bombChoice is will be the bomb that will have the amount subtracted to it.
		/// </summary>
		public void SubtractBombs(string bombName, int amount) {
			// Loop the amount of times we have type of bombs.
			for (int i = 0; i < typesOfBombs.Length; i++) {
				// IF the bomb names match.
				if (bombName == typesOfBombs [i].bombObject.name) {
					// Subtract the amount.
					typesOfBombs[i].bombAmount -= amount;
				}
			}
		}

		/// <summary>
		/// This method takes care of switching to a different bomb in your array in the Bomb.cs Inspector.  Say for example your player got an upgraded bomb which has a new look and does more damage, you would call this method
		/// and make sure the bomb they acquired will be changed and matched by bombChoice so when you want to drop bombs from that point on, you will be dropping the correct NEW bomb that your player acquired.
		/// </summary>
		public void ChangeBombChoice(int newBombChoice) {
			bombChoice = newBombChoice;
		}
	}

	[Serializable]
	public class Bomb_Types {
		public GameObject bombObject;
		public int bombAmount;
		public AudioClip bombSound;
	}

	class Bomb_Data {
		public string[] bombName;
		public int[] bombAmount;
	}

	class Bomb_Choice {
		public int bombChoice;
	}
}
