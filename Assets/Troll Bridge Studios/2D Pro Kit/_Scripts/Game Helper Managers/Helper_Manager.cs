using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityStandardAssets.ImageEffects;


namespace TrollBridge {

	/// <summary>
	/// Helper manager is a script where we place methods where we find ourselves using these methods multiple times in the project.  So to prevent wasted time in which we would have to go to each script and make changes we will have it here so that
	/// we only need to make a change in 1 area, which is here.
	/// </summary>
	public class Helper_Manager : MonoBehaviour {

		/// <summary>
		/// Clears all saved data.
		/// </summary>
		public void ClearAllSavedData(){
			// Remove all saved Player Prefs.
			PlayerPrefs.DeleteAll();
			// Make sure the current options are replaced into the PlayerPrefs.
			Grid_Helper.optionManager.SaveOptionSettings ();
			// Make sure the current keybinds are replaced into the PlayerPrefs.
			Grid_Helper.kBinds.SaveKeybinds ();

			// Clear the Setup.
			Grid_Helper.setup.ClearSetup();
			// Clear any State Lists.
			Grid_Helper.stateManager.ClearList();
		}


		public void ReloadManagers () {
			// Load our quests
			//Grid_Helper.questManager.StartQuest ("Quest_Status");
			// Load our crafts.
			//Grid_Helper.craftingManager.Load ("Craft_Status");
			// Load our ActionBars
			//Grid_Helper.actionbarManager.LoadActionBars ();
		}

		public bool DidWeWin (float percent) {
			// Multiplier to handle decimal drop rates.
			int multiplier = 0;
			// WHILE we have a decimal.
			while(percent % 10 != 0){
				// Increase the multiplier.
				++multiplier;
				// Multiply the drop percent by 10 and recheck to see if we still have a decimal.
				percent *= 10;
			}

			// Based on the multiplier lets get our top range.
			int topRange = (int) Mathf.Pow (10f, 2 + multiplier);
			// Get a random number from 1 to the topRange.
			int dropNumber = UnityEngine.Random.Range(1, topRange + 1);
			// IF the odds are in the drops favor.
			if (dropNumber <= percent) {
				// WE WIN... WOOOT WOOT.
				return true;
			} else {
				// We lost... Now take a lap around the stadium.
				return false;
			}
		}

		/// <summary>
		/// This method makes checks to see if this Character_Manager is related to our main player/hero.
		/// </summary>
		public bool IsPlayer (Character_Manager charaManager) {
			// IF the colliding gameobject doesnt have a Character_Manager Component,
			// ELSE IF the colliding gameobject's CharacterType is the Hero/Player.
			// ELSE everything else is false as we would of found the player in our previous ELSE IF.
			if (charaManager == null) {
				// The colliding object is not a Character so return false.
				return false;
			} else if (charaManager.characterType == CharacterType.Hero) {
				// We have found the player/hero so return true.
				return true;
			} else {
				// Not the player.
				return false;
			}
		}

		// Specific method only for the Twirl Component.
		public IEnumerator TwirlSmoothStep (Twirl twirlCamera, float twirlTime, float angleStart, float angleEnd) {
			// Loop for how ever long twirlTime is.
			for(float x = 0f; x < 1.0f; x += Time.deltaTime / twirlTime){
				// Alter the angle based on the time.
				twirlCamera.angle = Mathf.SmoothStep(angleStart, angleEnd, x);
				yield return null;
			}
			// Make sure the angle is fully at the end.
			twirlCamera.angle = angleEnd;
		}

		/// <summary>
		/// Lerps the GameObjects Transform 'goTransform' from StartLocation to EndLocation with the time it takes to do so being 'moveTime'.
		/// </summary>
		public IEnumerator SmoothStepGameObject (Transform goTransform, Transform StartLocation, Transform EndLocation, float moveTime) {
			// Loop for how ever long moveTime is.
			for(float x = 0f; x < 1.0f; x += Time.deltaTime / moveTime){
				// Move to the EndLocation in a SmoothStep fashion.
				goTransform.position = new Vector3 (
					Mathf.SmoothStep(StartLocation.position.x, EndLocation.position.x, x),
					Mathf.SmoothStep(StartLocation.position.y, EndLocation.position.y, x),
					Mathf.SmoothStep(StartLocation.position.z, EndLocation.position.z, x));
				yield return null;
			}
			// Make sure the GameObject has fully reached it destination.
			goTransform.position = EndLocation.position;
		}

