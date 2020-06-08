using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Think_MeleePosition : MonoStateBase
{
    protected override void OnBegin() { Get_Behaviours.followBehaviour.StartLookAt(); }
    protected override void OnExit() { Get_Behaviours.followBehaviour.StopLookAt(); }
    protected override void OnOneAwake() { }
    protected override void OnUpdate() { }
}
