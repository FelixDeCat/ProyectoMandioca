using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainTransitionJacinta : Switcheable
{
    public GameObject objTransition;

    public override void ABSOnFade(float f)
    {
       
    }

    public override void ABSOnTurnOff()
    {
        objTransition.SetActive(false);
    }

    public override void ABSOnTurnON()
    {
        objTransition.SetActive(true);

    }

    
}
