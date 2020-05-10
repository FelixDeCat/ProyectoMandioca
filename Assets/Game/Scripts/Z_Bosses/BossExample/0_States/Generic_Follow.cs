using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Follow : MonoStateBase
{
    protected override void OnBegin() => Get_Behaviours.followBehaviour.StartFollow();
    protected override void OnExit() => Get_Behaviours.followBehaviour.StopFollow();
    protected override void OnUpdate() { }
}
