using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA2Final;
using IA2Final.FSM;
using System;

[Serializable]
public class BetoBrain
{
    //Estados
    FinalBossIdle idleState;
    FinalBossMove moveState;
    FinalBossFly flyState;
    FinalBossLaser shootState;
    FinalBossSpawn spawnState;
    FinalBossStun stunState;
    FinalBossPoison lakePoisonState;
    FiniteStateMachine fsm;

    BetoBoss model;

    [SerializeField] int shootDamage = 5;
    [SerializeField] int lakePoisonDamage = 15;
    [SerializeField] int spawnDamage = 10;
    [SerializeField] float timeStuned = 5;
    [SerializeField] Transform fontPos = null;

    [SerializeField] Animator anim = null;
    [SerializeField] BossSkills laserSkill = null;
    [SerializeField] FinalThunderWaveSkill spawnSkill = null;
    [SerializeField] BossSkills expansiveSkill = null;
    [SerializeField] FinalPoisonLakeSkill poisonSkill = null;

    [SerializeField] AStarHelper aStar = null;
    public float minCharDistance = 10;

    Func<IEnumerator, Coroutine> Coroutine;

    public void Initialize(BetoBoss boss, Func<IEnumerator, Coroutine> _Coroutine, Rigidbody _rb)
    {
        laserSkill.Initialize();
        spawnSkill.Initialize();
        expansiveSkill.Initialize();
        poisonSkill.Initialize();

        model = boss;
        Coroutine = _Coroutine;

        idleState = new FinalBossIdle(model);
        moveState = new FinalBossMove(model, anim, aStar, Main.instance.GetChar().transform, minCharDistance);
        flyState = new FinalBossFly(model, anim, _rb);
        shootState = new FinalBossLaser(model, laserSkill);
        spawnState = new FinalBossSpawn(boss, spawnSkill);
        stunState = new FinalBossStun(boss, expansiveSkill, timeStuned);
        lakePoisonState = new FinalBossPoison(model, poisonSkill, anim, aStar, fontPos.transform.position);

        idleState.OnNeedsReplan += Replan;
        moveState.OnNeedsReplan += Replan;
        flyState.OnNeedsReplan += Replan;
        shootState.OnNeedsReplan += Replan;
        spawnState.OnNeedsReplan += Replan;
        stunState.OnNeedsReplan += Replan;
        lakePoisonState.OnNeedsReplan += Replan;
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

    public void PlanAndExecute()
    {
        int minLifeValue = Mathf.Clamp(Main.instance.GetChar().Life.Life - 12, 0, Main.instance.GetChar().Life.Life);

        var actions = new List<GOAPAction>{
                                              new GOAPAction(BetoStatesName.OnIdle)
                                                 .Pre(x => x.boolValues[GOAPParametersName.AttackOnCooldown])
                                                 .Effect(x => x.boolValues[GOAPParametersName.AttackOnCooldown] = false)
                                                 .LinkedState(idleState),

                                              new GOAPAction(BetoStatesName.OnMove)
                                                 .LinkedState(moveState),

                                              new GOAPAction(BetoStatesName.OnFly)
                                                 .LinkedState(flyState),

                                              new GOAPAction(BetoStatesName.OnShoot)
                                                 .LinkedState(shootState),

                                              new GOAPAction(BetoStatesName.OnSpawn)
                                                 .LinkedState(spawnState),

                                              new GOAPAction(BetoStatesName.OnPoisonLake)
                                                 .LinkedState(lakePoisonState),
                                          };

        var from = new GOAPState();

        from.values.floatValues[GOAPParametersName.CharDistance] = Vector3.Distance(model.transform.position, Main.instance.GetChar().transform.position);
        from.values.intValues[GOAPParametersName.CharLife] = Main.instance.GetChar().Life.Life;
        from.values.intValues[GOAPParametersName.OwnLife] = model.CurrentLife;
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

        laserSkill.InterruptSkill();
        spawnSkill.InterruptSkill();
        expansiveSkill.InterruptSkill();
        poisonSkill.InterruptSkill();
    }
}
