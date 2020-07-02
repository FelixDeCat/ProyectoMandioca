﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;
using UnityEngine.Events;

public class ABossGenericClean : EnemyBase
{
    [Header("Boss Options")]
    [SerializeField] int life;
    [SerializeField] StateMachineHandler stateMachineHandler;
    [SerializeField] InputSenderBase inputSender;
    [SerializeField] SensorsAndBehaviours sensors_and_behaviours;
    [SerializeField] FastSubscriberPerState fastSubscriber;
    [SerializeField] FeedbackManager feedbackManager;
    [SerializeField] TakeDamageComponent TakeDamageHandler;

    [SerializeField] UnityEvent EntityDeath;

    protected override void OnInitialize()
    {
        sensors_and_behaviours.Initialize(this);
        feedbackManager.SetRoot(rootTransform);
        stateMachineHandler.Initialize(sensors_and_behaviours, fastSubscriber, inputSender, feedbackManager);

        TakeDamageHandler.Initialize(
            life,
            OnDeath,
            delegate { },
            OnHit,
            rootTransform,
            rb,
            TakeDamageFeedback,
            DeathVector);
    }

    void DeathVector(Vector3 dir)
    {
        inputSender.OnDeath();
        sensors_and_behaviours.Behaviours.followBehaviour.StopFollow();
        sensors_and_behaviours.Behaviours.followBehaviour.StopLookAt();
        sensors_and_behaviours.Behaviours.followBehaviour.StopScape();
        sensors_and_behaviours.Behaviours.combatDirectorComponent.ExitCombat();
        sensors_and_behaviours.Behaviours.ragdollComponent.ActivateRagdoll(dir == Vector3.zero ? rootTransform.forward : dir , OnEndRagdollFall);
    }
    public void OnEndRagdollFall()
    {
        gameObject.SetActive(false);
        EntityDeath.Invoke();
    }

    void OnDeath()
    {
        inputSender.OnDeath();
    }
    void OnHit()
    {
        inputSender.OnHit();
    }

    protected override void OnReset()
    {
        //lo de el ragdoll
    }

    public override void Zone_OnPlayerEnterInThisRoom(Transform who)
    {
        //inputSender.BeginStateMachine();
    }

    void TakeDamageFeedback(Vector3 owner)
    {

        sensors_and_behaviours.Behaviours.cooldown_Damage.BeginCooldown();
        feedbackManager.Play_FeedbackOnHit();
        feedbackManager.Play_OnHitFlashEmission();
    }

    

    
    

    #region Desuso
    public override float ChangeSpeed(float newSpeed) { return newSpeed; }
    public override void IAInitialize(CombatDirector _director) { }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    public override void ToAttack() => attacking = true;
    protected override void OnUpdateEntity() { }
    protected override void TakeDamageFeedback(DamageData data) { }
    protected override void Die(Vector3 dir) { }
    protected override bool IsDamage() => true;
    #endregion
}
