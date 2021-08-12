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
    [SerializeField] FeedbackManager _feedBack = null;
    bool isInCooldown = false;
    public bool Predicate_CanTakeDamage() => isInCooldown;

    public float recalltime;

    public void Initialize(int life, Action OnDeath, Action OnGain, Action OnHit, Transform owner, Rigidbody _rb, Action<Vector3> _tkdFeedback, Action<Vector3> OnDeathVector)
    {
        LifeSystem = GetComponent<GenericLifeSystem>();
        damageReceiver = GetComponent<DamageReceiver>();

        LifeSystem.Initialize(life, OnHit, OnGain, OnDeath);
        damageReceiver.SetIsDamage(Predicate_CanTakeDamage).AddDead(OnDeathVector).AddTakeDamage(TakeDamage).Initialize(owner, rb, LifeSystem);
        tkdFeedback = _tkdFeedback;
        rb = _rb;
    }

    public void TakeDamage(DamageData dmgdata)
    {
        //DebugCustom.Log("Wendigo", "wendigo", "L:" + LifeSystem.Life + " D:" + dmgdata.damage);

        tkdFeedback.Invoke(dmgdata.owner_position);
        StopCoroutine(BeginDamage());
        StartCoroutine(BeginDamage());
        _feedBack.Play_GetDamage();
    }
    
    IEnumerator BeginDamage()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(recalltime);
        isInCooldown = false;
        yield return null;
    }
}
