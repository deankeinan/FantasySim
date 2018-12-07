using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrollBridge { 
public class TestingScript : MonoBehaviour
{

        private Character_Manager characterManager;
        InputField inputField;

    void Start()
    {
        //Fetch the Input Field component from the GameObject
        inputField = GetComponent<InputField>();
            characterManager = Character_Helper.GetPlayerManager().GetComponent<Character_Manager>();
        }

    void Update()
    {
        //Check if the Input Field is in focus and able to alter
        if (inputField.isFocused)
        {
                characterManager.canMove = false;
        }
        else{
            characterManager.canMove = true;

        }
     }
    }
}