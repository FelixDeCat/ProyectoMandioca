using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Range : MonoStateBase
{
    public Transform Hand;
    public GameObject ObjectModel;
    BasicThrowable current;

    protected override void OnOneAwake() { }
    protected override void OnBegin()
    {
        Get_Behaviours.followBehaviour.StartLookAt();
        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_TakeRock, TakeSomething);
        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_Throw, ThrowSomething);
    }
    protected override void OnExit()
    {
        Get_Behaviours.followBehaviour.StopLookAt();
        Get_Anim_Event_Subscriber.EraseSubscriptions();
    }
    protected override void OnUpdate() { }

    public void TakeSomething()
    {
        GameObject go = GameObject.Instantiate(ObjectModel);
        current = go.GetComponent<BasicThrowable>();
        current.BegigTrackTransform(Hand);
    }
    public void ThrowSomething()
    {
        current.EndTranckTransform();

        Vector3 targetPosition = Main.instance.GetChar().transform.position;
        Vector3 direction = targetPosition - Hand.position;
        direction.Normalize();
        current.Throw(Hand.position,direction, 4);
    }
}

