using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TrollBridge {

	public static class JsonHelper {

		/// <summary>
		/// Take in a type of items of T and wrapper it into a Json.
		/// </summary>
		public static string ToJson<T> (T items) {
			
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = items;
			return JsonUtility.ToJson(wrapper, true);
		}

		/// <summary>
		/// Take a Json string and return the items of type T.
		/// </summary>
		public static T FromJson<T>(string json) {
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
			return wrapper.Items;
		}

		[Serializable]
		private class Wrapper<T> {			
			public T Items;
		}
	}
}
