using System.Collections;
using UnityEngine;
using System;
using Tools.StateMachine;

public class CharacterHead : CharacterControllable
{
    public enum PlayerInputs
    {
        IDLE, MOVE, BEGIN_BLOCK, BLOCK, END_BLOCK, PARRY, CHARGE_ATTACK, RELEASE_ATTACK,
        TAKE_DAMAGE, DEAD, ROLL, STUN, ON_MENU_ENTER, ON_MENU_EXIT, FALLING, CHARGE_BOOMERANG_SHIELD, RELEASE_BOOMERANG_SHIELD, ENVAINAR
    };

    Action ChildrensUpdates;
    [SerializeField] CharacterInput _charInput = null;
    [SerializeField] bool StartWithoutWeapons = false;
    public bool Slowed { get; private set; }
    Func<bool> InDash;

    [Header("Movement Options")]
    AdventureMode _advMode;

    public Transform rayPivot;

    [SerializeField] Transform rot = null;
    public Transform Root { get { return rot; } }
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
    [SerializeField] CharacterAttack charAttack = new CharacterAttack();
    [SerializeField] float attackRecall = 1;
    [SerializeField] DamageReceiver dmgReceiver = null;
    public DamageReceiver DamageReceiver() => dmgReceiver;
    CustomCamera customCam;
    [SerializeField] float timeToDownWeapons = 5;
    public bool IsComboWomboActive;
    Action callback_IsComboTime_Enable, callback_IsComboTime_Disable = delegate { };
    bool canAddComboHit = true;
    [SerializeField] ComboWomboSystem combo_system = new ComboWomboSystem();

    [SerializeField] AudioClip chargeSound = null;
    [SerializeField] GameObject go_StunFeedback = null;

    float stunDuration;

    //Items
    bool imUsingItemOnWeapon = false;

    public Action ChangeWeaponPassives = delegate { };

    //Modelo del arma para feedback placeholder
    public GameObject currentWeapon;

    [Header("Interactable")]
    public InteractSensor sensor;
    [SerializeField] Interactable shieldInteractable = null;
    [SerializeField] Interactable swordInteractable = null;

    [Header("Life Options")]
    [Range(0f, 1f)]
    [SerializeField] float big_damage_limit_percent = 0.3f;

    [SerializeField] CharLifeSystem lifesystem = null;
    public CharLifeSystem Life => lifesystem;
    //Para saber si estoy en lo de caronte
    [SerializeField] bool _imInHell = false;
    public bool ImInHell() => _imInHell;
    
    Rigidbody rb;

    [HideInInspector]
    public bool isBuffed = false;
    float dmgReceived = 1f;

    [Header("Parry & Block Options")]
    public CharacterBlock charBlock;

    [SerializeField] float parryRecall = 0;
    [SerializeField] float takeDamageRecall = 0;
    public Transform ShieldForward;
    [SerializeField] CharFeedbacks feedbacks = null;

    private bool blockRoll;
    public bool BlockRoll { set { blockRoll = value; } }

    [HideInInspector] private bool canAttack = false;
    public void ToggleAttack(bool val) => canAttack = val;

    public Transform MyParent { get; private set; }

    private void Start()
    {
        MyParent = transform.parent;
        lifesystem
           .Configure_CharLifeSystem()
           .ADD_EVENT_OnGainLife(OnGainLife)
           .ADD_EVENT_OnLoseLife(OnLoseLife)
           .ADD_EVENT_Death(OnDeath)
           .ADD_EVENT_OnChangeValue(OnChangeLife);
    }

