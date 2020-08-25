using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

using MandragoraInputs = TrueDummyEnemy.DummyEnemyInputs;

public class MandragoraEnemy : EnemyBase
{
    [Header("Move Options")]
    [SerializeField] GenericEnemyMove movement = null;

    public AnimationCurve animEmisive;

    [Header("Combat Options")]
    [SerializeField] CombatComponent combatComponent = null;
    [SerializeField] int damage = 2;
    [SerializeField] float normalDistance = 8;
    [SerializeField] float cdToAttack = 1;
    [SerializeField] float parriedTime = 2;
    [SerializeField] float knockback = 20;

    private CombatDirector director;

    [Header("Life Options")]
    [SerializeField] float recallTime = 1;

    private bool cooldown = false;
    private float timercooldown = 0;

    [Header("Spawn Options")]
    [SerializeField] List<EnemyBase> enemiesTypes = new List<EnemyBase>();
    [SerializeField, Range(1, 25)] float enemiesToSpawn = 5;
    [SerializeField] PlayObject trapToDie = null;
    [SerializeField] float trapDuration = 5;
    [SerializeField] Transform rootToTrap = null;
    [SerializeField] bool mandragoraIsTrap = false;
    [SerializeField] TriggerDispatcher trigger = null;
    [SerializeField] SpawnerSpot spawnerSpot = null;

    [Header("Feedback")]
    [SerializeField] AnimEvent anim = null;
    [SerializeField] Animator animator = null;
    private Material[] myMat;
    [SerializeField] Color onHitColor = Color.white;
    [SerializeField] float onHitFlashTime = 0.1f;
    [SerializeField] RagdollComponent ragdoll = null;
    private const string takeHit_audioName = "woodChop";

    [SerializeField] EffectBase petrifyEffect = null;
    EventStateMachine<MandragoraInputs> sm;

    public MandragoraParticles particles;
    public MandragoraAudio sounds;
    [Serializable]
    public class MandragoraParticles
    {
        public ParticleSystem _spawnParticules = null;
        public ParticleSystem greenblood = null;
    }

    [Serializable]
    public class MandragoraAudio
    {
        public AudioClip _takeHit_AC;
        public AudioClip clip_walkEnt;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_SPAWN, new object[] { this });
        ParticlesManager.Instance.GetParticlePool(particles._spawnParticules.name, particles._spawnParticules, 5);
        ParticlesManager.Instance.GetParticlePool(particles.greenblood.name, particles.greenblood, 8);

        var smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
            myMat = smr.materials;

        AudioManager.instance.GetSoundPool(takeHit_audioName, AudioGroups.GAME_FX, sounds._takeHit_AC);
        AudioManager.instance.GetSoundPool("WalkEnt", AudioGroups.GAME_FX, sounds.clip_walkEnt, true);

        rb = GetComponent<Rigidbody>();
        combatComponent.Configure(AttackEntity);
        anim.Add_Callback("DealDamage", DealDamage);
        anim.Add_Callback("SpawnEnemy", SpawnBrothers);
        movement.Configure(rootTransform, rb);

        StartDebug();

        Main.instance.AddEntity(this);

        IAInitialize(Main.instance.GetCombatDirector());
        petrifyEffect?.AddStartCallback(() => sm.SendInput(MandragoraInputs.PETRIFIED));
        petrifyEffect?.AddEndCallback(() => sm.SendInput(MandragoraInputs.IDLE));
        PoolManager.instance.GetObjectPool(trapToDie.name, trapToDie);

