using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.EventClasses;

public class InteractableaDesactivator : MonoBehaviour
{
    public EventCounterPredicate pred;

    private void Start()
    {
        pred.Invoke(MyPred);
    }

    public bool MyPred()
    {
        return true;
    }
}