    protected override void OnInitialize()
    {
        _advMode = GetComponent<AdventureMode>();
        Main.instance.GetCombatDirector().AddNewTarget(this);
        rb = GetComponent<Rigidbody>();
        combo_system.SetSound(chargeSound);
        feedbacks.Initialize();

        charanim = new CharacterAnimator(anim_base);
        customCam = FindObjectOfType<CustomCamera>();

        move.Initialize(rb, rot, charanim, feedbacks, groundSensor);
        InDash += move.InCD;
        ChildrensUpdates += move.OnUpdate;
        move.SetCallbacks(OnBeginRoll, OnEndRoll);

        //Asi se que estoy en el infierno
        GameLoop.instance.ADD_EVENT_GoToHell(() => HellMode(true));
        GameLoop.instance.ADD_EVENT_BackFromHell(() => HellMode(false));

        charBlock
            .Initialize()
            .SetFeedbacks(feedbacks)
            .SetAnimator(charanim);
        charBlock.callback_OnParry += () => charanim.Parry();
        charBlock.callback_EndBlock += UNITYEVENT_PressUp_UpBlocking;
        ChildrensUpdates += charBlock.OnUpdate;

        dmgReceiver
            .SetBlock(charBlock.IsBlock, BlockFeedback)
            .SetParry(charBlock.IsParry, ParryFeedback)
            .SetIsDamage(() => move.InDash)
            .AddDead(Dead)
            .AddTakeDamage(TakeDamageFeedback)
            .Initialize(rot, null, lifesystem, move.AddForceToVelocity);

        charAttack
            .SetAnimator(charanim)
            .SetCharMove(move)
            .SetFeedbacks(feedbacks)
            .SetForward(rot)
            .Initialize(this);

        if (IsComboWomboActive)
        {
            combo_system.AddCallback_OnComboready(ActiveCombo);
            combo_system.AddCallback_OnComboReset(EndTime_DeactiveCombo);
        }

        charAttack.Add_callback_Normal_attack(ReleaseInNormal);
        charAttack.Add_callback_Heavy_attack(ReleaseInHeavy);

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

        SetStates();

        if (StartWithoutWeapons)
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
    protected override void OnUpdateEntity()
    {
        stateMachine.Update();
        ChildrensUpdates();
        charAttack.Refresh();
        combo_system.OnUpdate();
    }

    public void GetBackControl()
    {
        stateMachine.SendInput(PlayerInputs.IDLE);
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
        var bashDash = new EState<PlayerInputs>("BashDash");
        var onMenues = new EState<PlayerInputs>("OnMenues");
        var falling = new EState<PlayerInputs>("Falling");
        var boomerangCharge = new EState<PlayerInputs>("BoomerangCharge");
        var boomerangRelease = new EState<PlayerInputs>("BoomerangRelease");
        var envainar = new EState<PlayerInputs>("Envainar");

        ConfigureState.Create(idle)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            .SetTransition(PlayerInputs.ROLL, roll)
            .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.CHARGE_BOOMERANG_SHIELD, boomerangCharge)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.DEAD, dead)
            .SetTransition(PlayerInputs.STUN, stun)
            .SetTransition(PlayerInputs.FALLING, falling)
            .SetTransition(PlayerInputs.ENVAINAR, envainar)
            .Done();