        if (!mandragoraIsTrap) return;
        spawnerSpot.Initialize();
        for (int i = 0; i < enemiesTypes.Count; i++)
            PoolManager.instance.GetObjectPool(enemiesTypes[i].name, enemiesTypes[i]);
        On();
    }

    public override void SpawnEnemy()
    {
        animator.SetBool("entry", true);
        mandragoraIsTrap = false;

        base.SpawnEnemy();
    }

    public void AwakeMandragora()
    {
        animator.SetBool("entry", true);
        trigger.StopAllCoroutines();
        trigger.gameObject.SetActive(false);
    }

    void SpawnBrothers()
    {
        if (!mandragoraIsTrap) return;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int index = UnityEngine.Random.Range(0, enemiesTypes.Count);
            var enemy = spawnerSpot.SpawnPrefab(spawnerSpot.GetSurfacePos(), PoolManager.instance.GetObjectPool(enemiesTypes[index].name));
            enemy.GetComponent<EnemyBase>().SpawnEnemy();
        }

        mandragoraIsTrap = false;
        sm.SendInput(MandragoraInputs.IDLE);
    }

    void OnDead()
    {
        var pool = PoolManager.instance.GetObjectPool(trapToDie.name);
        var trap = pool.GetPlayObject(trapDuration);
        trap.transform.position = rootToTrap.position;
    }

    protected override void OnReset()
    {
        ragdoll.Ragdoll(false, Vector3.zero);
    }

    public override void Zone_OnPlayerExitInThisRoom()
    {
        //Debug.Log("Player enter the room");
        IAInitialize(Main.instance.GetCombatDirector());
    }

    public override void Zone_OnPlayerEnterInThisRoom(Transform who)
    {
        sm.SendInput(MandragoraInputs.DISABLE);
    }

    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
            sm.SendInput(MandragoraInputs.IDLE);

        director.AddNewTarget(this);

        canupdate = true;
    }

    protected override void OnUpdateEntity()
    {
        if (canupdate && !mandragoraIsTrap)
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

            sm?.Update();

            if (cooldown)
            {
                if (timercooldown < recallTime) timercooldown = timercooldown + 1 * Time.deltaTime;
                else { cooldown = false; timercooldown = 0; }
            }
        }
    }
    protected override void OnPause() { }
    protected override void OnResume() { }

    #region Attack
    public void DealDamage() { combatComponent.ManualTriggerAttack(); sm.SendInput(MandragoraInputs.ATTACK); }

    public void AttackEntity(DamageReceiver e)
    {
        dmgData.SetDamage(damage).SetDamageTick(false).SetDamageType(Damagetype.Normal).SetKnockback(knockback)
    .SetPositionAndDirection(transform.position);
        Attack_Result takeDmg = e.TakeDamage(dmgData);

        if (takeDmg == Attack_Result.parried)
        {
            combatComponent.Stop();
            sm.SendInput(MandragoraInputs.PARRIED);
        }
    }

    public override void ToAttack() => attacking = true;
    #endregion

    #region Effects
    public override float ChangeSpeed(float newSpeed)
    {
        if (newSpeed < 0)
            return movement.GetInitSpeed();

        movement.SetCurrentSpeed(newSpeed);

        return movement.GetInitSpeed();
    }
    #endregion

    #region Life Things

    public GenericLifeSystem Life() => lifesystem;

    protected override void TakeDamageFeedback(DamageData data)
    {
        if (sm.Current.Name == "Idle" || sm.Current.Name == "Persuit")
        {
            attacking = false;
            director.ChangeTarget(this, data.owner, entityTarget);
        }

        AudioManager.instance.PlaySound(takeHit_audioName);

        sm.SendInput(MandragoraInputs.TAKE_DAMAGE);

        ParticlesManager.Instance.PlayParticle(particles.greenblood.name, transform.position + Vector3.up);
        cooldown = true;

        StartCoroutine(OnHitted(myMat, onHitFlashTime, onHitColor));
    }

    protected override void Die(Vector3 dir)
    {
        sm.SendInput(MandragoraInputs.DIE);
        if (dir == Vector3.zero)
            ragdoll.Ragdoll(true, -rootTransform.forward);
        else
            ragdoll.Ragdoll(true, dir);
        death = true;
        director.RemoveTarget(this);
        Main.instance.RemoveEntity(this);
    }

    protected override bool IsDamage()
    {
        if (cooldown || Invinsible || sm.Current.Name == "Die") return true;
        else return false;
    }
    #endregion

    protected override void OnFixedUpdate() { }
    protected override void OnTurnOff()
    {
        sm.SendInput(MandragoraInputs.DISABLE);
        if (combat)
        {
            director.DeadEntity(this, entityTarget);
            entityTarget = null;
            combat = false;
        }
    }
    protected override void OnTurnOn() { sm.SendInput(MandragoraInputs.IDLE); }

    #region STATE MACHINE THINGS


    void SetStates()
    {
        var sleeping = new EState<MandragoraInputs>("Sleeping");
        var idle = new EState<MandragoraInputs>("Idle");
        var goToPos = new EState<MandragoraInputs>("Follow");
        var chasing = new EState<MandragoraInputs>("Chasing");
        var beginAttack = new EState<MandragoraInputs>("Begin_Attack");
        var attack = new EState<MandragoraInputs>("Attack");
        var parried = new EState<MandragoraInputs>("Parried");
        var takeDamage = new EState<MandragoraInputs>("Take_Damage");
        var die = new EState<MandragoraInputs>("Die");
        var disable = new EState<MandragoraInputs>("Disable");
        var petrified = new EState<MandragoraInputs>("Petrified");

        ConfigureState.Create(idle)
            .SetTransition(MandragoraInputs.GO_TO_POS, goToPos)
            .SetTransition(MandragoraInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(MandragoraInputs.DIE, die)
            .SetTransition(MandragoraInputs.PETRIFIED, petrified)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .SetTransition(MandragoraInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(goToPos)
            .SetTransition(MandragoraInputs.IDLE, idle)
            .SetTransition(MandragoraInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(MandragoraInputs.DIE, die)
            .SetTransition(MandragoraInputs.PETRIFIED, petrified)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .SetTransition(MandragoraInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(chasing)
            .SetTransition(MandragoraInputs.IDLE, idle)
            .SetTransition(MandragoraInputs.GO_TO_POS, goToPos)
            .SetTransition(MandragoraInputs.BEGIN_ATTACK, beginAttack)
            .SetTransition(MandragoraInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(MandragoraInputs.DIE, die)
            .SetTransition(MandragoraInputs.PETRIFIED, petrified)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(beginAttack)
            .SetTransition(MandragoraInputs.ATTACK, attack)
            .SetTransition(MandragoraInputs.DIE, die)
            .SetTransition(MandragoraInputs.PETRIFIED, petrified)
            .SetTransition(MandragoraInputs.PARRIED, parried)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(attack)
            .SetTransition(MandragoraInputs.IDLE, idle)
            .SetTransition(MandragoraInputs.DIE, die)
            .SetTransition(MandragoraInputs.PETRIFIED, petrified)
            .SetTransition(MandragoraInputs.PARRIED, parried)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(parried)
            .SetTransition(MandragoraInputs.IDLE, idle)
            .SetTransition(MandragoraInputs.PETRIFIED, petrified)
            .SetTransition(MandragoraInputs.DIE, die)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(petrified)
            .SetTransition(MandragoraInputs.IDLE, idle)
            .SetTransition(MandragoraInputs.BEGIN_ATTACK, beginAttack)
            .SetTransition(MandragoraInputs.DIE, die)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(MandragoraInputs.IDLE, idle)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .SetTransition(MandragoraInputs.PETRIFIED, petrified)
            .SetTransition(MandragoraInputs.DIE, die)
            .Done();

        ConfigureState.Create(die)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(MandragoraInputs.IDLE, idle)
            .Done();

        sm = new EventStateMachine<MandragoraInputs>(idle, DebugState);

        var head = Main.instance.GetChar();

        new DummyIdleState(idle, sm, movement, distancePos, normalDistance, this).SetAnimator(animator).SetRoot(rootTransform).SetDirector(director);

        new DummyFollowState(goToPos, sm, movement, normalDistance, distancePos, this).SetAnimator(animator).SetRoot(rootTransform);

        new DummyChasing(chasing, sm, IsAttack, distancePos, movement, this).SetDirector(director).SetRoot(rootTransform);

        new DummyAttAnt(beginAttack, sm, movement, this).SetAnimator(animator).SetDirector(director).SetRoot(rootTransform);

        new DummyAttackState(attack, sm, cdToAttack, this).SetAnimator(animator).SetDirector(director);

        new DummyParried(parried, sm, parriedTime, this).SetAnimator(animator).SetDirector(director);

        new DummyTDState(takeDamage, sm, recallTime).SetAnimator(animator);

        new DummyStunState(petrified, sm);

        new DummyDieState(die, sm, ragdoll, OnDead).SetAnimator(animator).SetDirector(director).SetRigidbody(rb);

        new DummyDisableState(disable, sm, EnableObject, DisableObject);
    }

    void DisableObject()
    {
        canupdate = false;
        movement.SetDefaultSpeed();
        combat = false;
    }

    void EnableObject() => Initialize();

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(spawnerSpot.spawnSpot.position, spawnerSpot.radious);
    }

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