		/// <summary>
		/// Rotates the sprite based on the time rotateTime on the z-axis.
		/// </summary>
		public IEnumerator RotateSprite(Transform _transform, float rotateTime, float rotateFrom, float rotateTo){
			// IF we have a null Transform.
			if(_transform == null){
				yield break;
			}
			// Loop in a lerp manner to get a smooth rotation.
			for(float x = 0.0f; x < 1.0f; x += Time.deltaTime / rotateTime){
				// IF the transform is destroyed before finishing rotation.
				if(_transform == null){
					yield break;
				}
				_transform.eulerAngles = new Vector3 (_transform.eulerAngles.x, _transform.eulerAngles.y, Mathf.SmoothStep(rotateFrom, rotateTo, x));
				yield return null;
			}
		}

		/// <summary>
		/// Sets the time scale.
		/// </summary>
		public void SetTimeScale(float timeScale){
			// Set the time scaling.
			Time.timeScale = timeScale;
		}

		/// <summary>
		/// Instantiates 'objectToSpawn' at position 'pos' and set its layer to 'layer'.
		/// </summary>
		public GameObject SpawnObject(GameObject objectToSpawn, Vector3 pos, int layer){
			// Spawn the Object.
			GameObject goObject = Instantiate (objectToSpawn) as GameObject;
			// Set the position.
			goObject.transform.position = pos;
			// Set the layer to the GameObject that created this Object.
			goObject.layer = layer;
			// Return the Object.
			return goObject;
		}

		/// <summary>
		/// We create a methode for spawning GameObjects due to the monotonous work that would be done when anything is spawned.  
		/// We need to make the GameObject and its children's layers that are being created be the same as the GameObject who 
		/// spawned it and its Sprite Renderer sorting layers name needs to be the same as well.
		/// </summary>
		public GameObject SpawnObject(GameObject objectToSpawn, Vector3 pos, Quaternion quat, GameObject dropper){
			// Spawn the Object.
			GameObject goObject = Instantiate (objectToSpawn, pos, quat) as GameObject;
			// Set the layer to the GameObject that created this Object.
			goObject.layer = dropper.layer;
			// IF the goObject GameObject has a Sprite Renderer AND IF the GameObject that spawned the goObject has a Sprite Renderer as well.
			if (goObject.GetComponent<SpriteRenderer> () != null && dropper.GetComponent<SpriteRenderer> () != null) {
				// Set the sorting layer name to the GameObject that created this Object.
				goObject.GetComponent<SpriteRenderer> ().sortingLayerName = dropper.GetComponent<SpriteRenderer> ().sortingLayerName;
			}
			// Loop through the amount of children this GameObject has.
			for (int i = 0; i < goObject.transform.childCount; i++) {
				// Set the childrens layer to the layer of the entity that dropped this GameObject.
				goObject.transform.GetChild (i).gameObject.layer = dropper.layer;
				// IF the goObject GameObject has a Sprite Renderer AND IF the GameObject that spawned the goObject has a Sprite Renderer as well.
				if (goObject.transform.GetChild (i).GetComponent<SpriteRenderer> () != null && dropper.GetComponent<SpriteRenderer> () != null) {
					// Set the sorting layer name to the GameObject that created this Object.
					goObject.transform.GetChild (i).GetComponent<SpriteRenderer> ().sortingLayerName = dropper.GetComponent<SpriteRenderer> ().sortingLayerName;
				}
			}
			// Return the Object.
			return goObject;
		}

