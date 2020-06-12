using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Throw : MonoStateBase
{
    public Transform Hand;
    public Throwable thowable;
    public float ThrowForce = 3;
    public int rockDamage = 3;
    Vector3 cap_position;

    public const string NAME_OBJECT = "WendigoRock";

    protected override void OnOneAwake()
    {
        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_Throw, ThrowSomething);
        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_StopLookAt, CatchDefinitiveTargetPosition);
        ThrowablePoolsManager.instance.CreateAPool(NAME_OBJECT, thowable);
    }

    protected override void OnBegin()
    {
        Get_Behaviours.followBehaviour.StartLookAt();
        
    }

    void CatchDefinitiveTargetPosition()
    {
        cap_position = Main.instance.GetChar().transform.position;
        Get_Behaviours.followBehaviour.StopLookAt(); 
    }

    void ThrowSomething()
    {
        Debug.Log("ENTRO ACA AL EVENTO");

        Get_FeedbackHandler.DisableRockInHand();
        Get_InputSender.SendBool("HasRock", false);

       // Vector3 targetPosition = cap_position;
        cap_position.y = cap_position.y + 2;
        Vector3 direction = cap_position - Hand.position;
        direction.Normalize();

        ThrowData data = new ThrowData();
        data.Configure(Hand.position, direction, ThrowForce, rockDamage, Get_Rigidbody.transform);

        ThrowablePoolsManager.instance.Throw(NAME_OBJECT, data);


        //thowable.Throw(Hand.position, direction, 3);
    }
    protected override void OnExit() { }
    
    protected override void OnUpdate() { }
}
