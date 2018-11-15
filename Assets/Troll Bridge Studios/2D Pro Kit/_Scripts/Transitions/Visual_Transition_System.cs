using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.ImageEffects;

namespace TrollBridge {

	/// <summary>
	/// Visual transition system handles the transitions between scenes with having a "Video Transition" look to it. Example being a "Fade" from one scene to another, "Slide" one scene to the next and a "Twirl" effect
	/// such as entering combat in Final Fantasy 7.
	/// </summary>
	public class Visual_Transition_System : MonoBehaviour {

		[Header ("Fade Transition")]
		// The GameObject that represents your Fade.
		public UnityEngine.UI.Image fadeGameObject;
		// The time for the full fade. Example with the current default I put as 0.2f    =>    0.2 = fade in (0.1) + fade out (0.1).
		public float fadeTime = 0.6f;

		[Header ("Slide Transition")]
		// The sliding GameObject.
		public GameObject slideGameObject;
		// The time it takes for the slide to happen. 
		public float slideTime;
		// Since our visual slide starts off screen we initially grab the Transform of that position so we have a spot to bring it when we want to slide it off.
		public Transform slideStart;
		// The location for our slideGameObject to move to.
		public Transform slideTo;

		[Header ("Twirl Transition")]
		// The GameObject that has the script that does the twirl effect.
		public Twirl twirlCamera;
		// How long you want the twirl time to be.
		public float twirlTime;


		/// <summary>
		/// Fade from one scene to another.
		/// </summary>
		public IEnumerator FadeCycle (string newScene, int sceneSpawnLocation) {
			// Fade out.
			yield return StartCoroutine(GUI_Helper.FadeImage (fadeGameObject, fadeTime / 2f, 0f, 1f));
			// Change the scene.
			Grid_Helper.helper.ChangeScene (newScene, sceneSpawnLocation);
			// Fade back in.
			yield return StartCoroutine(GUI_Helper.FadeImage (fadeGameObject, fadeTime / 2f, 1f, 0f));
		}

		/// <summary>
		/// Slides from one scene to another.
		/// </summary>
		public IEnumerator SlideCycle (string newScene, int sceneSpawnLocation) {
			// Slide in.
			yield return StartCoroutine (Grid_Helper.helper.SmoothStepGameObject (slideGameObject.transform, slideStart, slideTo, slideTime / 2f));
			// Change the scene.
			Grid_Helper.helper.ChangeScene (newScene, sceneSpawnLocation);
			// Slide out.
			yield return StartCoroutine (Grid_Helper.helper.SmoothStepGameObject (slideGameObject.transform, slideTo, slideStart, slideTime / 2f));
		}

		/// <summary>
		/// Twirls the cycle.
		/// </summary>
		public IEnumerator TwirlCycle (string newScene, int sceneSpawnLocation) {
			// Twirl in.
			yield return StartCoroutine (Grid_Helper.helper.TwirlSmoothStep (twirlCamera, twirlTime / 2f, twirlCamera.angle, 180f));
			// Change the scene.
			Grid_Helper.helper.ChangeSceneAsync (newScene, sceneSpawnLocation);
			// Twirl out.
			yield return StartCoroutine (Grid_Helper.helper.TwirlSmoothStep (twirlCamera, twirlTime / 2f, twirlCamera.angle, 0f));
		}
	}
}
