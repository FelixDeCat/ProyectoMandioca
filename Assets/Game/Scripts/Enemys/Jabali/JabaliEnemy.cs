using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

public class JabaliEnemy : EnemyBase
{
    [Header("Move Options")]
    [SerializeField] float speedMovement = 4;
    [SerializeField] float rotSpeed = 2;
    [SerializeField] float avoidWeight = 1;
    [SerializeField] float avoidRadious = 1;
    [SerializeField] Transform rootTransform = null;
    [SerializeField] LayerMask avoidMask = 0;
    private float currentSpeed;
    [SerializeField] LineOfSight lineOfSight = null;

    public AnimationCurve animEmisive;

    [Header("Combat Options")]
    [SerializeField] CombatComponent headAttack = null;
    [SerializeField] JabaliPushComponent pushAttack = null;
    [SerializeField] float normalDistance = 9;
    [SerializeField] float timeParried = 2;

    [Header("NormalAttack")]
    [SerializeField] int normalDamage = 4;
    [SerializeField] float distanceToAttack = 2;
    [SerializeField] float normalAttAnticipation = 0.5f;
    [SerializeField] float cdToHeadAttack = 1;

    [Header("PushAttack")]
    [SerializeField] int pushDamage = 8;
    [SerializeField] float distanceToCharge = 7;
    [SerializeField] float chargeTime = 2;
    [SerializeField] float stunChargeTime = 2;
    [SerializeField] float timeToObtainCharge = 5;
    [SerializeField] float chargeSpeed = 12;
    private bool chargeOk = true;
    private float cargeTimer;
    private CombatDirector director;


    [Header("Life Options")]
    [SerializeField] GenericLifeSystem lifesystem = null;
    [SerializeField] float tdRecall = 0.5f;
    [SerializeField] float forceRecall = 5;
    [SerializeField] float explosionForce = 20;

    [Header("Stuns/Effects")]
    [SerializeField, Range(0, 1)] float freezeSpeedSlowed = 0.5f;
    [SerializeField] float freezeTime = 4;
    [SerializeField] float petrifiedTime = 3;
    private bool cooldown = false;
    private float timercooldown = 0;
    private float currentAnimSpeed;
    private float stunTimer;
    private Action<EState<JabaliInputs>> EnterStun;
    private Action<string> UpdateStun;
    private Action<JabaliInputs> ExitStun;
    private Material[] myMat;

    [Header("Feedback")]
    [SerializeField] GameObject feedbackFireDot = null;
    [SerializeField] ParticleSystem greenblood = null;
    [SerializeField] AnimEvent anim = null;
    [SerializeField] Animator animator = null;
    [SerializeField] UnityEngine.UI.Text txt_debug = null;
    [SerializeField] Material freeze_shader = null;

    public bool isOnFire { get; private set; }

    Rigidbody rb;
    EventStateMachine<JabaliInputs> sm;

    private void Start() { rb = GetComponent<Rigidbody>(); }
    protected override void OnInitialize()
    {
        Debug.Log("OnInitialize");
        headAttack.Configure(HeadAttack);
        pushAttack.Configure(PushRelease, StunAfterCharge);
        anim.Add_Callback("DealDamage", DealDamage);
        anim.Add_Callback("Death", DeathAnim);
        lifesystem.AddEventOnDeath(Die);
        currentSpeed = speedMovement;
        StartDebug();

        Main.instance.AddEntity(this);

        IAInitialize(Main.instance.GetCombatDirector());
    }

    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
            sm.SendInput(JabaliInputs.IDLE);

