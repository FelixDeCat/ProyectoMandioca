using UnityEngine;
using Tools.StateMachine;
using System;

public class WendigoEnemy : EnemyWithCombatDirector
{
    //////////////////////////NOTAS/////////////////////////////
    //CombatDistance = vision
    //DistancePos = attackRange
    [SerializeField] bool showCombatDistance = false;
    [SerializeField] bool showAttackRange = false;
    [SerializeField] WendigoView view;
    [SerializeField] GenericEnemyMove moveComponent;
    [SerializeField] RagdollComponent ragdoll = null;
    CharacterGroundSensor groundSensor;
    bool isMelee;
    [SerializeField] SphereCollider meleeCollider;

    //No esta checkeado
    [SerializeField] float rotationSpeed;
    [Header("Combat Options")]
    [SerializeField] Throwable throwObject = null;
    [SerializeField] Transform shootPivot = null;
    [SerializeField] float throwForce = 6;
    [SerializeField] float cdToCast = 2;
    [SerializeField] int damage = 2;
    [SerializeField] float attackRecall = 2;
    [SerializeField] LineOfSight lineOfSight = null;

    [Header("Life Options")]
    [SerializeField] float recallTime = 1;

    private bool cooldown = false;
    private float timercooldown = 0;

    float timerToCast;
    bool stopCD;
    bool castingOver;

    [Header("Feedback")]
    [SerializeField] AnimEvent anim = null;
    private Material[] myMat;
    [SerializeField] Color onHitColor = Color.white;
    [SerializeField] float onHitFlashTime = 0.1f;
    ParticleSystem castPartTemp;
    [SerializeField] EffectBase petrifyEffect = null;
    EventStateMachine<WendigoInputs> sm;

    protected override void OnInitialize()     //Inicia las cosas
    {
        //TODO view
        //cosas
        base.OnInitialize();
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_SPAWN, new object[] { this });
        moveComponent.Configure(rootTransform, rb, groundSensor);

        //Particulas

        //Materials

        //Audio

        //RigidBody
        rb = GetComponent<Rigidbody>();

        //Animaciones

        //Cosas de main?
        Main.instance.AddEntity(this);

        //Combat Director
        IAInitialize(Main.instance.GetCombatDirector());

        //Effects

        //Pool de throws
        ThrowablePoolsManager.instance.CreateAPool(throwObject.name, throwObject);

