using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Palanca : Interactable
{
    public UnityEvent Excecute;
    
    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(pointToMessage.position, "", "", "Empujar", false, false);
    }

    public override void OnExecute(WalkingEntity collector)
    {
        Debug.Log("activado pá");
        Excecute.Invoke();
        WorldItemInfo.instance.Hide();
    }

    public override void OnInterrupt()
    {
    }

    public override void OnExit()
    {
        WorldItemInfo.instance.Hide();
    }
}
