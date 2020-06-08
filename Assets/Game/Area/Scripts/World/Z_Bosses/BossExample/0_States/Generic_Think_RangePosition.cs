using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Think_RangePosition : MonoStateBase
{
    protected override void OnBegin() 
    { 
        Get_Behaviours.followBehaviour.StartLookAt();
        Get_Behaviours.combatDirectorComponent.EnterCombat();

    }
    protected override void OnExit() 
    {
        Get_Behaviours.combatDirectorComponent.ExitCombat();
        Get_Behaviours.followBehaviour.StopLookAt(); 
    }
    protected override void OnOneAwake() { }
    protected override void OnUpdate() { }
}
