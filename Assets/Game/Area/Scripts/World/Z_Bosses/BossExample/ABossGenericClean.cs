using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ToolsMandioca.StateMachine;

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
    [SerializeField] RagdollComponent ragdoll;

    protected override void OnInitialize()
    {
        sensors_and_behaviours.Initialize(this);
        stateMachineHandler.Initialize(sensors_and_behaviours, fastSubscriber, inputSender);

        TakeDamageHandler.Initialize(
            life,
            OnDeath, 
            delegate { }, 
            OnHit, 
            rootTransform, 
            rb,
            TakeDamageFeedback,
            DeathVector);

        inputSender.StartStateMachine();
    }

    void DeathVector(Vector3 dir)
    {
        inputSender.OnDeath();

        sensors_and_behaviours.Behaviours.followBehaviour.StopFollow();
        sensors_and_behaviours.Behaviours.followBehaviour.StopLookAt();
        sensors_and_behaviours.Behaviours.followBehaviour.StopScape();

        Debug.Log("DEATH VECTOR");

        if (dir == Vector3.zero)
            ragdoll.Ragdoll(true, -rootTransform.forward);
        else
            ragdoll.Ragdoll(true, dir);
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
        feedbackManager.Play_HitDamageEmission();
    }

    public bool Predicate_CanTakeDamage() => !sensors_and_behaviours.Behaviours.cooldown_Damage.IsInCooldown;

    
    

    #region Desuso
    public override float ChangeSpeed(float newSpeed) { return newSpeed; }
    public override void IAInitialize(CombatDirector _director) { }
    [Obsolete("podemos empezar a usar component por separado, me vi obligado al component heredarlo de EntityBase... Apenas hay un tiempo muerto habria que desacoplat el take damage de los entity base")]
    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype) => Attack_Result.inmune;
    [Obsolete("podemos empezar a usar component por separado, me vi obligado al component heredarlo de EntityBase... Apenas hay un tiempo muerto habria que desacoplat el take damage de los entity base")]
    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype, EntityBase owner_entity) { return TakeDamage(dmg, attack_pos, damagetype); }
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
