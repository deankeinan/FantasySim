using UnityEngine;
using System;

namespace TrollBridge {

	/// <summary>
	/// Game timer.  This only works for single player games where its either a new game or continue game.  If you have profiles in your game then you will need to edit this script to account for those new profiles.
	/// </summary>
	public class Game_Timer : MonoBehaviour {

		// What is 1 hour ingame equate to real life time.
		[SerializeField] private int hourInGameConversion;
		[SerializeField] private int minuteInGameConversion;
		[SerializeField] private int secondInGameConversion;

		// Our current game time.
		[SerializeField] private int currentTimeHour = 11;
		[SerializeField] private int currentTimeMinute;
		[SerializeField] private int currentTimeSecond;
		// Are we in AM or PM.
		[SerializeField] private bool isAM;
		// This handles our seconds until we shoot the int of it to currentTimeSeconds.
		private float holderTimeSeconds;
		// Total seconds.
		private int totalSeconds;
		// This is used for our weight for the conversion of time.
		private float conversionWeight;
		// This is gate for our time to start/pause.
		private bool canTime = false;
		private bool changed = false;


		void Start(){
			// Actual time in seconds in an hour.
			int timeInHour = 3600;
			// Convert it all to seconds.
			int hourToSeconds = hourInGameConversion * 3600;
			int minuteToSeconds = minuteInGameConversion * 60;
			totalSeconds = hourToSeconds + minuteToSeconds + secondInGameConversion;
			// We now have our weight.
			conversionWeight = (float) timeInHour / (float) totalSeconds;
		}

		void Update () {
			// IF we are able to do our timer.
			if (canTime) {
				// Lets add to our time to our seconds.
				holderTimeSeconds += Time.deltaTime * conversionWeight;
				// IF our seconds + leftOverSeconds has reached 60 or over.
				if (holderTimeSeconds >= 60f) {
					// Add 1 to our minute.
					currentTimeMinute++;
					// The over shot seconds are transfered to the next count for the seconds.
					holderTimeSeconds = holderTimeSeconds - 60f;
				}
				// Display our seconds with no decimals.
				currentTimeSecond = (int)holderTimeSeconds;
				// IF our minute has reached over 60.
				if (currentTimeMinute >= 60) {
					// Add 1 to the hour.
					currentTimeHour++;
					// Reset the minute.
					currentTimeMinute = currentTimeMinute - 60;
				}
				// IF our hour has reached 12 AND we have not changed our AM/PM yet.
				if (currentTimeHour == 12 && !changed) {
					// Change the AM/PM.
					isAM = !isAM;
					// We changed from AM to PM or vice versa.
					changed = true;
				}
				// IF our hour goes past 12.
				if(currentTimeHour > 12){
					// Reset the hour to 1.
					currentTimeHour = 1;
					// Allow us to change the meridiens later.
					changed = false;
				}
			}
		}

		/// <summary>
		/// Gets the ingame hour.
		/// </summary>
		/// <returns>The hour.</returns>
		public int GetHour(){
			return currentTimeHour;
		}

		/// <summary>
		/// Gets the minute.
		/// </summary>
		/// <returns>The minute.</returns>
		public int GetMinute(){
			return currentTimeMinute;
		}

		/// <summary>
		/// Gets the second.
		/// </summary>
		/// <returns>The second.</returns>
		public int GetSecond(){
			return currentTimeSecond;
		}

		/// <summary>
		/// Gets the next hour.
		/// </summary>
		/// <returns>The next hour.</returns>
		public int GetNextHour(){
			// IF our current hour is 12,
			// ELSE our current hour is 1-11
			if (currentTimeHour == 12) {
				return 1;
			} else {
				return currentTimeHour + 1;
			}
		}

		/// <summary>
		/// Sets the changed variable.
		/// </summary>
		/// <param name="newChanged">If set to <c>true</c> new changed.</param>
		public void SetChanged(bool newChanged){
			changed = newChanged;
		}

		/// <summary>
		/// Gets the total seconds for our conversion.
		/// </summary>
		/// <returns>The total seconds.</returns>
		public int GetTotalSeconds(){
			return totalSeconds;
		}

		/// <summary>
		/// Gets the meridien.
		/// </summary>
		/// <returns><c>true</c>, if meridien was gotten, <c>false</c> otherwise.</returns>
		public bool GetMeridien(){
			return isAM;
		}

		public bool IsMeridienChangingNextHour(){
			// IF we are currently in the 11th hour,
			// ELSE we are not currently in the 11th hour.
			if (currentTimeHour == 11) {
				// We are changing next hour.
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Sets the game time.
		/// </summary>
		/// <param name="hour">Hour.</param>
		/// <param name="minute">Minute.</param>
		/// <param name="second">Second.</param>
		public void SetGameTime (int hour, int minute, int second) {
			currentTimeHour = hour;
			currentTimeMinute = minute;
			currentTimeSecond = second;
		}

		/// <summary>
		/// Sets the canTime.
		/// </summary>
		/// <param name="newTime">If set to <c>true</c> new time.</param>
		public void SetCanTime (bool newTime) {
			canTime = newTime;
		}

		/// <summary>
		/// Saves the time.
		/// </summary>
		public void SaveTime () {
			// Create a new Time_Data.
			Time_Data data = new Time_Data ();
			// Store our precious information.
			data.hour = currentTimeHour;
			data.minute = currentTimeMinute;
			data.second = currentTimeSecond;
			// Turn this precious information into precious JSON information.
			string timeToJson = JsonUtility.ToJson(data);
			// Save the encrypted precious JSON information.
			PlayerPrefs.SetString("GameTime", timeToJson);
		}

		/// <summary>
		/// Loads the time.
		/// </summary>
		public void LoadTime () {
			// Get the saved encrypted information.
			string keyJson = PlayerPrefs.GetString("GameTime");
			// If we have a null or empty string.
			if(String.IsNullOrEmpty(keyJson)){
				// We do nothing.
				return;
			}
			// Cast our Json data to Key_Data.
			Time_Data data = JsonUtility.FromJson<Time_Data> (keyJson);
			currentTimeHour = data.hour;
			currentTimeMinute = data.minute;
			currentTimeSecond = data.second;
		}
	}

	class Time_Data	{	
		public int hour;
		public int minute;
		public int second;
	}
}
