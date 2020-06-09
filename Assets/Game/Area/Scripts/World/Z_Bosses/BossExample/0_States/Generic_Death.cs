using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Death : MonoStateBase
{
    protected override void OnBegin() { 
        Get_Behaviours.followBehaviour.StopFollow();
        Get_Behaviours.followBehaviour.StopLookAt();
    }
    protected override void OnExit() { }
    protected override void OnOneAwake() { }
    protected override void OnUpdate() { }
}
