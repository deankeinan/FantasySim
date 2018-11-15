using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace TrollBridge {

	public class Dialogue_Data : MonoBehaviour {

		[Header ("Panel")]
		[SerializeField] private GameObject dialoguePanel;

		[Header ("Panel Parts")]
		[SerializeField] private Text nameText;
		[SerializeField] private Text dialogueText;
		[SerializeField] private UnityEngine.UI.Image portraitImage;
		[SerializeField] private GameObject replyPanel;

		[Header ("Prefab")]
		[SerializeField] private GameObject buttonGO;
		// Is the player engaged in an Dialogue_Setup.
		private bool IsActionKeyDialogued = false;

		// The List of Action Key Dialogues (Dialogue_Setup) currently inside.
		private List<Dialogue_Setup> ListOfActionKeyDialogues = new List<Dialogue_Setup>();

		// The Dialogue_Setup we are currently engaged in due to it being the closest.
		private Dialogue_Setup closestDialogue = null;


		/// <summary>
		/// Dialogue this instance.
		/// </summary>
		public void Dialogue () {
			// Lets begin our dialogue.
			closestDialogue.Dialogue ();
		}

		/// <summary>
		/// Nexts the dialogue.
		/// </summary>
		public void NextDialogue (int newLinkNode) {
			// Clear the current buttons.
			Grid_Helper.helper.DestroyGameObjectsByParent (replyPanel);
			// We have selected an option so lets set our reply panel inactive.
			replyPanel.SetActive (false);
			// Set the new Link Node for the dialogue.
			closestDialogue.SetDialogueNode (newLinkNode);
			// The reply panel is gone so lets set our bool accordingly.
			closestDialogue.SetIsDialogueReplyShowing (false);
			// Lets continue our dialougue.
			closestDialogue.Dialogue ();
		}

		/// <summary>
		/// Sends the character info.
		/// </summary>
		public void SendCharacterInfo (GameObject requestee, bool canMove, bool _isInteracting, GameObject requester) {
			// Reference Character Manager.
			Character_Manager charaManager = requestee.GetComponentInParent <Character_Manager> ();
			// IF there is a character script.
			if (charaManager != null) {
				// Lets make sure this stops moving since we are talking to it.
				charaManager.canMove = canMove;
				// Lets set our boolean to show we are in a interaction.
				charaManager.isInteracting = _isInteracting;
				// Assign our focus target.
				charaManager.interactionFocusTarget = requester;
			}
		}

		/// <summary>
		/// Assignings the dialogue.
		/// </summary>
		public void AssigningDialogue (string _nameText, string _dialogueText, Sprite _portraitImage) {
			nameText.text = _nameText;
			dialogueText.text = _dialogueText;
			portraitImage.sprite = _portraitImage;
		}


		public GameObject GetDialoguePanel () {
			return dialoguePanel;
		}

		/// <summary>
		/// Sets the name of the entity.
		/// </summary>
		public void SetEntityName (string _nameText) {
			nameText.text = _nameText;
		}

		/// <summary>
		/// Sets the dialogue text.
		/// </summary>
		public void SetDialogueText (string _dialogueText) {
			dialogueText.text = _dialogueText;
		}

		/// <summary>
		/// Gets the dialogue text.
		/// </summary>
		public Text GetDialogueText () {
			return dialogueText;
		}

		/// <summary>
		/// Sets the dialogue portrait.
		/// </summary>
		public void SetDialoguePortrait (Sprite _portraitImage) {
			portraitImage.sprite = _portraitImage;
		}

		/// <summary>
		/// Sets the closest dialogue.
		/// </summary>
		public void SetClosestDialogue (Dialogue_Setup newClosestDialogue) {
			closestDialogue = newClosestDialogue;
		}

		/// <summary>
		/// Gets the closest dialogue.
		/// </summary>
		public Dialogue_Setup GetClosestDialogue () {
			return closestDialogue;
		}

		/// <summary>
		/// Displays the dialogue panel.
		/// </summary>
		public void DisplayDialoguePanel (bool display) {
			// Display the dialogue panel based on 'display'.
			dialoguePanel.SetActive (display);
			// IF we are displaying the dialogue panel,
			// ELSE we are removing the dialogue panel.
			if (display) {
				// Lets Setup.
				SendCharacterInfo (GetClosestDialogue ().gameObject, false, true, Character_Helper.GetPlayer ());
			} else {
				// Reset to start standards.
				GetClosestDialogue().ResetDialogue ();
				ResetDialogue ();
			}
		}

		/// <summary>
		/// Displays the reply panel.
		/// </summary>
		public void DisplayReplyPanel (bool display) {
			replyPanel.SetActive (display);
		}

		/// <summary>
		/// Updates the dialogue entity data.
		/// </summary>
		public void UpdateDialogueEntityData (string jsonDialogueEntity, int dialogueTreeStart) {
			PlayerPrefs.SetInt (jsonDialogueEntity, dialogueTreeStart);
		}

		/// <summary>
		/// Creates the reply button.
		/// </summary>
		public GameObject CreateReplyButton (string _text) {
			// Create the button Prefab.
			GameObject button = Instantiate (buttonGO);
			// Set the text.
			button.GetComponentInChildren <Text> ().text = _text;
			// Set the parent.
			button.transform.SetParent (replyPanel.transform);
			// Set the scale.
			button.transform.localScale = new Vector3 (1f, 1f, 1f);
			// Return the button created.
			return button;
		}

		/// <summary>
		/// Adds the reply component.
		/// </summary>
		public void AssignReplyLink (GameObject replyGO, int _nodeLink) {
			// Set this buttons link node.
			replyGO.GetComponent <Reply_Button> ().SetLinkNode (_nodeLink);
		}

		/// <summary>
		/// Gets the dialogue entity data.
		/// </summary>
		public int GetDialogueEntityData (string jsonDialogueEntity) {
			return PlayerPrefs.GetInt (jsonDialogueEntity, 0);
		}

		/// <summary>
		/// Are we close enough to something that has a dialogue?
		/// </summary>
		public bool IsWithinDialogueInteraction () {
			// IF we are inside a Dialogue_Setup area.
			if (ListOfActionKeyDialogues.Count > 0) {
				// We have started a dialogue so return true.
				return true;
			}
			// No dialogue started so we return false.
			return false;
		}

		/// <summary>
		/// Finds the closest dialogue.
		/// </summary>
		public void FindClosestDialogue (GameObject player) {
			// IF we are not already engaged in a dialogue.
			if (!IsActionKeyDialogued) {
				// Preset a distance variable to detect the closest action key dialogue.
				float _dist = -1f;
				// Loop through all the Dialogue_Setups.
				for (int i = 0; i < ListOfActionKeyDialogues.Count; i++) {
					// See which one is the closest.
					float dist = Vector2.Distance (player.transform.position, ListOfActionKeyDialogues [i].transform.position);
					// IF this is the first time in here. Also this takes care of 1 Interactive NPC in the List.
					// ELSE IF we have more interactive npcs and we need to compare distance to see which is closest.
					if (_dist == -1f) {
						// Set the shortest distance.
						_dist = dist;
						// Set the closest action key dialogue.
						closestDialogue = ListOfActionKeyDialogues [i];
					} else if (dist < _dist) {
						// Set the shortest distance.
						_dist = dist;
						// Set the closest action key dialogue.
						closestDialogue = ListOfActionKeyDialogues [i];
					}
				}
				// We are now engaged in a dialogue.
				IsActionKeyDialogued = true;
			}
		}

		/// <summary>
		/// Adds to dialogue list.
		/// </summary>
		public void AddToDialogueList (Dialogue_Setup dialogueSetup) {
			ListOfActionKeyDialogues.Add (dialogueSetup);
		}

		/// <summary>
		/// Removes from dialogue list.
		/// </summary>
		public void RemoveFromDialogueList (Dialogue_Setup dialogueSetup) {
			ListOfActionKeyDialogues.Remove (dialogueSetup);
		}

		/// <summary>
		/// Resets the dialogue.
		/// </summary>
		public void ResetDialogue () {
			// Deactivate the reply panel
			DisplayReplyPanel (false);
			// Set the bool to let us know we are not currently in a Dialogue.
			IsActionKeyDialogued = false;
			// Clear the current buttons.
			Grid_Helper.helper.DestroyGameObjectsByParent (replyPanel);
			// Update some Character stats.
			SendCharacterInfo (closestDialogue.gameObject, true, false, null);
		}
	}
}
