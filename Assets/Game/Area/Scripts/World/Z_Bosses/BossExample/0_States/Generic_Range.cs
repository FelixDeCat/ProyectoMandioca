using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Range : MonoStateBase
{
    public Transform Hand;
    public GameObject go;
    public Throwable thowable;
    public bool isThrower;

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
        if (!isThrower)
        {
            Get_InputSender.SendBool("HasRock", true);
            thowable.gameObject.SetActive(true);
            thowable.BegigTrackTransform(Hand);
        }
        
    }
    public void ThrowSomething()
    {
        if (isThrower)
        {
            thowable.EndTranckTransform();

            Vector3 targetPosition = Main.instance.GetChar().transform.position;
            targetPosition.y = Hand.position.y;
            Vector3 direction = targetPosition - Hand.position;
            direction.Normalize();
            
            thowable.Throw(Hand.position, direction, 4);
        }
        
    }
}

