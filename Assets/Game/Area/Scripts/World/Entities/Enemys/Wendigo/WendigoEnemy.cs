using UnityEngine;
using Tools.StateMachine;
using System;

public class WendigoEnemy : EnemyWithCombatDirector
{
    //////////////////////////NOTAS/////////////////////////////
    //CombatDistance = vision
    //DistancePos = attackRange
    //SIEMPRE DISTANCEPOS < COMBATDISTANCE
    [SerializeField] bool showCombatDistance = false;
    [SerializeField] bool showAttackRange = false;
    [SerializeField] WendigoView view = null;
    [SerializeField] GenericEnemyMove moveComponent = null;
    [SerializeField] RagdollComponent ragdoll = null;
    CharacterGroundSensor groundSensor = null;
    bool isMelee;
    bool hasThrowable;
    [SerializeField] Throwable throwObject = null;
    [SerializeField] AnimEvent animEvents = null;
    [SerializeField] Transform shootPivot = null;
    [SerializeField] float throwMultipier = 6;
    [SerializeField] int damage = 2;
    [SerializeField] LineOfSight lineOfSight = null;
    [SerializeField] float throwTime;

    [SerializeField] float hitTime = 0.1f;
    [SerializeField] Color hitColor = Color.red;
    private bool cooldown = false;
    bool stopCD;
    bool isRotating;
    EventStateMachine<WendigoInputs> sm;

    EffectReceiver myEffectReceiver;
    protected override void OnInitialize()     //Inicia las cosas
    {
        //cosas
        base.OnInitialize();
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_SPAWN, new object[] { this });
        moveComponent.Configure(rootTransform, rb, groundSensor);

        //RigidBody
        rb = GetComponent<Rigidbody>();

        //Animaciones
        animEvents.Add_Callback("GrabaThing", GrabaThing);
        animEvents.Add_Callback("DoKick", DoKick);
        animEvents.Add_Callback("ThrowAThing", ThrowAThing);
        animEvents.Add_Callback("StopRotation", StopRotation);

        //Cosas de main?
        Main.instance.AddEntity(this);

        //Combat Director
        IAInitialize(Main.instance.GetCombatDirector());

        //Pool de throws
        ThrowablePoolsManager.instance.CreateAPool(throwObject.name, throwObject);

        //Vision
        lineOfSight.Configurate(rootTransform);

