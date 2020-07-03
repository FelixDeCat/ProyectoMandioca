using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

public class JabaliEnemy : EnemyBase
{
    [Header("Move Options")]
    [SerializeField] GenericEnemyMove movement;
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
    [SerializeField] float normalKockback = 30;

    [Header("PushAttack")]
    [SerializeField] int pushDamage = 8;
    [SerializeField] float distanceToCharge = 7;
    [SerializeField] float chargeTime = 2;
    [SerializeField] float stunChargeTime = 2;
    [SerializeField] float timeToObtainCharge = 5;
    [SerializeField] float chargeSpeed = 12;
    [SerializeField] float pushKnockback = 80;
    private bool chargeOk = false;
    private float cargeTimer;
    private CombatDirector director;


    [Header("Life Options")]
    [SerializeField] float tdRecall = 0.5f;

    [Header("Stuns/Effects")]
    private bool cooldown = false;
    private float timercooldown = 0;
    float stunTimer;
    private Action<EState<JabaliInputs>> EnterStun;
    private Action<string> UpdateStun;
    private Action<JabaliInputs> ExitStun;
    private Material[] myMat;

    [Header("Feedback")]
    [SerializeField] ParticleSystem greenblood = null;
    [SerializeField] AnimEvent anim = null;
    [SerializeField] Animator animator = null;
    [SerializeField] RagdollComponent ragdoll;
    [SerializeField] Color onHitColor;
    [SerializeField] float onHitFlashTime;
    [SerializeField] GameObject feedbackCharge;
    [SerializeField] List<AudioClip> mySounds;
    [SerializeField] EffectBase petrifyEffect;

    EventStateMachine<JabaliInputs> sm;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        myMat = GetComponentInChildren<SkinnedMeshRenderer>().materials;

        movement.Configure(rootTransform, rb);
        headAttack.Configure(HeadAttack);
        pushAttack.Configure(PushRelease, StunAfterCharge);
        lineOfSight.Configurate(rootTransform);
        AudioManager.instance.GetSoundPool(mySounds[0].name, AudioGroups.JABALI, mySounds[0], true);
        AudioManager.instance.GetSoundPool(mySounds[1].name, AudioGroups.JABALI, mySounds[1]);
        AudioManager.instance.GetSoundPool(mySounds[2].name, AudioGroups.JABALI, mySounds[2]);
        AudioManager.instance.GetSoundPool(mySounds[3].name, AudioGroups.JABALI, mySounds[3]);
        AudioManager.instance.GetSoundPool(mySounds[4].name, AudioGroups.JABALI, mySounds[4]);
        AudioManager.instance.GetSoundPool(mySounds[5].name, AudioGroups.JABALI, mySounds[5]);

        anim.Add_Callback("DealDamage", DealDamage);
        StartDebug();

        Main.instance.AddEntity(this);

        IAInitialize(Main.instance.GetCombatDirector());

