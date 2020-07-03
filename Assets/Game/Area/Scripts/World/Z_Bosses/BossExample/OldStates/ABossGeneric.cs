using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

public class ABossGeneric : EnemyBase
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

    
    public bool isOnFire { get; private set; }
    

    EventStateMachine<RangeDummyInput> sm;
    public Rigidbody GetRigidbody() => rb;

    protected override void OnInitialize()
    {
        //Cuando se implemente el dmg receiver, poner acá esta línea de código y setear las funciones TakeDamageFeedback, Die y IsDamage (InmuneFeedback solo si hace falta)
        //base.OnInitialize();
        rb.GetComponent<Rigidbody>();

        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_SPAWN, new object[] { this });

        //Debug.Log("OnInitialize");
        combatComponent.Configure(AttackEntity);
        anim.Add_Callback("DealDamage", DealDamage);
        anim.Add_Callback("Death", DeathAnim);
        lifesystem.AddEventOnDeath(Die);
        moveOptions.SetCurrentSpeed(moveOptions.GetOriginalSpeed());
        debug_options.StartDebug();

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
        if (sm == null)
            SetStates();
        else
        {
            sm.SendInput(RangeDummyInput.IDLE);
        }

        canupdate = true;
    }

    public override void ToAttack() => attacking = true;

    public void DealDamage() { combatComponent.ManualTriggerAttack(); sm.SendInput(RangeDummyInput.ATTACK); }

    public void AttackEntity(EntityBase e)
    {
    }

    protected override void OnUpdateEntity()
    {
        if (canupdate)
        {
            if (combat)
            {
                if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) > combatDistance + 2)
                {

                    combat = false;
                }
            }

            if(!combat && entityTarget == null)
            {
                if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) <= combatDistance)
                {
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

    float currentAnimSpeed;

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

    public void Die()
    {
        sm.SendInput(RangeDummyInput.DIE);
        
        death = true;
        Main.instance.RemoveEntity(this);
    }

    void DeathAnim()
    {
        //vector3, boolean, int
        gameObject.SetActive(false);
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

        sm = new EventStateMachine<RangeDummyInput>(idle, debug_options.DebugState);

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

    protected override void TakeDamageFeedback(DamageData data)
    {
    }

    protected override void Die(Vector3 dir)
    {
    }

    protected override bool IsDamage()
    {
        return true;
    }

    

    #endregion

    #region Debuggin

    public DebugOptions debug_options = new DebugOptions();
    [System.Serializable]
    public class DebugOptions
    {
        [SerializeField] UnityEngine.UI.Text txt_debug = null;
        public void DebugState(string state) { if (txt_debug != null) txt_debug.text = state; }
        public void ToogleDebug(bool val) { if (txt_debug != null) txt_debug.enabled = val; }
        internal void StartDebug() { if (txt_debug != null) txt_debug.enabled = DevelopToolsCenter.instance.EnemyDebuggingIsActive(); }
    }
    #endregion

}
