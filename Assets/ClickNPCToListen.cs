using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClickNPCToListen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            try
            {
                if (hit.transform.name == "Bill")
                {
                    Debug.Log("Clicked Bill");
                }
            }
            catch(NullReferenceException e){
                Debug.Log("No colliders hit from mouse click");
            }
        }
    }

    void OnMouseDown()
    {
        print("hello");
    }
}