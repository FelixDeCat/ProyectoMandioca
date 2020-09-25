using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class CombatDirectorElement : MonoBehaviour, ICombatDirector
{
    [Header("TEMP:/Combat director")]
    float distancePos = 1.5f;
    bool withPos;

    CombatDirector director;
    public EntityBase Target { get; set; }
    public bool Combat;
    public bool Attacking {get; set;}

    [SerializeField] UnityEvent CanAttack = null;

    public void Initialize(float _distancePos, CombatDirector _director)
    {
        director = _director;
        distancePos = _distancePos;
    }

    public void IAmReady()
    {
        Combat = true;
        director.PrepareToAttack(this, Main.instance.GetChar());
    }

    public void IAmNotReady()
    {
        Combat = false;
        director.DeleteToPrepare(this, Main.instance.GetChar());
    }
    public void AttackRelease()
    {
        director.AttackRelease(this, Main.instance.GetChar());
    }

    public void EnterCombat()
    {
        director.AddToList(this, Main.instance.GetChar());
        SetTarget(Main.instance.GetChar());
        Combat = true;
    }

    public void ExitCombat()
    {
        director.DeadEntity(this, Main.instance.GetChar());
        Target = null;
        Combat = false;
    }

    #region ICombatDirector Functions
    public Vector3 CurrentPos() => transform.position;
    public void SetTarget(EntityBase entity) => Target = entity;
    public bool IsInPos() => withPos;
    public EntityBase CurrentTarget() => Target;
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

