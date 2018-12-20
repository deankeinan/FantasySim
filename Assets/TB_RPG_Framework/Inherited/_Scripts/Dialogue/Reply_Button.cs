using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	/// <summary>
	/// Reply button allows us to reply to a comment in our dialogue to proceed to the next dialogue or finish the dialogue.
	/// </summary>
	public class Reply_Button : MonoBehaviour {

		[ReadOnlyAttribute] [SerializeField] private int linkNode = -1;

		/// <summary>
		/// Sets the link node.
		/// </summary>
		public void SetLinkNode (int _linkNode) {
			linkNode = _linkNode;
		}

		/// <summary>
		/// Gets the link node.
		/// </summary>
		public int GetLinkNode () {
			return linkNode;
		}

		/// <summary>
		/// Sends the link for next dialogue.
		/// </summary>
		public void SendLinkForNextDialogue () {
			// Lets go to the next dialogue.
			Grid_Helper.dialogueData.NextDialogue (GetLinkNode ());
		}
	}
}
