﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DummyEnemy : EnemyBase
{
    [SerializeField] CombatComponent combatComponent;
    [SerializeField] int damage;

    public GameObject obj_feedbackStun;
    public GameObject obj_feedbackShield;
    public GameObject obj_feedbackattack;
    PopSignalFeedback feedbackStun;
    PopSignalFeedback feedbackHitShield;
    PopSignalFeedback feedbackAttack;

    [SerializeField] ParticleSystem greenblood;
   
    public float time_stun;

    public AnimEvent anim;
    StatesMachine sm;
    public Animator animator;
    [SerializeField]
    private float _speedMovement;
    [SerializeField]
    private float _rotSpeed;
    [SerializeField]
    private float _petrifiedTime;
    [SerializeField]
    private float _distance;
    //public Follow follow;

    public Rigidbody _rb;

    [Header("Life Options")]
    [SerializeField] EnemyLifeBar lifesystem;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        combatComponent.Configure(AttackEntity);
        feedbackStun = new PopSignalFeedback(time_stun, obj_feedbackStun, EndStun);
        feedbackHitShield = new PopSignalFeedback(0.2f, obj_feedbackShield);
        feedbackAttack = new PopSignalFeedback(0.2f, obj_feedbackattack);

        anim.Add_Callback("DealDamage", DealDamage);
        sm = new StatesMachine();
        sm.Addstate(new StatesFollow(sm, transform, _rb, FindObjectOfType<CharacterHead>().transform, animator, _rotSpeed, _speedMovement, _distance));
        sm.Addstate(new StatesAttack(sm, animator, transform, FindObjectOfType<CharacterHead>().transform, _rotSpeed, _distance));
        sm.Addstate(new StatesPetrified(sm, _petrifiedTime));
        sm.ChangeState<StatesAttack>();

        //follow.Configure(_rb);
    }

    public void DealDamage()
    {
        combatComponent.ManualTriggerAttack();
    }

    public void EndStun() => combatComponent.Play();

    public void AttackEntity(EntityBase e)
    {
        if (e.TakeDamage(damage, transform.forward) == Attack_Result.parried)
        {
            combatComponent.Stop();
            feedbackStun.Show();
        }
        else if (e.TakeDamage(damage, transform.forward) == Attack_Result.blocked)
        {
            feedbackHitShield.Show();
        }
    }

    private void Update() { feedbackStun.Refresh();  feedbackHitShield.Refresh();sm.Update(); }

    /////////////////////////////////////////////////////////////////
    //////  En desuso
    /////////////////////////////////////////////////////////////////
    public override Attack_Result TakeDamage(int dmg, Vector3 dir)
    {
        lifesystem.Hit(dmg);
        greenblood.Play();

        return Attack_Result.sucessful; 
    }

    public override void Petrified()
    {
        base.Petrified();
        sm.ChangeState<StatesPetrified>();
    }

    public void Die()
    {
        Debug.Log("die");
        var listOfEnemy = Physics.OverlapSphere(transform.position, 10);
        foreach (var item in listOfEnemy)
        {
            if (item.GetComponent<EnemyBase>())
            {
                item.GetComponent<EnemyBase>().Petrified();
            }
        }
    }

    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdateEntity() { }


}
