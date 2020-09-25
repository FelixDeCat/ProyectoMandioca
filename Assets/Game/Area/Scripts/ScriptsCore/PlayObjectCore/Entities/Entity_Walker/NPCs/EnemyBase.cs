using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public abstract class EnemyBase : NPCBase
{
    #region Variables
    [HideInInspector] public bool death;

    #endregion

    public virtual float ChangeSpeed(float newSpeed) => 0;

    public UnityEvent OnDeath;

    #region Damage Receiver y Damage Data
    [Header("BaseThings")]
    [SerializeField] protected DamageData dmgData;
    [SerializeField] protected DamageReceiver dmgReceiver;
    [SerializeField] protected GenericLifeSystem lifesystem = null;
    protected Rigidbody rb;
    [SerializeField] protected Transform rootTransform = null;
    [SerializeField] protected Animator animator = null;
    float currentAnimSpeed;

    protected override void OnInitialize()
    {
        rb = GetComponent<Rigidbody>();
        dmgData?.Initialize(this);
        dmgReceiver.SetIsDamage(IsDamage).AddDead(Death).AddTakeDamage(TakeDamageFeedback).AddInmuneFeedback(InmuneFeedback).Initialize(rootTransform, rb, lifesystem);
    }

    public virtual void ResetEntity()
    {
        StopAllCoroutines();
        lifesystem.ResetLifeSystem();
        OnReset();
    }

    protected abstract void OnReset();

    protected abstract void TakeDamageFeedback(DamageData data);
    void Death(Vector3 dir) { Die(dir); OnDeath?.Invoke(); }
    protected abstract void Die(Vector3 dir);
    protected abstract bool IsDamage();
    protected virtual void InmuneFeedback() { }

    protected override void OnPause()
    {
        currentAnimSpeed = animator.speed;
        animator.speed = 0;
    }

    protected override void OnResume()
    {
        animator.speed = currentAnimSpeed;
    }

    #endregion

    public virtual void SpawnEnemy() { }
}
