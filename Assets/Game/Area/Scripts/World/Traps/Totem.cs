using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Totem : MonoBehaviour
{

    [SerializeField] protected CastingBar myCastingBar = null;
    [SerializeField] protected EffectStunnerStunned effectStun = null;
    [SerializeField] protected float timeToCast = 5f;
    [SerializeField] protected TotemFeedback feedback = null;
    [SerializeField] DamageReceiver damageReceiver = null;
    [SerializeField] _Base_Life_System life = null;

    [SerializeField] bool instantStart = false;

    [SerializeField] ParticleSystem ps_TakeDamage = null;
    [SerializeField] AudioClip ac_TakeDamage = null;

    float timer;
    protected bool onUpdate;
    bool casting;
    protected bool stuned;

    protected virtual void Start()
    {
        myCastingBar.AddEventListener_OnFinishCasting(EndCast);
        effectStun.AddStartCallback(GetStunned);
        effectStun.AddEndCallback(StunOver);
        feedback.Initialize(StartCoroutine, StopCoroutine);

        life.Initialize(life.life, () => { }, () => { }, () => { });
        damageReceiver.Initialize(
            transform,
            () => { return false; },
            (x) => { Dead(); },
            (x) => { InternalTakeDamage(); },
            null,
            life
            );

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
    }

    protected virtual void InternalTotemEnter() { }

    public void OnTotemExit()
    {
        if (!onUpdate) return;

        onUpdate = false;
        myCastingBar.InterruptCasting();
        casting = false;
        timer = 0;
        InternalTotemExit();
    }

    protected virtual void InternalTotemExit() { }

    protected void OnStartCast()
    {
        if(!stuned && !casting && onUpdate && InternalCondition())
        {
            myCastingBar.StartCasting();
            InternalStartCast();
            feedback.StartChargeFeedback(myCastingBar.castingTime);
        }
    }

    protected abstract bool InternalCondition();

    protected virtual void InternalStartCast() { }

    private void Update()
    {
        if (stuned) return;

        if (onUpdate)
        {
            if (casting)
            {
                timer += Time.deltaTime;

                if (timer >= timeToCast)
                {
                    casting = false;
                    timer = 0;
                    OnStartCast();
                }
            }

            UpdateTotem();
        }
    }

    protected virtual void UpdateTotem() { }

    void EndCast()
    {
        InternalEndCast();
        casting = true;
    }

    protected abstract void InternalEndCast();

    void GetStunned()
    {
        stuned = true;
        myCastingBar.InterruptCasting();
        feedback.InterruptCharge();
        InternalGetStunned();
    }

    protected void InterruptCast()
    {
        myCastingBar.InterruptCasting();
        feedback.InterruptCharge();
        OnStartCast();
    }

    protected virtual void InternalGetStunned() { }

    void StunOver()
    {
        stuned = false;
        myCastingBar.StartCasting();
        InternalStunOver();
    }

    protected virtual void InternalStunOver() { }

    protected void TakeDamageFeedback()
    {
        ParticlesManager.Instance.PlayParticle(ps_TakeDamage.name, myCastingBar.transform.position);
        AudioManager.instance.PlaySound(ac_TakeDamage.name);
    }

    protected abstract void InternalTakeDamage();

    protected virtual void Dead()
    {
        feedback.StopAll();
    }
}
