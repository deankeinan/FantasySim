using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace TrollBridge {

	/// <summary>
	/// Helper script that will deal with manipulation of anything that deals with Images and Texts.
	/// </summary>
	public static class GUI_Helper {

		// Fading for images.
		public static IEnumerator FadeImage(UnityEngine.UI.Image image, float fadeTime, float from, float to){
			// IF we have a null image.
			if(image == null){
				yield break;
			}
			// Get the images color.
			Color col = image.color;
			// Loop in a SmoothStep manner to get a smooth fade.
			for (float x = 0.0f; x < 1.0f; x += Time.deltaTime / fadeTime) {
				// IF the Image is destroyed before it is finished fading.
				if (image == null) {
					yield break;
				}
				// Smooth the alpha.
				col.a = Mathf.SmoothStep (from, to, x);
				// Set the color.
				image.color = col;
				yield return null;
			}
			Color endColor = image.color;
			endColor.a = to;
			image.color = endColor;
		}

		// Fading for text.
		public static IEnumerator FadeText(Text txt, float fadeTime, float from, float to){
			// IF we have a null text.
			if(txt == null){
				yield break;
			}
			// Get the text color.
			Color col = txt.color;
			// Loop through a fadeTime interval for fading text.
			for(float x = 0.0f; x < 1.0f; x += Time.deltaTime / fadeTime){
				// IF the Image is destroyed before it is finished fading.
				if(txt == null){
					yield break;
				}
				// Smooth the alpha
				col.a = Mathf.SmoothStep(from, to, x);
				// Set the color.
				txt.color = col;
				yield return null;
			}
			Color endColor = txt.color;
			endColor.a = to;
			txt.color = endColor;
		}

		// Grow/Shrink for images.
		public static IEnumerator GrowShrinkImage(UnityEngine.UI.Image image, float resizeTime, float fromX, float fromY, float toX, float toY){
			// IF we have a null image.
			if(image == null){
				yield break;
			}
			// Loop through a resizeTime interval for growing and shrinking images.
			for(float x = 0.0f; x < 1.0f; x += Time.deltaTime / resizeTime){
				// IF the image is destroyed before it is finished resizing.
				if(image == null){
					yield break;
				}
				// Smooth the sizing.
				image.rectTransform.localScale = new Vector2(Mathf.SmoothStep(fromX, toX, x), Mathf.SmoothStep(fromY, toY, x));
				yield return null;
			}
			// Make sure we reach the our destination by manually setting the end point incase the loop didnt finish on x = 1f.
			image.rectTransform.localScale = new Vector3(toX, toY, 1f);
		}

		// Make the text type out based on the text speed.
		public static IEnumerator TypeText(Text txt, float pauseTime, string dialogue, AudioClip typeSound){
			// IF we have a null text.
			if(txt == null){
				// We leave.
				yield break;
			}
			// Set a blank text.
			txt.text = "";
			// Loop and type out the text.
			for(int i = 0; i <= dialogue.Length; i++){
				txt.text = dialogue.Substring(0, i);
				Grid_Helper.soundManager.PlaySound(typeSound, Character_Helper.GetPlayer().transform.position, 1f, 1f);
				yield return new WaitForSeconds(pauseTime);
				yield return null;
			}
		}

		/// <summary>
		/// Get the UI length by searching with the 'uiTag' for the GameObject we want and the boolean 'width' to let us know if we want to get the size of the width or the height of the rectangle.
		/// </summary>
		public static float GetUILength(string uiTag, bool width){
			// IF we have a empty string.
			if (string.IsNullOrEmpty (uiTag)) {
				// GTFO as we do not have a tag to search for.
				return 0f;
			}
			// Look for the GameObject with the Tag uiTag.
			GameObject go = GameObject.FindGameObjectWithTag (uiTag);
			//  IF our GameObject 'go' exists.
			if (go != null) {
				// IF this GameObject has a RectTransform component. (It should if it's our UI.),
				// ELSE let the developer know a potential fix.
				if (go.GetComponent<RectTransform> () != null) {
					// IF we want the length of the width,
					// ELSE we want the lenght of the height.
					if (width) {
						// return the width.
						return go.GetComponent<RectTransform> ().rect.width;
					} else {
						// return the height.
						return go.GetComponent<RectTransform> ().rect.height;
					}
				} else {
					// Return 0f.
					return 0f;
				}
			}
			// Return 0f.
			return 0f;
		}

		/// <summary>
		/// Get the UI length by searching with the 'uiTag' for the GameObject we want and the boolean 'width' to let us know if we want to get the size of the width or the height of the rectangle.
		/// This will use ratios to determine the percent of the UI compared to the screen size.
		/// </summary>
		public static float GetUILengthRatio(string uiTag, bool width, Camera camera){
			// IF we have a empty string.
			if (string.IsNullOrEmpty (uiTag)) {
				// GTFO as we do not have a tag to search for.
				return 0f;
			}
			// Look for the GameObject with the Tag uiTag.
			GameObject go = GameObject.FindGameObjectWithTag (uiTag);
			//  IF our GameObject 'go' exists.
			if (go != null) {
				// IF this GameObject has a RectTransform component. (It should if it's our UI.),
				// ELSE let the developer know a potential fix.
				if (go.GetComponent<RectTransform> () != null) {
					// IF we want the length of the width,
					// ELSE we want the lenght of the height.
					if (width) {
						// return the width based on width percentage.
						return ((Screen.width / (camera.aspect * (camera.orthographicSize*2))) / 100) * go.GetComponent<RectTransform> ().rect.width;
					} else {
						// return the height.
						return ((Screen.height / (camera.orthographicSize*2)) / 100) * go.GetComponent<RectTransform> ().rect.height;
					}
				} else {
					// Return 0f.
					return 0f;
				}
			}
			// Return 0f.
			return 0f;
		}
	}
}
