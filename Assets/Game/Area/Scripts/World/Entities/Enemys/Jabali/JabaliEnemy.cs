using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ToolsMandioca.StateMachine;

public class JabaliEnemy : EnemyBase
{
    [Header("Move Options")]
    [SerializeField] GenericEnemyMove movement;
    [SerializeField] Transform rootTransform = null;
    [SerializeField] LineOfSight lineOfSight = null;

    public AnimationCurve animEmisive;

    [Header("Combat Options")]
    [SerializeField] CombatComponent headAttack = null;
    [SerializeField] JabaliPushComponent pushAttack = null;
    [SerializeField] float normalDistance = 9;
    [SerializeField] float timeParried = 2;

    [Header("NormalAttack")]
    [SerializeField] int normalDamage = 4;
    [SerializeField] float normalAttAnticipation = 0.5f;
    [SerializeField] float cdToHeadAttack = 1;

    [Header("PushAttack")]
    [SerializeField] int pushDamage = 8;
    [SerializeField] float distanceToCharge = 7;
    [SerializeField] float chargeTime = 2;
    [SerializeField] float stunChargeTime = 2;
    [SerializeField] float timeToObtainCharge = 5;
    [SerializeField] float chargeSpeed = 12;
    private bool chargeOk = false;
    private float cargeTimer;
    private CombatDirector director;


    [Header("Life Options")]
    [SerializeField] GenericLifeSystem lifesystem = null;
    [SerializeField] float tdRecall = 0.5f;
    [SerializeField] float forceRecall = 5;
    [SerializeField] float explosionForce = 20;
    [SerializeField] DamageData dmgData;
    [SerializeField] DamageReceiver dmgReceiver;

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
    [SerializeField] RagdollComponent ragdoll;
    [SerializeField] Color onHitColor;
    [SerializeField] float onHitFlashTime;
    [SerializeField] GameObject feedbackCharge;

    public bool isOnFire { get; private set; }

    Rigidbody rb;
    EventStateMachine<JabaliInputs> sm;

    protected override void OnInitialize()
    {
        rb = GetComponent<Rigidbody>();
        movement.Configure(rootTransform, rb);
        headAttack.Configure(HeadAttack);
        pushAttack.Configure(PushRelease, StunAfterCharge);
        lineOfSight.Configurate(rootTransform);

        dmgData.Initialize(this);
        dmgReceiver.Initialize(rootTransform, () =>
        {
            if (cooldown || Invinsible || sm.Current.Name == "Dead") return true;
            else return false;
        }, Die, TakeDamageFeedback, rb, lifesystem.Hit);


        anim.Add_Callback("DealDamage", DealDamage);
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

        director.AddNewTarget(this);

        canupdate = true;
    }

    public override void Zone_OnPlayerExitInThisRoom()
    {
        Debug.Log("Player enter the room");
        IAInitialize(Main.instance.GetCombatDirector());
    }

    public override void Zone_OnPlayerEnterInThisRoom(Transform who)
    {
        sm.SendInput(JabaliInputs.DISABLE);
    }

    protected override void OnUpdateEntity()
    {
        if (canupdate)
        {
            EffectUpdate();

            if (!death)
            {
                if (combat)
                {
                    if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) > combatDistance + 2)
                    {
                        director.DeadEntity(this, entityTarget);
                        entityTarget = null;
                        combat = false;
                    }
                }

                if (!combat && entityTarget == null)
                {
                    if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) <= combatDistance)
                    {
                        director.AddAwake(this);
                        combat = true;
                    }
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
    public void HeadAttack(DamageReceiver e)
    {
        dmgData.SetDamage(normalDamage).SetDamageTick(false).SetDamageType(Damagetype.parriable).SetKnockback(30)
            .SetPositionAndDirection(transform.position);
        Attack_Result takeDmg = e.TakeDamage(dmgData);

        if (takeDmg == Attack_Result.parried)
        {
            sm.SendInput(JabaliInputs.PARRIED);
        }
    }

    void PushRelease(DamageReceiver e)
    {
        dmgData.SetDamage(pushDamage).SetDamageTick(false).SetDamageType(Damagetype.inparry).SetKnockback(80)
            .SetPositionAndDirection(transform.position);
        Attack_Result takeDmg = e.TakeDamage(dmgData);

        if(e == entityTarget || e.GetComponent<CharacterHead>())
            pushAttack.Stop();
    }

    void PushAttack() { pushAttack.ManualTriggerAttack(); }

    public void DealDamage() { headAttack.ManualTriggerAttack(); }

    public override void ToAttack() => attacking = true;

    #endregion

