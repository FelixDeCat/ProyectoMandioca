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
        aStar.Initialize();

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
        stunState = new FinalBossStun(boss, expansiveSkill, timeStuned, _rb);
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
        Debug.Log("a ver de donde vengo");

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
        float distanceMinChar = minCharDistance + 1;

        var actions = new List<GOAPAction>{
                                              new GOAPAction(BetoStatesName.OnIdle)
                                                 .Pre(x => x.boolValues[GOAPParametersName.AttackOnCooldown])
                                                 .Effect(x => x.boolValues[GOAPParametersName.AttackOnCooldown] = false)
                                                 .LinkedState(idleState),

                                              new GOAPAction(BetoStatesName.OnMove)
                                                 .Pre(x=> x.floatValues[GOAPParametersName.CharDistance] < distanceMinChar || x.boolValues[GOAPParametersName.NearToFont])
                                                 .Pre(x => x.boolValues[GOAPParametersName.Flying]|| x.boolValues[GOAPParametersName.FlyCooldown])
                                                 .Effect(x => x.floatValues[GOAPParametersName.CharDistance] = distanceMinChar)
                                                 .Effect(x => x.boolValues[GOAPParametersName.NearToFont] = false)
                                                 .LinkedState(moveState),

                                              new GOAPAction(BetoStatesName.OnFly)
                                                 .Pre(x => !x.boolValues[GOAPParametersName.Flying])
                                                 .Pre(x => !x.boolValues[GOAPParametersName.FlyCooldown])
                                                 .Effect(x=>x.boolValues[GOAPParametersName.Flying] = true)
                                                 .LinkedState(flyState),

                                              new GOAPAction(BetoStatesName.OnShoot)
                                                 .Pre(x => x.boolValues[GOAPParametersName.Flying] || x.boolValues[GOAPParametersName.FlyCooldown])
                                                 .Pre(x => !x.boolValues[GOAPParametersName.AttackOnCooldown])
                                                 .Pre(x => !x.boolValues[GOAPParametersName.NearToFont])
                                                 .Effect(x=> x.boolValues[GOAPParametersName.AttackOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= x.boolValues[GOAPParametersName.SpawnCooldown] &&
                                                     (x.boolValues[GOAPParametersName.LakePoisonCooldown] ||
                                                     !x.boolValues[GOAPParametersName.LakePoisonCooldown] && !x.boolValues[GOAPParametersName.Flying])? 3000 : shootDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .LinkedState(shootState),

                                              new GOAPAction(BetoStatesName.OnSpawn)
                                                 .Pre(x => x.boolValues[GOAPParametersName.Flying] || x.boolValues[GOAPParametersName.FlyCooldown])
                                                 .Pre(x => !x.boolValues[GOAPParametersName.SpawnCooldown])
                                                 .Effect(x=> x.boolValues[GOAPParametersName.SpawnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= spawnDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .LinkedState(spawnState),

                                              new GOAPAction(BetoStatesName.OnPoisonLake)
                                                 .Pre(x => x.boolValues[GOAPParametersName.Flying])
                                                 .Pre(x => !x.boolValues[GOAPParametersName.LakePoisonCooldown])
                                                 .Effect(x=> x.boolValues[GOAPParametersName.LakePoisonCooldown] = true)
                                                 .Effect(x => {
                                                     x.floatValues[GOAPParametersName.CharDistance] = Vector3.Distance(Main.instance.GetChar().transform.position, fontPos.position);
                                                     if(x.floatValues[GOAPParametersName.CharDistance]>distanceMinChar)
                                                         x.floatValues[GOAPParametersName.CharDistance] = distanceMinChar;
                                                     } )
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= lakePoisonDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<minLifeValue) x.intValues[GOAPParametersName.CharLife] = minLifeValue;
                                                     } )
                                                 .Effect(x => x.boolValues[GOAPParametersName.NearToFont] = true)
                                                 .LinkedState(lakePoisonState)
                                                 .Cost(Mathf.Clamp(Vector3.Distance(model.transform.position, fontPos.position) * 0.5f, 1, 5)),
                                          };

        var from = new GOAPState();

        from.values.floatValues[GOAPParametersName.CharDistance] = Mathf.Clamp(Vector3.Distance(model.transform.position, Main.instance.GetChar().transform.position), 0, distanceMinChar);
        from.values.intValues[GOAPParametersName.CharLife] = Main.instance.GetChar().Life.Life;
        from.values.intValues[GOAPParametersName.OwnLife] = model.CurrentLife;
        from.values.boolValues[GOAPParametersName.Flying] = model.Flying;
        from.values.boolValues[GOAPParametersName.FlyCooldown] = model.FlyCooldown;
        from.values.boolValues[GOAPParametersName.AttackOnCooldown] = model.AttackOnCooldown;
        from.values.boolValues[GOAPParametersName.SpawnCooldown] = model.SpawnCooldown;
        from.values.boolValues[GOAPParametersName.NearToFont] = Vector3.Distance(model.transform.position, fontPos.position) < 5;
        from.values.boolValues[GOAPParametersName.LakePoisonCooldown] = model.LakeCooldown;

        var to = new GOAPState();
        to.values.floatValues[GOAPParametersName.CharDistance] = distanceMinChar;
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
        foreach (var item in plan)
            item.linkedState.Transitions.Add(BetoStatesName.OnStun, stunState);
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
