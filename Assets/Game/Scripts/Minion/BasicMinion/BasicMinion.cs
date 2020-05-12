using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ToolsMandioca.StateMachine;

public class BasicMinion : Minion
{
    [Header("Move Options")]
    [SerializeField] GenericEnemyMove movement = null;
    [SerializeField] Transform rootTransform = null;

    [Header("Combat Options")]
    [SerializeField] CombatComponent combatComponent = null;
    [SerializeField] int damage = 2;
    [SerializeField] float cdToAttack = 1;

    [Header("Life Options")]
    [SerializeField] GenericLifeSystem lifesystem = null;
    [SerializeField] float recallTime = 1;
    [SerializeField] float forceRecall = 2;
    [SerializeField] float explosionForce = 20;
    private float stunTimer;
    private Action<EState<BasicMinionInput>> EnterStun;
    private Action<string> UpdateStun;
    private Action<BasicMinionInput> ExitStun;

    private bool cooldown = false;
    private float timercooldown = 0;

    [Header("Feedback")]
    [SerializeField] ParticleSystem takeDamageFX = null;
    [SerializeField] AnimEvent anim = null;
    [SerializeField] Animator animator = null;
    [SerializeField] UnityEngine.UI.Text txt_debug = null;

    EventStateMachine<BasicMinionInput> sm;
    Rigidbody rb;

    protected override void OnInitialize()
    {
        rb = GetComponent<Rigidbody>();
        base.OnInitialize();
        combatComponent.Configure(AttackEntity);
        anim.Add_Callback("DealDamage", DealDamage);
        anim.Add_Callback("Death", DeathAnim);
        lifesystem.AddEventOnDeath(Die);
        movement.Configure(rootTransform, rb);
        StartDebug();

        Main.instance.AddEntity(this);

        IAInitialize();
    }

    public override void Zone_OnPlayerExitInThisRoom()
    {
        //Debug.Log("Player enter the room");
        IAInitialize();
    }

    public override void Zone_OnPlayerEnterInThisRoom(Transform who)
    {

    }

    protected override void IAInitialize()
    {
        if (!director)
            director = Main.instance.GetCombatDirector();
        if (sm == null)
            SetStates();
        else
            sm.SendInput(BasicMinionInput.IDLE);

        canupdate = true;
        if (entityTarget != null)
        {
            director.DeadEntity(this, entityTarget);
            entityTarget = null;
        }
    }

    protected override void OnUpdateEntity()
    {
        if (canupdate)
        {
            EffectUpdate();

            if (entityTarget == null)
            {
                var overlap = Physics.OverlapSphere(rootTransform.position, distanceToCombat);

                foreach (var enemy in overlap)
                {
                    if (enemy.GetComponent<EnemyBase>())
                    {
                        if (!enemy.GetComponent<EnemyBase>().death)
                        {
                            entityTarget = enemy.GetComponent<EnemyBase>();
                            director.AddToList(this, enemy.GetComponent<EnemyBase>());
                            break;
                        }
                    }
                }
            }

            if (sm != null)
                sm.Update();

            if (cooldown)
            {
                if (timercooldown < recallTime) timercooldown = timercooldown + 1 * Time.deltaTime;
                else { cooldown = false; timercooldown = 0; }
            }

        }
    }

    protected override void OnPause() { }
    protected override void OnResume() { }

    #region Attack
    public void DealDamage() { combatComponent.ManualTriggerAttack(); sm.SendInput(BasicMinionInput.ATTACK); }

    public void AttackEntity(EntityBase e)
    {
        Attack_Result takeDmg = e.TakeDamage(damage, transform.position, Damagetype.parriable);

        if (takeDmg == Attack_Result.parried)
            combatComponent.Stop();
    }

    #endregion

    #region Effects
    float currentAnimSpeed;
    private Material[] myMat;
    #endregion

