using UnityEngine;
using System.Collections;

namespace TrollBridge {

	/// <summary>
	/// The way this script works isn't actually playing sounds if you are to just straight up enable or disable.  
	/// There are 2 methods in here to call for the appropriate sound to play.  
	/// If this script played sounds when it is enabled or disabled then when you start or leave your scene all the sounds will play 
	/// at once and to make sure that doesn't happen requires coding restrictions. So the simplest way is to just have 2 
	/// methods and call them if this script exists on the GameObject from other scripts.
	/// </summary>
	public class Play_SoundOnActivation : MonoBehaviour {

		[Tooltip("The sounds to play when this GameObject is turned Active.")]
		public AudioClip[] turningActiveSound;
		[Tooltip("The sounds to play when this GameObject is turned Inactive.")]
		public AudioClip[] turningInactiveSound;


		/// <summary>
		/// Plays the active sounds.
		/// </summary>
		public void PlayActiveSounds(){
			// Loop through all the sounds you want to play while this GameObject is being Activated.
			for(int i = 0; i < turningActiveSound.Length; i++){
				// Play the active sounds.
				Grid_Helper.soundManager.PlaySound (turningActiveSound[i]);
			}
		}

		/// <summary>
		/// Play the inactive sounds.
		/// </summary>
		public void PlayInactiveSounds(){
			// Loop through all the sounds you want to play while the GameObject is being Deactivated.
			for(int i = 0; i < turningInactiveSound.Length; i++){
				// Play the inactive sound.
				Grid_Helper.soundManager.PlaySound (turningInactiveSound[i]);
			}
		}
	}
}