		/// <summary>
		/// We create a methode for spawning GameObjects due to the monotonous work that would be done when anything is spawned.  
		/// We need to make the GameObject and its children's layers that are being created be the same as the GameObject who 
		/// spawned it and its Sprite Renderer sorting layers name needs to be the same as well.
		/// 
		/// This method spawns GameObjects on the circumference of a circle with a radius of "float radius".
		/// </summary>
		public GameObject SpawnObject(GameObject objectToSpawn, Vector3 pos, Quaternion quat, GameObject dropper, float radius){
			// Get a random spot with a radius around the pos.
			Vector3 randomPos = RandomCircle (pos, radius);
			// Spawn the Object.
			GameObject goObject = Instantiate (objectToSpawn, randomPos, quat) as GameObject;
			// Set the layer to the GameObject that created this Object.
			goObject.layer = dropper.layer;
			// IF the goObject GameObject has a Sprite Renderer AND the GameObject that spawned the goObject has a Sprite Renderer as well.
			if (goObject.GetComponent<SpriteRenderer> () != null && dropper.GetComponent<SpriteRenderer> () != null) {
				// Set the sorting layer name to the GameObject that created this Object.
				goObject.GetComponent<SpriteRenderer> ().sortingLayerName = dropper.GetComponent<SpriteRenderer> ().sortingLayerName;
			}
			// Loop through the amount of children this gameobject has.
			for (int i = 0; i < goObject.transform.childCount; i++) {
				// Set the childrens layer to the layer of the entity that dropped this GameObject.
				goObject.transform.GetChild (i).gameObject.layer = dropper.layer;
				// IF the goObject GameObject has a Sprite Renderer AND the GameObject that spawned the goObject has a Sprite Renderer as well.
				if (goObject.transform.GetChild (i).GetComponent<SpriteRenderer> () != null && dropper.GetComponent<SpriteRenderer> () != null) {
					// Set the sorting layer name to the GameObject that created this Object.
					goObject.transform.GetChild (i).GetComponent<SpriteRenderer> ().sortingLayerName = dropper.GetComponent<SpriteRenderer> ().sortingLayerName;
				}
			}
			// Return the Object.
			return goObject;
		}

//		/// <summary>
//		/// Method that will generate a random 1 word string.  The allowed characters are a-z, A-Z and 0-9.  You can set the length of the string by the int "length".
//		/// </summary>
//		public string GenerateRandomString(int length, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"){
//			// IF the length is 0.
//			if(length == 0) throw new System.ArgumentOutOfRangeException("length", "Length cannot be less than 0.");
//			// IF the allowedChars is empty or null.
//			if(System.String.IsNullOrEmpty(allowedChars)) throw new System.ArgumentException("allowedChars cannot be empty.");
//			// Create a constant variable for our byteSize (0x100 = 256, 0-255).
//			const int byteSize = 0x100;
//			// Create an array of the HastSets of allowedChars.
//			var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
//			// IF our allowedCharSet length is larger than our byteSyze.
//			if(byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters", byteSize));
//
//			// Use a cryptographically-secure random number generator, so now the caller is protected.
//			using (var rngg = new System.Security.Cryptography.RNGCryptoServiceProvider())
//			{
//				// Create a StringBuilder
//				var result = new System.Text.StringBuilder ();
//				var buff = new byte[128];
//				// While the length of the result is less than the length of how long we want our string to be.
//				while(result.Length < length)
//				{
//					// Have our rng get the bytes of our byte[].
//					rngg.GetBytes (buff);
//					// For as long as our i is less than the length of our byte[] AND the result of the length is less than the length of the string we want.
//					for(var i = 0; i < buff.Length && result.Length < length; ++i)
//					{
//						// Divide the byte into allowedCharSet sized groups.
//						var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
//						// IF we have biasing.
//						if (outOfRangeStart <= buff [i])
//							// Go to next iteration.
//							continue;
//						// Add the character to our result.
//						result.Append (allowedCharSet[buff[i] % allowedCharSet.Length]);
//					}
//				}
//				// Return our final string.
//				return result.ToString ();
//			}
//		}

//		/// <summary>
//		/// Generate a set of keys for encrypting and decrypting the data to be harder to manipulate.
//		/// </summary>
//		public void GenerateNewKeys(){
//			// Our security keys for Cryptography security.
//			string publicKey;
//			string publicAndPrivateKey;
//			// This is where the magic happens for pooping out our keys.
//			AsymmetricEncryption.GenerateKeys (1024, out publicKey, out publicAndPrivateKey);
//			// Save these keys.
//			PlayerPrefs.SetString ("PK", publicKey);
//			PlayerPrefs.SetString ("PAPK", publicAndPrivateKey);
//		}

		/// <summary>
		/// Returns a random Vector3 that is at position 'center' and 'radius' distance away.
		/// </summary>
		public Vector3 RandomCircle(Vector3 center, float radius){
			float ang = UnityEngine.Random.Range(0, 360);
			Vector3 pos = Vector3.zero;
			pos.x = center.x + radius * Mathf.Cos (ang * Mathf.Deg2Rad);
			pos.y = center.y + radius * Mathf.Sin (ang * Mathf.Deg2Rad);
			pos.z = center.z;
			return pos;
		}

