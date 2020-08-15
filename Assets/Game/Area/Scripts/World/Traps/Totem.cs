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
            (x) => { },
            (x) => { TakeDamage(); },
            null,
            life
            );
    }

    public void OnTotemEnter()
    {
        if (onUpdate) return;

        onUpdate = true;
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
        casting = true;
    }

    protected virtual void InternalGetStunned() { }

    void StunOver()
    {
        stuned = false;
        myCastingBar.StartCasting();
        InternalStunOver();
    }

    protected virtual void InternalStunOver() { }

    protected abstract void TakeDamage();
}
