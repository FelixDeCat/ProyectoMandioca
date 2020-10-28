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
        _ent.canBeInterrupted = true;
        _ent.OnSkillAction -= Fly;
        _ent.flyModule.OnFinishMovement -= EndSkill;
    }

    protected override void OnExecute()
    {
        _ent.OnSkillAction += Fly;
        _ent.flyModule.OnFinishMovement += EndSkill;
        _ent.canBeInterrupted = false;

        _anim.Play("StartFly");
        
    }

    void Fly()
    {
        _ent.flyModule.GainMagicFly();
    }

    public override bool ExternalCondition()
    {
        return WorldState.instance.valoresBool["OnGround"] && _ent.heightLevel == 0;
    }

    protected override void OnFixedUpdate()
    {
        
    }

    protected override void OnInitialize()
    {
        _anim = owner.GetComponentInChildren<Animator>(); ;
        _ent = owner.GetComponent<Ente>();
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
