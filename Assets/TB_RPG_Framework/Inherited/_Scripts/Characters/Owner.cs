using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge {

	public class Owner : MonoBehaviour {

		[Header ("Debug")] 
		// The owner of this projectile gameobject and the damage this created projectile does.
		[ReadOnlyAttribute] public GameObject owner;
		[ReadOnlyAttribute] public float damage = 0f;
	}
}
