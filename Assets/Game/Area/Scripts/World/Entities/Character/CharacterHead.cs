using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using DevelopTools;
using Tools.EventClasses;
using Tools.StateMachine;
public class CharacterHead : CharacterControllable
{
    public enum PlayerInputs { 
        IDLE, MOVE, BEGIN_BLOCK, BLOCK, END_BLOCK, PARRY, CHARGE_ATTACK, RELEASE_ATTACK, 
        TAKE_DAMAGE, DEAD, ROLL, SPIN, STUN, PLAYER_LOCK_ON, ON_SKILL,ON_LOOK_SHOLDER,
        ON_MENU_ENTER, ON_MENU_EXIT
    };

    Action ChildrensUpdates;
    [SerializeField] CharacterInput _charInput = null;

    [SerializeField] bool StartWithoutWeapons;

    [Header("Dash Options")]
    [SerializeField] float teleportCD = 2;
    public bool Slowed { get; private set; }
    Func<bool> InDash;

    [Header("Movement Options")]
    float slowSpeed = 2;

    public Transform rayPivot;

    [SerializeField] Transform rot = null;
    [SerializeField] CharacterMovement move = new CharacterMovement();
    public CharacterInput getInput => _charInput;

    [SerializeField] CharacterGroundSensor groundSensor = null;

    public Transform lookatPosition;

    //Perdon por esto, pero lo necesito pra la skill del boomeran hasta tener la animacion y el estado "sin escudo"
    bool canBlock = false;
    public GameObject escudo;

    [Header("SlowMotion")]
    [SerializeField] float timeScale = 1;
    [SerializeField] float slowDuration = 2;
    [SerializeField] float speedAnim = 1;


    [Header("Animations")]
    [SerializeField] Animator anim_base = null;
    public AnimEvent charAnimEvent = null;
    public CharacterAnimator charanim;

    [Header("Attack Options")]
    [SerializeField] float dmg_normal = 5;
    [SerializeField] float dmg_heavy = 20;
    [SerializeField] float attackRange = 3;
    [SerializeField] float attackAngle = 90;
    [SerializeField] float timeToHeavyAttack = 1.5f;
    [SerializeField] float attackRecall = 1;
    float dmg;
    CharacterAttack charAttack;
    [SerializeField] DamageData dmgData = null;
    [SerializeField] DamageReceiver dmgReceiver = null;
    CustomCamera customCam;

    [SerializeField] GameObject go_StunFeedback = null;
    float spinDuration;
    float spinSpeed;
    float stunDuration;

    public Action ChangeWeaponPassives = delegate { };

    //Modelo del arma para feedback placeholder
    public GameObject currentWeapon;

    [Header("Interactable")]
    public InteractSensor sensor;

    [Header("Life Options")]
    [Range(0f, 1f)]
    [SerializeField] float big_damage_limit_percent = 0.3f;

    [SerializeField] CharLifeSystem lifesystem = null;
    public CharLifeSystem Life => lifesystem;

    Rigidbody rb;
    public LayerMask enemyLayer;

    [HideInInspector]
    public bool isBuffed = false;
    float dmgReceived = 1f;

    [Header("Parry & Block Options")]
    public CharacterBlock charBlock;

    [SerializeField] float knockbackOnParry = 650;

    [SerializeField] float parryRecall = 0;
    [SerializeField] float takeDamageRecall = 0;
    public Transform ShieldForward;

    [SerializeField] CharFeedbacks feedbacks = null;

    public bool Combat
    {
        set
        {
            if (value != Combat)
            {

            }
        } get => Combat;
    }

    private bool blockRoll;
    public bool BlockRoll {set { blockRoll = value; } }


    [HideInInspector] private bool canAttack = false; 
    public void ToggleAttack(bool val) => canAttack = val;


    private void Start()
    {
        lifesystem
           .Configure_CharLifeSystem()
           .ADD_EVENT_OnGainLife(OnGainLife)
           .ADD_EVENT_OnLoseLife(OnLoseLife)
           .ADD_EVENT_Death(OnDeath)
           .ADD_EVENT_OnChangeValue(OnChangeLife);
    }

