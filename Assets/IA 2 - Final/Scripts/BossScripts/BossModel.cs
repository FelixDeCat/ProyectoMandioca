using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossModel : EnemyBase
{
    [SerializeField] BossBrain brain = new BossBrain();
    public int maxStamina = 9;
    [SerializeField] float rotSpeed = 10;
    Transform target = null;
    [SerializeField] Transform shootPosition = null;
    [SerializeField] AnimEvent animEvent = null;
    [SerializeField] ThrowData throwData = new ThrowData();
    [SerializeField] Throwable projectile = null;
    [SerializeField] int meleDamage = 4;
    [SerializeField] float meleKnockback = 10;
    [SerializeField] CombatComponent meleAttack;

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
        ThrowablePoolsManager.instance.CreateAPool(projectile.name, projectile);
        animEvent.Add_Callback("NormalShoot", ShootEvent);
        animEvent.Add_Callback("MeleAttack", MeleEvent);
        meleAttack.Configure(MeleAttack);
        dmgData.SetDamage(meleDamage).SetDamageInfo(DamageInfo.NonBlockAndParry).SetKnockback(meleKnockback);
        brain.Initialize(this, StartCoroutine);
        StartCombat();
    }

    public void StartCombat() => brain.PlanAndExecute();

    void ShootEvent()
    {
        throwData.Position = shootPosition.position;
        throwData.Direction = rootTransform.forward;
        ThrowablePoolsManager.instance.Throw(projectile.name, throwData);
    }

    void MeleEvent()
    {
        meleAttack.ManualTriggerAttack();
    }

    void MeleAttack(DamageReceiver dmgReceiver)
    {
        dmgReceiver.TakeDamage(dmgData.SetPositionAndDirection(rootTransform.position, rootTransform.forward));
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
        rootTransform.forward = Vector3.Slerp(rootTransform.forward, newForward, rotSpeed * Time.deltaTime);
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
