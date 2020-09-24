using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Kick : MonoStateBase
{
    public Transform warningAttack_T;
    public ParticleSystem warningAttack_fb;

    protected override void OnOneAwake()
    {
        Get_Behaviours.activateDamage.Deactivate();
        ParticlesManager.Instance.GetParticlePool(warningAttack_fb.name, warningAttack_fb, 2);

        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_StopLookAt, Get_Behaviours.followBehaviour.StopLookAt);
        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_Close_Melee, Kick);
        Get_Anim_Event_Subscriber.SubscribeMeTo(AnimEventLabel.Boss_Close_Melee_End, EndKick);
    }
    protected override void OnBegin() 
    {
        Get_Behaviours.followBehaviour.StartLookAt();

        ParticlesManager.Instance.PlayParticle(warningAttack_fb.name, warningAttack_T.position + new Vector3(0,.4f,0));


    }

    public void Kick()
    {
        Get_Behaviours.activateDamage.Activate();
    }
    public void EndKick()
    {
        Get_Behaviours.activateDamage.Deactivate();
    }
    protected override void OnExit() 
    { 
        Get_Behaviours.followBehaviour.StopLookAt();
    }
    
    protected override void OnUpdate() { }
}