    protected override void OnInitialize()
    {
        Main.instance.GetCombatDirector().AddNewTarget(this);
        rb = GetComponent<Rigidbody>();

        feedbacks.Initialize();

        charanim = new CharacterAnimator(anim_base);
        customCam = FindObjectOfType<CustomCamera>();

        move.Initialize(GetComponent<Rigidbody>(), rot, charanim, feedbacks, IsGrounded);

        InDash += move.InCD;
        ChildrensUpdates += move.OnUpdate;
        move.SetCallbacks(OnBeginRoll, OnEndRoll);
        slowSpeed = move.GetDefaultSpeed * .6f;

        charBlock
            .Initialize()
            .SetFeedbacks(feedbacks)
            .SetAnimator(charanim);
        charBlock.callback_OnParry += () => charanim.Parry(true);
        charBlock.callback_EndBlock += UNITYEVENT_PressUp_UpBlocking;
        ChildrensUpdates += charBlock.OnUpdate;

        dmg = dmg_normal;
        dmgData.Initialize(this);
        dmgReceiver
            .SetBlock(charBlock.IsBlock, BlockFeedback)
            .SetParry(charBlock.IsParry, ParryFeedback)
            .Initialize(rot, () => move.InDash, Dead, TakeDamageFeedback, rb, lifesystem);

        charAttack = new CharacterAttack(attackRange, attackAngle, timeToHeavyAttack, charanim, rot,
                                         ReleaseInNormal, ReleaseInHeavy, dmg, feedbacks, dmgData, enemyLayer, move);
        charAttack.FirstAttackReady(true);

        charAnimEvent.Add_Callback("CheckAttackType", CheckAttackType);

        charAnimEvent.Add_Callback("DealAttackRight", DealRight);
        charAnimEvent.Add_Callback("DealAttackLeft", DealLeft);

        charAnimEvent.Add_Callback("ActiveRightAttackFeedback", RightAttacktFeedback);
        charAnimEvent.Add_Callback("ActiveLeftAttackFeedback", LeftAttacktFeedback);

        charAnimEvent.Add_Callback("OnDashAnimEnded", move.ANIM_EVENT_RollEnded);
        charAnimEvent.Add_Callback("Pasos", FootSteps);
        charAnimEvent.Add_Callback("OpenComboWindow", charAttack.ANIM_EVENT_OpenComboWindow);
        charAnimEvent.Add_Callback("CloseComboWindow", charAttack.ANIM_EVENT_CloseComboWindow);

        charAnimEvent.Add_Callback("OnThrow", ThrowCallback);
        debug_options.StartDebug();

        //  DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Speed for testing", false, ToogleSpeed);
        // DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Use LockOn", false, UseLockOn);

        SetStates();

        originalNormal = dmg_normal;
        originalHeavy = dmg_heavy;

        if(StartWithoutWeapons)
        {
            ToggleShield(false);
            ToggleSword(false);
        }
        else
        {
            ToggleShield(true);
            ToggleSword(true);
        }
    }
    
    public void StopMovement() { move.MovementHorizontal(0); move.MovementVertical(0); }
    void SlowMO()
    {
        Main.instance.GetTimeManager().DoSlowMotion(timeScale, slowDuration);
        customCam.DoFastZoom(speedAnim);
    }
    bool IsGrounded() => groundSensor.IsGrounded();
    protected override void OnUpdateEntity()
    {
        stateMachine.Update();
        ChildrensUpdates();
        charAttack.Refresh();
    }

    #region SET STATES
    EventStateMachine<PlayerInputs> stateMachine;


