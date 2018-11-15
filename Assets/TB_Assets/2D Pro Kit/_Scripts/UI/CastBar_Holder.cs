using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrollBridge {

	/// <summary>
	/// Script used to hold and make easier reference to our progressbar and our progresstext of a cast bar.
	/// </summary>
	public class CastBar_Holder : MonoBehaviour {

		[Tooltip ("The Transform of our Progress Bar as that will be moved to show progress.")]
		public Transform progressBar;
		[Tooltip ("The Text to our Progress Bar as that will be altered show progress.")]
		public Text progressText;
	}
}