        petrifyEffect?.AddStartCallback(() => sm.SendInput(JabaliInputs.PETRIFIED));
        petrifyEffect?.AddEndCallback(() => sm.SendInput(JabaliInputs.IDLE));
    }
    protected override void OnReset()
    {
        //lo de el ragdoll
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
                        director.AddToList(this, Main.instance.GetChar());
                        SetTarget(Main.instance.GetChar()); 
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
        dmgData.SetDamage(normalDamage).SetDamageTick(false).SetDamageType(Damagetype.Normal).SetKnockback(normalKockback)
            .SetPositionAndDirection(transform.position);
        Attack_Result takeDmg = e.TakeDamage(dmgData);

        if (takeDmg == Attack_Result.parried)
        {
            sm.SendInput(JabaliInputs.PARRIED);
        }
    }

    void PushRelease(DamageReceiver e)
    {
        dmgData.SetDamage(pushDamage).SetDamageTick(false).SetDamageType(Damagetype.Normal).SetKnockback(pushKnockback)
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
    protected override void TakeDamageFeedback(DamageData data)
    {
        if (sm.Current.Name == "Idle" || sm.Current.Name == "Persuit")
        {
            attacking = false;
            director.ChangeTarget(this, data.owner, entityTarget);
        }

        AudioManager.instance.PlaySound(mySounds[2].name, rootTransform);

        sm.SendInput(JabaliInputs.TAKE_DMG);

        greenblood.Play();
        cooldown = true;

        StartCoroutine(OnHitted(myMat, onHitFlashTime, onHitColor));
    }

    protected override void Die(Vector3 dir)
    {
        death = true;
        director.RemoveTarget(this);
        sm.SendInput(JabaliInputs.DEAD);
        if (dir == Vector3.zero)
            ragdoll.Ragdoll(true, -rootTransform.forward);
        else
            ragdoll.Ragdoll(true, dir);
        Main.instance.RemoveEntity(this);
    }

    protected override bool IsDamage()
    {
        if (cooldown || Invinsible || sm.Current.Name == "Dead" || sm.Current.Name == "Push") return true;
        else return false;
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
        EnterStun += (input) => {
            animator.SetBool("Stun", true);
        };

        UpdateStun = (name) => {
            stunTimer += Time.deltaTime;

            if (stunTimer >= stunChargeTime)
                sm.SendInput(JabaliInputs.IDLE);
        };

        ExitStun += (input) => {
            animator.SetBool("Stun", false);
            stunTimer = 0;
            chargeOk = false;
        };

        sm.SendInput(JabaliInputs.PETRIFIED);
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
            .SetTransition(JabaliInputs.PARRIED, parried)
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

        new JabaliCharge(chargePush, sm, chargeTime, mySounds[0].name, mySounds[1].name).SetAnimator(animator).SetDirector(director)
            .SetThis(this).SetRigidbody(rb).SetRoot(rootTransform);

        new JabaliPushAttack(push, sm, chargeSpeed, PushAttack, feedbackCharge, pushAttack.Play, mySounds[3].name)
            .SetAnimator(animator).SetRigidbody(rb).SetRoot(rootTransform)
            .SetThis(this).SetDirector(director);

        new JabaliAttackAnticipation(headAnt, sm, movement, normalAttAnticipation).SetAnimator(animator).SetDirector(director).SetThis(this).SetRoot(rootTransform);

        new JabaliHeadAttack(headAttack, sm, cdToHeadAttack, mySounds[4].name).SetAnimator(animator).SetDirector(director).SetThis(this);

        new JabaliTD(takeDamage, sm, tdRecall).SetAnimator(animator);

        new JabaliParried(parried, sm, timeParried).SetAnimator(animator);

        new JabaliStun(petrified, sm, StartStun, TickStun, EndStun);

        new JabaliDeath(dead, sm, ragdoll, mySounds[5].name).SetThis(this).SetDirector(director);

        disable.OnEnter += (input) => DisableObject();

        disable.OnExit += (input) => EnableObject();
    }

    bool IsChargeOk() => chargeOk;

    void StartStun(EState<JabaliInputs> input) => EnterStun?.Invoke(input);

    void TickStun(string name) => UpdateStun?.Invoke(name);

    void EndStun(JabaliInputs input) { ExitStun?.Invoke(input); EnterStun = (x) => { }; ExitStun = (x) => { }; } 

    void DisableObject()
    {
        canupdate = false;
        movement.SetDefaultSpeed();
        combat = false;
    }

    void EnableObject() => Initialize();

    #endregion

    #region Debuggin
    UnityEngine.UI.Text txt_debug = null;
    public GameObject canvasDebugModel;
    void DebugState(string state) { if (txt_debug != null) txt_debug.text = state; }//viene de la state machine
    public void ToogleDebug(bool val)
    {
        if (val)
        {
            if (txt_debug == null)
            {
                GameObject go = Instantiate(canvasDebugModel, this.transform);
                txt_debug = go.GetComponentInChildren<UnityEngine.UI.Text>();
            }
            txt_debug.enabled = val;
        }
        else
        {
            if (txt_debug != null)
                txt_debug.enabled = val;
        }
    }
    void StartDebug() { if (txt_debug != null) txt_debug.enabled = DevelopToolsCenter.instance.EnemyDebuggingIsActive(); }// inicializacion
    #endregion
}
