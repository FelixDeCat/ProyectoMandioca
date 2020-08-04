using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealMision : Interactable
{
    public Mision mision;

    public override void OnEnter(WalkingEntity entity) 
    {
        WorldItemInfo.instance.Show(pointToMessage.position, "Deal Mision", "Entrega una mision", "Hablar");
    }
    public override void OnExecute(WalkingEntity collector) 
    {
        MisionManager.instancia.AddMision(mision);
    }
    public override void OnExit() 
    {
        WorldItemInfo.instance.Hide();
    }
}