    public Transform GetLookatPosition() { return lookatPosition; }
    void SetStates()
    {
        var idle = new EState<PlayerInputs>("Idle");
        var move = new EState<PlayerInputs>("Move");
        var beginBlock = new EState<PlayerInputs>("Begin_Block");
        var block = new EState<PlayerInputs>("Block");
        var endBlock = new EState<PlayerInputs>("End_Block");
        var parry = new EState<PlayerInputs>("Parry");
        var roll = new EState<PlayerInputs>("Roll");
        var attackCharge = new EState<PlayerInputs>("Charge_Attack");
        var attackRelease = new EState<PlayerInputs>("Release_Attack");
        var takeDamage = new EState<PlayerInputs>("Take_Damage");
        var dead = new EState<PlayerInputs>("Dead");
        var stun = new EState<PlayerInputs>("Stun");
        var onSkill = new EState<PlayerInputs>("OnSkill");
        var bashDash = new EState<PlayerInputs>("BashDash");
        var LookAtOverSholder = new EState<PlayerInputs>("LookAtOverSholder");
        var onMenues = new EState<PlayerInputs>("OnMenues");

        ConfigureState.Create(idle)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            //.SetTransition(PlayerInputs.PARRY, parry)
            .SetTransition(PlayerInputs.ROLL, roll)
            .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.DEAD, dead)
            .SetTransition(PlayerInputs.STUN, stun)
            .SetTransition(PlayerInputs.ON_SKILL, onSkill)
            .SetTransition(PlayerInputs.ON_LOOK_SHOLDER,LookAtOverSholder)
            .Done();