		/// <summary>
		/// Checks the state of our GameObject to know if it is placed or not and if we need to Unregister it.
		/// </summary>
		public void CheckState(bool isPlaced, GameObject stateGO){
			// IF this item was placed in the scene by the developer lets just make it inactive as if we want to save this item's state we cannot destroy it.
			// ELSE this item was created during gameplay, so to prevent memory leaks and/or errors we unregister and destroy the gameobject so we dont save an inactive GameObject that will never be active in the scene.
			if (isPlaced) {
				// Set the gameobject inactive.
				stateGO.SetActive (false);
			} else {
				// IF there is a State Transform script.
				if (stateGO.GetComponent<State_Transform> () != null) {
					// Unregister this item as when we pick it up it gets destroyed as there 
					// is no need for an item that you just picked up.
					Grid_Helper.stateManager.Unregister (stateGO);
				}
				// We destroy what is out there.
				Destroy (stateGO);
			}
		}

		/// <summary>
		/// Currently in the Demo we save valuable information when we transfer to another scene only as this is to prevent any unwanted hiccups while playing.  
		/// When you enter an area that is where you will start if your game were to crash or if the player wants to stop playing and come back later 
		/// (Remember this is only in the demo, you can have your save setup however you want).
		/// </summary>
		public void ChangeScene(string newScene, int spawnLocation){
			// IF there is a player.
			if(Character_Helper.GetPlayer() != null){
				// Save the player setup.
				SavePlayerSetup();
			}
			// Save any scene based information.
			SaveSceneSetup (newScene, spawnLocation);
			// Save the player prefs to disk.
			PlayerPrefs.Save ();
			// Change the scene.
			SceneManager.LoadScene (newScene);
		}

		/// <summary>
		/// Currently in the Demo we save valuable information when we transfer to another scene only as this is to prevent any unwanted hiccups while playing.  
		/// When you enter an area that is where you will start if your game were to crash or if the player wants to stop playing and come back later 
		/// (Remember this is only in the demo, you can have your save setup however you want).
		/// 
		/// This only differs by loading the scene with LoadSceneAsync as it allows for a smoother transfer of scene loading. 
		/// If you have some sort of animation happening like a twirling/spinning camera while you want to change scenes like in some Turn-Based games like Final Fantasy.
		/// </summary>
		public void ChangeSceneAsync(string newScene, int spawnLocation){
			// IF there is a player.
			if (Character_Helper.GetPlayer () != null) {
				// Save the player setup.
				SavePlayerSetup ();
			}
			// Save any scene based information.
			SaveSceneSetup (newScene, spawnLocation);
			// Save the player prefs to disk.
			PlayerPrefs.Save ();
			// Change the scene in a more smooth manner.
			SceneManager.LoadSceneAsync (newScene);
		}

		private void SavePlayerSetup(){
			// Save the player data.
			Character_Helper.GetPlayerManager ().GetComponent<Character_Manager> ().SavePlayer ();
			// Save the action bars.
			//Grid_Helper.actionbarManager.SaveActionBars ();
		}

		private void SaveSceneSetup(string newScene, int spawnLocation){
			// Save Quest information.
			SaveQuestSetup ();
			// Save Craft information.
			SaveCraftSetup ();

			// IF we are using our Setup script.
			if (Grid_Helper.setup != null) {
				// Set the scene the player is about to go to.
				Grid_Helper.setup.SetSceneStartName (newScene);
				// Set the sceneSpawnLocation.
				Grid_Helper.setup.SetSceneSpawnLocation (spawnLocation);
				// Save the Setup.
				Grid_Helper.setup.Save ();
			}

			// IF we are using our State Manager script.
			if (Grid_Helper.stateManager != null) {
				// Save the changes to the State Handler.
				Grid_Helper.stateManager.Save ();
				// Clear the list of our State Handlers.
				Grid_Helper.stateManager.ClearList ();
			}
		}

		private void SaveQuestSetup () {
			// Save our quest.
			//Grid_Helper.questManager.Save ("Quest_Status");
		}

		private void SaveCraftSetup () {
			// Save our craft.
			//Grid_Helper.craftingManager.Save ("Craft_Status");
		}

