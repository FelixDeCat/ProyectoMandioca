﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA2Final;
using IA2Final.FSM;
using System;

[Serializable]
public class BossBrain
{
    //Estados
    BossIdleState idleState;
    BossMeleState meleState;
    BossShootState shootState;
    BossFlameState flameState;
    BossShootAbility shootAbState;
    BossStunState stunState;
    BossTPState tpState;
    BossSpawnState spawnState;
    FiniteStateMachine fsm;

    BossModel model;

    public float distanceToMele = 5;
    [SerializeField] int meleDamage = 10;
    [SerializeField] int shootDamage = 5;
    [SerializeField] int flameDamage = 15;
    [SerializeField] int spawnDamage = 10;
    [SerializeField] int phantomShootDamage = 20;
    [SerializeField] int tpDamage = 5;
    [SerializeField] int flameStaminaNeed = 3;
    [SerializeField] int phantomShootStamina = 4;
    [SerializeField] int spawnStaminaNeed = 5;
    [SerializeField] int lifeToChangePhase = 50;
    [SerializeField] Animator anim = null;
    [SerializeField] BossSkills flameSkill = null;
    [SerializeField] BossSkills phantomSkill = null;
    [SerializeField] BossSkills tpSkill = null;
    [SerializeField] SpawnSkill spawnSkill = null;

    int dashCount = 0;
    int heavyCount = 0;
    int parryCount = 0;
    string charAbilityMostUsed = "Heavy";
    Func<IEnumerator, Coroutine> Coroutine;

    public void Initialize(BossModel boss, Func<IEnumerator, Coroutine> _Coroutine)
    {
        flameSkill.Initialize();
        phantomSkill.Initialize();
        tpSkill.Initialize();
        spawnSkill.Initialize();
        model = boss;
        Coroutine = _Coroutine;
        idleState = new BossIdleState(model);
        meleState = new BossMeleState(model, anim);
        shootState = new BossShootState(model, anim);
        flameState = new BossFlameState(model, flameSkill);
        shootAbState = new BossShootAbility(model, phantomSkill);
        stunState = new BossStunState();
        tpState = new BossTPState(model, tpSkill);
        spawnState = new BossSpawnState(model, spawnSkill);


        idleState.OnNeedsReplan += Replan;
        meleState.OnNeedsReplan += Replan;
        shootState.OnNeedsReplan += Replan;
        flameState.OnNeedsReplan += Replan;
        shootAbState.OnNeedsReplan += Replan;
        stunState.OnNeedsReplan += Replan;
        tpState.OnNeedsReplan += Replan;
        spawnState.OnNeedsReplan += Replan;

        Main.instance.GetChar().AddListenerToDash(() => { dashCount += 1; if (dashCount > heavyCount && dashCount > parryCount) charAbilityMostUsed = "Dash"; });
        Main.instance.GetChar().AddParry(() => { parryCount += 1; if (parryCount > dashCount && parryCount > heavyCount) charAbilityMostUsed = "Parry"; });
        Main.instance.GetChar().GetCharacterAttack().Add_callback_Heavy_attack(() =>
        {
            heavyCount += 1;
            if (heavyCount > dashCount && heavyCount > parryCount) charAbilityMostUsed = "Heavy";
        });
    }

    float timeToPlan = 0.3f;
    float timeFrame;

    void Replan()
    {
        if (Time.time >= timeToPlan + timeFrame)
            timeFrame = Time.time;
        else
            return;

        fsm.Active = false;

        PlanAndExecute();
    }

