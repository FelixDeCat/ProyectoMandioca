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

    float timer;
    bool onUpdate;
    bool casting;
    bool stuned;

    private void Start()
    {
        myCastingBar.AddEventListener_OnFinishCasting(EndCast);
        effectStun.AddStartCallback(GetStunned);
        effectStun.AddEndCallback(StunOver);
    }

    public void OnTotemEnter()
    {
        onUpdate = true;
        OnStartCast();
        InternalTotemEnter();
    }

    protected virtual void InternalTotemEnter() { }

    public void OnTotemExit()
    {
        onUpdate = false;
        myCastingBar.InterruptCasting();
        casting = false;
        timer = 0;
        InternalTotemExit();
    }

    protected virtual void InternalTotemExit() { }

    void OnStartCast()
    {
        if(!stuned && !casting && onUpdate)
        {
            myCastingBar.StartCasting();
            InternalStartCast();
        }
    }

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
        }
    }


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
        InternalGetStunned();
    }

    protected virtual void InternalGetStunned() { }

    void StunOver()
    {
        stuned = false;
        myCastingBar.StartCasting();
        InternalStunOver();
    }

    protected virtual void InternalStunOver() { }
}
