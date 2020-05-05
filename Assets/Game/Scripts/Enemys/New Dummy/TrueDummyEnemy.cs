using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

public class TrueDummyEnemy : EnemyBase
{
    [Header("Move Options")]
    [SerializeField] float speedMovement = 4;
    [SerializeField] float rotSpeed = 2;
    [SerializeField] float avoidWeight = 2;
    [SerializeField] float avoidRadious = 2;
    [SerializeField] Transform rootTransform = null;
    private float currentSpeed;

    public AnimationCurve animEmisive;

    [Header("Combat Options")]
    [SerializeField] CombatComponent combatComponent = null;
    [SerializeField] int damage = 2;
    [SerializeField] float distanceToAttack = 3;
    [SerializeField] float normalDistance = 8;
    [SerializeField] float cdToAttack = 1;
    [SerializeField] float parriedTime = 2;
    private CombatDirector director;

    [Header("Life Options")]
    [SerializeField] GenericLifeSystem lifesystem = null;
    [SerializeField] float recallTime = 1;
    [SerializeField] float forceRecall = 4;
    [SerializeField] float explosionForce = 20;
    [SerializeField] float petrifiedTime = 2;
    private float stunTimer;
    private Action<EState<DummyEnemyInputs>> EnterStun;
    private Action<string> UpdateStun;
    private Action<DummyEnemyInputs> ExitStun;

    [Range(0,1)]
    [SerializeField] float freezeSpeedSlowed = 0.5f;
    [SerializeField] float freezeTime = 4;
    private bool cooldown = false;
    private float timercooldown = 0;

    [Header("Feedback")]
    [SerializeField] GameObject feedbackFireDot = null;
    [SerializeField] ParticleSystem greenblood = null;
    [SerializeField] AnimEvent anim = null;
    [SerializeField] Animator animator = null;
    [SerializeField] UnityEngine.UI.Text txt_debug = null;
    [SerializeField] Material freeze_shader = null;

    
    public bool isOnFire { get; private set; }
    

    EventStateMachine<DummyEnemyInputs> sm;
    Rigidbody rb;

    protected override void OnInitialize()
    {
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_SPAWN, new object[] { this });

        //Debug.Log("OnInitialize");
        rb = GetComponent<Rigidbody>();
        combatComponent.Configure(AttackEntity);
        anim.Add_Callback("DealDamage", DealDamage);
        anim.Add_Callback("Death", DeathAnim);
        lifesystem.AddEventOnDeath(Die);
        currentSpeed = speedMovement;
        StartDebug();

        Main.instance.AddEntity(this);

