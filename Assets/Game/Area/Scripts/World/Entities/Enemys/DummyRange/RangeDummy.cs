using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

public class RangeDummy : EnemyBase
{

    [System.Serializable]
    public class MoveOptions
    {
        [Header("transform parent de rotacion")]
        [SerializeField] Transform rootTransform = null;
        [Header("Rotacion")]
        [SerializeField] float rotSpeed = 2;
        [Header("Movimiento")]
        [SerializeField] float speedMovement = 4;
        [Header("Avoidance")]
        [SerializeField] float avoidWeight = 2;
        [SerializeField] float avoidRadious = 2;

        private float currentSpeed = 0;

        public float GetRotationSpeed() => rotSpeed;
        public Transform GetRootTransform() => rootTransform;
        public float GetOriginalSpeed() => speedMovement;
        public Vector3 GetMyPosition() => rootTransform.transform.position;
        
        //current speed functions
        public float GetCurrentSpeed() => currentSpeed; 
        public void SetCurrentSpeed(float _value) => currentSpeed = _value;
        public void MultiplyCurrentSpeed(float _value) => currentSpeed *= _value;
        public void AddCurrentSpeed(float _value) => currentSpeed += _value;
        public void DivideCurrentSpeed(float _value) => currentSpeed /= _value;
        public float ResetSpeedToOriginal() => currentSpeed = speedMovement;
    }
    public MoveOptions moveOptions = new MoveOptions();

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
    [SerializeField] float recallTime = 1;
    [SerializeField] float forceRecall = 4;
    [SerializeField] float explosionForce = 20;
    [SerializeField] float petrifiedTime = 2;
    private float stunTimer;
    private Action<EState<RangeDummyInput>> EnterStun;
    private Action<string> UpdateStun;
    private Action<RangeDummyInput> ExitStun;

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
    public Animator GetAnimator() => animator;
    public AnimEvent GetAnimEvent() => anim;
    [SerializeField] UnityEngine.UI.Text txt_debug = null;
    [SerializeField] Material freeze_shader = null;
    [SerializeField] Color onHitColor;
    [SerializeField] float onHitFlashTime;

    public bool isOnFire { get; private set; }
    

