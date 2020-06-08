using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TakeDamageComponent : MonoBehaviour
{
    DamageReceiver damageReceiver;
    GenericLifeSystem LifeSystem;
    Action<Vector3> tkdFeedback;
    Rigidbody rb;

    Func<bool> pred = delegate { return false; };

    public void Initialize(int life, Action OnDeath, Action OnGain, Action OnHit, Transform owner, Rigidbody _rb, Action<Vector3> _tkdFeedback)
    {
        LifeSystem = GetComponent<GenericLifeSystem>();
        damageReceiver = GetComponent<DamageReceiver>();

        LifeSystem.Initialize(life, OnHit, OnGain, OnDeath);
        damageReceiver.Initialize(owner, pred, DeathVector, TakeDamage, rb, LifeSystem);
        tkdFeedback = _tkdFeedback;
        rb = _rb;
    }

    public void TakeDamage(DamageData dmgdata)
    {
        Debug.Log("TakeDamage");
        tkdFeedback.Invoke(dmgdata.owner_position);
    }

    void DeathVector(Vector3 v3)
    {
        //esto es para el ragdoll
    }
}
