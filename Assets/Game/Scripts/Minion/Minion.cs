﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Minion : Companion, ICombatDirector
{
    public EntityBase owner;
    public EntityBase entityTarget;
    protected CombatDirector director;

    [SerializeField] protected float distanceToTarget = 3;
    [SerializeField] protected float maxDistanceToTarget = 9;
    [SerializeField] protected float distanceToOwner = 10;
    [SerializeField] protected float distanceToCombat;

    protected bool death;

    protected override void OnInitialize()
    {
        Main.instance.eventManager.TriggerEvent(GameEvents.MINION_SPAWN, new object[] { this });

        Main.instance.GetCombatDirector().AddNewTarget(this);

        IAInitialize();
    }

    protected abstract void IAInitialize();

    public void SetTarget(EntityBase _target)
    {
        entityTarget = _target;
    }

    protected void AddEffectTick(Action Effect)
    {
        EffectUpdate += Effect;
    }

    Dictionary<int, float> effectsTimer = new Dictionary<int, float>();
    protected Action EffectUpdate = delegate { };

    System.Random key = new System.Random(1);

    protected void AddEffectTick(Action Effect, float duration, Action EndEffect)
    {
        int myNumber = key.Next();
        effectsTimer.Add(myNumber, 0);

        Action MyUpdate = Effect;
        Action MyEnd = EndEffect;
        MyEnd += () => effectsTimer.Remove(myNumber);
        MyEnd += () => EffectUpdate -= MyUpdate;

        MyUpdate += () =>
        {
            effectsTimer[myNumber] += Time.deltaTime;

            if (effectsTimer[myNumber] >= duration)
                MyEnd();
        };

        AddEffectTick(MyUpdate);
    }

    #region CombatDirector
    bool inPos;
    public bool attacking;
    public Transform currentTargetPos;

    public Vector3 CurrentTargetPos()
    {
        Vector3 result = currentTargetPos.position - currentTargetPos.localPosition + currentTargetPos.localPosition * distanceToTarget;
        return result;
    }

    public Transform CurrentTargetPosDir()
    {
        return currentTargetPos;
    }

    public void SetTargetPosDir(Transform pos)
    {
        currentTargetPos = pos;
    }

    public float GetDistance() => distanceToTarget;

    public EntityBase CurrentTarget() => entityTarget;

    public Vector3 CurrentPos() => transform.position;

    public void ToAttack() => attacking = true;

    public bool IsInPos() => inPos;

    public void SetBool(bool isPos) => inPos = isPos;

    public virtual void ResetCombat()
    {
        entityTarget = null;
        SetTargetPosDir(null);
        SetBool(false);
    }
    #endregion
}
