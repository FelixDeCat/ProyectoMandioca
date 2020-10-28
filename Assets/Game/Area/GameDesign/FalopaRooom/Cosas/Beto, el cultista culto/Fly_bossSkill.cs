using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Fly_bossSkill : GOAP_Skills_Base
{
    Rigidbody _rb;
    Ente _ent;

    public Vector3 _dest_pos;
    public Waypoint wp;
    Vector3 _dir;
    Animator _anim;


    float speedScaler;

    protected override void OnEndSkill()
    {
        _ent.flyModule.OnFinishMovement -= EndSkill;
    }

    protected override void OnExecute()
    {
        _ent = owner.GetComponent<Ente>();

        _anim.Play("StartFly");
        _ent.flyModule.GainMagicFly();

        _ent.flyModule.OnFinishMovement += EndSkill;
    }

    public override bool ExternalCondition()
    {
        return WorldState.instance.valoresBool["OnGround"];
    }

    protected override void OnFixedUpdate()
    {
        
    }

    public void GoUp()
    {
        
    }

    public void GoDown()
    {
 
    }

    protected override void OnInitialize()
    {
        _anim = owner.GetComponentInChildren<Animator>(); ;

    }

    protected override void OnPause()
    {
        
    }

    protected override void OnResume()
    {
        
    }

    protected override void OnTurnOff()
    {
        
    }

    protected override void OnTurnOn()
    {
        
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnInterruptSkill()
    {
        
    }
   
}
