using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Taunt : MonoStateBase
{
    protected override void OnOneAwake() { }
    protected override void OnBegin()
    {
        Debug.Log("Entro a Taunt");
        Get_FeedbackHandler.Play_BegginFightClip();
        Get_InputSender.StartStateMachine();
    }
    protected override void OnExit() 
    {

    }
    protected override void OnUpdate() 
    {

    }
}