    public void PlanAndExecute()
    {
        int minLifeValue = Mathf.Clamp(Main.instance.GetChar().Life.Life - 12, 0, Main.instance.GetChar().Life.Life);

        var actions = new List<GOAPAction>{
                                              new GOAPAction(GOAPStatesName.OnIdle)
                                                 .Pre(x => x.boolValues[GOAPParametersName.AbilityOnCooldown] ? true :false)
                                                 .Pre(x => x.boolValues[GOAPParametersName.AttackOnCooldown] ? true : false)
                                                 .Effect(x => x.boolValues[GOAPParametersName.AbilityOnCooldown] = false)
                                                 .Effect(x => x.boolValues[GOAPParametersName.AttackOnCooldown] = false)
                                                 .LinkedState(idleState),

                                              new GOAPAction(GOAPStatesName.OnMeleAttack)
                                                 .Pre(x => !x.boolValues[GOAPParametersName.AttackOnCooldown] ? true : false)
                                                 .Pre(x => x.floatValues[GOAPParametersName.CharDistance] <= distanceToMele ? true : false)
                                                 .Effect(x => x.boolValues[GOAPParametersName.AttackOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= meleDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     })
                                                 .LinkedState(meleState).Cost(3),

                                              new GOAPAction(GOAPStatesName.OnShootAttack)
                                                 .Pre(x=> x.floatValues[GOAPParametersName.CharDistance]>distanceToMele ? true : false)
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AttackOnCooldown] ? true : false)
                                                 .Effect(x=> x.boolValues[GOAPParametersName.AttackOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= shootDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .LinkedState(shootState),

                                              new GOAPAction(GOAPStatesName.OnFlameAbility)
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AbilityOnCooldown] ? true : false)
                                                 .Pre(x=>!x.boolValues[GOAPParametersName.ShieldActive])
                                                 .Pre(x => x.stringValues[GOAPParametersName.CharAbilityMostUsed] != "Dash" ? true : false)
                                                 .Effect(x=>x.boolValues[GOAPParametersName.AbilityOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= flameDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Dash")
                                                 .LinkedState(flameState),

                                              new GOAPAction(GOAPStatesName.OnSpawnAbility)
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AbilityOnCooldown] ? true : false)
                                                 .Pre(x=>!x.boolValues[GOAPParametersName.ShieldActive])
                                                 .Pre(x => x.stringValues[GOAPParametersName.CharAbilityMostUsed] != "Heavy" ? true : false)
                                                 .Effect(x=>x.boolValues[GOAPParametersName.AbilityOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= spawnDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Heavy")
                                                 .Effect(x=>x.boolValues[GOAPParametersName.ShieldActive] = true)
                                                 .LinkedState(spawnState),

                                              new GOAPAction(GOAPStatesName.OnShootAbility)
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AbilityOnCooldown] ? true : false)
                                                 .Pre(x=>!x.boolValues[GOAPParametersName.ShieldActive])
                                                 .Pre(x => x.stringValues[GOAPParametersName.CharAbilityMostUsed] != "Parry" ? true : false)
                                                 .Effect(x=>x.boolValues[GOAPParametersName.AbilityOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= phantomShootDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Parry")
                                                 .LinkedState(shootAbState),

                                              new GOAPAction(GOAPStatesName.OnStunAbility)
                                                 .Pre(x=>x.intValues[GOAPParametersName.OwnLife] < lifeToChangePhase ? true: false)
                                                 .Pre(x=>x.intValues[GOAPParametersName.Stamina] <= 0 ? true : false)
                                                 .Effect(x => x.intValues[GOAPParametersName.Stamina] = model.maxStamina)
                                                 .LinkedState(stunState),

                                              new GOAPAction(GOAPStatesName.OnTPAbility)
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.TPOnCooldown])
                                                 .Pre(x => x.floatValues[GOAPParametersName.CharDistance] <= distanceToMele ? true : false)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= tpDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .Effect(x=>x.boolValues[GOAPParametersName.TPOnCooldown] = true)
                                                 .Effect(x=>x.floatValues[GOAPParametersName.CharDistance] = 20)
                                                 .LinkedState(tpState),

                                              //new GOAPAction(GOAPStatesName.OnFlameAbility)
                                              //   .Pre(x=> x.intValues[GOAPParametersName.Stamina] > 0)
                                              //   .Pre(x => x.stringValues[GOAPParametersName.CharAbilityMostUsed] != "Dash" ? true : false)
                                              //   .Pre(x=>!x.boolValues[GOAPParametersName.ShieldActive])
                                              //   .Effect(x=>x.intValues[GOAPParametersName.Stamina] -= flameStaminaNeed)
                                              //   .Effect(x => {
                                              //       x.intValues[GOAPParametersName.CharLife] -= flameDamage;
                                              //       if(x.intValues[GOAPParametersName.CharLife]<0) x.intValues[GOAPParametersName.CharLife] = 0;
                                              //       } )
                                              //   .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Dash")
                                              //   .LinkedState(flameState),

