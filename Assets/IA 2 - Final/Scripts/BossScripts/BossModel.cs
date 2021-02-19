using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossModel : EnemyBase
{
    [SerializeField] BossBrain brain = null;
    public int maxStamina = 9;
    public int CurrentStamina { get; private set; }
    public int CurrentLife { get => lifesystem.Life; }
    public string MyAbilityMostUsed { get; private set; }
    string lastAbilityUsed = "";

    protected override void OnInitialize()
    {
        base.OnInitialize();
        brain.Initialize(this);
        CurrentStamina = maxStamina;
        MyAbilityMostUsed = "";
    }

    protected override void OnUpdateEntity()
    {
    }

    protected override void OnFixedUpdate()
    {
    }

    protected override void OnReset()
    {
    }

    protected override void OnTurnOff()
    {
    }

    protected override void OnTurnOn()
    {
    }

    public void ChangeLasAbility(string last)
    {
        if(last == lastAbilityUsed)
            MyAbilityMostUsed = last;
        else
        {
            MyAbilityMostUsed = "";
            lastAbilityUsed = last;
        }
    }

    protected override void TakeDamageFeedback(DamageData data)
    {
    }

    protected override bool IsDamage()
    {
        return false;
    }

    protected override void Die(Vector3 dir)
    {
    }
}