    #region Life Things
    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype dmgtype)
    {
        if (cooldown || sm.Current.Name == "Die") return Attack_Result.inmune;

        Vector3 aux = (transform.position - attack_pos).normalized;

        if (dmgtype == Damagetype.explosion)
            rb.AddForce(aux * explosionForce, ForceMode.Impulse);
        else
            rb.AddForce(aux * forceRecall, ForceMode.Impulse);

        sm.SendInput(BasicMinionInput.TAKE_DAMAGE);

        takeDamageFX.Play();

        cooldown = true;

        bool death = lifesystem.Hit(dmg);
        return death ? Attack_Result.death : Attack_Result.sucessful;
    }

    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype, EntityBase owner_entity)
    {

        if (sm.Current.Name == "Die") return Attack_Result.inmune;

        if (sm.Current.Name != "Attack" && entityTarget != owner_entity)
        {
            attacking = false;
            //if (entityTarget == null) throw new System.Exception("entity target es null");//esto rompe cuando vengo desde el Damage in Room
            director.ChangeTarget(this, owner_entity, entityTarget);
        }

        return TakeDamage(dmg, attack_pos, damagetype);
    }

    public void Die()
    {
        sm.SendInput(BasicMinionInput.DIE);
                death = true;
        director.DeadEntity(this, entityTarget,this);
        Main.instance.RemoveEntity(this);
    }

    void DeathAnim()
    {
        //vector3, boolean, int
        director.RemoveTarget(this);
        Main.instance.eventManager.TriggerEvent(GameEvents.MINION_DEAD, new object[] { transform.position });
        gameObject.SetActive(false);
    }
    #endregion

    protected override void OnFixedUpdate() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }

    #region STATE MACHINE THINGS
    public enum BasicMinionInput { IDLE, BEGIN_ATTACK, ATTACK, GO_TO_POS, DIE, TAKE_DAMAGE, STUN };
    void SetStates()
    {
        var idle = new EState<BasicMinionInput>("Idle");
        var goToPos = new EState<BasicMinionInput>("Follow");
        var beginAttack = new EState<BasicMinionInput>("Begin_Attack");
        var attack = new EState<BasicMinionInput>("Attack");
        var takeDamage = new EState<BasicMinionInput>("Take_Damage");
        var die = new EState<BasicMinionInput>("Die");
        var stun = new EState<BasicMinionInput>("Stun");

        ConfigureState.Create(idle)
            .SetTransition(BasicMinionInput.GO_TO_POS, goToPos)
            .SetTransition(BasicMinionInput.TAKE_DAMAGE, takeDamage)
            .SetTransition(BasicMinionInput.BEGIN_ATTACK, beginAttack)
            .SetTransition(BasicMinionInput.DIE, die)
            .SetTransition(BasicMinionInput.STUN, stun)
            .Done();

        ConfigureState.Create(goToPos)
            .SetTransition(BasicMinionInput.IDLE, idle)
            .SetTransition(BasicMinionInput.TAKE_DAMAGE, takeDamage)
            .SetTransition(BasicMinionInput.DIE, die)
            .SetTransition(BasicMinionInput.STUN, stun)
            .Done();

        ConfigureState.Create(beginAttack)
            .SetTransition(BasicMinionInput.ATTACK, attack)
            .SetTransition(BasicMinionInput.DIE, die)
            .SetTransition(BasicMinionInput.STUN, stun)
            .Done();

        ConfigureState.Create(attack)
            .SetTransition(BasicMinionInput.IDLE, idle)
            .SetTransition(BasicMinionInput.DIE, die)
            .SetTransition(BasicMinionInput.STUN, stun)
            .Done();

        ConfigureState.Create(stun)
            .SetTransition(BasicMinionInput.IDLE, idle)
            .SetTransition(BasicMinionInput.ATTACK, attack)
            .SetTransition(BasicMinionInput.BEGIN_ATTACK, beginAttack)
            .SetTransition(BasicMinionInput.DIE, die)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(BasicMinionInput.IDLE, idle)
            .SetTransition(BasicMinionInput.STUN, stun)
            .SetTransition(BasicMinionInput.DIE, die)
            .Done();

        ConfigureState.Create(die)
            .Done();

        sm = new EventStateMachine<BasicMinionInput>(idle, DebugState);

        var head = Main.instance.GetChar();

        //Asignando las funciones de cada estado
        new BIdle(idle, sm, movement, IsAttack, distanceToTarget, maxDistanceToTarget, owner, distanceToOwner).SetAnimator(animator).SetRoot(rootTransform)
            .SetDirector(director).SetThis(this);

        new BGoToTarget(goToPos, sm, movement, maxDistanceToTarget, distanceToOwner, owner).SetAnimator(animator).SetRoot(rootTransform).SetThis(this);

        new BBeginAttack(beginAttack, sm, movement).SetAnimator(animator).SetDirector(director).SetRoot(rootTransform).SetThis(this);

        new BAttack(attack, sm, cdToAttack).SetAnimator(animator).SetDirector(director).SetThis(this);

        new BTakeDmg(takeDamage, sm, recallTime).SetAnimator(animator);

        new BStun(stun, sm, StartStun, TickStun, EndStun);

        new BDie(die, sm).SetAnimator(animator).SetDirector(director).SetRigidbody(rb);
    }

    bool IsAttack() => attacking;

    void StartStun(EState<BasicMinionInput> input) => EnterStun(input);

    void TickStun(string name) => UpdateStun(name);

    void EndStun(BasicMinionInput input) => ExitStun(input);

    #endregion

    #region Debuggin
    void DebugState(string state) { if (txt_debug != null) txt_debug.text = state; }//viene de la state machine
    public void ToogleDebug(bool val) { if (txt_debug != null) txt_debug.enabled = val; }//apaga y prende debug desde afuera
    void StartDebug() { if (txt_debug != null) txt_debug.enabled = DevelopToolsCenter.instance.EnemyDebuggingIsActive(); }// inicializacion
    #endregion

}
