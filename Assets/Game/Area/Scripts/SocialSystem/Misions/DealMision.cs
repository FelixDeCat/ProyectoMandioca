using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DealMision : Interactable
{
    public Mision mision;

    public UnityEvent OnStartMision;
    public UnityEvent OnEndMision;

    public LinkMisionToFase linker;
    

    public override void OnEnter(WalkingEntity entity) 
    {
        WorldItemInfo.instance.Show(pointToMessage.position, "Deal Mision", "Entrega una mision", "Hablar");
    }
    public override void OnExecute(WalkingEntity collector) 
    {
        if (MisionManager.instancia.AddMision(mision, EndMision))
        {
            OnStartMision.Invoke();
            linker.BeginLink();
        }
    }

    void EndMision(Mision m)
    {
        Debug.Log("endmision");
        OnEndMision.Invoke();
    }
    public override void OnExit() 
    {
        WorldItemInfo.instance.Hide();
    }
}
