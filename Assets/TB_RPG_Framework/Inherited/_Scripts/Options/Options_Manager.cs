using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace TrollBridge {

	/// <summary>
	/// Options manager is our system in which we take care of the options for the game.
	/// </summary>
	public class Options_Manager : MonoBehaviour {

		// The Key that activates the Options Menu.
		public KeyCode optionsKey;
		// The Panel the options are on.
		public GameObject optionsPanel;
		// Optional, the backdrop so that while this is up the user cannot click anything else except for the options currently displayed.
		public GameObject optionsPanelBackdrop;
		// The scenes that you do not want the optionsKey to work on. These are the scenes such as Main Menu, cut scenes, etc.
		public string[] noOptionKeyScenes;

		// The music slider that is used for dictating the volume of the music.
		public Slider musicSlider;
		// the music toggle that is used for muting and unmuting the music.
		public Toggle musicToggle;
		// The sfx slider that is used for dictating the volume of the sfx.
		public Slider sfxSlider;
		// The sfx toggle that is used for muting and unmuting the sfx.
		public Toggle sfxToggle;

		// The Panel that the scaling will be applied to.
		public GameObject scalePanelOne;
		// The slider that adjusts the scaling.
		public Slider UISliderOne;
		// The text to display to notify the user what the scaling is.
		public Text UISliderOneText;
		// For faster calculation on UI scaling.
		private bool uiSliderOneTextExist;

		// The Panel that the scaling will be applied to.
		public GameObject scalePanelTwo;
		// The slider that adjusts the scaling.
		public Slider UISliderTwo;
		// The text to display to notify the user what the scaling is.
		public Text UISliderTwoText;
		// For faster calculation on UI scaling.
		private bool uiSliderTwoTextExist;

		// The Panel that the scaling will be applied to.
		public GameObject scalePanelThree;
		// The slider that adjusts the scaling.
		public Slider UISliderThree;
		// The text to display to notify the user what the scaling is.
		public Text UISliderThreeText;
		// For faster calculation on UI scaling.
		private bool uiSliderThreeTextExist;

		// The fullscreen toggle.
		public Toggle fullscreen;

		// Variables used for the scaling.
		private float uiScaleOne;
		private float uiScaleTwo;
		private float uiScaleThree;

		
		void Awake(){
			// Load any save game options.
			LoadOptionsSettings();

			// Set the fullscreen option to what was either made by default 
			// or saved from last session.
			if(fullscreen != null && Application.platform != RuntimePlatform.WebGLPlayer){
				fullscreen.isOn = Screen.fullScreen;
			}
			// Set the toggles and sliders in Sound_Manager.
			Grid_Helper.soundManager.SetSlidersAndToggles(musicSlider, musicToggle, sfxSlider, sfxToggle);
		}

		void Update() {
			// IF the user hits The Options Key.
			if(Input.GetKeyUp(optionsKey)){
				// Currently we only want to show the options when we have the player created and in the scene.  Think of scenes where there wont be a player... Main menu, Credits, any type of cenamtic... These types of scenes we either do not want
				// to show the options menu or we have a button somewhere in the scene to bring it up (Main Menu).
				if(Character_Helper.GetPlayer() == null){
					// No player no options menu.
					return;
				}
				// IF the panel is not showing then display it.
				// ELSE the panel is showing then remove it.
				if(!optionsPanel.activeInHierarchy){
					OptionsDisplay(true);
				}else{
					OptionsDisplay(false);
				}
			}
		}

		void ScalePanelOneCheck(){
			// IF the scalePanel is equal to null,
			// ELSE we have a scalePanel.
			if(scalePanelOne == null){
				return;
			}else{
				// Do we have UISliderText.
				uiSliderOneTextExist = UISliderOneText != null;
				// Make the UISlider interactable.
				UISliderOne.interactable = true;
				// Since we have a UI panel on the scene lets grab the UI scaling from our saved settings.
				scalePanelOne.transform.localScale = new Vector3(uiScaleOne, uiScaleOne, 1f);
			}
			// Set the value of the Slider.
			UISliderOne.value = uiScaleOne;
		}

		void ScalePanelTwoCheck(){		
			// IF the scalePanel is equal to null,
			// ELSE we have a scalePanel.
			if(scalePanelTwo == null){
				return;
			}else{
				// Do we have UISliderText
				uiSliderTwoTextExist = UISliderTwoText != null;
				// Make the UISlider interactable.
				UISliderTwo.interactable = true;
				// Since we have a UI panel on the scene lets grab the UI scaling from our saved settings.
				scalePanelTwo.transform.localScale = new Vector3(uiScaleTwo, uiScaleTwo, 1f);
			}
			// Set the value of the Slider.
			UISliderTwo.value = uiScaleTwo;
		}

		void ScalePanelThreeCheck(){		
			// IF the scalePanel is equal to null,
			// ELSE we have a scalePanel.
			if(scalePanelThree == null){
				return;
			}else{
				// Do we have UISliderText
				uiSliderThreeTextExist = UISliderThreeText != null;
				// Make the UISlider interactable.
				UISliderThree.interactable = true;
				// Since we have a UI panel on the scene lets grab the UI scaling from our saved settings.
				scalePanelTwo.transform.localScale = new Vector3(uiScaleThree, uiScaleThree, 1f);
			}
			// Set the value of the Slider.
			UISliderThree.value = uiScaleThree;
		}

		/// <summary>
		/// Display your options panel the opposite of what its current activeness is.
		/// </summary>
		public void OptionsDisplay () {
			// Turn the options panel the opposite of its activeness in the hierarchy.
			optionsPanel.SetActive (!optionsPanel.activeInHierarchy);

			// IF the options is being displayed,
			// ELSE the  options is being removed.
			if (optionsPanel.activeInHierarchy) {
				// Activate the backdrop.
				Grid_Helper.optionManager.OptionsPanelBackdropDisplay (true);
			} else {
				// De-Activate the backdrop.
				Grid_Helper.optionManager.OptionsPanelBackdropDisplay (false);
			}
		}

		/// <summary>
		/// Display your options panel based on the parameter 'active'.
		/// </summary>
		public void OptionsDisplay (bool active) {
			// Set the options panel activeness based on 'active'.
			optionsPanel.SetActive (active);

			// IF the options is being displayed,
			// ELSE the  options is being removed.
			if (active) {
				// Activate the backdrop.
				Grid_Helper.optionManager.OptionsPanelBackdropDisplay (true);
			} else {
				// De-Activate the backdrop.
				Grid_Helper.optionManager.OptionsPanelBackdropDisplay (false);
			}
		}

		/// <summary>
		/// Display the activeness of the backdrop display that cancels UIs on click based on 'active'.
		/// </summary>
		public void OptionsPanelBackdropDisplay (bool active) {
			// IF we have a backdrop panel.
			if (optionsPanelBackdrop != null) {
				// Set the activeness of the backdrop panel based on 'active'.
				optionsPanelBackdrop.SetActive (active);
			}
		}

		/// <summary>
		/// Scale our UI's.
		/// </summary>
		public void SliderRescaleUI(int sliderNumber) {

			float scale;

			switch (sliderNumber) {
			case 0:
				scale = ReScaleUI (scalePanelOne, UISliderOne, UISliderOneText, uiSliderOneTextExist);
				if (scale != -1) {
					uiScaleOne = scale;
				}
				break;
			case 1:
				scale = ReScaleUI (scalePanelTwo, UISliderTwo, UISliderTwoText, uiSliderTwoTextExist);
				if (scale != -1) {
					uiScaleTwo = scale;
				}
				break;
			case 2:
				scale = ReScaleUI (scalePanelThree, UISliderThree, UISliderThreeText, uiSliderThreeTextExist);
				if (scale != -1) {
					uiScaleThree = scale;
				}
				break;

			default:
				Debug.Log ("A wrong slider number was used for scaling the UI.  Numbers to choose are either 0, 1 or 2.  The number you chose was " + sliderNumber);
				break;
			}
		}

		/// <summary>
		/// Rescale UI.
		/// </summary>
		private float ReScaleUI(GameObject scalePanel, Slider UISlider, Text UISliderText, bool uiSliderTextExist){
			// IF there is a scale panel in the scene.
			// ELSE there isnt but we still need to set the options variables.
			if (scalePanel != null) {
				// What ever the value is we need to make sure we apply this to all the children in the Panel Parent.
				scalePanel.transform.localScale = new Vector3 (UISlider.value, UISlider.value, 1f);
				// IF we have slider text.
				if (uiSliderTextExist) {
					// Set the text to display the UI scaling number.
					UISliderText.text = UISlider.value.ToString ("F2");
				}
				
				return UISlider.value;
			} else {
				// IF we have slider text.
				if (uiSliderTextExist) {
					// Set the text to display the UI scaling number.
					UISliderText.text = UISlider.value.ToString ("F2");
				}
				return -1;
			}
		}

		/// <summary>
		/// Music toggle.
		/// </summary>
		public void MusicToggle(){
			Grid_Helper.soundManager.MuteUnMuteBGMusic(musicToggle.isOn);
		}

		/// <summary>
		/// Music slider.
		/// </summary>
		public void MusicSlider(){
			Grid_Helper.soundManager.ChangeMusicVolume(musicSlider.value);
		}

		/// <summary>
		/// SFX toggle.
		/// </summary>
		public void SFXToggle(){
			Grid_Helper.soundManager.MuteUnMuteSound(sfxToggle.isOn);
		}

		/// <summary>
		/// SFX slider.
		/// </summary>
		public void SFXSlider(){
			Grid_Helper.soundManager.ChangeSFXVolume(sfxSlider.value);
		}

		/// <summary>
		/// Change screen mode.
		/// </summary>
		public void ChangeScreenMode(){
			Grid_Helper.helper.ChangeScreenMode(fullscreen.isOn);
		}

		/// <summary>
		/// Saves the option settings.
		/// </summary>
		public void SaveOptionSettings(){
			// Create a new State_Data.
			Options_Settings GameOptions = new Options_Settings ();
			// Save the information.
			GameOptions.SFXToggle = sfxToggle.isOn;
			GameOptions.SFXVolume = sfxSlider.value;
			GameOptions.MusicVolume = musicSlider.value;
			GameOptions.MusicToggle = musicToggle.isOn;
			GameOptions.UIOneScaling = uiScaleOne;
			GameOptions.UITwoScaling = uiScaleTwo;
			GameOptions.UIThreeScaling = uiScaleThree;
			// Turn the data into Json data.
			string optionsToJson = JsonUtility.ToJson (GameOptions);
			// Store the data.
			PlayerPrefs.SetString ("Options", optionsToJson);
		}

		/// <summary>
		/// Loads the options settings.
		/// </summary>
		public void LoadOptionsSettings(){
			// Load the json data.
			string optionsJson = PlayerPrefs.GetString ("Options");
			// Load the data structure.
			Options_Settings GameOptions = new Options_Settings();
			// IF there is nothing in this string,
			// ELSE there is stuff in this string.
			if (String.IsNullOrEmpty (optionsJson)) {
				// Load the Defaults.
				GameOptions.Default ();
			} else {
				// Load the saved Json data to our class data.
				GameOptions = JsonUtility.FromJson<Options_Settings> (optionsJson);
			}
			// Set the options.
			SetOptionsSettings(GameOptions);
		}

		/// <summary>
		/// Since these are global settings across the board and apply to every scene we assign these here.
		/// You may notice I do not assign anything with the UI settings because it is handled in the ScalePanelCheck 
		/// functions as that checks for the existance of the UI and assigns the scaling before it is displayed to the user.
		/// </summary>
		void SetOptionsSettings(Options_Settings GameOptions){
			// Set the music volume and toggle.
			musicSlider.value = GameOptions.MusicVolume;
			musicToggle.isOn = GameOptions.MusicToggle;
			Grid_Helper.soundManager.ChangeMusicVolume(musicSlider.value);
			Grid_Helper.soundManager.MuteUnMuteBGMusic(musicToggle.isOn);

			// Set the SFX volume and toggle.
			sfxSlider.value = GameOptions.SFXVolume;
			sfxToggle.isOn = GameOptions.SFXToggle;
			Grid_Helper.soundManager.ChangeSFXVolume(sfxSlider.value);
			Grid_Helper.soundManager.MuteUnMuteSound(sfxToggle.isOn);

			// Set the scaling of the panels.
			uiScaleOne = GameOptions.UIOneScaling;
			uiScaleTwo = GameOptions.UITwoScaling;
			uiScaleThree = GameOptions.UIThreeScaling;
			ScalePanelOneCheck ();
			ScalePanelTwoCheck ();
			ScalePanelThreeCheck ();
		}
	}

	[Serializable]
	class Options_Settings {
		// The Music toggle
		public bool MusicToggle;
		// The Music volume.
		public float MusicVolume;
		// The SFX Toggle.
		public bool SFXToggle;
		// The SFX volume.
		public float SFXVolume;
		// The scaling of UI 1 out of 3.
		public float UIOneScaling;
		// The scaling of UI 2 out of 3.
		public float UITwoScaling;
		// The scaling of UI 3 out of 3.
		public float UIThreeScaling;

		public void Default () {
			// Set the defaults of our variables.
			MusicToggle = false;
			MusicVolume = 0.5f;

			SFXToggle = false;
			SFXVolume = 0.5f;

			UIOneScaling = 1f;

			UITwoScaling = 1f;

			UIThreeScaling = 1f;
		}
	}
}