    EventStateMachine<RangeDummyInput> sm;
    public Rigidbody GetRigidbody() => rb;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_SPAWN, new object[] { this });

        combatComponent.Configure(AttackEntity);
        anim.Add_Callback("DealDamage", DealDamage);
        anim.Add_Callback("Death", DeathAnim);
        lifesystem.AddEventOnDeath(Die);
        moveOptions.SetCurrentSpeed(moveOptions.GetOriginalSpeed());
       // debug_options.StartDebug();

        Main.instance.AddEntity(this);

        IAInitialize(Main.instance.GetCombatDirector());
    }

    protected override void OnReset()
    {
        //lo de el ragdoll
    }

    public override void Zone_OnPlayerExitInThisRoom()
    {
        //Debug.Log("Player enter the room");
        IAInitialize(Main.instance.GetCombatDirector());
    }

    public override void Zone_OnPlayerEnterInThisRoom(Transform who)
    {
        sm.SendInput(RangeDummyInput.DISABLE);
    }

    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
        {
            sm.SendInput(RangeDummyInput.IDLE);
        }

        director.AddNewTarget(this);
        canupdate = true;
    }

    public void DealDamage() { combatComponent.ManualTriggerAttack(); sm.SendInput(RangeDummyInput.ATTACK); }

    public override void ToAttack() => attacking = true;

    public void AttackEntity(EntityBase e)
    {
    }

    protected override void OnUpdateEntity()
    {
        if (canupdate)
        {
            EffectUpdate();

            if (combat)
            {
                if (Vector3.Distance(entityTarget.transform.position, transform.position) > combatDistance + 2)
                {
                    director.DeadEntity(this, entityTarget);
                    entityTarget = null;
                    combat = false;
                }
            }

            if(!combat && entityTarget == null)
            {
                if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) <= combatDistance)
                {
                    director.AddToList(this, Main.instance.GetChar());
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

        moveOptions.MultiplyCurrentSpeed(freezeSpeedSlowed);

        animator.speed *= freezeSpeedSlowed;

        var smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
        {
            myMat = smr.materials;

            Material[] mats = smr.materials;
            mats[0] = freeze_shader;
            smr.materials = mats;
        }

        AddEffectTick(() => { }, freezeTime, () => 
        {
            moveOptions.DivideCurrentSpeed(freezeSpeedSlowed);
            animator.speed /= freezeSpeedSlowed;
            var smr2 = GetComponentInChildren<SkinnedMeshRenderer>();
            if (smr2 != null)
            {
                Material[] mats = smr2.materials;
                smr2.materials = myMat;
            }
        });
    }

    protected override void TakeDamageFeedback(DamageData data)
    {
        if (sm.Current.Name == "Idle" || sm.Current.Name == "Persuit")
        {
            attacking = false;
            director.ChangeTarget(this, data.owner, entityTarget);
        }

        sm.SendInput(RangeDummyInput.TAKE_DAMAGE);

        greenblood.Play();
        cooldown = true;

        StartCoroutine(OnHitted(myMat, onHitFlashTime, onHitColor));
    }

    protected override void Die(Vector3 dir)
    {
        sm.SendInput(RangeDummyInput.DIE);
        //if (dir == Vector3.zero)
        //    ragdoll.Ragdoll(true, -rootTransform.forward);
        //else
        //    ragdoll.Ragdoll(true, dir);
        death = true;
        director.RemoveTarget(this);
        Main.instance.RemoveEntity(this);

        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_DEAD, new object[] { transform.position, petrified, expToDrop });
    }

    protected override bool IsDamage()
    {
        if (cooldown || Invinsible || sm.Current.Name == "Die") return true;
        else return false;
    }

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
                    sm.SendInput(RangeDummyInput.BEGIN_ATTACK);
                else if (name == "Attack")
                    sm.SendInput(RangeDummyInput.ATTACK);
                else
                    sm.SendInput(RangeDummyInput.IDLE);
            }
        };

        ExitStun = (input) => {
            animator.speed = currentAnimSpeed;
            stunTimer = 0;
        };

        sm.SendInput(RangeDummyInput.PETRIFIED);
    }

    public override float ChangeSpeed(float newSpeed)
    {
        //Si le mando negativo me devuelve la original
        //para guardarla en el componente WebSlowedComponent
        if (newSpeed < 0)
            return moveOptions.GetOriginalSpeed();

        //Busco el estado follow para poder cambiarle la velocidad
        moveOptions.SetCurrentSpeed(newSpeed);

        return moveOptions.GetOriginalSpeed();
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
        sm.SendInput(RangeDummyInput.DIE);

        director.DeadEntity(this, entityTarget, this);
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

    #region STATE MACHINE THINGS
    public enum RangeDummyInput { IDLE, BEGIN_ATTACK,ATTACK, GO_TO_POS, DIE, DISABLE, TAKE_DAMAGE, PETRIFIED, PARRIED };
    void SetStates()
    {
        var idle = new EState<RangeDummyInput>("Idle");
        var goToPos = new EState<RangeDummyInput>("Follow");
        var beginAttack = new EState<RangeDummyInput>("Begin_Attack");
        var attack = new EState<RangeDummyInput>("Attack");
        var parried = new EState<RangeDummyInput>("Parried");
        var takeDamage = new EState<RangeDummyInput>("Take_Damage");
        var die = new EState<RangeDummyInput>("Die");
        var disable = new EState<RangeDummyInput>("Disable");
        var petrified = new EState<RangeDummyInput>("Petrified");

        ConfigureState.Create(idle)
            .SetTransition(RangeDummyInput.GO_TO_POS, goToPos)
            .SetTransition(RangeDummyInput.TAKE_DAMAGE, takeDamage)
            .SetTransition(RangeDummyInput.BEGIN_ATTACK, beginAttack)
            .SetTransition(RangeDummyInput.DIE, die)
            .SetTransition(RangeDummyInput.PETRIFIED, petrified)
            .SetTransition(RangeDummyInput.DISABLE, disable)
            .Done();

        ConfigureState.Create(goToPos)
            .SetTransition(RangeDummyInput.IDLE, idle)
            .SetTransition(RangeDummyInput.TAKE_DAMAGE, takeDamage)
            .SetTransition(RangeDummyInput.DIE, die)
            .SetTransition(RangeDummyInput.PETRIFIED, petrified)
            .SetTransition(RangeDummyInput.DISABLE, disable)
            .Done();

        ConfigureState.Create(beginAttack)
            .SetTransition(RangeDummyInput.ATTACK, attack)
            .SetTransition(RangeDummyInput.DIE, die)
            .SetTransition(RangeDummyInput.PETRIFIED, petrified)
            .SetTransition(RangeDummyInput.PARRIED, parried)
            .Done();

        ConfigureState.Create(attack)
            .SetTransition(RangeDummyInput.IDLE, idle)
            .SetTransition(RangeDummyInput.DIE, die)
            .SetTransition(RangeDummyInput.PETRIFIED, petrified)
            .SetTransition(RangeDummyInput.PARRIED, parried)
            .Done();

        ConfigureState.Create(parried)
            .SetTransition(RangeDummyInput.IDLE, idle)
            .SetTransition(RangeDummyInput.PETRIFIED, petrified)
            .SetTransition(RangeDummyInput.DIE, die)
            .Done();

        ConfigureState.Create(petrified)
            .SetTransition(RangeDummyInput.IDLE, idle)
            .SetTransition(RangeDummyInput.ATTACK, attack)
            .SetTransition(RangeDummyInput.BEGIN_ATTACK, beginAttack)
            .SetTransition(RangeDummyInput.DIE, die)
            .SetTransition(RangeDummyInput.DISABLE, disable)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(RangeDummyInput.IDLE, idle)
            .SetTransition(RangeDummyInput.DISABLE, disable)
            .SetTransition(RangeDummyInput.PETRIFIED, petrified)
            .SetTransition(RangeDummyInput.DIE, die)
            .Done();

        ConfigureState.Create(die)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(RangeDummyInput.IDLE, idle)
            .Done();

       // sm = new EventStateMachine<RangeDummyInput>(idle, debug_options.DebugState);

        var head = Main.instance.GetChar();

        //Asignando las funciones de cada estado
        //new DummyIdleState(idle, sm, IsAttack, distanceToAttack, normalDistance, rotSpeed, this).SetAnimator(animator).SetRoot(rootTransform)
        //                                                                                                             .SetDirector(director);

        //new DummyFollowState(goToPos, sm, avoidRadious, avoidWeight, rotSpeed, GetCurrentSpeed, CurrentTargetPos, normalDistance, this).SetAnimator(animator).SetRigidbody(rb)
        //                                                                                                  .SetRoot(rootTransform);

        //new DummyAttAnt(beginAttack, sm, rotSpeed, this).SetAnimator(animator).SetDirector(director).SetRoot(rootTransform);

        //new DummyAttackState(attack, sm, cdToAttack, this).SetAnimator(animator).SetDirector(director);

        //new DummyParried(parried, sm, parriedTime, this).SetAnimator(animator).SetDirector(director);

        //new DummyTDState(takeDamage, sm, recallTime).SetAnimator(animator);

        //new DummyStunState(petrified, sm, StartStun, TickStun, EndStun);

        //new DummyDieState(die, sm).SetAnimator(animator).SetDirector(director).SetRigidbody(rb);

        //new DummyDisableState(disable, sm, EnableObject, DisableObject);
    }

    void StartStun(EState<RangeDummyInput> input) { EnterStun(input); }

    void TickStun(string name) { UpdateStun(name); }

    void EndStun(RangeDummyInput input) { ExitStun(input); }

    void DisableObject()
    {
        canupdate = false;
        moveOptions.ResetSpeedToOriginal();
        combat = false;
    }

    void EnableObject() { Initialize(); }

    #endregion

    //#region Debuggin

    //public DebugOptions debug_options = new DebugOptions();
    //[System.Serializable]
    //public class DebugOptions
    //{
    //    [SerializeField] UnityEngine.UI.Text txt_debug = null;
    //    public void DebugState(string state) { if (txt_debug != null) txt_debug.text = state; }
    //    public void ToogleDebug(bool val) { if (txt_debug != null) txt_debug.enabled = val; }
    //    internal void StartDebug() { if (txt_debug != null) txt_debug.enabled = DevelopToolsCenter.instance.EnemyDebuggingIsActive(); }
    //}
    //#endregion

}
