using System.Collections;
using UnityEngine;
using LitJson;
using System.IO;
using System.Collections.Generic;
using System;

namespace TrollBridge {

	/// <summary>
	/// Item database system in which we pull our information and create a list of our Items.
	/// </summary>
	public class Item_Database : MonoBehaviour {

		[SerializeField] private string itemFileName = "Items";
		private List<Item> database = new List<Item> ();
		private JsonData itemData;


		void Start() {
			itemData = JsonMapper.ToObject (File.ReadAllText (Application.streamingAssetsPath + "/Items/" + itemFileName + ".json"));
			ConstructItemDatabase ();
		}

		/// <summary>
		/// Fetchs the item by ID.
		/// </summary>
		public Item FetchItemByID(int id) {
			for (int i = 0; i < database.Count; i++) {
				if (database [i].ID == id) {
					return database [i];
				}
			}
			return null;
		}

		/// <summary>
		/// Fetchs the name of the item by Name.
		/// </summary>
		public Item FetchItemByName (string itemName) {
			for (int i = 0; i < database.Count; i++) {
				if (database [i].Title == itemName) {
					return database [i];
				}
			}
			return null;
		}

		/// <summary>
		/// Constructs the item database.
		/// </summary>
		private void ConstructItemDatabase() {
			// Loop the amount of json items we have.
			for (int i = 0; i < itemData.Count; i++) {
				// We need to add the total amount of attributes but if something doesn't exists we just fill in a default value.  This makes working in Items.json a less pain in the ass for when you want to add something new to it such as having
				// to fill out attributes we do not care about.  Such as Weapon type for example, we do not care about the "usage" attribute so we handle it this way so you do not have to add that attribute in for when you add a Weapon type.
				// Of course if you wish to alter this to your own liking you can 100% do so.  I just wanted to give this example so that you know about it for when you work on your own attributes.

				database.Add (new Item (
					ItemDataInt (i, "id"),
					ItemDataString (i, "title"), 
					ItemDataInt (i, "value", "price"),
					ItemDataString (i, "value", "currency"),
					ItemDataString (i, "rarity"),
					ItemDataString (i, "type"),
					ItemDataString (i, "subtype"),
					ItemDataInt (i, "color", "r"),
					ItemDataInt (i, "color", "g"),
					ItemDataInt (i, "color", "b"),
					ItemDataInt (i, "color", "a"),
					ItemDataInt (i, "stats", "damage"),
					ItemDataInt (i, "stats", "armour"),
					ItemDataInt (i, "stats", "magicarmour"),
					ItemDataInt (i, "stats", "movespeed"),
					ItemDataInt (i, "stats", "health"),
					ItemDataInt (i, "stats", "mana"),
					ItemDataInt (i, "usage", "restorehp"),
					ItemDataInt (i, "usage", "restoremp"),
					ItemDataInt (i, "bonus", "health"),
					ItemDataInt (i, "bonus", "mana"),
					ItemDataString (i, "description"),
					ItemDataBool (i, "inventoryitem"),
					ItemDataBool (i, "stackable"),
					ItemDataString (i, "slug"),
					ItemDataString (i, "weaponslug"),
					ItemDataString (i, "pickupsound"),
					ItemDataString (i, "usedsound"),
					ItemDataString (i, "attacksound")
				));
			}
		}

		/// <summary>
		/// The data in our json that is an boolean.
		/// </summary>
		private bool ItemDataBool (int index, string mainAttr){
			// IF this item has the attribute mainAttr.
			if(itemData [index].Keys.Contains(mainAttr)){
				// Return our boolean.
				return (bool)itemData [index] [mainAttr];
			}
			return false;
		}

		/// <summary>
		/// The data in our json that is an int.
		/// </summary>
		private int ItemDataInt (int index, string mainAttr){
			// IF this item has the attribute mainAttr.
			if(itemData [index].Keys.Contains(mainAttr)){
				// Return the attribute.
				return (int)itemData [index] [mainAttr];
			}
			// Return the default.
			return 0;
		}

