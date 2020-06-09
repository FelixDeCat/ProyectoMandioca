using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Throw : MonoStateBase
{
    public Transform Hand;
    public BasicThrowable thowable;

    protected override void OnBegin()
    {
        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_Throw, ThrowSomething);
    }

    void ThrowSomething()
    {
        thowable.EndTranckTransform();

        Get_InputSender.SendBool("HasRock", false);

        Vector3 targetPosition = Main.instance.GetChar().transform.position;
        Vector3 direction = targetPosition - Hand.position;
        direction.Normalize();
        thowable.Throw(Hand.position, direction, 4);
    }
    protected override void OnExit() { }
    protected override void OnOneAwake() { }
    protected override void OnUpdate() { }
}
