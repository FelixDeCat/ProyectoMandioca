using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Scape : MonoStateBase
{
    protected override void OnOneAwake() { }
    protected override void OnBegin() => Get_Behaviours.followBehaviour.StartScape();
    protected override void OnExit() => Get_Behaviours.followBehaviour.StopScape();
    protected override void OnUpdate() { }
}
