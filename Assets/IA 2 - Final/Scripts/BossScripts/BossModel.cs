using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossModel : EnemyBase
{
    [SerializeField] BossBrain brain = new BossBrain();
    public int maxStamina = 9;
    [SerializeField] float rotSpeed = 10;
    [SerializeField] Transform root = null;
    [SerializeField] Transform target = null;

    [SerializeField] float attackCooldownTime = 3;
    [SerializeField] float tpCooldownTime = 10;
    [SerializeField] float abilityCooldownTime = 8;

    #region Properties
    public int CurrentStamina { get; private set; }
    public int CurrentLife { get => lifesystem.Life; }
    public string MyAbilityMostUsed { get; private set; }
    public bool AttackOnCooldown { get; private set; }
    public bool AbilityOnCooldown { get; private set; }
    public bool TPOnCooldown { get; private set; }
    public bool ShieldActive { get; set; }
    #endregion
    string lastAbilityUsed = "";
    CDModule cdModule = new CDModule();

    protected override void OnInitialize()
    {
        base.OnInitialize();
        target = Main.instance.GetChar().Root;
        CurrentStamina = maxStamina;
        MyAbilityMostUsed = "";
        brain.Initialize(this);
    }

    protected override void OnUpdateEntity()
    {
        cdModule.UpdateCD();
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

    #region Functions to States
    public void ChangeLasAbility(string last)
    {
        if (last == lastAbilityUsed)
            MyAbilityMostUsed = last;
        else
        {
            MyAbilityMostUsed = "";
            lastAbilityUsed = last;
        }
    }

    public void RotateToChar()
    {
        Vector3 newForward = (target.position - transform.position).normalized;
        root.forward = Vector3.Slerp(root.forward, newForward, rotSpeed * Time.deltaTime);
    }

    public bool DistanceToCharacter() => Vector3.Distance(transform.position, target.position) <= brain.distanceToMele ? true : false;

    public void AttackCooldown()
    {
        AttackOnCooldown = true;
        cdModule.AddCD("AttackCD", () => AttackOnCooldown = false, attackCooldownTime);
    }

    public void AbilityCooldown()
    {
        AbilityOnCooldown = true;
        cdModule.AddCD("AbilityCD", () => AbilityOnCooldown = false, abilityCooldownTime);
    }

    public void TPCooldown()
    {
        TPOnCooldown = true;
        cdModule.AddCD("TPCD", () => TPOnCooldown = false, tpCooldownTime);
    }

    #endregion
}
