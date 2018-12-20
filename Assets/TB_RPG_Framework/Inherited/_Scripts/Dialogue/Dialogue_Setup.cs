using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace TrollBridge {

	[RequireComponent(typeof(Collider2D))]
	public class Dialogue_Setup : MonoBehaviour {
		
		// The name you want to appear when the dialogue pops up.
		[SerializeField] private string entityName;
		// The Unique Name Identifier used when we load up the current dialogue this entity should be using.
		[SerializeField] private string uniqueName;
		// The names of the dialogue trees associated with this entity (Just put the name and don't put .json).
		[SerializeField] private string[] dialogueTreeNames;

		// Types of ways to display our text in the dialogue.
		[SerializeField] private bool typeText;
		// The sound to be played when a piece of text is typed out.
		[SerializeField] private AudioClip typeSound;

		// String that will be placed for buttons in your UI for this Dialogue_Setup.
		[SerializeField] private string startingDialogue;

		// The quest that is completed when this Dialogue is finished.
		//[SerializeField] private int questToBeCompletedID = -1;

		private JsonData dialogueData;
		private List<DialogueNode> dialogueTreeList = new List<DialogueNode>();
		private int dialogueNode = 0;
		private int counter = 0;
		private bool isDialogueReplyShowing = false;
		private bool isLastLink = false;
		private bool isTextTransitionIn = false;
	

		void Start () {

		}

		/// <summary>
		/// Raises the trigger enter 2d event.
		/// </summary>
		void OnTriggerEnter2D(Collider2D coll){
			// Attempt to grab the Character_Manager script in this gameobjects parent.
			Character_Manager character = coll.GetComponentInParent<Character_Manager>();
			// IF the colliding object doesnt have the Character Manager script.
			if (character == null) {
				return;
			}
			// IF the colliding gameobject isnt a Hero/Player.
			if (character.characterType != CharacterType.Hero) {
				// Return as we only care about the Hero/Player in this script.
				return;
			}
			// Assign this to our list of dialogues.
			Grid_Helper.dialogueData.AddToDialogueList (this);
		}


		/// <summary>
		/// Raises the trigger exit 2d event.
		/// </summary>
		void OnTriggerExit2D(Collider2D coll) {
			// Attempt to grab the Character_Manager script
			Character_Manager character = coll.GetComponentInParent<Character_Manager> ();
			// IF the colliding object doesnt have the Character Manager script.
			if (character == null) {
				return;
			}
			// IF the colliding gameobject isnt a Hero/Player.
			if (character.characterType != CharacterType.Hero) {
				// Return as we only care about the Hero/Player in this script.
				return;
			}
			// We remove this script from our player state list.
			Grid_Helper.dialogueData.RemoveFromDialogueList (this);
			// IF the closest dialogue is this one.
			if (Grid_Helper.dialogueData.GetClosestDialogue () == this) {
				// Remove the dialogue.
				Grid_Helper.dialogueData.DisplayDialoguePanel (false);
			}
		}

		/// <summary>
		/// Constructs the dialogue.
		/// </summary>
		private void ConstructDialogue () {
			// We first load the set of dialogues.
			dialogueData = JsonMapper.ToObject (File.ReadAllText (Application.streamingAssetsPath + "/Dialogue Tree/" + dialogueTreeNames [Grid_Helper.dialogueData.GetDialogueEntityData (uniqueName)] + ".json"));
			// Loop the amount of times we have dialogue interactions.  This is the amount of IDs we have in our Json file.
			for (int i = 0; i < dialogueData.Count; i++) {
				// We fill our dialogue tree with our prompt, replies and the links to the next dialogue.
				dialogueTreeList.Add (new DialogueNode (
					GetPrompt (i, "prompt"),
					GetReplies (i, "replies"),
					GetNodeLinks (i, "dialoguelinks"),
					GetPortrait (i, "portrait")
				));
			}
		}


		/// <summary>
		/// Dialogue this instance.
		/// </summary>
		public void Dialogue () {
			// IF our text is transitioning in.
			if (isTextTransitionIn) {
				// Stop all coroutines.
				StopAllCoroutines ();
				// Need to check and see if we are currently in a ui transition or text transition.
				SkipTransition ();
				// Since we have finished the skip we just leave.
				return;
			}

			// IF we currently have replies that the user can click on.  (We stop here as to continue the dialogue we are not pressing any key but rather selecting an option).
			if (isDialogueReplyShowing) {
				// Done.
				return;
			}

			// IF this is the start of the dialogue,
			if (counter == 0) {
				// Lets increase our counter.
				counter++;
				// Lets Construct our Dialogue.
				ConstructDialogue ();
			}

			// IF this is the last of the dialogue.
			if (CheckLastDialogue ()) {
				return;
			}

			// Assign the dialogue.
			AssignDialogue ();

			// IF we have typed out text,
			// ELSE we have no special type of text so lets jump right into the replys.
			if (typeText) {
				// Now that everything is assigned we can start our Coroutines.
				StartCoroutine (DisplayDialogue ());
			} else {
				// Handle setting up Reply Buttons if we have any.
				HandleReplys ();
			}
			// Display the Dialogue.
			Grid_Helper.dialogueData.DisplayDialoguePanel (true);
		}

		private void SkipTransition () {
			// Finish all the Transitions for skipping.
			AssignDialogue ();
			// Handle setting up Reply Buttons if we have any.
			HandleReplys ();
			// We are done transitioning in.
			isTextTransitionIn = false;
		}

		private IEnumerator DisplayDialogue () {
			// IF we have typed text.
			if (typeText) {
				// Our text is transitioning in.
				isTextTransitionIn = true;
				// We need to know how to deal with our text.
				yield return StartCoroutine (GUI_Helper.TypeText (Grid_Helper.dialogueData.GetDialogueText(), 0.075f, dialogueTreeList[dialogueNode].prompt, typeSound));
				// Our text is transitioning in.
				isTextTransitionIn = false;
			}
			// Handle setting up Reply Buttons if we have any.
			HandleReplys ();
		}

		private bool CheckLastDialogue () {
			// Need an initial check to see if we are on the last link and if we are we reset and return.
			if (isLastLink) {
				//// IF completing this dialogue is part of a quest.
				//if (questToBeCompletedID != -1) {
				//	// Send this to go to the next part.
				//	Grid_Helper.questManager.QuestTalking (this, questToBeCompletedID, GetComponent<Quest_TalkPart> ().GetQuestPart ());
				//}
				// Turn the dialogue off.
				Grid_Helper.dialogueData.DisplayDialoguePanel (false);
				// This is the last dialogue.
				return true;
			}

			// IF there are 0 link nodes. (This is the last of the dialogue.)
			if (GetNodeLinkSize () == 0) {
				// Mark it so we know we are on the last Node.
				isLastLink = true;
			}

			// This isnt the last dialogue.
			return false;
		}

		private void HandleReplys () {
			// IF there are replies and we let those dictate the direction in our dialogue tree,
			// ELSE there are no replies then we are basically just wanting to display text.
			if (GetReplySize () != 0) {
				// We have choices, so lets halt the process of our dialogue based on selection input.
				isDialogueReplyShowing = true;
				// Loop the amount of times we have replys.
				for (int i = 0; i < GetReplySize (); i++) {
					// Lets create our reply setup.
					GameObject replyGameObject = Grid_Helper.dialogueData.CreateReplyButton (dialogueTreeList [dialogueNode].replies [i]);
					// IF we have a node link to match with the reply.
					if (GetNodeLinkSize () > i) {
						// Store the link node.
						Grid_Helper.dialogueData.AssignReplyLink (replyGameObject, dialogueTreeList [dialogueNode].nodeLinks [i]);
					}
				}
				// Lets display our reply panel.
				Grid_Helper.dialogueData.DisplayReplyPanel (true);
			} else {
				// We have NO choices, so lets continue the process of our dialogue.
				isDialogueReplyShowing = false;
				// Lets remove our reply panel.
				Grid_Helper.dialogueData.DisplayReplyPanel (false);
				// IF we are not on our last link then we can safely get the next node.
				if (!isLastLink) {
					// Set the next node in the dialogue to be what the first nodelink is as there will be only 1.
					SetDialogueNode (GetFirstLink ());
				}
			}
		}

		private void AssignDialogue () {
			// Lets fill the dialogue with the name of the person/thing you are talking to, the prompt/dialogue that person/thing says and an image (optional).
			Grid_Helper.dialogueData.AssigningDialogue (entityName, dialogueTreeList[dialogueNode].prompt, dialogueTreeList[dialogueNode].portrait);
		}

		private int GetDialogueNode () {
			// Return the location where we are in our dialogue.
			return dialogueNode;
		}

		/// <summary>
		/// Sets the dialogue node.
		/// </summary>
		public void SetDialogueNode (int _node) {
			dialogueNode = _node;
		}

		/// <summary>
		/// Sets the is dialogue reply showing.
		/// </summary>
		public void SetIsDialogueReplyShowing (bool _isDialogueReplyShowing) {
			isDialogueReplyShowing = _isDialogueReplyShowing;
		}

		private int GetReplySize () {
			return dialogueTreeList [dialogueNode].replies.Length;
		}

		private int GetNodeLinkSize () {
			return dialogueTreeList [dialogueNode].nodeLinks.Length;
		}

		private int GetFirstLink () {
			return dialogueTreeList [dialogueNode].nodeLinks [0];
		}

		public string GetStartingDialogue () {
			return startingDialogue;
		}

		public void ResetDialogue () {
			// Stop all coroutines.
			StopAllCoroutines ();
			// Reset our dialogue showing bool.
			isDialogueReplyShowing = false;
			// Reset our isLastLink.
			isLastLink = false;
			// We are not in a transition anymore.
			isTextTransitionIn = false;
			// Reset the counter.
			counter = 0;
			// Reset the dialogueNode.
			dialogueNode = 0;
			// Lets clear what we have at this moment and refill it.
			dialogueTreeList.Clear ();
			// IF we have not filled in our dialogue data yet.
			if(dialogueData != null){
				// Lets clear our JSON Data.
				dialogueData.Clear ();
			}
		}

		/// <summary>
		/// Gets the prompt.
		/// </summary>
		private string GetPrompt (int index, string attribute) {
			// IF this dialogue has the attribute.
			if(dialogueData [index].Keys.Contains(attribute)){
				// Return the string prompt.
				return (string)dialogueData [index] [attribute];
			}
			// Return.
			return "";
		}

		private string GetPortrait (int index, string attribute) {
			// Since we are grabbing a single string we just call a method we already have done for getting the string from the JSON.  
			// We make this method though just to help out with visually showing what is happening.
			return GetPrompt (index, attribute);
		}

		private int GetQuestCompleteID (int index, string attribute) {
			// IF this dialogue has the attribute.
			if(dialogueData [index].Keys.Contains(attribute)){
				// Return the int.
				return (int)dialogueData [index] [attribute];
			}
			// Return that we found nothing.
			return -1;
		}

		/// <summary>
		/// Gets the replies.
		/// </summary>
		private string[] GetReplies (int index, string attribute) {
			// IF this dialogue has the attribute.
			if (dialogueData [index].Keys.Contains (attribute)) {
				// Lets create an array the size of the amount of replies we have in our json file.
				string[] replyArray = new string[dialogueData [index] [attribute].Count];
				// Loop based on the amount of replies we have.
				for (int i = 0; i < replyArray.Length; i++) {
					// Get each reply and store it in the array.
					replyArray [i] = (string) dialogueData [index] [attribute] [(i + 1).ToString ()];
				}
				// Return the string[] replies.
				return replyArray;
			}
			// Return an empty array.
			return new string[0];
		}

		/// <summary>
		/// Gets the node links.
		/// </summary>
		private int[] GetNodeLinks (int index, string attribute) {
			// IF this dialogue has the attribute.
			if (dialogueData [index].Keys.Contains (attribute)) {
				// Lets create an array the size of the amount of links we have in our json file.
				int[] linkArray = new int[dialogueData[index] [attribute].Count];
				// Loop based on the amount of links we have.
				for (int i = 0; i < linkArray.Length; i++) {
					// Get each link and store it in the array.
					linkArray [i] = (int)dialogueData [index] [attribute] [(i + 1).ToString ()];
				}
				// Return the int[] links.
				return linkArray;
			}
			// Return an empty array.
			return new int[0];
		}

		/// <summary>
		/// Dialogue node.
		/// </summary>
		public class DialogueNode {
			public string prompt;
			public string[] replies;
			public int[] nodeLinks;
			public Sprite portrait;

			public DialogueNode (string _prompt, string[] _replies, int[] _nodeLinks, string _portraitString) {
				this.prompt = _prompt;
				this.replies = _replies;
				this.nodeLinks = _nodeLinks;
				this.portrait = Grid_Helper.setup.GetSprite(_portraitString);
			}
		}
	}
}