        ConfigureState.Create(LookAtOverSholder)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.ROLL, roll)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.DEAD, dead)
            .SetTransition(PlayerInputs.STUN, stun)
            .SetTransition(PlayerInputs.ON_SKILL, onSkill)
            .SetTransition(PlayerInputs.ON_LOOK_SHOLDER,idle)
            .Done();

        ConfigureState.Create(move)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            //.SetTransition(PlayerInputs.PARRY, parry)
            .SetTransition(PlayerInputs.ROLL, roll)
            .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.DEAD, dead)
            .SetTransition(PlayerInputs.ON_SKILL, onSkill)
            .SetTransition(PlayerInputs.STUN, stun)
            .SetTransition(PlayerInputs.ON_LOOK_SHOLDER, LookAtOverSholder)
            .Done();

        ConfigureState.Create(onSkill)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            //.SetTransition(PlayerInputs.PARRY, parry)
            .SetTransition(PlayerInputs.ROLL, roll)
            .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.DEAD, dead)
            // .SetTransition(PlayerInputs.SPIN, spin)
            .SetTransition(PlayerInputs.STUN, stun)
            .SetTransition(PlayerInputs.ON_LOOK_SHOLDER, LookAtOverSholder)
            .Done();

        ConfigureState.Create(beginBlock)
             .SetTransition(PlayerInputs.BLOCK, block)
             .SetTransition(PlayerInputs.END_BLOCK, endBlock)
             //.SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
             .SetTransition(PlayerInputs.ROLL, bashDash)
             .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
             .SetTransition(PlayerInputs.DEAD, dead)
             .Done();

        ConfigureState.Create(block)
            .SetTransition(PlayerInputs.END_BLOCK, endBlock)
            .SetTransition(PlayerInputs.ROLL, bashDash)
            //.SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.PARRY, parry)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(endBlock)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.ROLL, roll)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(parry)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(roll)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.DEAD, dead)
            //.SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .Done();

        ConfigureState.Create(bashDash)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(onMenues)
            .SetTransition(PlayerInputs.ON_MENU_EXIT, idle)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();


        ConfigureState.Create(attackCharge)
            .SetTransition(PlayerInputs.RELEASE_ATTACK, attackRelease)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(attackRelease)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            .SetTransition(PlayerInputs.DEAD, dead)
            .SetTransition(PlayerInputs.ROLL, roll)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(stun)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.MOVE, move)
            .Done();

        ConfigureState.Create(dead)
            .Done();

        stateMachine = new EventStateMachine<PlayerInputs>(idle, debug_options.DebugState);

        new CharIdle(idle, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetMovement(this.move);

        new CharLookOverSholder(LookAtOverSholder, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetCustomCamera(customCam);

        new CharOnMenues(onMenues, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetSensor(sensor)
            .SetMovement(this.move);

        new CharMove(move, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetMovement(this.move);

        new CharBeginBlock(beginBlock, stateMachine, anim_base)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetMovement(this.move).SetBlock(charBlock);

        new CharBlock(block, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetMovement(this.move)
            .SetBlock(charBlock);

        new CharEndBlock(endBlock, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetBlock(charBlock);

        new CharBashDash(bashDash, stateMachine).SetMovement(this.move).SetAttack(charAttack);

        new CharParry(parry, stateMachine, parryRecall)
            .SetMovement(this.move)
            .SetBlock(charBlock);

        new CharRoll(roll, stateMachine)
            .SetMovement(this.move)
            .SetBlock(charBlock)
            .SetFeedbacks(feedbacks);

        new CharChargeAttack(attackCharge, stateMachine, anim_base)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetMovement(this.move)
            .SetAttack(charAttack);

        new CharReleaseAttack(attackRelease, stateMachine, attackRecall, HeavyAttackRealease, ChangeHeavy)
            .SetMovement(this.move)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetAttack(charAttack)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical).SetFeedbacks(feedbacks);

        new CharTakeDmg(takeDamage, stateMachine, takeDamageRecall);

        new CharStun(stun, stateMachine)
            .Configurate(GetStunDuration, go_StunFeedback)
            .SetMovement(this.move)
            .SetAnimator(charanim);

        new CharOnSkill(onSkill, stateMachine, CanExecuteSkill)
            .SetMovement(this.move);

        new CharDead(dead, stateMachine);
    }

    float GetLeftHorizontal() => moveX;
    float GetLeftVertical() => moveY;
    float GetRightHorizontal() => rotateX;
    float GetRightVertical() => rotateY;
    float GetSpinDuration() => spinDuration;
    float GetSpinSpeed() => spinSpeed;
    float GetStunDuration() => stunDuration;


    #endregion

    #region Menues
    public void InputGoToMenues(bool enter)
    {
        stateMachine.SendInput(enter ? PlayerInputs.ON_MENU_ENTER : PlayerInputs.ON_MENU_EXIT);
    }
    #endregion

    #region Status Effect
    public void SetSlow()
    {
        move.SetSpeed(slowSpeed);
        Slowed = true;
    }
    public void SetNormalSpeed()
    {
        move.SetSpeed();
        Slowed = false;
    }
    public void SetFastSpeed()
    {
        move.SetSpeed(10);
        Slowed = false;
    }
    #endregion

    #region SkillRequest
    public Action RequestExecuteASkill(Action request)
    {
        stateMachine.SendInput(PlayerInputs.ON_SKILL);
        CanExecute = request;
        return GetBackControl;
    }
    Action CanExecute = delegate { };
    void CanExecuteSkill()
    {
        Debug.Log("canExecuteSkill");
        CanExecute.Invoke();
        CanExecute = delegate { };
    }
    void GetBackControl()
    {
        stateMachine.SendInput(PlayerInputs.IDLE);
    }
    #endregion

    #region Throw Something
    Action<Vector3> throwCallback;
    public void ThrowSomething(Action<Vector3> throwInPosition)
    {
        Main.instance.GetChar().charanim.StartThrow();
        throwCallback = throwInPosition;
    }
    void ThrowCallback()
    {
        throwCallback.Invoke(escudo.transform.position);
    }
    #endregion

    #region Spin
    public void StartSpin(float _spinDuration, float _spinSpeed, float _stunDuration)
    {
        spinDuration = _spinDuration;
        spinSpeed = _spinSpeed;
        stunDuration = _stunDuration;
        stateMachine.SendInput(PlayerInputs.SPIN);
    }
    #endregion

    #region Pause & Resume
    protected override void OnPause() { }
    protected override void OnResume() { }
    #endregion

    #region Life
    void OnLoseLife() { }
    void OnGainLife() => customCam.BeginShakeCamera();
    void OnDeath()
    {
        Main.instance.RemoveEntity(this);
        Main.instance.eventManager.TriggerEvent(GameEvents.ON_PLAYER_DEATH);
        Main.instance.GetCombatDirector().RemoveTarget(this);
    }
    void OnChangeLife(int current, int max) { }
    #endregion

    #region Attack
    /////////////////////////////////////////////////////////////////
    void RightAttacktFeedback() { feedbacks.particles.slash_right.Play(); }
    void LeftAttacktFeedback() { feedbacks.particles.slash_left.Play(); }
    public Vector3 DirAttack { get; private set; }
    void DealLeft() { DirAttack = rot.right; DealAttack(); }
    void DealRight() { DirAttack = -rot.right; DealAttack(); }
    public void EVENT_OnAttackBegin()
    {
        if (!canAttack) return;
        //if (stateMachine.Current.Name != "Release_Attack")
        stateMachine.SendInput(PlayerInputs.CHARGE_ATTACK);

        charAttack.UnfilteredAttack();
    }
    public void EVENT_OnAttackEnd() { stateMachine.SendInput(PlayerInputs.RELEASE_ATTACK); }
    public void CheckAttackType() => charAttack.BeginCheckAttackType();//tengo la espada arriba
    public void DealAttack()
    {
        charAttack.ConfigureDealsSuscessful(DealSucessfullNormal, DealSucessfullHeavy, KillInNormal, KillInHeavy, BreakObject);
        charAttack.Attack(isHeavyRelease);
    }

    #region Resultados de los ataques
    void DealSucessfullNormal()
    {
        //Main.instance.GetTimeManager().DoHitStop();
        Main.instance.Vibrate(0.7f, 0.1f);
        Main.instance.CameraShake();
    }
    void DealSucessfullHeavy()
    {
        ChangeHeavy(false);
        Main.instance.Vibrate(1f, 0.2f);
        Main.instance.CameraShake();
    }
    void KillInNormal()
    {
        Main.instance.GetTimeManager().DoHitStop();
        Main.instance.Vibrate(0.7f, 0.1f);
        Main.instance.CameraShake();
    }
    void KillInHeavy()
    {
        SlowMO();
        ChangeHeavy(false);
        Main.instance.Vibrate(1f, 0.5f);
        Main.instance.CameraShake();
    }
    void BreakObject()
    {
        Main.instance.CameraShake();
    }
    #endregion
    void ReleaseInNormal()
    {
        ChangeDamageAttack((int)dmg_normal);
        ChangeAngleAttack(attackAngle);
        ChangeRangeAttack(attackRange);
        charanim.NormalAttack();
    }

    bool isHeavyRelease;
    void ReleaseInHeavy()
    {
        ChangeHeavy(true);
        ChangeDamageAttack((int)dmg_heavy);
        ChangeAngleAttack(attackAngle * 2);
        ChangeRangeAttack(attackRange + 1);
        charanim.HeavyAttack();
        Debug.Log("IsHeavyAttacking");
    }
    void ChangeHeavy(bool y) { isHeavyRelease = y; }

    bool HeavyAttackRealease() { return isHeavyRelease; }

    ///////////BigWeaponSkill

    public void ChangeDamageAttack(int newDamageValue) => charAttack.ChangeDamageBase(newDamageValue);
    public float ChangeRangeAttack(float newRangeValue = -1) => charAttack.currentWeapon.ModifyAttackrange(newRangeValue);
    public float ChangeAngleAttack(float newAngleValue = -1) => charAttack.currentWeapon.ModifyAttackAngle(newAngleValue);
    public CharacterAttack GetCharacterAttack() => charAttack;
    private void OnDrawGizmos()
    {
        if (charAttack == null)
            return;

        Vector3 attackRange_endPoint =
            transform.position + charAttack.forwardPos.forward * charAttack.currentWeapon.GetWpnRange();

        Vector3 initPos = rot.position + Vector3.up;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, attackRange_endPoint);
        Gizmos.DrawLine(initPos, initPos + rot.forward * 2);
        Gizmos.DrawLine(initPos, initPos + (rot.forward + rot.right) * 2);
        Gizmos.DrawLine(initPos, initPos + (rot.forward - rot.right) * 2);
        Gizmos.DrawCube(attackRange_endPoint, new Vector3(.6f, .6f, .6f));
    }

    /////////////////////////////////////////////////////////////////
    #endregion

    #region Block & Parry
    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public void UNITYEVENT_PressDown_Block()
    {
        //Puesto para no poder bloquear cuando el personaje tira el escudo en el boomeranSkill
        if (!canBlock)
            return;
        if (charBlock.CurrentBlockCharges > 0)
            stateMachine.SendInput(PlayerInputs.BEGIN_BLOCK);
    }
    public void ToggleBlock(bool val) => canBlock = val;
    public void UNITYEVENT_PressUp_UpBlocking() => stateMachine.SendInput(PlayerInputs.END_BLOCK);
    public EntityBlock GetCharBlock() => charBlock;
    public void EVENT_Parry() => stateMachine.SendInput(PlayerInputs.PARRY);
    public void AddParry(Action listener) => charBlock.callback_OnParry += listener;
    public void RemoveParry(Action listener) => charBlock.callback_OnParry -= listener;
    public void PerfectParry() { feedbacks.particles.parryParticle.Play(); stateMachine.SendInput(PlayerInputs.PARRY); }
    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    #endregion

    #region Roll
    void OnBeginRoll()
    {
        //Activar trail o feedback x del roll
    }
    void OnEndRoll()
    {
        //desactivar trail o feedback x del roll
        stateMachine.SendInput(PlayerInputs.IDLE);
    }
    public void RollDash()
    {
        if (!groundSensor.IsGrounded() || blockRoll) return;
        if (!InDash() && move.CanUseDash)
        {
            //Chequeo si tengo el teleport activado. Sino, sigo normalmente con el roll
            if (move.TeleportActive)
            {
                if (move.CheckIfCanTeleport()) stateMachine.SendInput(PlayerInputs.ROLL);
                return;
            }
            stateMachine.SendInput(PlayerInputs.ROLL);
            SetNormalSpeed();
        }
    }
    public void AddListenerToDash(Action listener) => move.Dash += listener;
    public void RemoveListenerToDash(Action listener) => move.Dash -= listener;
    public void ChangeDashForTeleport()
    {
        move.SetDashCD(teleportCD);
        move.TeleportActive = true;
        move.Dash -= move.Roll;
        move.Dash += move.Teleport;
    }
    public void ChangeTeleportForDash()
    {
        move.SetDashCD();
        move.TeleportActive = false;
        move.Dash -= move.Teleport;
        move.Dash += move.Roll;
    }

    public CharacterMovement GetCharMove()
    {
        return move;
    }

    #endregion

    #region Movimiento y Rotacion
    float rotateX;
    float rotateY;
    float moveX;
    float moveY;
    public void LeftHorizontal(float axis) => moveX = axis;
    public void LeftVerical(float axis) => moveY = axis;
    public void RightHorizontal(float axis) => rotateX = axis;
    public void RightVerical(float axis) => rotateY = axis;
    #endregion

    #region Take Damage
    void ParryFeedback(EntityBase entity)
    {
        feedbacks.sounds.Play_Parry();
        Main.instance.eventManager.TriggerEvent(GameEvents.ON_PLAYER_PARRY, new object[] { entity });
        PerfectParry();
        Main.instance.GetTimeManager().DoSlowMotion(timeScale, slowDuration);
        customCam.DoFastZoom(10);

        entity?.GetComponent<EnemyBase>().AddForceToRb(entity.transform.position - transform.position, knockbackOnParry, ForceMode.Impulse);
    }

    void BlockFeedback(EntityBase entity)
    {
        feedbacks.particles.blockParticle.Play();
        charanim.BlockSomething();
        charBlock.SetBlockCharges(-1);
        feedbacks.sounds.Play_Block();
    }

    void TakeDamageFeedback(DamageData data)
    {
        float maxLife = Life.GetMax();
        float dmgreceived = data.damage;

        float perc = dmgreceived / maxLife;
        if (perc >= big_damage_limit_percent)
        {
            feedbacks.sounds.Play_TakeBigDamage();
            customCam.BeginShakeCamera(0.5f);
        }
        else
        {
            feedbacks.sounds.Play_TakeNormalDamage();
            customCam.BeginShakeCamera();
        }


        feedbacks.particles.hitParticle.Play();
        Main.instance.Vibrate();
    }

    void Dead(Vector3 dir) { }

    #endregion

    #region Interact
    public void UNITY_EVENT_OnInteractDown()
    {
        sensor.OnInteractDown();
    }
    public void UNITY_EVENT_OnInteractUp()
    {
        sensor.OnInteractUp();
    }
    #endregion

    #region Debuggin
    public DebugOptions debug_options = new DebugOptions();
    [System.Serializable]
    public class DebugOptions
    {
        [SerializeField] UnityEngine.UI.Text txt_debug = null;
        public void DebugState(string state) { if (txt_debug != null) txt_debug.text = state; }
        public void StartDebug() { if (txt_debug != null) txt_debug.enabled = false; DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Character State Machine Debug", true, ToogleDebug); }
        string ToogleDebug(bool active) { if (txt_debug != null) txt_debug.enabled = active; return active ? "debug activado" : "debug desactivado"; }
    }
    #endregion

    #region BuffedState
    float originalNormal;
    float originalHeavy;
    public void ActivateBuffState(float damageBuff, float damageReceived, float speedAcceleration, float scale, float duration)
    {
        charanim.SetUpdateMode(AnimatorUpdateMode.UnscaledTime);
        isBuffed = true;
        dmg_normal = originalNormal + damageBuff;
        dmg_heavy = originalHeavy + damageBuff;
        move.SetSpeed(move.GetDefaultSpeed + speedAcceleration);
        dmgReceived = damageReceived;
        Main.instance.GetTimeManager().DoSlowMo(scale);
    }
    public void DesactivateBuffState(float damageBuff)
    {
        charanim.SetUpdateMode(AnimatorUpdateMode.Normal);
        isBuffed = false;
        dmg_normal = originalNormal;
        dmg_heavy = originalHeavy;
        move.SetSpeed();
        dmgReceived = 1;
        Main.instance.GetTimeManager().StopSlowMo();
    }
    #endregion

    #region Stun
    public void GetStunned(float _stunDuration)
    {
        stunDuration = _stunDuration;
        Debug.Log("GetStunned");
        stateMachine.SendInput(PlayerInputs.STUN);
    }
    #endregion

    #region ToggleWeapons
    public void ToggleSword(bool check)
    {
        ToggleAttack(check);
        currentWeapon.SetActive(check);
    }
    public void ToggleShield(bool check)
    {
        ToggleBlock(check);
        charBlock.shield.SetActive(check);
    }
    #endregion

    #region Fuera de uso
    void FootSteps()
    {
        feedbacks.sounds.Play_FootStep();
    }
    protected override void OnTurnOn() { }
    protected override void OnTurnOff() { }
    protected override void OnFixedUpdate() { }
    #region Items
    public override void OnReceiveItem(ItemWorld itemworld)
    {
        base.OnReceiveItem(itemworld);
    }
    #endregion
    #region Change Weapon
    bool isValue;
    public void ChangeTheWeapon(float w)
    {
        //if (!isValue && !charAttack.inAttack)
        //{
        //    if (w == 1 || w == -1)
        //    {
        //        charAttack.ChangeWeapon((int)w);
        //        ChangeWeaponPassives();
        //        feedbackCW.Stop();
        //        feedbackCW.Play();
        //        isValue = true;
        //    }
        //}
        //else
        //{
        //    if (w != 1 && w != -1)
        //    {
        //        isValue = false;
        //    }
        //}
    }
    public void ChangeDamage(float f)
    {
        charAttack.BuffOrNerfDamage(f);
    }

    #endregion
    #region Guilt
    public Action<int> AddScreamAction = delegate { };
    public void CollectScream()
    {
        AddScreamAction(1);
    }
    #endregion
    #endregion

    public void UEVENT_ShootOverSholder()
    {
        stateMachine.SendInput(PlayerInputs.ON_LOOK_SHOLDER);
    }
}