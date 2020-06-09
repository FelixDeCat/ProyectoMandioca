using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_TakeInPlace : MonoStateBase
{
    public Transform Hand;
    public BasicThrowable thowable;
    public GameObject go;

    protected override void OnBegin()
    {
        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_TakeRock, TakeSomething);
    }

    public void TakeSomething()
    {
        go.SetActive(true);
        Get_InputSender.SendBool("HasRock", true);
        thowable.gameObject.SetActive(true);
        thowable.BegigTrackTransform(Hand);
    }

    protected override void OnExit() { }
    protected override void OnOneAwake() { }
    protected override void OnUpdate() { }
}
