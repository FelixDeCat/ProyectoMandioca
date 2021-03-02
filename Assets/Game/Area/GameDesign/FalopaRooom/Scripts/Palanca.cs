using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Palanca : Interactable
{
    public UnityEvent Excecute;
    
    public override void OnEnter(WalkingEntity entity)
    {
        ContextualBarSimple.instance.Show();
        ContextualBarSimple.instance.Set_Sprite_Button_Custom(InputImageDatabase.InputImageType.interact);
    }

    public override void OnExecute(WalkingEntity collector)
    {
        Excecute.Invoke();
        ContextualBarSimple.instance.Hide();
    }

    public override void OnInterrupt()
    {
    }

    public override void OnExit(WalkingEntity collector)
    {
        ContextualBarSimple.instance.Hide();
        collector.GetComponent<InteractSensor>()?.Dissappear(this);
    }
}
