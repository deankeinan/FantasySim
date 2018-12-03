using UnityEngine;
using System;
using System.Collections.Generic;


namespace TrollBridge
{
    public class StealFromDungeon : SmartAction
    {
        //Is/was stealing successful
        private bool stolen = false;

        //Item to be stolen
        private Item_GameObject target;

        //path to object (not sure if this needs to be here)
        private Vector2[] path;

        public StealFromDungeon()
        {
            //This will be fed to prolog using Unity prolog as 
            //IsTrue("prolog file", "notOutcharacter", character.name, "steal")
            addPrecondition("notOutOfCharacter", "steal");

            //Condition is IsTrue("prolog file" , "believes, character.name, "unseen")
            addPrecondition("believes", "notSeen");
        }

        //returns whether action was successful
        public override bool Completed()
        {
            return stolen;
        }

        public override bool Perform(GameObject agent)
        {
      
            Vector2 start = agent.transform.position; //character position
            Vector2 end = target.transform.position; //position of item

            if(start != null && end != null)
            {
                //Finds and navigate path to item
                //Item should be picked up on collision if I interpret TrollBridge code correctly
                agent.transform.position = Vector2.Lerp(start, end, 2);
            }

            return true;
        }

        //Precondition(s) that are not Prolog facts
        public override bool ProcedurePrecondition(GameObject agent)
        {
            int value = 0;

            //will try to steal most valuable item
            Item_GameObject mostVal = null;

            //Find all items in dungeon
            Item_GameObject[] items = (Item_GameObject[])UnityEngine.GameObject.FindObjectsOfType(typeof(Item_GameObject));

            //Find most valuable item
            foreach (Item_GameObject item in items)
            {
               if (item.price > value)
                {
                    mostVal = item;
                    value = item.price;
                }
            }

            //set stealing target as most valuable item
            target = mostVal;

            //Returns false if no items in dungeon
            return value != 0;
        }
    }
}
