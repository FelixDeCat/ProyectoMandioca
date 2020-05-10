using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Follow : MonoStateBase
{
    public BehavioursManager behavioursManager;
    protected override void OnBegin() => behavioursManager.followBehaviour.StartFollow();
    protected override void OnExit() => behavioursManager.followBehaviour.StopFollow();
    protected override void OnUpdate() { }
}
