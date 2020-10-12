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
    [SerializeField] RagdollComponent ragdoll = null;
    ParticleSystem castPartTemp;

    [SerializeField] EffectBase petrifyEffect = null;
    EventStateMachine<WendigoInputs> sm;

    public DataBaseWendigoParticles particles;
    public DataBaseWendigoSounds sounds;
    public class DataBaseWendigoParticles
    {
        public ParticleSystem castParticles = null;
        public ParticleSystem takeDmg = null;
    }

    [System.Serializable]
    public class DataBaseWendigoSounds
    {
        public AudioClip takeDmgSound;
        public AudioClip attackSound;
    }
    protected override void OnInitialize()     //Inicia las cosas
    {
        //TODO view
        //cosas
        base.OnInitialize();
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_SPAWN, new object[] { this });

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

    public enum WendigoInputs { IDLE, PREPARERANGE, PREPAREMELEE, RANGEAR, MELEEAR, PETRIFY, DEATH, DISABLED };
    private void SetStates()
    {
        //Settacion de estados
        var idle = new EState<WendigoInputs>("Idle");
        var prepRange = new EState<WendigoInputs>("PrepareRange");
        var prepMelee = new EState<WendigoInputs>("PrepareMelee");
        var range = new EState<WendigoInputs>("RangeAttack");
        var melee = new EState<WendigoInputs>("MeleeAttack");
        var petry = new EState<WendigoInputs>("Petrify");
        var death = new EState<WendigoInputs>("Death");
        var disable = new EState<WendigoInputs>("Disabled");

        //Crear y Transiciones

        //Iniciacion de clases de estados

    }

    protected override void OnUpdateEntity()
    {
        //Cosas, hay que estudiar primero
    }
    protected override void OnPause()
    {
        base.OnPause();
        if (death) ragdoll.ResumeRagdoll();
    }
    protected override void OnReset()
    {
        //Reset
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

    protected override void OnTurnOn()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnTurnOff()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnFixedUpdate()
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    protected override void Die(Vector3 dir)
    {
        throw new System.NotImplementedException();
    }

    protected override bool IsDamage()
    {
        throw new System.NotImplementedException();
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