        ConfigureState.Create(move)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            .SetTransition(PlayerInputs.ROLL, roll)
            .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.CHARGE_BOOMERANG_SHIELD, boomerangCharge)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.DEAD, dead)
            .SetTransition(PlayerInputs.STUN, stun)
            .SetTransition(PlayerInputs.FALLING, falling)
            .SetTransition(PlayerInputs.ENVAINAR, envainar)
            .Done();

        ConfigureState.Create(beginBlock)
             .SetTransition(PlayerInputs.BLOCK, block)
             .SetTransition(PlayerInputs.END_BLOCK, endBlock)
             .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
             .SetTransition(PlayerInputs.ROLL, bashDash)
             .SetTransition(PlayerInputs.PARRY, parry)
             .SetTransition(PlayerInputs.DEAD, dead)
             .Done();

        ConfigureState.Create(block)
            .SetTransition(PlayerInputs.END_BLOCK, endBlock)
            .SetTransition(PlayerInputs.ROLL, bashDash)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.PARRY, parry)
            .SetTransition(PlayerInputs.DEAD, dead)
            .SetTransition(PlayerInputs.FALLING, falling)
            .Done();

        ConfigureState.Create(endBlock)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.ROLL, roll)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            .SetTransition(PlayerInputs.DEAD, dead)
            .SetTransition(PlayerInputs.FALLING, falling)
            .Done();

        ConfigureState.Create(parry)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.DEAD, dead)
            .SetTransition(PlayerInputs.END_BLOCK, endBlock)
            .Done();

        ConfigureState.Create(roll)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(falling)
           .SetTransition(PlayerInputs.IDLE, idle)
           .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
           .SetTransition(PlayerInputs.DEAD, dead)
           .Done();

        ConfigureState.Create(bashDash)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(onMenues)
            .SetTransition(PlayerInputs.ON_MENU_EXIT, idle)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();


        ConfigureState.Create(attackCharge)
            .SetTransition(PlayerInputs.RELEASE_ATTACK, attackRelease)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(attackRelease)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.CHARGE_BOOMERANG_SHIELD, boomerangCharge)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            .SetTransition(PlayerInputs.DEAD, dead)
            .SetTransition(PlayerInputs.ROLL, roll)
            .SetTransition(PlayerInputs.FALLING, falling)
            .Done();

        ConfigureState.Create(boomerangCharge)
            .SetTransition(PlayerInputs.RELEASE_BOOMERANG_SHIELD, boomerangRelease)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(boomerangRelease)
           .SetTransition(PlayerInputs.IDLE, idle)
           .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
           .SetTransition(PlayerInputs.MOVE, move)
           .SetTransition(PlayerInputs.DEAD, dead)
           .SetTransition(PlayerInputs.ROLL, roll)
           .SetTransition(PlayerInputs.FALLING, falling)
           .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.DEAD, dead)
            .SetTransition(PlayerInputs.FALLING, falling)
            .Done();

        ConfigureState.Create(stun)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.FALLING, falling)
            .Done();

        ConfigureState.Create(envainar)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.ON_MENU_ENTER, onMenues)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.FALLING, falling)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(dead)
            .Done();

        stateMachine = new EventStateMachine<PlayerInputs>(idle, debug_options.DebugState);

        new CharIdle(idle, stateMachine, charanim)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetMovement(this.move);

        new CharOnMenues(onMenues, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetSensor(sensor)
            .SetMovement(this.move);

        new CharMove(move, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetMovement(this.move);

        Action<bool> ChangeAttack = (x) => Attacking = x;

        new CharBeginBlock(beginBlock, stateMachine, anim_base, ChangeAttack)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetMovement(this.move).SetBlock(charBlock);

        new CharBlock(block, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetMovement(this.move)
            .SetBlock(charBlock);

        new CharEndBlock(endBlock, stateMachine, ChangeAttack)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetBlock(charBlock);

        new CharBashDash(bashDash, stateMachine, ChangeAttack, charanim, feedbacks.particles.trailBashDash)
            .SetMovement(this.move)
            .SetAttack(charAttack)
            .SetBlock(charBlock);

        new CharParry(parry, stateMachine, parryRecall, ChangeAttack)
            .SetMovement(this.move)
            .SetBlock(charBlock);

        new CharRoll(roll, stateMachine)
            .SetMovement(this.move)
            .SetBlock(charBlock)
            .SetFeedbacks(feedbacks);

        new CharChargeAttack(attackCharge, stateMachine, anim_base, ChangeAttack)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetMovement(this.move)
            .SetAttack(charAttack);

        new CharReleaseAttack(attackRelease, stateMachine, attackRecall, HeavyAttackRealease, ChangeHeavy, ChangeAttack, charanim)
            .SetMovement(this.move)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetAttack(charAttack)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical).SetFeedbacks(feedbacks);

        new CharBoomerangCharge(boomerangCharge, charanim, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetMovement(this.move);

        new CharBoomerangRelease(boomerangRelease, stateMachine, ThrowCallback, charanim)
            .SetMovement(this.move)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical);


        new CharTakeDmg(takeDamage, stateMachine, () => takeDamageRecall, charanim);

        new CharStun(stun, stateMachine)
            .SetMovement(this.move)
            .SetAnimator(charanim);

        new CharEnvainar(envainar, stateMachine, DownWeaponsFunction, UpWeaponsFunction, () => !canAttack && !canBlock ? false : true, () => UpWeapons);

        CharFalling tempFalling = new CharFalling(falling, stateMachine);
        tempFalling.SetAnimator(charanim).SetMovement(this.move).SetLeftAxis(GetLeftHorizontal, GetLeftVertical);

        new CharDead(dead, stateMachine);

        groundSensor.SetFallingSystem(this.move.fallMaxDistance, () => stateMachine.SendInput(PlayerInputs.FALLING), tempFalling.ActivateCD);
    }

    float GetLeftHorizontal() => moveX;
    float GetLeftVertical() => moveY;
    float GetStunDuration() => stunDuration;

    #endregion

    #region Caronte

    void HellMode(bool val)
    {
        if (val)
        {
            _imInHell = true;
            _advMode.UnregisterEvents();
        }
        else
        {
            _imInHell = false;
            _advMode.RegisterEvents();
        }
    }

    #endregion

    #region Combat Check
    public bool Combat { get; set; }


    public Action UpWeaponsAction = delegate { };
    public Action DownWeaponsAction = delegate { };
    bool inTrap;
    public bool InTrap
    {
        set
        {
            if (value == inTrap) return;
            inTrap = value;
            if (value == true && !attacking)
                UpWeaponsFunction();
            else if (value == false && !attacking)
                StartCoroutine(DownWeaponsCoroutine());
        }
        get => inTrap;
    }
    bool attacking;
    public bool Attacking
    {
        set
        {
            if (value == attacking) return;
            attacking = value;
            if (value == true && !inTrap)
                UpWeaponsFunction();
            else if (value == false && !inTrap)
                StartCoroutine(DownWeaponsCoroutine());
        }
        get => attacking;
    }

    public bool UpWeapons { private set; get; }

    IEnumerator DownWeaponsCoroutine()
    {
        float timer = 0;
        bool isOver = false;
        while (!isOver)
        {
            if (attacking || inTrap || !UpWeapons || imUsingItemOnWeapon) yield break;

            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();

            if (timer >= timeToDownWeapons)
            {
                if (stateMachine.Current.Name != "Roll")
                    isOver = true;
            }
        }

        DownWeaponsFunction();
    }

    void UpWeaponsFunction()
    {
        charanim.InCombat(1);

        UpWeapons = true;

        UpWeaponsAction?.Invoke();

        Main.instance.gameUiController.ChangeCombat(1);
    }
    void DownWeaponsFunction()
    {
        attacking = false;
        inTrap = false;
        Combat = false;
        charanim.InCombat(0);
        UpWeapons = false;
        DownWeaponsAction?.Invoke();
        imUsingItemOnWeapon = false;
        Main.instance.gameUiController.ChangeCombat(0);
    }

    public void EVENT_WeaponsToggle()
    {
        stateMachine.SendInput(PlayerInputs.ENVAINAR);
    }

    #endregion

    #region Menues
    public void InputGoToMenues(bool enter)
    {
        stateMachine.SendInput(enter ? PlayerInputs.ON_MENU_ENTER : PlayerInputs.ON_MENU_EXIT);
    }
    #endregion

    #region Status Effect
    public void SetSlow(float slowness = 3)
    {
        move.SetSpeed(slowness);
        Slowed = true;
    }
    public void SetNormalSpeed()
    {
        move.SetSpeed();
        Slowed = false;
    }
    public void SetFastSpeed(float fastess = 10)
    {
        move.SetSpeed(fastess);
        Slowed = false;
    }
    #endregion

    #region Item Effects
    public void TurnOnGreekOilEffect(float duration)
    {
        DownWeaponsAction += TurnOffGreekOilEffect;
        imUsingItemOnWeapon = true;
        StartCoroutine(StartEffectOnSword(duration));
        feedbacks.particles.fire_greekOil.gameObject.SetActive(true);
    }

    IEnumerator StartEffectOnSword(float duration)
    {
        charAttack.Add_callback_SecondaryEffect(PrenderFuegoAEnemigo);
        float count = 0;

        while (count < duration)
        {
            count += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        TurnOffGreekOilEffect();      
    }

    void PrenderFuegoAEnemigo(EffectReceiver alQueAfecto){alQueAfecto.TakeEffect(EffectName.OnFire);}

    public void TurnOffGreekOilEffect()
    {
        DownWeaponsAction -= TurnOffGreekOilEffect;
        imUsingItemOnWeapon = false;
        charAttack.Remove_callback_SecondaryEffect(PrenderFuegoAEnemigo);
        feedbacks.particles.fire_greekOil.gameObject.SetActive(false);
    }
    #endregion

    #region Throw Something
    Action<Vector3> throwCallback;
    public void ThrowSomething(Action<Vector3> throwInPosition)
    {
        throwCallback = throwInPosition;
        stateMachine.SendInput(PlayerInputs.RELEASE_BOOMERANG_SHIELD);
    }
    void ThrowCallback()
    {
        throwCallback.Invoke(escudo.transform.position);
        charanim.StartThrow(false);
    }
    public void ChargeThrowShield()
    {
        stateMachine.SendInput(PlayerInputs.CHARGE_BOOMERANG_SHIELD);
    }
    #endregion

    #region Pause & Resume
    Vector3 force;
    float animSpeed;
    protected override void OnPause()
    {
        animSpeed = anim_base.speed;
        anim_base.speed = 0;
        force = rb.velocity;
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rb.isKinematic = true;
    }
    protected override void OnResume()
    {
        anim_base.speed = animSpeed;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.velocity = force;
    }
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
    void RightAttacktFeedback() { feedbacks.particles.slash_right.Play(); }
    void LeftAttacktFeedback() { feedbacks.particles.slash_left.Play(); }
    public Vector3 DirAttack { get; private set; }
    void DealLeft() { DirAttack = rot.right; DealAttack(); }
    void DealRight() { DirAttack = -rot.right; DealAttack(); }
    public void EVENT_OnAttackBegin()
    {
        if (!canAttack) return;
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
        Main.instance.Vibrate(0.7f, 0.1f);
        Main.instance.CameraShake();

        if (canAddComboHit)
        {
            canAddComboHit = false;
            combo_system.AddHit();
            StartCoroutine(CDToAddComboHit());
        }
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
    public void ComboWombo_Subscribe(Action enter, Action exit) { callback_IsComboTime_Enable = enter; callback_IsComboTime_Disable = exit; }
    void ActiveCombo()
    {
        IsComboWomboActive = true;
        feedbacks.particles.HeavyLoaded.Play();
        callback_IsComboTime_Enable?.Invoke();
    }
    void EndTime_DeactiveCombo()
    {
        Invoke("DelayDisableCallback", 0.1f);
    }

    void ResetCombo()
    {
        IsComboWomboActive = false;
        charAttack.ResetHeavyAttackTime();
    }
    void DelayDisableCallback()
    {
        callback_IsComboTime_Disable?.Invoke();
    }

    IEnumerator CDToAddComboHit()
    {
        yield return new WaitForSeconds(combo_system.cdToAddHit);
        canAddComboHit = true;
    }

    IEnumerator ComboDurationAvaliableProc()
    {
        yield return new WaitForSeconds(combo_system.timeToCombo);
        ResetCombo();
    }
    #endregion
    void ReleaseInNormal()
    {
        ChangeDamageAttack((int)charAttack.Dmg_normal);
        ChangeAngleAttack(charAttack.AttackAngle);
        ChangeRangeAttack(charAttack.AttackRange);
        charanim.NormalAttack();
    }

    bool isHeavyRelease;
    void ReleaseInHeavy()
    {
        if(IsComboWomboActive)
            Life.Heal(Mathf.RoundToInt(Life.GetMax() * .13f));

        ChangeHeavy(true);
        ChangeDamageAttack((int)charAttack.Dmg_Heavy);
        ChangeAngleAttack(charAttack.AttackAngle * 2);
        ChangeRangeAttack(charAttack.AttackRange + 1);
        charanim.HeavyAttack();
        ResetCombo();
    }
    void ChangeHeavy(bool y) { isHeavyRelease = y; }

    bool HeavyAttackRealease() { return isHeavyRelease; }

    public void ChangeDamageAttack(int newDamageValue) => charAttack.ChangeDamageBase(newDamageValue);
    public float ChangeRangeAttack(float newRangeValue = -1) => charAttack.CurrentWeapon.ModifyAttackrange(newRangeValue);
    public float ChangeAngleAttack(float newAngleValue = -1) => charAttack.CurrentWeapon.ModifyAttackAngle(newAngleValue);
    public CharacterAttack GetCharacterAttack() => charAttack;
    #endregion

    #region Block & Parry
    public void UNITYEVENT_PressDown_Block()
    {
        if (!canBlock)
            return;
        if (charBlock.CurrentBlockCharges > 0)
            stateMachine.SendInput(PlayerInputs.BEGIN_BLOCK);
    }
    public void ToggleBlock(bool val) => canBlock = val;
    public void UNITYEVENT_PressUp_UpBlocking() => stateMachine.SendInput(PlayerInputs.END_BLOCK);
    public EntityBlock GetCharBlock() => charBlock;
    public void AddParry(Action listener) => charBlock.callback_OnParry += listener;
    public void RemoveParry(Action listener) => charBlock.callback_OnParry -= listener;
    public void PerfectParry() { feedbacks.particles.parryParticle.Play(); stateMachine.SendInput(PlayerInputs.PARRY); }

    public void UnequipShield(Vector3 dir)
    {
        if (!canBlock) return;

        shieldInteractable.gameObject.SetActive(true);
        shieldInteractable.transform.SetParent(null);
        shieldInteractable.transform.SetPositionAndRotation(charBlock.shield.transform.position, charBlock.shield.transform.rotation);
        shieldInteractable.GetComponent<Rigidbody>()?.AddForce(dir * 20, ForceMode.Impulse);
        stateMachine.SendInput(PlayerInputs.END_BLOCK);

        ToggleShield(false);
    }

    public void UnequipSword(Vector3 dir)
    {
        if (!canAttack) return;

        swordInteractable.gameObject.SetActive(true);
        swordInteractable.transform.SetParent(null);
        swordInteractable.transform.SetPositionAndRotation(currentWeapon.transform.position, currentWeapon.transform.rotation);
        swordInteractable.GetComponent<Rigidbody>()?.AddForce(dir * 5, ForceMode.Impulse);
        stateMachine.SendInput(PlayerInputs.RELEASE_ATTACK);

        ToggleSword(false);
    }
    #endregion

    #region Roll
    void OnBeginRoll()
    {
        //Activar trail o feedback x del roll
    }
    void OnEndRoll()
    {
        stateMachine.SendInput(PlayerInputs.IDLE);
    }
    public void RollDash()
    {
        if (!groundSensor.IsInGround || blockRoll) return;
        if (!InDash() && move.CanUseDash)
        {
            stateMachine.SendInput(PlayerInputs.ROLL);

            if(!ImInHell())
                SetNormalSpeed();
        }
    }
    public void AddListenerToDash(Action listener) => move.Dash += listener;
    public void RemoveListenerToDash(Action listener) => move.Dash -= listener;
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
            charanim.SetTypeDamge(1);
            takeDamageRecall = 2.5f;
        }
        else
        {
            feedbacks.sounds.Play_TakeNormalDamage();
            customCam.BeginShakeCamera();
            charanim.SetTypeDamge(0);
            takeDamageRecall = 0;
        }
        stateMachine.SendInput(PlayerInputs.TAKE_DAMAGE);

        if(stateMachine.Current.Name == "Take_Damage" && perc >= big_damage_limit_percent)
        {
            Vector3 newForward = (data.owner_position - transform.position).normalized;
            newForward.y = 0;

            Root.forward = newForward;
        }

        feedbacks.particles.hitParticle.Play();
        Main.instance.Vibrate();
    }

    void Dead(Vector3 dir) {     }

    #endregion

    #region Interact
    public void UNITY_EVENT_OnInteractDown()
    {
        var temp = sensor.OnInteractDown();

        if(temp != null)
        {
            if(temp.delayTime > 0) charanim.SetInteract(true, 0);
            else charanim.SetInteract(true, 1);
        }

        if (IsComboWomboActive)
        {
            charAttack.ForceHeavy();
            charanim.HeavyAttack();
            IsComboWomboActive = false;
        }
    }
    public void UNITY_EVENT_OnInteractUp()
    {
        sensor.OnInteractUp();
        charanim.SetInteract(false, 0);
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
    public override void OnReceiveItem(InteractCollect itemworld)
    {
        base.OnReceiveItem(itemworld);
    }
    #endregion
    #endregion
}