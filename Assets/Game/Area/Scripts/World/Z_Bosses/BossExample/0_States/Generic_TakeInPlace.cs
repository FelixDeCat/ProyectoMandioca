using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_TakeInPlace : MonoStateBase
{

    protected override void OnBegin()
    {
        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_TakeRock, TakeSomething);
    }

    public void TakeSomething()
    {
        Get_FeedbackHandler.Play_PluckRock();
        Get_InputSender.SendBool("HasRock", true);
        Get_FeedbackHandler.EnableRockInHand();
    }

    protected override void OnExit() { }
    protected override void OnOneAwake() { }
    protected override void OnUpdate() { }
}
