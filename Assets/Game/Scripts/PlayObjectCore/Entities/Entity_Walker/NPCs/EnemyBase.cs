﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyBase : NPCBase, ICombatDirector
{
    #region Variables
    //encapsular esto
    [HideInInspector] public bool attacking;

    //tal vez seria genial un script para subscribirse a los eventos estos
    //ahora mismo tiene como 15 referencias y es una paja navegarlas
    //varias skills lo usan
    //o tal vez usar el event manager.
    //otra cosa, todos lo estan usando en el AttackEntity... de hecho todos tienen su combat component
    //habria que replantear todo esto cuando nos adentremos mas en el weapon system
    public Action OnParried;

    //por lo que veo... varios le estan diciendo death = true
    //habria que llevarlo mas arriba en la jerarquia
    [HideInInspector] public bool death;

    //esto lo veo bien aca por ahora... mas adelante cuando tengamos que
    //hacer lo de los niveles con la curva tambien es probable que tambien tenga su sistemita
    [SerializeField] protected int expToDrop = 1;

    #endregion

    //sacar de aca
    #region Obligacion (llevar la logica a donde corresponde)
    //-------------- OBLIGACION 
    [Header("TEMP:/Obligacion")]
    public bool target;
    [SerializeField] protected GameObject targetFeedBack = null;
    public bool Invinsible;
    public virtual void IsTarget() { target = true; targetFeedBack.SetActive(true); }
    public virtual void IsNormal() { target = false; targetFeedBack.SetActive(false); }
    public void Mortal() => Invinsible = false;
    #endregion
    #region Control (llevar la logica a donde corresponde)
    //-------------- CONTROL (llevar la logica a donde corresponde)
    [Header("TEMP:/Control")]
    public bool minionTarget;
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
    #endregion

    //hacer components
    #region Combat Sensor (hacer component)
    //estas dos cosas tambien tendrian que tener un component... un sensor mas que nada
    //son dos cosas que se estan usando para hacer checkeos, cuando
    //los checkeos los tienen que hacer los components
    [Header("TEMP:/sensor combat")]
    [SerializeField] protected float combatDistance = 20;
    public bool combat;
    #endregion
    #region Combat Director Functions (hacer component)
    //cuando haya tiempo hacer un combat director connector component
    [Header("TEMP:/Combat director")]
    [SerializeField, Range(0.5f, 15)] float distancePos = 1.5f;
    protected bool withPos;
    protected EntityBase entityTarget;
    protected Transform _target;
    public Transform CurrentTargetPos() => _target;
    public void SetTargetPosDir(Transform pos) { _target = pos; _target.localPosition *= distancePos; }
    public Vector3 CurrentPos() => transform.position;
    public void SetTarget(EntityBase entity) => entityTarget = entity;
    public bool IsInPos() => withPos;
    public EntityBase CurrentTarget() => entityTarget;
    public Transform CurrentTargetPosDir() { _target.localPosition /= distancePos; return _target; }
    public float GetDistance() => distancePos;
    public void SetBool(bool isPos) => withPos = isPos;
    public abstract void IAInitialize(CombatDirector _director);
    public abstract float ChangeSpeed(float newSpeed);
    public void ToAttack() { attacking = true; }
    protected bool IsAttack() { return attacking; }
    #endregion
    #region Timer Effect (hacer component o Handler de efectos activos)
    //timer de efectos... me gusta esto (y) no se quien lo hizo, estaria bueno que tambien sea un component
    Dictionary<int, float> effectsTimer = new Dictionary<int, float>();
    protected Action EffectUpdate = delegate { };
    System.Random key = new System.Random(1);
    protected void AddEffectTick(Action Effect)
    {
        EffectUpdate += Effect;
    }
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
    #endregion
}
