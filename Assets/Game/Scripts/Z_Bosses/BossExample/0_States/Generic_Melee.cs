using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Melee : MonoStateBase
{
    protected override void OnBegin() 
    {
        Get_Anim_Event.Add_Callback("Boss_StopLookAt", Get_Behaviours.followBehaviour.StopLookAt);
        Get_Behaviours.followBehaviour.StartLookAt();
    }
    protected override void OnExit() => Get_Behaviours.followBehaviour.StopLookAt();
    protected override void OnUpdate() { }
}
