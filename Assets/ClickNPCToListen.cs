using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



namespace TrollBridge
{

    public class ClickNPCToListen : MonoBehaviour
    {

        private Dialogue_Setup dialogueSetup;
        GameObject lastClicked;
        

        public void SetDialogueSetup(Dialogue_Setup newDialogueSetup)
        {
            dialogueSetup = newDialogueSetup;
        }

        public void DialogueButtonPressed()
        {
            // Remove the Multi Dialogue Window.
            //Grid_Helper.multipleInteractionsData.DisplayMultipleInteractionsUI (false);
            // Set the closest Dialogue.
            Grid_Helper.dialogueData.SetClosestDialogue(dialogueSetup);
            // Construct the dialogue with the closest GameObject with a Dialogue_Setup.
            Grid_Helper.dialogueData.Dialogue();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

                //try
                //{
                if(hit)
                {
                    if (hit.transform.name == "Bill" || hit.transform.name == "Jane")
                    {
                        lastClicked = hit.transform.gameObject;
                        Debug.Log("Clicked an NPC");
                        dialogueSetup = (Dialogue_Setup)lastClicked.GetComponent("Dialogue_Setup");
                        DialogueButtonPressed();
                        Grid_Helper.inventory.OpenInventory();
                    }
                }
                else{
                    if(lastClicked){
                        dialogueSetup = (Dialogue_Setup)lastClicked.GetComponent("Dialogue_Setup");
                        Grid_Helper.dialogueData.DisplayDialoguePanel(false);
                        Grid_Helper.inventory.CloseInventory();

                    }
                }
                   
            }
        }

    }
}



