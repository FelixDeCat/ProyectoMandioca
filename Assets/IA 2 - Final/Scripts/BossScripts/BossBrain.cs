using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA2Final;
using IA2Final.FSM;

public class BossBrain : MonoBehaviour
{
    //Estados
    BossIdleState idleState;
    BossMeleState meleState;
    BossShootState shootState;
    BossFlameState flameState;
    BossShootAbility shootAbState;
    BossStunState stunState;
    BossTPState tpState;
    FiniteStateMachine fsm;

    [SerializeField] float distanceToMele = 5;
    [SerializeField] int meleDamage = 10;
    [SerializeField] int shootDamage = 5;
    [SerializeField] int flameDamage = 15;
    [SerializeField] int phantomShootDamage = 20;
    [SerializeField] int tpDamage = 5;
    int flameStaminaNeed = 3;
    int phantomShootStamina = 4;
    int lifeToChangePhase = 50;
    int maxStamina = 10;
    int currentStamina;
    int currentLife;

    string flameName = "Flame";
    string shootName = "PhantomShoot";

    private void Awake()
    {
        idleState = new BossIdleState();
        meleState = new BossMeleState();
        shootState = new BossShootState();
        flameState = new BossFlameState();
        shootAbState = new BossShootAbility();
        stunState = new BossStunState();
        tpState = new BossTPState();

        idleState.OnNeedsReplan += Replan;
        meleState.OnNeedsReplan += Replan;
        shootState.OnNeedsReplan += Replan;
        flameState.OnNeedsReplan += Replan;
        shootAbState.OnNeedsReplan += Replan;
        stunState.OnNeedsReplan += Replan;
        tpState.OnNeedsReplan += Replan;
    }

    void Start()
    {
        PlanAndExecute();
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

    private void PlanAndExecute()
    {
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
                                                     if(x.intValues[GOAPParametersName.CharLife]<0) x.intValues[GOAPParametersName.CharLife] = 0;
                                                     })
                                                 .LinkedState(meleState).Cost(3),

                                              new GOAPAction(GOAPStatesName.OnShootAttack)
                                                 .Pre(x=> x.floatValues[GOAPParametersName.CharDistance]>distanceToMele ? true : false)
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AttackOnCooldown] ? true : false)
                                                 .Effect(x=> x.boolValues[GOAPParametersName.AttackOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= shootDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<0) x.intValues[GOAPParametersName.CharLife] = 0;
                                                     } )
                                                 .LinkedState(shootState),

                                              new GOAPAction(GOAPStatesName.OnFlameAbility)
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AbilityOnCooldown] ? true : false)
                                                 .Pre(x => x.stringValues[GOAPParametersName.CharAbilityMostUsed] != "Dash" ? true : false)
                                                 .Effect(x=>x.boolValues[GOAPParametersName.AbilityOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= flameDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<0) x.intValues[GOAPParametersName.CharLife] = 0;
                                                     } )
                                                 .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Dash")
                                                 .LinkedState(flameState),

                                              new GOAPAction(GOAPStatesName.OnShootAbility)
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.AbilityOnCooldown] ? true : false)
                                                 .Pre(x => x.stringValues[GOAPParametersName.CharAbilityMostUsed] != "Parry" ? true : false)
                                                 .Effect(x=>x.boolValues[GOAPParametersName.AbilityOnCooldown] = true)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= phantomShootDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<0) x.intValues[GOAPParametersName.CharLife] = 0;
                                                     } )
                                                 .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Parry")
                                                 .LinkedState(shootAbState),

                                              new GOAPAction(GOAPStatesName.OnStunAbility)
                                                 .Pre(x=>x.intValues[GOAPParametersName.OwnLife] < lifeToChangePhase ? true: false)
                                                 .Pre(x=>x.intValues[GOAPParametersName.Stamina] <= 0 ? true : false)
                                                 .Effect(x => x.intValues[GOAPParametersName.Stamina] = maxStamina)
                                                 .LinkedState(stunState),

                                              new GOAPAction(GOAPStatesName.OnTPAbility)
                                                 .Pre(x=> !x.boolValues[GOAPParametersName.TPOnCooldown])
                                                 .Pre(x => x.floatValues[GOAPParametersName.CharDistance] <= distanceToMele ? true : false)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= tpDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<0) x.intValues[GOAPParametersName.CharLife] = 0;
                                                     } )
                                                 .Effect(x=>x.boolValues[GOAPParametersName.TPOnCooldown] = true)
                                                 .Effect(x=>x.floatValues[GOAPParametersName.CharDistance] = 20)
                                                 .LinkedState(tpState),

                                              new GOAPAction(GOAPStatesName.OnFlameAbility)
                                                 .Pre(x=> x.intValues[GOAPParametersName.Stamina] > 0)
                                                 .Pre(x => x.stringValues[GOAPParametersName.CharAbilityMostUsed] != "Dash" ? true : false)
                                                 .Effect(x=>x.intValues[GOAPParametersName.Stamina] -= flameStaminaNeed)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= flameDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<0) x.intValues[GOAPParametersName.CharLife] = 0;
                                                     } )
                                                 .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Dash")
                                                 .LinkedState(flameState),

                                              new GOAPAction(GOAPStatesName.OnShootAbility)
                                                 .Pre(x=> x.intValues[GOAPParametersName.Stamina] > 0)
                                                 .Pre(x => x.stringValues[GOAPParametersName.CharAbilityMostUsed] != "Parry" ? true : false)
                                                 .Effect(x=>x.intValues[GOAPParametersName.Stamina] -= phantomShootStamina)
                                                 .Effect(x => {
                                                     x.intValues[GOAPParametersName.CharLife] -= phantomShootDamage;
                                                     if(x.intValues[GOAPParametersName.CharLife]<0) x.intValues[GOAPParametersName.CharLife] = 0;
                                                     } )
                                                 .Effect(x=>x.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Parry")
                                                 .LinkedState(idleState),
                                          };

        var from = new GOAPState();

        from.values.floatValues[GOAPParametersName.CharDistance] = Vector3.Distance(transform.position, Main.instance.GetChar().transform.position);
        from.values.intValues[GOAPParametersName.CharLife] = Main.instance.GetChar().Life.Life;
        from.values.intValues[GOAPParametersName.OwnLife] = currentLife;
        from.values.stringValues[GOAPParametersName.CharAbilityMostUsed] = "Dash";
        from.values.boolValues[GOAPParametersName.TPOnCooldown] = false;
        from.values.boolValues[GOAPParametersName.AbilityOnCooldown] = true;
        from.values.intValues[GOAPParametersName.Stamina] = currentStamina;
        from.values.stringValues[GOAPParametersName.LastOwnAbility] = "";
        from.values.boolValues[GOAPParametersName.AttackOnCooldown] = true;

        var to = new GOAPState();
        to.values.intValues[GOAPParametersName.CharLife] = 0;

        var planner = new GoapPlanner();
        planner.OnPlanCompleted += OnPlanCompleted;
        planner.OnCantPlan += OnCantPlan;

        planner.Run(from, to, actions, StartCoroutine);
    }


    private void OnPlanCompleted(IEnumerable<GOAPAction> plan)
    {
        fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        fsm.Active = true;
    }

    private void OnCantPlan()
    {
        //TODO: debuggeamos para ver por qué no pudo planear y encontrar como hacer para que no pase nunca mas

        Debug.Log("No pude planear");
    }
}
