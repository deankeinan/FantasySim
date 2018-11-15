using UnityEngine;
using System.Collections;

namespace TrollBridge {

	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	/// <summary>
	/// Item game object script holds the information of our item that we get from our JSON data.
	/// </summary>
	public class Item_GameObject : MonoBehaviour {


		[Tooltip("Variable to tell if this item can be picked up yet or not." +
			"This is a useful variable if you want to make another script that will handle the activation of picking this item up. Currently I supply a timer based script (Item_Pickup_Timer) that activates the item to be picked up after a time you put.")]
		public bool canPickUp = true;
		[Tooltip("Set to true if this Item was manually placed by you in the scene, if not we will know this item was created after hitting play.")]
		public bool isPlaced = false;
		[Tooltip("The ID you want this Item to be from your Items.json file in the Streaming Assets folder.")]
		public int id;
		[Tooltip("The amount of this Item.")]
		public int amount = 1;
		[ReadOnlyAttribute]
		public bool inventoryItem;
		[ReadOnlyAttribute]
		public string title;
		[ReadOnlyAttribute]
		public int price;
		[ReadOnlyAttribute]
		public string currency;
		[ReadOnlyAttribute]
		public string rarity;
		[ReadOnlyAttribute]
		public string type;
		[ReadOnlyAttribute]
		public string subType;
		[ReadOnlyAttribute]
		public float r;
		[ReadOnlyAttribute]
		public float g;
		[ReadOnlyAttribute]
		public float b;
		[ReadOnlyAttribute]
		public float a;
		[ReadOnlyAttribute]
		public int damage;
		[ReadOnlyAttribute]
		public int armour;
		[ReadOnlyAttribute]
		public int magicArmour;
		[ReadOnlyAttribute]
		public float movementSpeed;
		[ReadOnlyAttribute]
		public float health;
		[ReadOnlyAttribute]
		public float mana;
		[ReadOnlyAttribute]
		public int restoreHP;
		[ReadOnlyAttribute]
		public int restoreMP;
		[ReadOnlyAttribute]
		public float bonusHealth;
		[ReadOnlyAttribute]
		public string description;
		[ReadOnlyAttribute]
		public bool stackable;
		[ReadOnlyAttribute]
		public string slug;
		[ReadOnlyAttribute]
		public AudioClip pickupSound;
		[ReadOnlyAttribute]
		public AudioClip usedSound;

		// The Rigid Body of this GameObject.
		private Rigidbody2D rigBody;
		// The Collider of this GameObject.
		private Collider2D colliderTwoD;


		void OnValidate() {
			// IF we have a 0 amount of this item.
			if (amount < 1) {
				// Change to 1 because we cant have 0 of any existing item.
				amount = 1;
			}
		}

		void Awake() {
			// Get the RigidBody2D on this GameObject.
			rigBody = GetComponent<Rigidbody2D> ();
			// Get the Collider2D on this GameObject;
			colliderTwoD = GetComponent<Collider2D> ();
		}

		void Start() {
			Item item = Grid_Helper.itemDataBase.FetchItemByID (id);

			title = item.Title;
			price = item.Price;
			currency = item.Currency;
			rarity = item.Rarity;
			type = item.Type;
			subType = item.SubType;
			r = item.R;
			g = item.G;
			b = item.B;
			a = item.A;
			GetComponent<SpriteRenderer> ().color = new Color (r, g, b, a);
			damage = item.Damage;
			armour = item.Armour;
			magicArmour = item.MagicArmour;
			movementSpeed = item.MoveSpeed;
			health = item.Health;
			mana = item.Mana;
			restoreHP = item.RestoreHP;
			bonusHealth = item.BonusHealth;
			description = item.Description;
			inventoryItem = item.InventoryItem;
			stackable = item.Stackable;
			slug = item.Slug;
			pickupSound = item.PickUpSound;
			usedSound = item.UsedSound;
		}

		void Update () {
			// IF this GameObject is not moving.
			// ELSE we turn the trigger off so it can detect walls
			if (rigBody.velocity == Vector2.zero) {
				colliderTwoD.isTrigger = true;
			} else {
				colliderTwoD.isTrigger = false;
			}
		}

		void OnTriggerEnter2D(Collider2D coll) {
			AddItemByCollision (coll.gameObject);
		}

