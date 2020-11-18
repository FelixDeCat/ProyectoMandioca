using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenericLifeSystem : _Base_Life_System
{

    bool isdeath;

    public event Action DeadCallback = delegate { };

    public Action OnHitCallback = delegate { };

    public void AddEventOnHit(Action listener) { OnHitCallback = listener; }
    public void AddEventOnDeath(Action listener) { DeadCallback += listener; }
    public void RemoveEventOnDeath(Action listener) { DeadCallback -= listener; DeadCallback = delegate { }; }

    private void Awake()=>CreateLifeBar();
    protected void CreateLifeBar()
    {
        lifesystem.Config(life, OnHitCallback, EVENT_OnGainLife, EVENT_OnDeath, life);
    }
    void EVENT_OnGainLife() { }
    void EVENT_OnDeath()
    {
        if (!isdeath)
        {
            DeadCallback.Invoke();
            DeadCallback = delegate { };
            isdeath = true;
        }
    }

    //public bool Hit(int _val)
    //{
    //    return lifeSystemEnemy.Hit(_val);
    //}

    public void ChangeLife(int newLife)
    {
        lifesystem.SetLife(newLife);
    }
}
