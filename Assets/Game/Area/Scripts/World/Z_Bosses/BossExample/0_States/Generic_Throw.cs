using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Throw : MonoStateBase
{
    public Transform Hand;
    public BasicThrowable thowable;
    public GameObject go;

    protected override void OnBegin()
    {
        Get_Behaviours.followBehaviour.StartLookAt();
        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_Throw, ThrowSomething);
    }

    void ThrowSomething()
    {
        go.SetActive(true);

        thowable.EndTranckTransform();

        Get_InputSender.SendBool("HasRock", false);

        Vector3 targetPosition = Main.instance.GetChar().transform.position;
        targetPosition.y = Main.instance.GetChar().transform.position.y + 2;
        Vector3 direction = targetPosition - Hand.position;
        direction.Normalize();
        thowable.Throw(Hand.position, direction, 3);
    }
    protected override void OnExit() { }
    protected override void OnOneAwake() { }
    protected override void OnUpdate() { }
}