		void OnTriggerStay2D(Collider2D coll) {
			AddItemByCollision (coll.gameObject);
		}

		void OnCollisionEnter2D(Collision2D coll) {
			AddItemByCollision (coll.gameObject);
		}

		void OnCollisionStay2D(Collision2D coll) {
			AddItemByCollision (coll.gameObject);
		}

		/// <summary>
		/// Add item to the players collection when picked/collided up/with.
		/// </summary>
		private void AddItemByCollision(GameObject coll) {
			// IF the timer is up so that we can pick up the item.
			if (!canPickUp) {
				// Cant pick it up so we need to GTFO of here.
				return;
			}
			// IF the colliding object is the players Weapon.
			if (coll.tag == "Weapon") {
				// Just leave as we want the player to physically collide with the GameObject to pickup (unless you do not care then remove this check).... 
				// UNLESS :D you want to make like a grappling hook then just make sure the tag is not labeled a weapon.
				return;
			}
			// IF the colliding gameobject doesnt have a Character_Manager Component.
			if (coll.GetComponentInParent<Character_Manager> () == null) {
				// The colliding object is not a player so they cannot pick up the item, if you want to make it so where enemies can pick up items then adjust code here.
				return;
			}
			// IF the colliding gameobject's CharacterType isnt the Hero/Player.
			if (coll.GetComponentInParent<Character_Manager> ().characterType != CharacterType.Hero) {
				// We only care about picking items up if its the player so lets GTFO.
				return;
			}


			// IF this is an item that goes into your inventory.
			// ELSE there is pre set slots for certain items.  Think of keys and bombs in Binding of Isaac.
			if (inventoryItem) {
				// Handle Inventory Stuff.  If we ended up NOT collecting the item we just leave. True if we collected the item, False if we didn't.
				if (!HandleInventory ()) {
					return;
				}
				// Add the item to the inventory.
				Grid_Helper.inventory.AddItem (id, amount);
			} else {
				// Handle the Types Stuff.
				HandleTypes (coll.GetComponentInParent<Character_Manager> ());
			}
			// Play the Pickup Sound.
			Grid_Helper.soundManager.PlaySound (pickupSound);
			// Check if there is a State handler script.
			Grid_Helper.helper.CheckState(isPlaced, gameObject);
		}

		/// <summary>
		/// Handles the inventory.
		/// </summary>
		private bool HandleInventory() {
			// IF this item is already in the inventory AND it is stackable OR there is a free spot in the inventory.
			if ((Grid_Helper.inventory.items.Contains (Grid_Helper.itemDataBase.FetchItemByID (id)) && stackable) || Grid_Helper.inventory.GetFreeSpots () >= 1) {
				// We are placing an item in our inventory so return true.
				return true;
			}
			// We have an item but we dont have any room for it.
			return false;
		}

		/// <summary>
		/// Handles the action of the type of items.
		/// </summary>
		private void HandleTypes(Character_Manager playerManager) {
			// IF we pick up a Key,
			// ELSE IF we pick up a Bomb,
			// ELSE IF we pick up a type of Currency,
			// ELSE IF we pick up a stat increase Item,
			// ELSE IF *tbc*
			if (type == "Key") {
				// Get the Key script.
				Key key = playerManager.GetComponentInChildren<Key> ();
				// Add to the keys.
				key.AddSubtractKeys (title, amount);
			} else if (type == "Bomb") {
				// Get the Bombs script.
				Bombs bombs = playerManager.GetComponentInChildren<Bombs> ();
				// Add to the bombs.
				bombs.AddBombs (subType, amount);
			} else if (type == "Currency") {
				// Get the Money script.
				Money money = playerManager.GetComponentInChildren<Money> ();
				// Add to the currency.
				money.AddMoney (currency, amount);
			} else if (type == "Stat Increase") {
				// Get the Character Stats (or any data structure if you choose to make your own or use another Asset), 
				Character_Stats charStats = playerManager.GetComponentInChildren<Character_Stats> ();
				// Then Add to the stats.
				charStats.IncreaseMaxHealth (bonusHealth);
//				charStats.IncreaseMaxMana (bonusMana);
//				charStats.IncreaseBaseDamage (bonusDamage);
//				charStats.IncreaseBaseMoveSpeed (bonusMoveSpeed);
			}
		}
	}
}
