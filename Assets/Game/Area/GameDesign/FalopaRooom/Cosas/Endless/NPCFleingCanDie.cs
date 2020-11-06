using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(DamageReceiver), typeof(GenericLifeSystem))]
public class NPCFleingCanDie : NPCFleing
{
    DamageReceiver damageReceiver;
    GenericLifeSystem lifeSystem;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        
        damageReceiver = GetComponent<DamageReceiver>();
        lifeSystem = GetComponent<GenericLifeSystem>();

        lifeSystem.Initialize(lifeSystem.life, ()=> { }, () => { }, OnDead);

        damageReceiver.Initialize(transform, rb, lifeSystem);
        damageReceiver.AddTakeDamage(OnTakeDamage);

        onArrivedEvent += () => Main.instance.GetVillageManager().ReTarget();
        onArrivedEvent += () => Main.instance.GetVillageManager().RemoveFromVillagersAlive(this);
    }
          

    void OnTakeDamage(DamageData data)
    {
        if (data.owner == Main.instance.GetChar()) return;
        data.GetComponent<CombatDirectorElement>().ChangeTarget(transform);
    }

    void OnDead()
    {
        Main.instance.GetVillageManager().RemoveFromVillagersAlive(this);
        Main.instance.GetVillageManager().ReTarget();
        gameObject.SetActive(false);
    }
}