		/// <summary>
		/// Launches a GameObject in the direction based on the x and y min/max.
		/// </summary>
		public void LaunchItem(GameObject item, float xMin, float xMax, float yMin, float yMax) {
			float x = UnityEngine.Random.Range (xMin, xMax);
			float y = UnityEngine.Random.Range (yMin, yMax);
			item.GetComponent<Rigidbody2D> ().AddForce (new Vector2(x, y) * 400f);
		}

		/// <summary>
		/// Launches a GameObject in the opposite direction of 'pos'.
		/// </summary>
		public void LaunchAwayFromPosition(GameObject item, Vector3 pos, float forcePower){
			Vector2 normPos = new Vector2 (item.transform.position.x - pos.x, item.transform.position.y - pos.y).normalized;
			item.GetComponent<Rigidbody2D> ().AddForce (normPos * forcePower);
		}

		// Make a method for setting parent for objects.
		public void SetParentTransform(Transform parent, GameObject child){
			// IF we actually have a child gameobject and a parent to place it in.
			if (child != null && parent != null) {
				// Set its parent to the current child GameObject.
				child.transform.SetParent (parent);
			}
		}
			
		public void DestroyCharacterManagerGameObject (GameObject animatorGO) {
			// IF you have a setup to where you have a Character_Manager as your parent to this gameobject,
			// ELSE IF you have a setup to where you have a Character_Manager on this gameobject,
			// ELSE there is no Character_Manager and we just destroy this gameobject.
			if (animatorGO.GetComponentInParent <Character_Manager> () != null) {
				// Destroy this GameObject.
				Destroy (animatorGO.GetComponentInParent <Character_Manager> ().gameObject);
			} else if (animatorGO.GetComponent <Character_Manager> ()) {
				// Destroy this GameObject.
				Destroy (animatorGO.gameObject);
			} else {
				// Destroy this GameObject.
				Destroy (animatorGO.gameObject);
			}
		}

		// Remove all the gameobjects by tags.
		public void DestroyGameObjectsByTags(string[] destroyTags){
			// Loop through all the destroy tags.
			for (int i = 0; i < destroyTags.Length; i++) {
				// Grab all GameObjects with the tags for being destroyed.
				GameObject[] objects = GameObject.FindGameObjectsWithTag (destroyTags [i]);
				// Loop through the array.
				for (int j = 0; j < objects.Length; j++) {
					// Destroy each object.
					Destroy (objects [j]);
				}
			}
		}

		// Destroy gameobjects by parent.
		public void DestroyGameObjectsByParent(GameObject parent){
			// Get each child.
			foreach(Transform child in parent.transform){
				// Destroy each child.
				Destroy (child.gameObject);
			}
		}

		// Set the gameobjects activity by tags.
		public void SetActiveGameObjectsByTag(string activeTag, bool isActive){
			// Grab all GameObjects with this tag.
			GameObject[] objects = GameObject.FindGameObjectsWithTag (activeTag);
			// Loop through all the GameObjects with this same tag.
			for (int j = 0; j < objects.Length; j++) {
				// Play any sounds when activating or deactivating.
				PlaySoundActiveness (objects [j], isActive);
				// Set the active of each object.
				objects [j].SetActive (isActive);
			}
		}

		// Set the gameobjects activity by tags.
		public void SetActiveGameObjectsByTags(string[] activeTags, bool isActive){
			// Loop through all the active tags.
			for(int i = 0; i < activeTags.Length; i++){
				// Grab all GameObjects with this tag.
				GameObject[] objects = GameObject.FindGameObjectsWithTag(activeTags[i]);
				// Loop through all the GameObjects with this same tag.
				for(int j = 0; j < objects.Length; j++){
					// Play any sounds when activating or deactivating.
					PlaySoundActiveness(objects[j], isActive);
					// Set the active of each object.
					objects[j].SetActive(isActive);
				}
			}
		}

		// Set the gameobjects activity by parent
		public void SetActiveGameObjectsByParent(GameObject parent, bool isActive){
			// Get the amount of children.
			int children = parent.transform.childCount;
			// Loop the amount of times as you have children.
			for(int i = 0; i < children; i++){
				// Play any sounds when activating or deactivating.
				PlaySoundActiveness(parent.transform.GetChild (i).gameObject, isActive);
				// Set the actual activity of this gameobject.
				parent.transform.GetChild (i).gameObject.SetActive (isActive);
			}
		}