    #region TakeDamage Things
    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype dmgtype)
    {
        SetTarget(entityTarget);

        if (cooldown || Invinsible || sm.Current.Name == "Dead") return Attack_Result.inmune;

        //Debug.Log("damagetype" + dmgtype.ToString());

        Vector3 aux = (this.transform.position - attack_pos).normalized;

        if (dmgtype == Damagetype.explosion)
            rb.AddForce(aux * explosionForce, ForceMode.Impulse);
        else
            rb.AddForce(aux * forceRecall, ForceMode.Impulse);

        sm.SendInput(JabaliInputs.TAKE_DMG);

        greenblood.Play();
        cooldown = true;

        StartCoroutine(OnHitted(myMat, onHitFlashTime, onHitColor));

        bool death = lifesystem.Hit(dmg);
        return death ? Attack_Result.death : Attack_Result.sucessful;
    }

    void TakeDamageFeedback(DamageData data)
    {
        if (sm.Current.Name == "Idle" || sm.Current.Name == "Persuit")
        {
            attacking = false;
            director.ChangeTarget(this, data.owner, entityTarget);
        }

        sm.SendInput(JabaliInputs.TAKE_DMG);

        greenblood.Play();
        cooldown = true;

        StartCoroutine(OnHitted(myMat, onHitFlashTime, onHitColor));
    }

    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype, EntityBase owner_entity)
    {
        if (sm.Current.Name == "Dead") return Attack_Result.inmune;

        if (entityTarget != owner_entity)
        {
            if (sm.Current.Name == "Idle" || sm.Current.Name == "Persuit")
            {
                attacking = false;
                //if (entityTarget == null) throw new System.Exception("entity target es null");//esto rompe cuando vengo desde el Damage in Room
                director.ChangeTarget(this, owner_entity, entityTarget);
            }
        }

        return TakeDamage(dmg, attack_pos, damagetype);
    }

    public void Die(Vector3 dir)
    {
        death = true;
        director.RemoveTarget(this);
        sm.SendInput(JabaliInputs.DEAD);
        if (dir == Vector3.zero)
            ragdoll.Ragdoll(true, -rootTransform.forward);
        else
            ragdoll.Ragdoll(true, dir);
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_DEAD, new object[] { transform.position, petrified, expToDrop });
        Main.instance.RemoveEntity(this);
    }

    #endregion

    #region Effects Things
    public override float ChangeSpeed(float newSpeed)
    {
        if (newSpeed < 0)
            return movement.GetInitSpeed();

        movement.SetCurrentSpeed(newSpeed);

        return movement.GetInitSpeed();
    }

    void StunAfterCharge()
    {
        EnterStun = (input) => {
            animator.SetBool("Stun", true);
        };

        UpdateStun = (name) => {
            stunTimer += Time.deltaTime;

            if (stunTimer >= stunChargeTime)
                sm.SendInput(JabaliInputs.IDLE);
        };

        ExitStun = (input) => {
            animator.SetBool("Stun", false);
            stunTimer = 0;
            chargeOk = false;
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
                sm.SendInput(JabaliInputs.IDLE);
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

        movement.MultiplySpeed(freezeSpeedSlowed);
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
            movement.DivideSpeed(freezeSpeedSlowed);
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

    public enum JabaliInputs { IDLE, PERSUIT, CHARGE_PUSH, PUSH, HEAD_ANTICIP, HEAD_ATTACK, TAKE_DMG, PARRIED, PETRIFIED, DEAD, DISABLE, CHASING }

    void SetStates()
    {
        var idle = new EState<JabaliInputs>("Idle");
        var chasing = new EState<JabaliInputs>("Chasing");
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
            .SetTransition(JabaliInputs.TAKE_DMG, takeDamage)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .SetTransition(JabaliInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(persuit)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.TAKE_DMG, takeDamage)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .SetTransition(JabaliInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(chasing)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.PERSUIT, persuit)
            .SetTransition(JabaliInputs.TAKE_DMG, takeDamage)
            .SetTransition(JabaliInputs.CHARGE_PUSH, chargePush)
            .SetTransition(JabaliInputs.HEAD_ANTICIP, headAnt)
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

        new JabaliIdle(idle, sm, movement, distanceToCharge, distancePos, normalDistance, IsChargeOk).SetThis(this).SetDirector(director)
            .SetRoot(rootTransform);

        new JabaliPersuit(persuit, sm, movement, lineOfSight.OnSight, IsChargeOk, normalDistance, distancePos, distanceToCharge).SetAnimator(animator)
            .SetThis(this).SetRoot(rootTransform);

        new JabaliChasing(chasing, sm, IsAttack, IsChargeOk, distanceToCharge, distancePos, movement).SetDirector(director).SetThis(this).SetRoot(rootTransform);

        new JabaliCharge(chargePush, sm, chargeTime).SetAnimator(animator).SetDirector(director).SetThis(this).SetRigidbody(rb).SetRoot(rootTransform);

        new JabaliPushAttack(push, sm, chargeSpeed, PushAttack, feedbackCharge, pushAttack.Play).SetAnimator(animator).SetRigidbody(rb).SetRoot(rootTransform)
            .SetThis(this).SetDirector(director);

        new JabaliAttackAnticipation(headAnt, sm, movement, normalAttAnticipation).SetAnimator(animator).SetDirector(director).SetThis(this).SetRoot(rootTransform);

        new JabaliHeadAttack(headAttack, sm, cdToHeadAttack).SetAnimator(animator).SetDirector(director).SetThis(this);

        new JabaliTD(takeDamage, sm, tdRecall).SetAnimator(animator);

        new JabaliParried(parried, sm, timeParried).SetAnimator(animator);

        new JabaliStun(petrified, sm, StartStun, TickStun, EndStun);

        new JabaliDeath(dead, sm, ragdoll).SetThis(this).SetDirector(director);

        disable.OnEnter += (input) => DisableObject();

        disable.OnExit += (input) => EnableObject();
    }

    bool IsChargeOk() => chargeOk;

    void StartStun(EState<JabaliInputs> input) => EnterStun(input);

    void TickStun(string name) => UpdateStun(name);

    void EndStun(JabaliInputs input) => ExitStun(input);

    void DisableObject()
    {
        canupdate = false;
        movement.SetDefaultSpeed();
        combat = false;
    }

    void EnableObject() => Initialize();

    #endregion

    #region Debuggin
    void DebugState(string state) { if (txt_debug != null) txt_debug.text = state; }//viene de la state machine
    public void ToogleDebug(bool val) { if (txt_debug != null) txt_debug.enabled = val; }//apaga y prende debug desde afuera
    void StartDebug() { if (txt_debug != null) txt_debug.enabled = DevelopToolsCenter.instance.EnemyDebuggingIsActive(); }// inicializacion
    #endregion
}
