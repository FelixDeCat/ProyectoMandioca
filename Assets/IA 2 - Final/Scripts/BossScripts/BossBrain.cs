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

    BossModel model;

    [SerializeField] float distanceToMele = 5;
    [SerializeField] int meleDamage = 10;
    [SerializeField] int shootDamage = 5;
    [SerializeField] int flameDamage = 15;
    [SerializeField] int phantomShootDamage = 20;
    [SerializeField] int tpDamage = 5;
    [SerializeField] int flameStaminaNeed = 3;
    [SerializeField] int phantomShootStamina = 4;
    [SerializeField] int lifeToChangePhase = 50;

    bool abilityCooldown = true;
    bool attackCooldown = true;
    bool tpCooldown = false;

    int dashCount = 0;
    int heavyCount = 0;
    int parryCount = 0;
    string charAbilityMostUsed = "Heavy";

    public void Initialize(BossModel boss)
    {
        model = boss;
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

        PlanAndExecute();


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
                                                 .Effect(x => x.intValues[GOAPParametersName.Stamina] = model.maxStamina)
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
        from.values.intValues[GOAPParametersName.OwnLife] = model.CurrentLife;
        from.values.stringValues[GOAPParametersName.CharAbilityMostUsed] = charAbilityMostUsed;
        from.values.boolValues[GOAPParametersName.TPOnCooldown] = tpCooldown;
        from.values.boolValues[GOAPParametersName.AbilityOnCooldown] = abilityCooldown;
        from.values.intValues[GOAPParametersName.Stamina] = model.CurrentStamina;
        from.values.stringValues[GOAPParametersName.LastOwnAbility] = model.MyAbilityMostUsed;
        from.values.boolValues[GOAPParametersName.AttackOnCooldown] = attackCooldown;

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

    public void ActualizeAbCD(bool value) => abilityCooldown = value;

    public void ActualizeAttCD(bool value) => attackCooldown = value;

    public void ActualizeTPCD(bool value) => tpCooldown = value;
}