        IAInitialize(Main.instance.GetCombatDirector());
    }

    public override void OnPlayerExitInThisRoom()
    {
        //Debug.Log("Player enter the room");
        IAInitialize(Main.instance.GetCombatDirector());
    }

    public override void OnPlayerEnterInThisRoom(Transform who)
    {
        sm.SendInput(DummyEnemyInputs.DISABLE);
    }

    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
        {
            sm.SendInput(DummyEnemyInputs.IDLE);
        }

        canupdate = true;
    }

    public void DealDamage() { combatComponent.ManualTriggerAttack(); sm.SendInput(DummyEnemyInputs.ATTACK); }

    public void AttackEntity(EntityBase e)
    {
        Attack_Result takeDmg = e.TakeDamage(damage, transform.position, Damagetype.parriable);

        if (takeDmg == Attack_Result.parried)
        {
            combatComponent.Stop();
            sm.SendInput(DummyEnemyInputs.PARRIED);

            //Tira evento si es parrieado. Seguro haya que cambiarlo
            if (OnParried != null)
                OnParried();
        }
    }

    protected override void OnUpdateEntity()
    {
        if (canupdate)
        {
            EffectUpdate();

            if (combat)
            {
                if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) > combatDistance + 2)
                {
                    director.RemoveToAttack(this,entityTarget);
                    combat = false;
                }
            }

            if(!combat && entityTarget == null)
            {
                if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) <= combatDistance)
                {
                    director.AddAwake(this);
                    combat = true;
                }
            }

            if (sm != null)
            {
                sm.Update();
            }

            if (cooldown) {
                if (timercooldown < recallTime)  timercooldown = timercooldown + 1 * Time.deltaTime;
                else {  cooldown = false; timercooldown = 0; }
            }

        }
    }

    protected override void OnPause() { }
    protected override void OnResume() { }

    private Material[] myMat;
    public override void OnFreeze()
    {
        base.OnFreeze();

        Debug.Log("entré al freeze");

        currentSpeed *= freezeSpeedSlowed;
        animator.speed *= freezeSpeedSlowed;

        var smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
        {
            myMat = smr.materials;

            Material[] mats = smr.materials;
            mats[0] = freeze_shader;
            smr.materials = mats;
        }

        AddEffectTick(() => { }, freezeTime, () => {
            currentSpeed /= freezeSpeedSlowed;
            animator.speed /= freezeSpeedSlowed;
            var smr2 = GetComponentInChildren<SkinnedMeshRenderer>();
            if (smr2 != null)
            {
                Material[] mats = smr2.materials;
                smr2.materials = myMat;
            }
        });
    }

    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype dmgtype)
    {
        SetTarget(entityTarget);

        if (cooldown || Invinsible || sm.Current.Name == "Die") return Attack_Result.inmune;

       // Debug.Log("damagetype" + dmgtype.ToString()); ;

        Vector3 aux = this.transform.position - attack_pos;
        aux.Normalize();
        rb = GetComponent<Rigidbody>();
        if (dmgtype == Damagetype.explosion)
        {
            Debug.Log(rb);
            rb.AddForce(aux * explosionForce, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(aux * forceRecall, ForceMode.Impulse);
        }

        sm.SendInput(DummyEnemyInputs.TAKE_DAMAGE);

        greenblood.Play();

        cooldown = true;

        bool death = lifesystem.Hit(dmg);
        return death ? Attack_Result.death : Attack_Result.sucessful;
    }

    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype, EntityBase owner_entity)
    {

        if (sm.Current.Name == "Die") return Attack_Result.inmune;

        if (sm.Current.Name != "Attack" && entityTarget != owner_entity)
        {

            if (!entityTarget)
            {
                SetTarget(owner_entity); 
            }

            attacking = false;
            //if (entityTarget == null) throw new System.Exception("entity target es null");//esto rompe cuando vengo desde el Damage in Room
            director.RemoveToAttack(this, entityTarget);
            SetTarget(owner_entity);
            director.AddToAttack(this, entityTarget);
        }

        return TakeDamage(dmg, attack_pos, damagetype);
    }

    public override void HalfLife()
    {
        base.HalfLife();
        TakeDamage(lifesystem.life / 2, Main.instance.GetChar().transform.position, Damagetype.normal);
        if (!base.target)
            Invinsible = true;
    }

    public override void InstaKill() { base.InstaKill(); }

    float currentAnimSpeed;

    public override void OnPetrified()
    {
        base.OnPetrified();

        EnterStun = (input) => {
            currentAnimSpeed = animator.speed;
            animator.speed = 0;
        };

        UpdateStun = (name) => {
            stunTimer += Time.deltaTime;

            if (stunTimer >= petrifiedTime)
            {
                if (name == "Begin_Attack")
                    sm.SendInput(DummyEnemyInputs.BEGIN_ATTACK);
                else if (name == "Attack")
                    sm.SendInput(DummyEnemyInputs.ATTACK);
                else
                    sm.SendInput(DummyEnemyInputs.IDLE);
            }
        };

        ExitStun = (input) => {
            animator.speed = currentAnimSpeed;
            stunTimer = 0;
        };

        sm.SendInput(DummyEnemyInputs.PETRIFIED);
    }

    public override float ChangeSpeed(float newSpeed)
    {
        //Si le mando negativo me devuelve la original
        //para guardarla en el componente WebSlowedComponent
        if (newSpeed < 0)
            return speedMovement;

        //Busco el estado follow para poder cambiarle la velocidad
        currentSpeed = newSpeed;

        return speedMovement;
    }

    public override void OnFire()
    {
        if (isOnFire)
            return;

        isOnFire = true;
        feedbackFireDot.SetActive(true);
        base.OnFire();

        lifesystem.DoTSystem(30, 2, 1, Damagetype.Fire, () =>
        {
            isOnFire = false;
            feedbackFireDot.SetActive(false);
        });
    }

    public void Die()
    {
        sm.SendInput(DummyEnemyInputs.DIE);

        if (target)
        {
           List<EnemyBase>myEnemys= Main.instance.GetNoOptimizedListEnemies();
            for (int i = 0; i < myEnemys.Count; i++)
            {
                myEnemys[i].Invinsible = false;
            }
        }
        director.RemoveToAttack(this, entityTarget);
        death = true;
        Main.instance.RemoveEntity(this);
    }

    void DeathAnim()
    {
        //vector3, boolean, int
        gameObject.SetActive(false);
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_DEAD, new object[] { transform.position, petrified, expToDrop });
    }

    protected override void OnFixedUpdate() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }

    public override void ToAttack() { attacking = true; }

    #region STATE MACHINE THINGS
    public enum DummyEnemyInputs { IDLE, BEGIN_ATTACK,ATTACK, GO_TO_POS, DIE, DISABLE, TAKE_DAMAGE, PETRIFIED, PARRIED };
    void SetStates()
    {
        var idle = new EState<DummyEnemyInputs>("Idle");
        var goToPos = new EState<DummyEnemyInputs>("Follow");
        var beginAttack = new EState<DummyEnemyInputs>("Begin_Attack");
        var attack = new EState<DummyEnemyInputs>("Attack");
        var parried = new EState<DummyEnemyInputs>("Parried");
        var takeDamage = new EState<DummyEnemyInputs>("Take_Damage");
        var die = new EState<DummyEnemyInputs>("Die");
        var disable = new EState<DummyEnemyInputs>("Disable");
        var petrified = new EState<DummyEnemyInputs>("Petrified");

        ConfigureState.Create(idle)
            .SetTransition(DummyEnemyInputs.GO_TO_POS, goToPos)
            .SetTransition(DummyEnemyInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(DummyEnemyInputs.BEGIN_ATTACK, beginAttack)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(goToPos)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(beginAttack)
            .SetTransition(DummyEnemyInputs.ATTACK, attack)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.PARRIED, parried)
            .Done();

        ConfigureState.Create(attack)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.PARRIED, parried)
            .Done();

        ConfigureState.Create(parried)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .Done();

        ConfigureState.Create(petrified)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.ATTACK, attack)
            .SetTransition(DummyEnemyInputs.BEGIN_ATTACK, beginAttack)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .Done();

        ConfigureState.Create(die)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .Done();

        sm = new EventStateMachine<DummyEnemyInputs>(idle, DebugState);

        var head = Main.instance.GetChar();

        //Asignando las funciones de cada estado
        new DummyIdleState(idle, sm, IsAttack, distanceToAttack, normalDistance, rotSpeed, this).SetAnimator(animator).SetRoot(rootTransform)
                                                                                                                     .SetDirector(director);

        new DummyFollowState(goToPos, sm, avoidRadious, avoidWeight, rotSpeed, GetCurrentSpeed, CurrentTargetPos, normalDistance, this).SetAnimator(animator).SetRigidbody(rb)
                                                                                                          .SetRoot(rootTransform);

        new DummyAttAnt(beginAttack, sm, rotSpeed, this).SetAnimator(animator).SetDirector(director).SetRoot(rootTransform);

        new DummyAttackState(attack, sm, cdToAttack, this).SetAnimator(animator).SetDirector(director);

        new DummyParried(parried, sm, parriedTime, this).SetAnimator(animator).SetDirector(director);

        new DummyTDState(takeDamage, sm, recallTime).SetAnimator(animator);

        new DummyStunState(petrified, sm, StartStun, TickStun, EndStun);

        new DummyDieState(die, sm).SetAnimator(animator).SetDirector(director).SetRigidbody(rb);

        new DummyDisableState(disable, sm, EnableObject, DisableObject);
    }

    float GetCurrentSpeed() { return currentSpeed; }

    void StartStun(EState<DummyEnemyInputs> input) { EnterStun(input); }

    void TickStun(string name) { UpdateStun(name); }

    void EndStun(DummyEnemyInputs input) { ExitStun(input); }

    void DisableObject()
    {
        canupdate = false;
        currentSpeed = speedMovement;
        combat = false;
    }

    void EnableObject() { Initialize(); }

    #endregion

    #region Debuggin
    void DebugState(string state) { if (txt_debug != null) txt_debug.text = state; }//viene de la state machine
    public void ToogleDebug(bool val) { if (txt_debug != null) txt_debug.enabled = val; }//apaga y prende debug desde afuera
    void StartDebug() { if (txt_debug != null) txt_debug.enabled = DevelopToolsCenter.instance.EnemyDebuggingIsActive(); }// inicializacion
    #endregion

}
