using System.Collections;
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
    [SerializeField] float timeStuned = 5;
    public float lifeToChangePhase = 0.5f;
    [SerializeField] Animator anim = null;
    [SerializeField] BossSkills flameSkill = null;
    [SerializeField] BossSkills phantomSkill = null;
    [SerializeField] BossSkills tpSkill = null;
    [SerializeField] SpawnSkill spawnSkill = null;
    [SerializeField] BossSkills phantomSkillSecondStage = null;
    [SerializeField] BossSkills flameSkillSecondStage = null;
    [SerializeField] SpawnSkill spawnSkillSecondStage = null;

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
        flameSkillSecondStage.Initialize();
        phantomSkillSecondStage.Initialize();
        spawnSkillSecondStage.Initialize();
        model = boss;
        Coroutine = _Coroutine;
        idleState = new BossIdleState(model);
        meleState = new BossMeleState(model, anim);
        shootState = new BossShootState(model, anim);
        flameState = new BossFlameState(model, flameSkill,flameStaminaNeed);
        shootAbState = new BossShootAbility(model, phantomSkill,phantomShootStamina);
        stunState = new BossStunState(timeStuned, model.yMinPos, model.yMaxPos, model.ascendSpeed, model);
        tpState = new BossTPState(model, tpSkill);
        spawnState = new BossSpawnState(model, spawnSkill,spawnStaminaNeed);


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

        fsm.CurrentState.Exit(null);
        fsm.Active = false;

        PlanAndExecute();
    }

    public void ChangePhase()
    {
        fsm.CurrentState.Exit(null);
        DesactiveFSM();
        flameSkill.InterruptSkill();
        phantomSkill.InterruptSkill();
        spawnSkill.InterruptSkill();
        tpSkill.InterruptSkill();
        flameState.ChangeSkill(flameSkillSecondStage);
        shootAbState.ChangeSkill(phantomSkillSecondStage);
        spawnState.ChangeSkill(spawnSkillSecondStage);
    }

    public void PlanAndExecute()
    {
        int minLifeValue = Mathf.Clamp(Main.instance.GetChar().Life.Life - 12, 0, Main.instance.GetChar().Life.Life);

        float flameCost = Mathf.Clamp(model.flameCount - model.shootCount - model.spawnCount + (dashCount / 2) - parryCount - heavyCount, 1, 4);
        float phantomCost = Mathf.Clamp(model.shootCount - model.flameCount - model.spawnCount - (dashCount / 2) + parryCount - heavyCount, 1, 4);
        float spawnCost = Mathf.Clamp(-model.flameCount - model.shootCount + model.spawnCount - (dashCount / 2) - parryCount + heavyCount, 1, 4);


        var actions = new List<GOAPAction>{
                                              new GOAPAction(GOAPStatesName.OnIdle)
                                                 .Pre(x =>{if(x.boolValues[GOAPParametersName.ShieldActive])
                                                         return true;
                                                     else
                                                     {
                                                         if(x.boolValues[GOAPParametersName.AbilityOnCooldown]) return true;
                                                             else return false; } })
                                                 .Pre(x => x.boolValues[GOAPParametersName.AttackOnCooldown] ? true : false)
                                                 .Effect(x => x.boolValues[GOAPParametersName.AbilityOnCooldown] = false)
                                                 .Effect(x => x.boolValues[GOAPParametersName.AttackOnCooldown] = false)
                                                 .LinkedState(idleState),

                                              new GOAPAction(GOAPStatesName.OnMeleAttack)
                                                 .Pre(x=>x.intValues[GOAPParametersName.OwnLife] > model.lifesystem.LifeMax * lifeToChangePhase)
                                                 .Pre(x => !x.boolValues[GOAPParametersName.AttackOnCooldown] ? true : false)
                                                 .Pre(x => x.floatValues[GOAPParametersName.CharDistance] <= distanceToMele ? true : false)
                                                 .Effect(x => x.boolValues[GOAPParametersName.AttackOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= x.boolValues[GOAPParametersName.ShieldActive] ?12 : meleDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     })
                                                 .LinkedState(meleState).Cost(3),

                                              new GOAPAction(GOAPStatesName.OnShootAttack)
                                                 .Pre(x=> x.floatValues[GOAPParametersName.CharDistance]>distanceToMele || x.intValues[GOAPParametersName.OwnLife] <= model.lifesystem.LifeMax * lifeToChangePhase ? true : false)
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AttackOnCooldown] ? true : false)
                                                 .Pre(x=> (x.intValues[GOAPParametersName.Stamina] > 0) || (x.boolValues[GOAPParametersName.ShieldActive] && x.intValues[GOAPParametersName.Stamina] <= 0))
                                                 .Effect(x=> x.boolValues[GOAPParametersName.AttackOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= x.boolValues[GOAPParametersName.ShieldActive] ?3000 : shootDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .LinkedState(shootState),

                                              new GOAPAction(GOAPStatesName.OnFlameAbility)
                                                 .Pre(x=>x.intValues[GOAPParametersName.OwnLife] > model.lifesystem.LifeMax * lifeToChangePhase)
                                                 .Pre(x=>x.stringValues[GOAPParametersName.LastOwnAbility] != "Flame")
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AbilityOnCooldown] ? true : false)
                                                 .Pre(x=>!x.boolValues[GOAPParametersName.ShieldActive])
                                                 .Effect(x=>x.boolValues[GOAPParametersName.AbilityOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= flameDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Dash")
                                                 .Effect(x=>x.stringValues[GOAPParametersName.LastOwnAbility] = "Flame")
                                                 .LinkedState(flameState).Cost(flameCost),

                                              new GOAPAction(GOAPStatesName.OnSpawnAbility)
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AbilityOnCooldown] ? true : false)
                                                 .Pre(x=>x.intValues[GOAPParametersName.OwnLife] > model.lifesystem.LifeMax * lifeToChangePhase)
                                                 .Pre(x=>x.stringValues[GOAPParametersName.LastOwnAbility] != "Spawn")
                                                 .Pre(x=>!x.boolValues[GOAPParametersName.ShieldActive])
                                                 .Effect(x=>x.boolValues[GOAPParametersName.AbilityOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= spawnDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Heavy")
                                                 .Effect(x=>x.stringValues[GOAPParametersName.LastOwnAbility] = "Spawn")
                                                 .Effect(x=>x.boolValues[GOAPParametersName.ShieldActive] = true)
                                                 .LinkedState(spawnState).Cost(spawnCost),

                                              new GOAPAction(GOAPStatesName.OnShootAbility)
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AbilityOnCooldown] ? true : false)
                                                 .Pre(x=>x.intValues[GOAPParametersName.OwnLife] > model.lifesystem.LifeMax * lifeToChangePhase)
                                                 .Pre(x=>x.stringValues[GOAPParametersName.LastOwnAbility] != "Phantom")
                                                 .Pre(x=>!x.boolValues[GOAPParametersName.ShieldActive])
                                                 .Effect(x=>x.boolValues[GOAPParametersName.AbilityOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= phantomShootDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Parry")
                                                 .Effect(x=>x.stringValues[GOAPParametersName.LastOwnAbility] = "Phantom")
                                                 .LinkedState(shootAbState).Cost(phantomCost),

                                              new GOAPAction(GOAPStatesName.OnStunAbility)
                                                 .Pre(x=>x.intValues[GOAPParametersName.OwnLife] <= model.lifesystem.LifeMax * lifeToChangePhase ? true: false)
                                                 .Pre(x=>!x.boolValues[GOAPParametersName.ShieldActive])
                                                 .Pre(x=>x.intValues[GOAPParametersName.Stamina] <= 0 ? true : false)
                                                 .Effect(x => x.intValues[GOAPParametersName.Stamina] = model.maxStamina)
                                                 .LinkedState(stunState),

                                              new GOAPAction(GOAPStatesName.OnTPAbility)
                                                 .Pre(x=>x.intValues[GOAPParametersName.OwnLife] > model.lifesystem.LifeMax * lifeToChangePhase)
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.TPOnCooldown])
                                                 .Pre(x => x.floatValues[GOAPParametersName.CharDistance] <= distanceToMele ? true : false)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= tpDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .Effect(x=>x.boolValues[GOAPParametersName.TPOnCooldown] = true)
                                                 .Effect(x=>x.floatValues[GOAPParametersName.CharDistance] = 20)
                                                 .LinkedState(tpState),

                                              new GOAPAction(GOAPStatesName.OnFlameAbility)
                                                 .Pre(x=>x.intValues[GOAPParametersName.OwnLife] <= model.lifesystem.LifeMax * lifeToChangePhase)
                                                 .Pre(x=> x.intValues[GOAPParametersName.Stamina] > 0)
                                                 .Pre(x=>x.stringValues[GOAPParametersName.LastOwnAbility] != "Flame")
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AbilityOnCooldown] ? true : false)
                                                 .Pre(x=>!x.boolValues[GOAPParametersName.ShieldActive])
                                                 .Effect(x=>x.intValues[GOAPParametersName.Stamina] -= flameStaminaNeed)
                                                 .Effect(x=>x.boolValues[GOAPParametersName.AbilityOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= flameDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Dash")
                                                 .Effect(x=>x.stringValues[GOAPParametersName.LastOwnAbility] = "Flame")
                                                 .LinkedState(flameState).Cost(flameCost),

                                              new GOAPAction(GOAPStatesName.OnSpawnAbility)
                                                 .Pre(x=>x.intValues[GOAPParametersName.OwnLife] <= model.lifesystem.LifeMax * lifeToChangePhase)
                                                 .Pre(x=> x.intValues[GOAPParametersName.Stamina] > 0)
                                                 .Pre(x=>x.stringValues[GOAPParametersName.LastOwnAbility] != "Spawn")
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AbilityOnCooldown] ? true : false)
                                                 .Pre(x=>!x.boolValues[GOAPParametersName.ShieldActive])
                                                 .Effect(x=>x.intValues[GOAPParametersName.Stamina] -= spawnStaminaNeed)
                                                 .Effect(x=>x.boolValues[GOAPParametersName.AbilityOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= spawnDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Heavy")
                                                 .Effect(x=>x.stringValues[GOAPParametersName.LastOwnAbility] = "Spawn")
                                                 .Effect(x=>x.boolValues[GOAPParametersName.ShieldActive] = true)
                                                 .LinkedState(spawnState).Cost(spawnCost),

                                              new GOAPAction(GOAPStatesName.OnShootAbility)
                                                 .Pre(x=>x.intValues[GOAPParametersName.OwnLife] <= model.lifesystem.LifeMax * lifeToChangePhase)
                                                 .Pre(x=> x.intValues[GOAPParametersName.Stamina] > 0)
                                                 .Pre(x=>x.stringValues[GOAPParametersName.LastOwnAbility] != "Phantom")
                                                 .Pre(x=>!x.boolValues[GOAPParametersName.ShieldActive])
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AbilityOnCooldown] ? true : false)
                                                 .Effect(x=>x.intValues[GOAPParametersName.Stamina] -= phantomShootStamina)
                                                 .Effect(x=>x.boolValues[GOAPParametersName.AbilityOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= phantomShootDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Parry")
                                                 .Effect(x=>x.stringValues[GOAPParametersName.LastOwnAbility] = "Phantom")
                                                 .LinkedState(shootAbState).Cost(phantomCost),
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
        if (Main.instance.GetChar().Life.Life <= 0) return;
        fsm = GoapPlanner.ConfigureFSM(plan, Coroutine);
        if (fsm != null) fsm.Active = true;
    }

    public void ActiveFSM() { if (fsm != null) fsm.Active = true; }
    public void DesactiveFSM() { if (fsm != null) fsm.Active = false; }

    private void OnCantPlan()
    {
        //TODO: debuggeamos para ver por qué no pudo planear y encontrar como hacer para que no pase nunca mas

        Debug.Log("No pude planear");
        //Replan();
    }

    public void ResetBrain()
    {
        fsm.CurrentState.Exit(null);
        DesactiveFSM();
        flameSkill.InterruptSkill();
        phantomSkill.InterruptSkill();
        spawnSkill.InterruptSkill();
        tpSkill.InterruptSkill();
        flameSkillSecondStage.InterruptSkill();
        phantomSkillSecondStage.InterruptSkill();
        spawnSkillSecondStage.InterruptSkill();
        flameState.ChangeSkill(flameSkill);
        shootAbState.ChangeSkill(phantomSkill);
        spawnState.ChangeSkill(spawnSkill);
    }
}
