using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Idle : MonoStateBase
{
    protected override void OnBegin()
    {
        Debug.Log("IDLE: OnBegin");
    }

    protected override void OnExit()
    {
        Debug.Log("IDLE: OnExit");
    }

    protected override void OnUpdate()
    {
        Debug.Log("IDLE: OnUpdate");
    }
}
