using UnityEngine;

namespace TrollBridge{

	/// <summary>
	/// Grid helper script is the ultimate reference script in which we allow ourselfs to have easy access to our other scripts.  
	/// We simply access other scripts by Grid_Helper.soundManager.PlaySound ().
	/// </summary>
	static class Grid_Helper {

		public static Helper_Manager helper;
		public static Animation_Helper animHelper;
		public static Sound_Manager soundManager;
		public static State_Manager stateManager;
		public static Item_Database itemDataBase;
		public static Inventory inventory;
		public static Tooltip tooltip;
		public static Options_Manager optionManager;
		public static Setup setup;
		public static Move_GameObject endPanel;
		public static Visual_Transition_System visualTransition;
		public static Keybinds kBinds;
		public static Dialogue_Data dialogueData;
		public static UI_Update uiUpdate;

		static Grid_Helper (){
			GameObject g;
			g = SafeFind ("_Holder");
			helper = (Helper_Manager)SafeComponent (g, "Helper_Manager");
			animHelper = (Animation_Helper)SafeComponent (g, "Animation_Helper");
			itemDataBase = (Item_Database)SafeComponent (g, "Item_Database");
			inventory = (Inventory)SafeComponent (g, "Inventory");
			tooltip = (Tooltip)SafeComponent (g, "Tooltip");
			stateManager = (State_Manager)SafeComponent (g, "State_Manager");
			setup = (Setup)SafeComponent (g, "Setup");
			dialogueData = (Dialogue_Data)SafeComponent (g, "Dialogue_Data");
			uiUpdate = (UI_Update)SafeComponent (g, "UI_Update");

			g = SafeFind ("Sound_Manager");
			soundManager = (Sound_Manager)SafeComponent (g, "Sound_Manager");

			g = SafeFind ("OptionsCanvas");
			optionManager = (Options_Manager)SafeComponent (g, "Options_Manager");
			kBinds = (Keybinds)SafeComponent (g, "Keybinds");

			g = SafeFind ("DemoEndPanel");
			endPanel = (Move_GameObject)SafeComponent (g, "Move_GameObject");

			g = SafeFind ("Scene Change Visuals");
			visualTransition = (Visual_Transition_System)SafeComponent (g, "Visual_Transition_System");
		}

		private static GameObject SafeFind(string s){
			GameObject g = GameObject.Find (s);
			if(g == null){
				BigProblem ("The " +s+ " GameObject is not in this scene.");
			}
			return g;
		}

		private static Component SafeComponent(GameObject g, string s){
			Component c = g.GetComponent (s);
			if(c == null){
				BigProblem ("The " +s+ " Component is not attached to the " +g.name+ " GameObject.");
			}
			return c;
		}

		private static void BigProblem(string error){
			Debug.Log ("Cannot proceed : " + error);
			Debug.Break ();
		}
	}
}