		/// <summary>
		/// The data in our json that is an int.
		/// </summary>
		private int ItemDataInt (int index, string mainAttr, string altAttr){
			// IF this item has the attribute mainAttr.
			if(itemData [index].Keys.Contains(mainAttr)){
				// IF this item has the alternate attribute altAttr.
				if (itemData [index] [mainAttr].Keys.Contains (altAttr)) {
					// Return the attribute.
					return (int)itemData [index] [mainAttr] [altAttr];
				}
			}
			// Return the default.
			return 0;
		}

		/// <summary>
		/// The data in our json that is an string.
		/// </summary>
		private string ItemDataString(int index, string mainAttr){
			// IF this item has the attribute mainAttr.
			if(itemData [index].Keys.Contains(mainAttr)){
				// Return the attribute.
				return itemData [index] [mainAttr].ToString();
			}
			// Return the default.
			return "";
		}

		/// <summary>
		/// The data in our json that is an string.
		/// </summary>
		private string ItemDataString(int index, string mainAttr, string altAttr){
			// IF this item has the attribute mainAttr.
			if(itemData [index].Keys.Contains(mainAttr)){
				// IF this item has the alternate attribute altAttr.
				if (itemData [index] [mainAttr].Keys.Contains (altAttr)) {
					// Return the attribute.
					return itemData [index] [mainAttr] [altAttr].ToString();
				}
			}
			// Return the default.
			return "";
		}
	}
		
	public class Item {
		
		public int ID { get; set; }
		public string Title { get; set; }
		public int Price { get; set; }
		public string Currency { get; set; }
		public string Rarity { get; set; }
		public string Type { get; set; }
		public string SubType { get; set; }
		public float R { get; set; }
		public float G { get; set; }
		public float B { get; set; }
		public float A { get; set; }
		public int Damage { get; set; }
		public int Armour { get; set; }
		public int MagicArmour { get; set; }
		public float MoveSpeed { get; set; }
		public int Health { get; set; }
		public int Mana { get; set; }
		public int RestoreHP { get; set; }
		public int RestoreMana { get; set; }
		public float BonusHealth { get; set; }
		public float BonusMana { get; set; }
		public string Description { get; set; }
		public bool InventoryItem { get; set; }
		public bool Stackable { get; set; }
		public string Slug { get; set; }
		public string WeaponSlug { get; set; }
		public Sprite SpriteImage { get; set; }
		public AudioClip PickUpSound { get; set; }
		public AudioClip UsedSound { get; set; }
		public AudioClip AttackSound { get; set; }

		public Item(int id, string title, int price, string currency, string rarity, string type, string subType, float r, float g, float b, float a, int damage, int armour, int magicArmour, float moveSpeed, int health, int mana, int restoreHP, int restoreMP, float bonusHealth, float bonusMana, string description, bool inventoryItem, bool stackable, string slug, string weaponSlug, string pickupSound, string usedSound, string attackSound)
		{
			this.ID = id;
			this.Title = title;
			this.Price = price;
			this.Currency = currency;
			this.Rarity = rarity;
			this.Type = type;
			this.SubType = subType;
			this.R = r / 255f;
			this.G = g / 255f;
			this.B = b / 255f;
			this.A = a / 255f;
			this.Damage = damage;
			this.Armour = armour;
			this.MagicArmour = magicArmour;
			this.MoveSpeed = moveSpeed;
			this.Health = health;
			this.Mana = mana;
			this.RestoreHP = restoreHP;
			this.RestoreMana = RestoreMana;
			this.BonusHealth = bonusHealth;
			this.BonusMana = bonusMana;
			this.Description = description;
			this.InventoryItem = inventoryItem;
			this.Stackable = stackable;
			this.Slug = slug;
			this.WeaponSlug = weaponSlug;
			this.SpriteImage = Grid_Helper.setup.GetSprite (slug);
			this.PickUpSound = Resources.Load<AudioClip> ("Sounds/" + pickupSound);
			this.UsedSound = Resources.Load<AudioClip> ("Sounds/" + usedSound);
			this.AttackSound = Resources.Load<AudioClip> ("Sounds/" + attackSound);
		}

		public Item() {
			this.ID = -1;
		}
	}
}