		// Set the GameObjects activity.
		public void SetActiveGameObject(GameObject gameObjectToActivate, bool isActive){
			// Play any sounds when activating or deactivating.
			PlaySoundActiveness(gameObjectToActivate, isActive);
			// Set the actual activity of this gameobject.
			gameObjectToActivate.SetActive (isActive);
		}


		// Set the gameobjects activity.
		public void SetActiveGameObjects(GameObject[] gameObjectsToActivate, bool isActive){
			// Loop through all the gameobjects.
			for (int i = 0; i < gameObjectsToActivate.Length; i++) {
				// Play any sounds when activating or deactivating.
				PlaySoundActiveness(gameObjectsToActivate[i], isActive);
				// Set the activity of the gameobject.
				gameObjectsToActivate [i].SetActive (isActive);
			}
		}

		/// <summary>
		/// This method will play a sound(s) when it activates a GameObject if this GameObject has Play_SoundOnActivation.
		/// </summary>
		private void PlaySoundActiveness(GameObject gameObjectToActivate, bool isActive){
			// IF we have 1 component of Play_SoundOnActivation.
			if (gameObjectToActivate.GetComponent<Play_SoundOnActivation> () != null) {
				// Grab all the Play Sound components.
				Play_SoundOnActivation[] playSound = gameObjectToActivate.GetComponents<Play_SoundOnActivation> ();
				// Loop through all the activate sounds.
				for (int j = 0; j < playSound.Length; j++) {
					// IF we are activating the GameObject,
					// ELSE we are deactivating the GameObject.
					if (isActive) {
						// Play the sound when the gameobject is activated.
						playSound [j].PlayActiveSounds ();
					} else {
						// Play the sound whent he gameobject is deactivated.
						playSound [j].PlayInactiveSounds ();
					}
				}
			}
		}

		/// <summary>
		/// Loop and see what we can damage based on CharacterType.
		/// </summary>
		public void DamageCharacterTypeLoop (Transform original, Character_Manager otherChar, CharacterType[] damageTheseTypes, 
			float damage, float joltAmount) {

			// Loop through all the Character Types this GameObject collides with to see if we are allowed to damage them.
			for (int i = 0; i < damageTheseTypes.Length; i++) {
				// IF the colliding objects characterType matches one of the types that can take damage OR the selection 
				// is an ALL choice.
				if (otherChar.characterType == damageTheseTypes [i] || damageTheseTypes[i] == CharacterType.All) {
					// Check for Immunity.
					Immunity_Time _ITS = otherChar.characterEntity.GetComponentInParent<Immunity_Time> ();
					// IF the colliding object has the Immunity_Time script,
					// ELSE just damage the target.
					if (_ITS != null) {
						// IF we are Vulnerable.
						if (_ITS.GetVulnerability ()) {
							// We passed the prerequisites to do damage... Yay!
							otherChar.TakeDamage (damage, original, joltAmount);
							// IF the Character dies while taking damage.
							if(otherChar.GetComponentInChildren <Character_Stats> ().CurrentHealth <= 0f){
								// We just leave as there is no need to alter the Change Vulnerability.
								return;
							}
							// Since there is a Immunity_Time script on the colliding object we need to make sure we set the 
							// vulnerability of the colliding gameobject to false;
							_ITS.ChangeVulnerability (false);
						}
					} else {
						// We passed the prerequisites to do damage... Yay!.
						otherChar.TakeDamage (damage, original, joltAmount);
					}
					// Found 1 so we return.
					return;
				}
			}
		}

		/// <summary>
		/// Changes the screen mode to either windowed or fullscreen.
		/// </summary>
		public void ChangeScreenMode(bool isFullscreen){
			Screen.fullScreen = isFullscreen;
		}

		public string RemoveClone(string name){
			// See if there is a (Clone) string in the name.
			int index = name.IndexOf ("(Clone)");
			// IF there is a (Clone) string in the name.
			if(index != -1){
				// Remove part of the string.
				name = name.Remove (index);
				return name;
			}
			return name;
		}

		public void TwoDirectionAnimation (float moveHorizontal, float moveVertical, Animator anim) {
			// IF we are moving we set the animation IsMoving to true,
			// ELSE we are not moving.
			if (moveHorizontal != 0 || moveVertical != 0) {
				// Set walk animations.
				Grid_Helper.animHelper.SetAnimationsWalk (anim);
			} else {
				// Set idle animations.
				Grid_Helper.animHelper.SetAnimationsIdle (anim);
				// We leave if our character is not moving as we don't want to give access to looking in other directions.
				return;
			}

			// IF we are wanting to go in the positive X direction,
			// ELSE we are wanting to go in the negative x direction.
			if (moveHorizontal > 0) {
				anim.SetInteger ("Direction", 4);

			} else if (moveHorizontal < 0) {
				anim.SetInteger ("Direction", 2);
			}
		}

