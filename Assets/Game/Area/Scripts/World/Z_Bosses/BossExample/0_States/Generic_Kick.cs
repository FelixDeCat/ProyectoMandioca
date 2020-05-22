using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Kick : MonoStateBase
{

    protected override void OnOneAwake()
    {
        Get_Behaviours.activateDamage.Deactivate();
    }
    protected override void OnBegin() 
    {
        Get_Behaviours.followBehaviour.StartLookAt();

        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_StopLookAt, Get_Behaviours.followBehaviour.StopLookAt);
        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_Close_Melee, Kick);
        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_Close_Melee_End, EndKick);


    }

    public void Kick()
    {
        Get_Behaviours.activateDamage.Activate();
    }
    public void EndKick()
    {
        Get_Behaviours.activateDamage.Deactivate();
    }
    protected override void OnExit() 
    { 
        Get_Behaviours.followBehaviour.StopLookAt();
    }
    
    protected override void OnUpdate() { }
}
