using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ToolsMandioca.StateMachine;

public class ABossGenericClean : EnemyBase
{
    [Header("Boss Options")]
    [SerializeField] StateMachineHandler stateMachineHandler;
    [SerializeField] SensorsAndBehaviours sensors_and_behaviours;
    [SerializeField] FastSubscriberPerState fastSubscriber;
    [SerializeField] FeedbackManager feedbackManager;

    protected override void OnInitialize()
    {
        sensors_and_behaviours.Initialize();
        stateMachineHandler.Initialize(sensors_and_behaviours, fastSubscriber);
        var take_dam_behaviour = sensors_and_behaviours.Behaviours.takeDamageComponent;
        var life_behaviour = sensors_and_behaviours.Behaviours.LifeSystemBase;
        var hit_sensor = sensors_and_behaviours.Sensor.sensor_hit;
        var death_sensor = sensors_and_behaviours.Sensor.sensor_death;
        life_behaviour.AddEventOnDeath(death_sensor.Hand_Enter);
        life_behaviour.AddEventOnHit(hit_sensor.Hand_Enter);
        take_dam_behaviour.SubscribeMeTo(NewTakeDamage, Predicate_CanTakeDamage);
    }

    public override void Zone_OnPlayerEnterInThisRoom(Transform who)
    {
        stateMachineHandler.BeginBoss();
    }

    public bool Predicate_CanTakeDamage() => !sensors_and_behaviours.Behaviours.cooldown_Damage.IsInCooldown;

    
    public Attack_Result NewTakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype, EntityBase entity)
    {
        sensors_and_behaviours.Behaviours.cooldown_Damage.BeginCooldown();
        Vector3 vector_direction = (this.transform.position - attack_pos).normalized;

        ////esto tambien en otro component/// apenas haya tiempo crear un component "KNOCKBACK"
        //if (damagetype == Damagetype.explosion)
        //    rb.AddForce(vector_direction * explosionForce, ForceMode.Impulse);
        //else
        //    rb.AddForce(vector_direction * forceRecall, ForceMode.Impulse);

        feedbackManager.Play_FeedbackOnHit();
        feedbackManager.Play_HitDamageEmission();
        bool death = sensors_and_behaviours.Behaviours.LifeSystemBase.Hit(dmg);
        return death ? Attack_Result.death : Attack_Result.sucessful;
    }


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

    protected override void TakeDamageFeedback(DamageData data)
    {
    }

    protected override void Die(Vector3 dir)
    {
    }

    protected override bool IsDamage()
    {
        return true;
    }
    #endregion


}
