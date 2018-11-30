using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SmartAction : MonoBehaviour
{
    //Preconditions are prolog facts
    //Check StealFromDungeon.cs for an example
    private HashSet<KeyValuePair<string, object>> preconditions;

    //No effects yet
    private HashSet<KeyValuePair<string, object>> effects;

    public SmartAction()
    {
        preconditions = new HashSet<KeyValuePair<string, object>>();
        effects = new HashSet<KeyValuePair<string, object>>();
    }

    //Success of action
    public abstract bool Completed();

    //What the character does while perfoming action
    public abstract bool Perform(GameObject agent);

    //Precondtions that cannot be represented as Prolog Facts
    public abstract bool ProcedurePrecondition(GameObject agent);

    //Used to set preconditions for action
    public void addPrecondition(string key, object value)
    {
        preconditions.Add(new KeyValuePair<string, object>(key, value));
    }
    
    public void addEffect(string key, object value)
    {
        effects.Add(new KeyValuePair<string, object>(key, value));
    }

    //returns the list of Prolog preconditions
    public HashSet<KeyValuePair<string, object>> Preconditions
    {
        get
        {
            return preconditions;
        }
    }

    public HashSet<KeyValuePair<string, object>> Effects
    {
        get
        {
            return effects;
        }
    }
}

    