        myEffectReceiver = GetComponent<EffectReceiver>();
    }
    //AnimEventFunctions
    public void GrabaThing()
    {
        hasThrowable = true;
        view.TurnOnThing();
        sm.SendInput(WendigoEnemy.WendigoInputs.OBSERVATION);
    }
    public void DoKick()
    {
        //isMelee = false;
        dmgData.SetKnockback(300);
        Main.instance.GetChar().GetComponent<DamageReceiver>().TakeDamage(dmgData);
    }
    public void StopRotation()
    {
        isRotating = false;
    }
    public void ThrowAThing()
    {
        view.TurnOffThing();
        hasThrowable = false;
        if (combatElement.Combat)
        {
            Vector3 direction = (combatElement.CurrentTarget().transform.position + Vector3.up - shootPivot.position).normalized;
            Vector3 dir = combatElement.CurrentTarget() ? direction : transform.forward;
            ThrowData newData = new ThrowData().Configure(shootPivot.position, dir, distancePos * throwMultipier, damage, rootTransform);
            ThrowablePoolsManager.instance.Throw(throwObject.name, newData);
        }
        sm.SendInput(WendigoEnemy.WendigoInputs.OBSERVATION);
    }

    //StateMachine
    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
            sm.SendInput(WendigoInputs.IDLE);

        stopCD = true;
    }

    public enum WendigoInputs { IDLE, OBSERVATION, GRABTHING, PREPARERANGE, PREPAREMELEE, RANGEAR, MELEEAR, PETRIFY, DEATH, DISABLED };
    private void SetStates()
    {
        //Settacion de estados
        var idle = new EState<WendigoInputs>("Idle");
        var obs = new EState<WendigoInputs>("Observation");
        var prepRange = new EState<WendigoInputs>("PrepareRange");
        var grabThing = new EState<WendigoInputs>("GrabThing");
        var range = new EState<WendigoInputs>("RangeAttack");
        var prepMelee = new EState<WendigoInputs>("PrepareMelee");
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
        .SetTransition(WendigoInputs.GRABTHING, grabThing)
        .SetTransition(WendigoInputs.OBSERVATION, obs)
        .SetTransition(WendigoInputs.DEATH, death)
        .Done();

        ConfigureState.Create(prepRange)
        .SetTransition(WendigoInputs.OBSERVATION, obs)
        .SetTransition(WendigoInputs.RANGEAR, range)
        .SetTransition(WendigoInputs.PETRIFY, petry)
        .SetTransition(WendigoInputs.DEATH, death)
        .Done();

        ConfigureState.Create(grabThing)
               .SetTransition(WendigoInputs.OBSERVATION, obs)
               .Done();

        ConfigureState.Create(range)
               .SetTransition(WendigoInputs.OBSERVATION, obs)
               .Done();

        ConfigureState.Create(prepMelee)
       .SetTransition(WendigoInputs.OBSERVATION, obs)
        .SetTransition(WendigoInputs.MELEEAR, melee)
        .SetTransition(WendigoInputs.PETRIFY, petry)
        .SetTransition(WendigoInputs.DEATH, death)
        .Done();

        ConfigureState.Create(melee)
               .SetTransition(WendigoInputs.OBSERVATION, obs)
               .SetTransition(WendigoInputs.PREPARERANGE, prepRange)
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
        new WendigoMelee(melee, view, Rotate, sm).SetDirector(director);
        new WendigoGrabRock(grabThing, () => hasThrowable = true, view, sm);
        new WendigoPrepareRange(prepRange, view, moveComponent, combatElement, sm, throwTime).SetRoot(rootTransform);
        new WendigoRange(prepRange, view, Rotate, sm);


    }
    protected void Rotate()
    {
        if (isRotating)
        {
            if (combatElement.CurrentTarget())
            {

                Vector3 dirForward = (combatElement.CurrentTarget().transform.position - rootTransform.position).normalized;
                Vector3 fowardRotation = moveComponent.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));
                moveComponent.Rotation(fowardRotation.normalized);
            }
        }
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
                        isRotating = true;
                        sm.SendInput(WendigoInputs.PREPAREMELEE);
                    }
                }
                //Si tiene algo para tirar
                else if (hasThrowable)
                {
                    //Si esta en la distancia de ataque
                    if (dist <= distancePos)
                    {
                        if (sm.Current.Name != "PrepareRange")
                        {
                            //El unico que puede disparar el throwable es el PrepareRange
                            isRotating = true;
                            sm.SendInput(WendigoInputs.PREPARERANGE);
                        }
                    }
                    //Si no, te observa y va hacia vos
                    else if (sm.Current.Name != "Observation")
                    {
                        sm.SendInput(WendigoInputs.OBSERVATION);
                    }
                }
                //Si no tiene piedra
                else
                {
                    //Por si acaso, agregar cd aca
                    //GrabRock funciona distinto a los prepare. Este se encarga de todo
                    sm.SendInput(WendigoInputs.GRABTHING);
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
                    combatElement.EnterCombat(Main.instance.GetChar().transform);
                    sm.SendInput(WendigoInputs.OBSERVATION);
                }
            }

            if (!combatElement.Combat)
            {
                if (dist >= combatDistance && sm.Current.Name != "Idle")
                {
                    sm.SendInput(WendigoInputs.IDLE);
                    combatElement.ExitCombat();
                }
            }

            myEffectReceiver?.UpdateStates();

        }
        //Update de StateMachine
        if (sm != null)
            sm.Update();

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
        view.Reset();
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
        Gizmos.color = Color.magenta;
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
    protected override void OnResume()
    {
        base.OnResume();
        if (death) ragdoll.ResumeRagdoll();
    }

    protected override void OnFixedUpdate()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnReceiveItem(InteractCollect itemworld)
    {
        base.OnReceiveItem(itemworld);
    }


    public override float ChangeSpeed(float newSpeed)
    {
        return base.ChangeSpeed(newSpeed);
    }

    protected override void TakeDamageFeedback(DamageData data)
    {
        view.Damaged();
        StartCoroutine(OnHitted(hitTime, hitColor));
    }

    protected override void Die(Vector3 dir)
    {
        sm.SendInput(WendigoInputs.IDLE);
        if (dir == Vector3.zero)
            ragdoll.Ragdoll(true, -rootTransform.forward);
        else
            ragdoll.Ragdoll(true, dir);

        /*
                if (castPartTemp != null)
                    ParticlesManager.Instance.StopParticle(castPartTemp.name, castPartTemp);
                    */
        death = true;
        combatElement.ExitCombat();
        Main.instance.RemoveEntity(this);
        myEffectReceiver?.EndAllEffects();
    }

    protected override bool IsDamage()
    {
        if (cooldown || sm.Current.Name == "Death") return true;
        else return false;
    }

}
