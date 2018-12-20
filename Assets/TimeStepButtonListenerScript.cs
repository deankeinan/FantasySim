using UnityEngine;
using UnityEngine.UI;

public class TimeStepButtonListenerScript : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    public Button m_YourFirstButton;
    private GameObject holder;
    private EventManager events;

    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
          holder = GameObject.FindWithTag("Holder");
          events = (EventManager)holder.GetComponent("EventManager");

          m_YourFirstButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("You have clicked the button!");
        events.TriggerEvent("timestep");
    }
}