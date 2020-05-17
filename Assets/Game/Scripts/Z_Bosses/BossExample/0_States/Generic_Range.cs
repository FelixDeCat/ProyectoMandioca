using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Range : MonoStateBase
{
    public Transform Hand;
    public GameObject ObjectModel;
    

    protected override void OnBegin()
    {
        Get_Behaviours.followBehaviour.StartLookAt();
        Get_Anim_Event_Subscriber.SubscriveMeTo(AnimEventLabel.Boss_TakeRock, TakeSomething);
        Get_Anim_Event_Subscriber.SubscriveMeTo(AnimEventLabel.Boss_Throw, ThrowSomething);
    }
    protected override void OnExit()
    {
        Get_Behaviours.followBehaviour.StopLookAt();
        Get_Anim_Event_Subscriber.EraseSubscriptions();
    }
    protected override void OnUpdate() { }

    public void TakeSomething()
    {
        //equipo en la mano
    }
    public void ThrowSomething()
    {
        //desequipo en la mano
        //lanzo desde esa position
    }
}

