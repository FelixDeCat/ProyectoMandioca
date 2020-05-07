using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyBase : NPCBase, ICombatDirector
{

    [HideInInspector] public bool attacking;
    [SerializeField] protected GameObject targetFeedBack = null;
    public Action OnParried;
    [HideInInspector] public bool minionTarget;
    [HideInInspector] public bool Invinsible;
    [HideInInspector] public bool death;
    [SerializeField] protected int expToDrop = 1;

    public virtual void Awake()
    {
        
    }    

    public virtual void IsTarget()
    {
        target = true;
        targetFeedBack.SetActive(true);
    }
    public virtual void IsNormal()
    {
        target = false;
        targetFeedBack.SetActive(false);
    }

    [SerializeField] protected float combatDistance = 20;
    protected bool combat;

    public void Mortal()
    {
        Invinsible = false;
    }

    #region Combat Director Functions
    protected bool withPos;


    protected EntityBase entityTarget;

    protected Transform _target;

    [SerializeField, Range(0.5f, 15)] float distancePos = 1.5f;

    public Transform CurrentTargetPos()
    {
        return _target;
    }

    public void SetTargetPosDir(Transform pos)
    {
        _target = pos;

        _target.localPosition *= distancePos;
    }

    public Vector3 CurrentPos()
    {
        return transform.position;
    }

    public void SetTarget(EntityBase entity)
    {
        entityTarget = entity;
    }

    public bool IsInPos() { return withPos; }

    public EntityBase CurrentTarget() { return entityTarget; }

    public Transform CurrentTargetPosDir()
    {
        _target.localPosition /= distancePos;
        return _target;
    }

    public float GetDistance()
    {
        return distancePos;
    }

    public void SetBool(bool isPos)
    {
        withPos = isPos;
    }

    public abstract void ToAttack();

    public abstract void IAInitialize(CombatDirector _director);

    public abstract float ChangeSpeed(float newSpeed);
    #endregion

    protected bool IsAttack() { return attacking; }

    public virtual void GetFocusedOnParry()
    {
        foreach (var e in Main.instance.GetEnemies())
        {
            if (e != this)
                e.minionTarget = false;
            else
                minionTarget = true;
        }
    }

    protected void AddEffectTick(Action Effect)
    {
        EffectUpdate += Effect;
    }

    Dictionary<int, float> effectsTimer =  new Dictionary<int, float>();
    protected Action EffectUpdate = delegate {}; 

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
}
