﻿using UnityEngine;
using System.Collections;

namespace TrollBridge {

	public class Weapon_Attack : MonoBehaviour {

//		private SpriteRenderer playerRenderer;
//		private SpriteRenderer weaponRenderer;
//		[SerializeField] private Animator anim;

//		void OnEnable(){
//			// Get the Equipment script.
//			Equipment equipment = Character_Manager.GetPlayerManager ().GetComponentInChildren<Equipment> ();
//			// Get the Sprite Renderer of the player.
//			playerRenderer = Character_Manager.GetPlayer().GetComponent<SpriteRenderer> ();
//			// Get the Weapon the player has.
//			Item weapon = equipment.GetWeapon ();
//			// Get the sprite renderer of the this weapon.
//			weaponRenderer = GetComponent<SpriteRenderer> ();
//			// Get the Sprite Image of the weapon and set it to this GameObject's Sprite.
//			weaponRenderer.sprite = weapon.SpriteImage;
//			// Set the coloring of the Sprite renderer.
//			weaponRenderer.color = new Color (weapon.R, weapon.G, weapon.B, weapon.A);
//		}

//		void Update(){
//			// IF the weapon should be displayed behind of the player,
//			// ELSE the weapon should be displayed infront the player.
//			if (anim.GetInteger ("Direction") == 1) {
//				weaponRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
//			} else {
//				weaponRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
//			}
//		}
	}
}