                                              //new GOAPAction(GOAPStatesName.OnSpawnAbility)
                                              //   .Pre(x=> x.intValues[GOAPParametersName.Stamina] > 0)
                                              //   .Pre(x=>!x.boolValues[GOAPParametersName.ShieldActive])
                                              //   .Pre(x => x.stringValues[GOAPParametersName.CharAbilityMostUsed] != "Heavy" ? true : false)
                                              //   .Effect(x=>x.intValues[GOAPParametersName.Stamina] -= spawnStaminaNeed)
                                              //   .Effect(x => {
                                              //       x.intValues[GOAPParametersName.CharLife] -= spawnDamage;
                                              //       if(x.intValues[GOAPParametersName.CharLife]<0) x.intValues[GOAPParametersName.CharLife] = 0;
                                              //       } )
                                              //   .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Heavy")
                                              //   .Effect(x=>x.boolValues[GOAPParametersName.ShieldActive] = true)
                                              //   .LinkedState(spawnState),

                                              //new GOAPAction(GOAPStatesName.OnShootAbility)
                                              //   .Pre(x=> x.intValues[GOAPParametersName.Stamina] > 0)
                                              //   .Pre(x=>!x.boolValues[GOAPParametersName.ShieldActive])
                                              //   .Pre(x => x.stringValues[GOAPParametersName.CharAbilityMostUsed] != "Parry" ? true : false)
                                              //   .Effect(x=>x.intValues[GOAPParametersName.Stamina] -= phantomShootStamina)
                                              //   .Effect(x => {
                                              //       x.intValues[GOAPParametersName.CharLife] -= phantomShootDamage;
                                              //       if(x.intValues[GOAPParametersName.CharLife]<0) x.intValues[GOAPParametersName.CharLife] = 0;
                                              //       } )
                                              //   .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Parry")
                                              //   .LinkedState(idleState),
                                          };

        var from = new GOAPState();

        from.values.floatValues[GOAPParametersName.CharDistance] = Vector3.Distance(model.transform.position, Main.instance.GetChar().transform.position);
        from.values.intValues[GOAPParametersName.CharLife] = Main.instance.GetChar().Life.Life;
        from.values.intValues[GOAPParametersName.OwnLife] = model.CurrentLife;
        from.values.stringValues[GOAPParametersName.CharAbilityMostUsed] = charAbilityMostUsed;
        from.values.boolValues[GOAPParametersName.TPOnCooldown] = model.TPOnCooldown;
        from.values.boolValues[GOAPParametersName.AbilityOnCooldown] = model.AbilityOnCooldown;
        from.values.intValues[GOAPParametersName.Stamina] = model.CurrentStamina;
        from.values.boolValues[GOAPParametersName.ShieldActive] = model.ShieldActive;
        from.values.stringValues[GOAPParametersName.LastOwnAbility] = model.MyAbilityMostUsed;
        from.values.boolValues[GOAPParametersName.AttackOnCooldown] = model.AttackOnCooldown;

        var to = new GOAPState();
        to.values.intValues[GOAPParametersName.CharLife] = minLifeValue;

        var planner = new GoapPlanner();
        planner.OnPlanCompleted += OnPlanCompleted;
        planner.OnCantPlan += OnCantPlan;

        planner.Run(from, to, actions, Coroutine);
    }

    private void OnPlanCompleted(IEnumerable<GOAPAction> plan)
    {
        fsm = GoapPlanner.ConfigureFSM(plan, Coroutine);
        if (fsm != null) fsm.Active = true;
    }

    public void ActiveFSM() { if (fsm != null) fsm.Active = true; }
    public void DesactiveFSM() { if (fsm != null) fsm.Active = false; }

    private void OnCantPlan()
    {
        //TODO: debuggeamos para ver por qué no pudo planear y encontrar como hacer para que no pase nunca mas

        Debug.Log("No pude planear");
    }

    public void ResetBrain()
    {
        DesactiveFSM();
        flameSkill.InterruptSkill();
        phantomSkill.InterruptSkill();
        spawnSkill.InterruptSkill();
        tpSkill.InterruptSkill();
    }
}
