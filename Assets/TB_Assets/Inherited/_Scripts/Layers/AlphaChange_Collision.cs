using UnityEngine;
using UnityEngine.Tilemaps;

namespace TrollBridge {

	[RequireComponent(typeof(Collider2D))]
	/// <summary>
	/// Alpha change collision handles the actions of changing the alpha of this GameObject if the Player collides with it.
	/// </summary>
	public class AlphaChange_Collision : MonoBehaviour {

		[Tooltip("The alpha to be changed when a collision happens.")]
		[SerializeField] private float onEnterAlphaChange = 0.3f;
		// The SpriteRenderer or TileMapper that will be altered.
		[SerializeField] private SpriteRenderer spriteAlphaChange;
		[SerializeField] private Tilemap tileMapAlphaChange;
		// The new Layer Name when a collision enter happens.
		[SerializeField] private LayerMask enterLayerName;

		private float initialAlpha;

		void Start () {
			// IF we have a sprite alpha Sprite Renderer,
			// ELSE IF we have a Tilemap,
			// ELSE we dont have either which sucks ass and we need to fix that.
			if (spriteAlphaChange != null) {
				initialAlpha = spriteAlphaChange.color.a;
			} else if (tileMapAlphaChange != null) {
				initialAlpha = tileMapAlphaChange.color.a;
			} else {
				Debug.Log ("You need to assign a variable to either the 'spriteAlphaChange' or 'tileMapAlphaChange' for this script to to work.");
			}
		}
		
		void OnTriggerEnter2D (Collider2D coll) {
			// IF this colliding gameobjects tag is "Player".
			if (coll.gameObject.tag == "Player") {
				// IF the Player's layer is the same as the layer we check.
				if (coll.gameObject.layer == (int)Mathf.Log (enterLayerName.value, 2)) {
					// IF we have a sprite alpha Sprite Renderer,
					// ELSE we have a Tilemap.
					if (spriteAlphaChange != null) {
						// Change the alpha.
						spriteAlphaChange.color = new Color (spriteAlphaChange.color.r, spriteAlphaChange.color.g, spriteAlphaChange.color.b, onEnterAlphaChange);
					} else {
						// Change the alpha.
						tileMapAlphaChange.color = new Color (tileMapAlphaChange.color.r, tileMapAlphaChange.color.g, tileMapAlphaChange.color.b, onEnterAlphaChange);
					}
				}
			}
		}

		void OnTriggerExit2D (Collider2D coll) {
			// IF this colliding gameobjects tag is "Player".
			if (coll.gameObject.tag == "Player") {
				// IF the Player's layer is the same as the layer we check.
				if (coll.gameObject.layer == (int)Mathf.Log (enterLayerName.value, 2)) {
					// IF we have a sprite alpha Sprite Renderer,
					// ELSE we have a Tilemap.
					if (spriteAlphaChange != null) {
						// Change the alpha.
						spriteAlphaChange.color = new Color (spriteAlphaChange.color.r, spriteAlphaChange.color.g, spriteAlphaChange.color.b, initialAlpha);
					} else {
						// Change the alpha.
						tileMapAlphaChange.color = new Color (tileMapAlphaChange.color.r, tileMapAlphaChange.color.g, tileMapAlphaChange.color.b, initialAlpha);
					}
				}
			}
		}
	}
}