        //Vision
        lineOfSight.Configurate(rootTransform);
    }
    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
            sm.SendInput(WendigoInputs.IDLE);

        stopCD = true;
    }

    public enum WendigoInputs { IDLE, OBSERVATION, PREPARERANGE, PREPAREMELEE, RANGEAR, MELEEAR, PETRIFY, DEATH, DISABLED };
    private void SetStates()
    {
        //Settacion de estados
        var idle = new EState<WendigoInputs>("Idle");
        var obs = new EState<WendigoInputs>("Observation");
        var prepRange = new EState<WendigoInputs>("PrepareRange");
        var prepMelee = new EState<WendigoInputs>("PrepareMelee");
        var range = new EState<WendigoInputs>("RangeAttack");
        var melee = new EState<WendigoInputs>("MeleeAttack");
        var petry = new EState<WendigoInputs>("Petrify");
        var death = new EState<WendigoInputs>("Death");
        var disable = new EState<WendigoInputs>("Disabled");

        //Crear y Transiciones. Tree en Discord
        ConfigureState.Create(idle)
        .SetTransition(WendigoInputs.OBSERVATION, obs)
        .Done();

        ConfigureState.Create(obs)
        .SetTransition(WendigoInputs.IDLE, idle)
        .SetTransition(WendigoInputs.PREPARERANGE, prepRange)
        .SetTransition(WendigoInputs.PREPAREMELEE, prepMelee)
        .SetTransition(WendigoInputs.OBSERVATION, obs)
        .SetTransition(WendigoInputs.DEATH, death)
        .Done();

        ConfigureState.Create(prepRange)
        .SetTransition(WendigoInputs.OBSERVATION, obs)
        .SetTransition(WendigoInputs.RANGEAR, range)
        .SetTransition(WendigoInputs.PETRIFY, petry)
        .SetTransition(WendigoInputs.DEATH, death)
        .Done();

        ConfigureState.Create(prepMelee)
       .SetTransition(WendigoInputs.OBSERVATION, obs)
        .SetTransition(WendigoInputs.MELEEAR, melee)
        .SetTransition(WendigoInputs.PETRIFY, petry)
        .SetTransition(WendigoInputs.DEATH, death)
        .Done();

        ConfigureState.Create(range)
               .SetTransition(WendigoInputs.OBSERVATION, obs)
               .Done();

        ConfigureState.Create(melee)
               .SetTransition(WendigoInputs.OBSERVATION, obs)
               .Done();

        ConfigureState.Create(petry)
       .SetTransition(WendigoInputs.OBSERVATION, obs)
        .SetTransition(WendigoInputs.DEATH, death)
               .Done();

        ConfigureState.Create(death)
        .SetTransition(WendigoInputs.DISABLED, disable)
        .Done();

        ConfigureState.Create(disable)
        .SetTransition(WendigoInputs.IDLE, idle)
        .Done();

        //Iniciacion de clases de estados

        sm = new EventStateMachine<WendigoInputs>(idle, null);

        new WendigoIdle(idle, view, sm);
        new WendigoObservation(obs, view, moveComponent, combatElement, sm).SetRigidbody(rb).SetRoot(rootTransform);
        new WendigoPrepareMelee(prepMelee, view, sm);
        new WendigoMelee(melee, view, dmgData, meleeCollider, sm).SetDirector(director);

        //var head = Main.instance.GetChar();   //Es necesario?

    }
    protected override void OnTurnOn()
    {
        view.DebugText("TurnOn");
    }
    protected override void OnTurnOff()
    {
        view.DebugText("TurnOff");
    }
    protected override void OnUpdateEntity()
    {
        //Cosas, hay que estudiar primero

        //Si no esta muerto
        if (!death)
        {
            float dist = Vector3.Distance(Main.instance.GetChar().transform.position, transform.position);
            view.DistanceText(dist.ToString());
            //Si esta en combate
            if (combatElement.Combat)
            {
                //Si es Melee (auspiciado por el TriggerDispatcher)
                if (isMelee)
                {
                    if (sm.Current.Name != "PrepareMelee")
                    {

                        //El unico que puede disparar el ataque es el PrepareMelee
                        sm.SendInput(WendigoInputs.PREPAREMELEE);
                    }
                }
                else if (sm.Current.Name != "Observation")
                {
                    sm.SendInput(WendigoInputs.OBSERVATION);
                }

                //Si no esta en la combatDistance
                if (dist >= combatDistance && sm.Current.Name != "Idle")
                {
                    sm.SendInput(WendigoInputs.IDLE);
                    combatElement.ExitCombat();
                }
            }

            //No esta en combate
            if (!combatElement.Combat && combatElement.Target == null)
            {
                //Si esta en la combatdistance
                if (dist <= combatDistance && sm.Current.Name != "Observation")
                {
                    combatElement.EnterCombat();
                    sm.SendInput(WendigoInputs.OBSERVATION);
                }
            }
        }
        //Update de StateMachine
        if (sm != null)
            sm.Update();

        Debug.Log(sm.Current.Name);
    }
    protected override void OnPause()
    {
        base.OnPause();
        if (death) ragdoll.ResumeRagdoll();
    }
    protected override void OnReset()
    {
        //Reset
        ragdoll.Ragdoll(false, Vector3.zero);
        death = false;
        sm.SendInput(WendigoInputs.DISABLED);
    }
    //Attack
    //StateMachin
    void DisableObject()
    {
        canupdate = false;
        combatElement.Combat = false;

    }

    void EnableObject() => Initialize();
    public void OnDrawGizmos()
    {
        if (showCombatDistance)
        {
            Gizmos.DrawWireSphere(transform.position, combatDistance);
        }
        if (showAttackRange)
        {
            Gizmos.DrawWireSphere(transform.position, distancePos);
        }

    }

    public void PlayerInMelee(bool isit)
    {
        isMelee = isit;
    }
    //NotChecked

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    protected override void OnFixedUpdate()
    {
        //throw new System.NotImplementedException();
    }

    public override void ReturnToSpawner()
    {
        base.ReturnToSpawner();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnReceiveItem(InteractCollect itemworld)
    {
        base.OnReceiveItem(itemworld);
    }

    public override void OnStun()
    {
        base.OnStun();
    }

    public override float ChangeSpeed(float newSpeed)
    {
        return base.ChangeSpeed(newSpeed);
    }

    protected override void TakeDamageFeedback(DamageData data)
    {
        //Aca va el view
    }

    protected override void Die(Vector3 dir)
    {
        throw new System.NotImplementedException();
    }

    protected override bool IsDamage()
    {
        if (cooldown || sm.Current.Name == "Death") return true;
        else return false;
    }

    protected override void InmuneFeedback()
    {
        base.InmuneFeedback();
    }

    protected override void OnResume()
    {
        base.OnResume();
    }

    public override void SpawnEnemy()
    {
        base.SpawnEnemy();
    }

    public override void ResetEntity()
    {
        base.ResetEntity();
    }
}
