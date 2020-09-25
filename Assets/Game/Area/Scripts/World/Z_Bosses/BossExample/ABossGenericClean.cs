using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;
using UnityEngine.Events;

public class ABossGenericClean : EnemyBase
{
    [Header("Boss Options")]
    [SerializeField] int life = 8;
    [SerializeField] StateMachineHandler stateMachineHandler = null;
    [SerializeField] InputSenderBase inputSender = null;
    [SerializeField] SensorsAndBehaviours sensors_and_behaviours = null;
    [SerializeField] FastSubscriberPerState fastSubscriber = null;
    [SerializeField] FeedbackManager feedbackManager = null;
    [SerializeField] TakeDamageComponent TakeDamageHandler = null;

    [SerializeField] UnityEvent EntityDeath = null;

    protected override void OnInitialize()
    {
        sensors_and_behaviours.Initialize(this);
        rb = sensors_and_behaviours.rigidBody;
        feedbackManager.SetRoot(rootTransform);
        stateMachineHandler.Initialize(sensors_and_behaviours, fastSubscriber, inputSender, feedbackManager);

        Main.instance.AddEntity(this);

        TakeDamageHandler.Initialize(
            life,
            DeathBoss,
            delegate { },
            OnHit,
            rootTransform,
            rb,
            TakeDamageFeedback,
            DeathVector);

        //quitar esto de aca... cuando tenga la grilla esto tiene que volar
        sensors_and_behaviours.Sensor.StartSensors();


        fastSubscriber.animevent.Add_Callback("Wendigo_Walk", feedbackManager.Play_Sound_WendigoWalk);
    }

    protected override void OnTurnOff() 
    {
        //Debug.LogWarning("se esta apagando");
        //sensors_and_behaviours.Sensor.StartSensors(); 
        //inputSender.Execute(false); 
    }
    protected override void OnTurnOn() 
    { 
        //Debug.LogWarning("se esta prendiendo"); 
        //sensors_and_behaviours.Sensor.StopSensors(); 
        //inputSender.Execute(true); 
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

    void DeathBoss()
    {
        inputSender.OnDeath();
        Main.instance.RemoveEntity(this);
    }
    void OnHit()
    {
        inputSender.OnHit();
    }

    protected override void OnReset()
    {
        //lo de el ragdoll
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
    Vector3 force;
    protected override void OnPause()
    {
        base.OnPause();
        force = rb.velocity;
        rb.isKinematic = true;
        if (death) sensors_and_behaviours.Behaviours.ragdollComponent.PauseRagdoll();
    }
    protected override void OnResume()
    {
        base.OnResume();
        rb.isKinematic = false;
        rb.velocity = force;
        if (death) sensors_and_behaviours.Behaviours.ragdollComponent.ResumeRagdoll();
    }
    protected override void OnUpdateEntity() { }
    protected override void TakeDamageFeedback(DamageData data) { }
    protected override void Die(Vector3 dir) { }
    protected override bool IsDamage() => true;
    #endregion
}
