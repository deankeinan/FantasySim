using UnityEngine;

namespace TrollBridge {

	public enum CharacterType {Hero, Enemy, Neutral, All}
	/// <summary>
	/// Character is all about what each GameObject 'Characterish' entity has or is shared accross everyone.
	/// </summary>
	public abstract class Character : MonoBehaviour {

		[Header ("Character")]
		// The type of character this is. The hero (Player), Enemy (normal mobs / bosses) or Neutral.
		public CharacterType characterType;
		// The GameObject that represents the character entity.
		public GameObject characterEntity;

		[Header ("Status")]
		// Status (un)altering effects.
		public bool canMove = true;
		public float alterSpeed = 1f;
		public bool canAttack = true;
		public bool immuneToSlow = false;
		public bool immuneToStun = false;
		public bool immuneToSilence = false;
		public bool isInteractable = false;

		[Header ("Inverts")]
		// The invert movement for the X and Y. 1 for normal and -1 for opposite.
		public int playerInvertX = 1;
		public int playerInvertY = 1;

		[Header ("Components")]
		// The Light source for this character. (Think of a Spotlight so enemies and players can be seen in the dark if we want them to be.)
		[ReadOnlyAttribute] public Light lightSource;
		// The character Collider2D.
		[ReadOnlyAttribute] public Collider2D characterCollider;
		// The Equipment of the character.
		[ReadOnlyAttribute] public Equipment characterEquipment;
		// The Money/Currency of the character.
		[ReadOnlyAttribute] public Money characterCurrency;
		// The stats of the character.
		[ReadOnlyAttribute] public Character_Stats characterStats;
		// The Class of the character.
		[ReadOnlyAttribute] public Character_Class characterClass;
		// The Character Animator.
		[ReadOnlyAttribute] public Animator characterAnimator;
		// The Rigidbody2D of the character.
		[ReadOnlyAttribute] public Rigidbody2D characterRigidBody2D;

		[Header ("Animation Type")]
		// Is there a Two Direction Animation.
		[ReadOnlyAttribute] public bool twoDirAnim = false;
		// Is there a Four Direction Animation.
		[ReadOnlyAttribute] public bool fourDirAnim = false;
		// Is there a Eight Direction Animation.
		[ReadOnlyAttribute] public bool eightDirAnim = false;

		[Header ("Character Locations")]
		// Transform locations that will be located a bit to the north, south, east and west of our
		// Player/Enemy/NPC to be used for things like skills that launch projectiles.
		[ReadOnlyAttribute] public Transform locationCenter;
		[ReadOnlyAttribute] public Transform projectileLocationNorth;
		[ReadOnlyAttribute] public Transform projectileLocationSouth;
		[ReadOnlyAttribute] public Transform projectileLocationEast;
		[ReadOnlyAttribute] public Transform projectileLocationWest;
		[ReadOnlyAttribute] public GameObject meleeWeapon;
		[ReadOnlyAttribute] public Transform meleeLocationNorth;
		[ReadOnlyAttribute] public Transform meleeLocationSouth;
		[ReadOnlyAttribute] public Transform meleeLocationEast;
		[ReadOnlyAttribute] public Transform meleeLocationWest;

		[Header ("Interaction")]
		// Is there a Interaction currently running.
		[ReadOnlyAttribute] public bool isInteracting = false;
		// The focus target for any interaction.
		[ReadOnlyAttribute] public GameObject interactionFocusTarget;

		[Header ("Position")]
		// Position variables for the character gameobject.
		[ReadOnlyAttribute] public Transform characterTransform;
		[ReadOnlyAttribute] public Vector2 characterPrevLocation;
		[ReadOnlyAttribute] public Vector2 characterAmountMoved;


		void OnEnable () {
			// Add this to our List.
			Character_Helper.Register (this);
		}

		void OnDisable () {
			// Remove from our List.
			Character_Helper.Unregister (this);
		}
	}
}
