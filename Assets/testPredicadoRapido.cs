using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.EventClasses;

public class testPredicadoRapido : MonoBehaviour
{
    public EventCounterPredicate pred;

    private void Start()
    {
        pred.Invoke(MyPred);
    }

    bool MyPred()
    {
        return true;
    }
}
