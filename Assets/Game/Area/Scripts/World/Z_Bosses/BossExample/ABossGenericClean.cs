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

    protected override void OnInitialize()
    {
        sensors_and_behaviours.Initialize(this);
        stateMachineHandler.Initialize(sensors_and_behaviours, fastSubscriber, inputSender);
        var hit_sensor = sensors_and_behaviours.Sensor.sensor_hit;
        var death_sensor = sensors_and_behaviours.Sensor.sensor_death;

        TakeDamageHandler.Initialize(
            life, 
            sensors_and_behaviours.Sensor.sensor_death.Hand_Enter, 
            delegate { }, 
            sensors_and_behaviours.Sensor.sensor_hit.Hand_Enter, 
            rootTransform, 
            rb,
            TakeDamageFeedback);

        inputSender.StartStateMachine();


        
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
