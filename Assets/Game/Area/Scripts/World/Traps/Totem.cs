using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Totem : PlayObject
{
    [SerializeField] protected CastingBar myCastingBar = null;
    [SerializeField] protected EffectStunnerStunned effectStun = null;
    [SerializeField] protected float timeToCast = 5f;
    [SerializeField] protected TotemFeedback feedback = null;
    [SerializeField] protected DamageReceiver damageReceiver = null;
    [SerializeField] _Base_Life_System life = null;

    [SerializeField] bool instantStart = false;

    [SerializeField] ParticleSystem ps_TakeDamage = null;
    [SerializeField] AudioClip ac_TakeDamage = null;

    float timer;
    protected bool onUpdate;
    bool castingCD;
    bool casting;
    float timerCasting;
    protected bool stuned;

    protected override void OnInitialize()
    {
        effectStun.AddStartCallback(GetStunned);
        effectStun.AddEndCallback(StunOver);
        feedback.Initialize(StartCoroutine, StopCoroutine);

        life.Initialize(life.life, () => { }, () => { }, () => { });
        damageReceiver.AddDead((x) => Dead()).AddTakeDamage((x) => InternalTakeDamage()).AddInmuneFeedback(InmuneFeedback).Initialize(transform, null, life);

        AudioManager.instance.GetSoundPool(ac_TakeDamage.name, AudioGroups.GAME_FX, ac_TakeDamage);
        ParticlesManager.Instance.GetParticlePool(ps_TakeDamage.name, ps_TakeDamage);
    }

    public void OnTotemEnter()
    {
        if (onUpdate) return;

        onUpdate = true;

        if (instantStart)
            EndCast();
        else
            OnStartCast();

        InternalTotemEnter();

        On();
    }

    protected virtual void InternalTotemEnter() { }

    public void OnTotemExit()
    {
        if (!onUpdate) return;

        onUpdate = false;
        casting = false;
        timeToCast = 0;
        castingCD = false;
        timer = 0;
        InternalTotemExit();
    }

    protected virtual void InternalTotemExit() { }

    protected void OnStartCast()
    {
        if(!stuned && !castingCD && onUpdate && InternalCondition())
        {
            casting = true;
            InternalStartCast();
        }
    }

    protected abstract bool InternalCondition();

    protected virtual void InternalStartCast() { }

    protected override void OnUpdate()
    {

        if (stuned) return;

        if (onUpdate)
        {
            if (castingCD)
            {
                timer += Time.deltaTime;

                if (timer >= timeToCast)
                {
                    castingCD = false;
                    timer = 0;
                    OnStartCast();
                }
            }
            else if (casting)
            {
                timerCasting += Time.deltaTime;

                if (timerCasting >= myCastingBar.castingTime)
                {
                    casting = false;
                    timerCasting = 0;
                    feedback.StartChargeFeedback(EndCast);
                }
            }

            UpdateTotem();
        }
    }

    protected virtual void UpdateTotem() { }

    void EndCast()
    {
        InternalEndCast();
        castingCD = true;
    }

    protected abstract void InternalEndCast();

    void GetStunned()
    {
        stuned = true;
        casting = false;
        timerCasting = 0;
        feedback.InterruptCharge();
        InternalGetStunned();
    }

    protected void InterruptCast()
    {
        casting = false;
        timerCasting = 0;
        feedback.InterruptCharge();
        OnStartCast();
    }

    protected virtual void InternalGetStunned() { }

    void StunOver()
    {
        stuned = false;
        OnStartCast();
        InternalStunOver();
    }

    protected virtual void InternalStunOver() { }

    protected void TakeDamageFeedback()
    {
        ParticlesManager.Instance.PlayParticle(ps_TakeDamage.name, myCastingBar.transform.position);
        AudioManager.instance.PlaySound(ac_TakeDamage.name);
    }

    protected virtual void InmuneFeedback()
    {

    }

    protected abstract void InternalTakeDamage();

    protected virtual void Dead()
    {
        feedback.StopAll();
        casting = false;
        timerCasting = 0;
        feedback.InterruptCharge();
    }

    protected override void OnTurnOn() { }

    protected override void OnTurnOff()
    {
        feedback.StopAll();
        castingCD = false;
        stuned = false;
        timer = 0;
        casting = false;
        timerCasting = 0;
        feedback.InterruptCharge();
    }

    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
}