        canupdate = true;
    }

    public override void OnPlayerExitInThisRoom()
    {
        Debug.Log("Player enter the room");
        IAInitialize(Main.instance.GetCombatDirector());
    }

    public override void OnPlayerEnterInThisRoom(Transform who)
    {
        sm.SendInput(JabaliInputs.DISABLE);
    }

    protected override void OnUpdateEntity()
    {
        if (canupdate)
        {
            EffectUpdate();
            if (!combat && entityTarget == null)
            {
                if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) <= combatDistance)
                {
                    director.AddAwake(this);
                    combat = true;
                }
            }

            if (sm != null)
                sm.Update();

            if (cooldown)
            {
                if (timercooldown < tdRecall) timercooldown = timercooldown + 1 * Time.deltaTime;
                else { cooldown = false; timercooldown = 0; }
            }

            if (!chargeOk)
            {
                cargeTimer += Time.deltaTime;

                if (cargeTimer >= timeToObtainCharge)
                {
                    chargeOk = true;
                    cargeTimer = 0;
                }
            }
        }
    }

    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnFixedUpdate() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }

    #region Attack Things
    public void HeadAttack(EntityBase e)
    {
        Attack_Result takeDmg = e.TakeDamage(normalDamage, transform.position, Damagetype.parriable);

        if (takeDmg == Attack_Result.parried)
        {
            sm.SendInput(JabaliInputs.PARRIED);

            //Tira evento si es parrieado. Seguro haya que cambiarlo
            if (OnParried != null)
                OnParried();
        }
    }

    void PushRelease(EntityBase e)
    {
        Attack_Result takeDmg = e.TakeDamage(normalDamage, transform.position, Damagetype.inparry);
    }

    void PushAttack() { pushAttack.ManualTriggerAttack(); }

    public void DealDamage() { headAttack.ManualTriggerAttack(); }

    public override void ToAttack() { attacking = true; }

    #endregion

    #region TakeDamage Things
    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype dmgtype)
    {
        SetTarget(entityTarget);

        if (cooldown || Invinsible || sm.Current.Name == "Dead") return Attack_Result.inmune;

        Debug.Log("damagetype" + dmgtype.ToString());

        Vector3 aux = (this.transform.position - attack_pos).normalized;

        if (dmgtype == Damagetype.explosion)
            rb.AddForce(aux * explosionForce, ForceMode.Impulse);
        else
            rb.AddForce(aux * forceRecall, ForceMode.Impulse);

        sm.SendInput(JabaliInputs.TAKE_DMG);

        greenblood.Play();
        cooldown = true;

        bool death = lifesystem.Hit(dmg);
        return death ? Attack_Result.death : Attack_Result.sucessful;
    }

    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype, EntityBase owner_entity)
    {
        if (sm.Current.Name == "Dead") return Attack_Result.inmune;

        if (entityTarget != owner_entity)
        {
            if (sm.Current.Name == "Idle" || sm.Current.Name == "Persuit")
            {
                if (!entityTarget)
                    SetTarget(owner_entity);

                attacking = false;
                //if (entityTarget == null) throw new System.Exception("entity target es null");//esto rompe cuando vengo desde el Damage in Room
                director.RemoveToAttack(this, entityTarget);
                SetTarget(owner_entity);
                director.AddToAttack(this, entityTarget);
            }
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

    public void Die()
    {
        if (target)
        {
            List<EnemyBase> myEnemys = Main.instance.GetNoOptimizedListEnemies();
            for (int i = 0; i < myEnemys.Count; i++)
                myEnemys[i].Invinsible = false;
        }
        director.RemoveToAttack(this, entityTarget);
        sm.SendInput(JabaliInputs.DEAD);
        death = true;
        Main.instance.RemoveEntity(this);
    }

    void DeathAnim()
    {
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_DEAD, new object[] { transform.position, petrified, expToDrop });
        gameObject.SetActive(false);
    }

    #endregion

    #region Effects Things
    public override float ChangeSpeed(float newSpeed)
    {
        if (newSpeed < 0)
            return speedMovement;

        currentSpeed = newSpeed;

        return speedMovement;
    }

    void StunAfterCharge()
    {
        EnterStun = (input) => {
            animator.SetBool("Stun", true);
        };

        UpdateStun = (name) => {
            stunTimer += Time.deltaTime;

            if (stunTimer >= stunChargeTime)
            {
                sm.SendInput(JabaliInputs.IDLE);
            }
        };

        ExitStun = (input) => {
            animator.SetBool("Stun", false);
            stunTimer = 0;
        };

        sm.SendInput(JabaliInputs.PETRIFIED);
    }

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
                sm.SendInput(JabaliInputs.IDLE);
            }
        };

        ExitStun = (input) => {
            animator.speed = currentAnimSpeed;
            stunTimer = 0;
        };

        sm.SendInput(JabaliInputs.PETRIFIED);
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

    public override void OnFreeze()
    {
        base.OnFreeze();

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

    #endregion

    #region SET STATES

    public enum JabaliInputs { IDLE, PERSUIT, CHARGE_PUSH, PUSH, HEAD_ANTICIP, HEAD_ATTACK, TAKE_DMG, PARRIED, PETRIFIED, DEAD, DISABLE }

    void SetStates()
    {
        var idle = new EState<JabaliInputs>("Idle");
        var persuit = new EState<JabaliInputs>("Persuit");
        var chargePush = new EState<JabaliInputs>("Charge_Push");
        var push = new EState<JabaliInputs>("Push");
        var headAnt = new EState<JabaliInputs>("Head_Anticipation");
        var headAttack = new EState<JabaliInputs>("Head_Attack");
        var takeDamage = new EState<JabaliInputs>("Take_DMG");
        var parried = new EState<JabaliInputs>("Parried");
        var petrified = new EState<JabaliInputs>("Petrified");
        var dead = new EState<JabaliInputs>("Dead");
        var disable = new EState<JabaliInputs>("Disable");

        ConfigureState.Create(idle)
            .SetTransition(JabaliInputs.PERSUIT, persuit)
            .SetTransition(JabaliInputs.CHARGE_PUSH, chargePush)
            .SetTransition(JabaliInputs.HEAD_ANTICIP, headAnt)
            .SetTransition(JabaliInputs.TAKE_DMG, takeDamage)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(persuit)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.TAKE_DMG, takeDamage)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(chargePush)
            .SetTransition(JabaliInputs.PUSH, push)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(push)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(headAnt)
            .SetTransition(JabaliInputs.HEAD_ATTACK, headAttack)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(headAttack)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.PARRIED, parried)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(parried)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(petrified)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.CHARGE_PUSH, chargePush)
            .SetTransition(JabaliInputs.PUSH, push)
            .SetTransition(JabaliInputs.HEAD_ANTICIP, headAnt)
            .SetTransition(JabaliInputs.HEAD_ATTACK, headAttack)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(dead)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(JabaliInputs.IDLE, idle)
            .Done();

        sm = new EventStateMachine<JabaliInputs>(idle, DebugState);

        new JabaliIdle(idle, sm, distanceToCharge, normalDistance, distanceToAttack, IsAttack, IsChargeOk).SetThis(this).SetRotation(rootTransform, rotSpeed);

        new JabaliPersuit(persuit, sm, lineOfSight.OnSight, IsChargeOk, distanceToAttack, distanceToCharge - 1)
            .SetMovement(rb, this, avoidRadious, avoidWeight, GetCurrentSpeed, avoidMask).SetRotation(rootTransform, rotSpeed).SetAnimator(animator);

        new JabaliCharge(chargePush, sm, chargeTime).SetAnimator(animator).SetDirector(director).SetThis(this);

        new JabaliPushAttack(push, sm, chargeSpeed, PushAttack).SetAnimator(animator).SetRigidbody(rb);

        new JabaliAttackAnticipation(headAnt, sm, normalAttAnticipation).SetAnimator(animator).SetRotation(rootTransform, rotSpeed).SetDirector(director).SetThis(this);

        new JabaliHeadAttack(headAttack, sm, cdToHeadAttack).SetAnimator(animator).SetDirector(director).SetThis(this);

        new JabaliTD(takeDamage, sm, tdRecall).SetAnimator(animator);

        new JabaliParried(parried, sm, timeParried).SetAnimator(animator);

        new JabaliStun(petrified, sm, StartStun, TickStun, EndStun);

        new JabaliDeath(dead, sm).SetAnimator(animator);

        disable.OnEnter += (input) => DisableObject();

        disable.OnExit += (input) => EnableObject();
    }
    bool IsChargeOk() { return chargeOk; }

    void StartStun(EState<JabaliInputs> input) { EnterStun(input); }

    void TickStun(string name) { UpdateStun(name); }

    void EndStun(JabaliInputs input) { ExitStun(input); }

    float GetCurrentSpeed() { return currentSpeed; }

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
