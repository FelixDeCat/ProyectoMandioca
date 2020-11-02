using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class CombatDirectorElement : MonoBehaviour, ICombatDirector
{
    [Header("TEMP:/Combat director")]
    [HideInInspector] public float distancePos = 1.5f;
    bool withPos;

    CombatDirector director;
    public Transform Target { get; set; }
    public bool Combat;
    public bool Attacking;

    public void Initialize(float _distancePos, CombatDirector _director)
    {
        director = _director;
        distancePos = _distancePos;
    }

    public void IAmReady()
    {
        Combat = true;
        director.PrepareToAttack(this, Target);
    }

    public void IAmNotReady()
    {
        Combat = false;
        director.DeleteToPrepare(this, Target);
    }
    public void AttackRelease()
    {
        director.AttackRelease(this, Target);
    }

    public void EnterCombat(Transform _target)
    {
        director.AddToList(this, _target);
        SetTarget(_target);
        Combat = true;
    }

    public void ExitCombat()
    {
        director.DeadEntity(this, Target);
        Target = null;
        Combat = false;
    }

    public void ChangeTarget(Transform newTarget)
    {
        director.ChangeTarget(this, newTarget, Target);
    }

    #region ICombatDirector Functions
    public Vector3 CurrentPos() => transform.position;
    public void SetTarget(Transform entity) => Target = entity;
    public bool IsInPos() => withPos;
    public Transform CurrentTarget() => Target;
    public float GetDistance() => distancePos;
    public void SetBool(bool isPos) => withPos = isPos;
    public void ResetCombat()
    {
        Target = null;
        Combat = false;
        SetBool(false);
    }
    public void ToAttack() => Attacking = true;
    #endregion
}

