using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA_Felix;
using DungeonGenerator.Components;

public abstract class Entity : MonoBehaviour, IDamageable
{
    [Header("Entity_BASE")]
    public int life_Max_value = 100;
    public int life_Initial_value = 100;

    //public LifeManager life_manager;
    public RigidbodyPathFinder rbPathFinder;
    public Rigidbody rb;
    public Transform icon_position;

    public UI_World_Feedback icon;

    /// <summary>
    /// ACA EMPIEZA TODO... si instancio a una nueva entidad tengo que llamar aca
    /// </summary>
    public void Initialize()
    {
        //life_manager.Config(life_Max_value, OnLoseLife, OnGainLife, OnDeath, life_Initial_value);
        if(rbPathFinder) rbPathFinder.Initialize(rb);
        OnInitialize();
    }
    protected abstract void OnInitialize();
    protected abstract void OnDeath();
    protected abstract void OnGainLife();
    protected abstract void OnLoseLife();
    public abstract void ReceiveDamage(int damage, Vector3 destinity, bool isenemy);
}