		public void FourDirectionAnimation (float moveHorizontal, float moveVertical, Animator anim){
			// IF we are moving we set the animation IsMoving to true,
			// ELSE we are not moving.
			if (moveHorizontal != 0 || moveVertical != 0) {
				// Set walk animations.
				Grid_Helper.animHelper.SetAnimationsWalk (anim);
			} else {
				// Set idle animations.
				Grid_Helper.animHelper.SetAnimationsIdle (anim);
				// We leave if our player is not moving as we don't want to give access to looking in other directions.
				return;
			}

			// IF we are wanting to go in the positive X direction,
			// ELSE IF we are wanting to move in the negative X direction,
			// ELSE IF we are wanting to move in the negative Y direction,
			// ELSE IF we are wanting to move in the positive Y direction.
			if (moveHorizontal > 0 && Mathf.Abs (moveVertical) <= Mathf.Abs (moveHorizontal)) {
				anim.SetInteger ("Direction", 4);

			} else if (moveHorizontal < 0 && Mathf.Abs (moveVertical) <= Mathf.Abs (moveHorizontal)) {
				anim.SetInteger ("Direction", 2);

			} else if (moveVertical < 0 && Mathf.Abs (moveVertical) > Mathf.Abs (moveHorizontal)) {
				anim.SetInteger ("Direction", 3);

			} else if (moveVertical > 0 && Mathf.Abs (moveVertical) > Mathf.Abs (moveHorizontal)) {
				anim.SetInteger ("Direction", 1);
			}
		}

		public void EightDirectionAnimation (float moveHorizontal, float moveVertical, Animator anim){
			// IF we are moving we set the animation IsMoving to true,
			// ELSE we are not moving.
			if (moveHorizontal != 0 || moveVertical != 0) {
				anim.SetBool ("IsMoving", true);
				anim.SetBool ("IsIdle", false);
			} else {
				anim.SetBool ("IsMoving", false);
				anim.SetBool ("IsIdle", true);
				// We leave if our player is not moving as we don't want to give access to looking in other directions.  Say your player gets Stunned/Rooted/Snared/etc for 3 seconds, you should not be able to make your character look around nor display any other
				// animation other than idle or an animation that you have created specifically for being Stunned/Rooted/Snared/etc.
				return;
			}

			// IF we are going bottom right - Direction 8.
			// ELSE IF we are going bottom left - Direction 7.
			// ELSE IF we are going top left - Direction 6.
			// ELSE IF we are going top right - Direction 5.
			// ELSE IF we are going right - Direction 4.
			// ELSE IF we are going down - Direction 3.
			// ELSE IF we are going left - Direction 2.
			// ELSE IF we are going up - Direction 1.
			if (moveHorizontal > 0 && moveVertical < 0) {
				// Set the down right animation.
				anim.SetInteger ("Direction", 8);

			} else if (moveHorizontal < 0 && moveVertical < 0) {
				// Set the down left animation.
				anim.SetInteger ("Direction", 7);

			} else if (moveHorizontal < 0 && moveVertical > 0) {
				// Set the up left animation.
				anim.SetInteger ("Direction", 6);

			} else if (moveHorizontal > 0 && moveVertical > 0) {
				// Set the up right animation.
				anim.SetInteger ("Direction", 5);

			} else if (moveHorizontal > 0) {
				// Set the right animation.
				anim.SetInteger ("Direction", 4);

			} else if (moveVertical < 0) {
				// Set the down animation.
				anim.SetInteger ("Direction", 3);

			} else if (moveHorizontal < 0) {
				// Set the left animation.
				anim.SetInteger ("Direction", 2);

			} else if (moveVertical > 0) {
				// Set the up animation.
				anim.SetInteger ("Direction", 1);
			}
		}

		public void ClearConsole(){
			var logEnt = System.Type.GetType ("UnityEditorInternal.LogEntries, UnityEditor.dll");
			var clearMeth = logEnt.GetMethod ("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
			clearMeth.Invoke (null, null);
		}
	}
}