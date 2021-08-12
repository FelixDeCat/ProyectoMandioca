using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

using MandragoraInputs = TrueDummyEnemy.DummyEnemyInputs;

public class MandragoraEnemy : EnemyWithCombatDirector
{
    [Header("Move Options")]
    [SerializeField] GenericEnemyMove movement = null;
    [SerializeField] CharacterGroundSensor groundSensor = null;

    [Header("Combat Options")]
    [SerializeField] CombatComponent combatComponent = null;
    [SerializeField] LineOfSight lineOfSight = null;
    [SerializeField] int damage = 2;
    [SerializeField] float normalDistance = 8;
    [SerializeField] float cdToAttack = 1;
    [SerializeField] float parriedTime = 2;
    [SerializeField] float knockback = 20;

    [Header("Life Options")]
    [SerializeField] float recallTime = 1;

    private bool cooldown = false;
    CDModule cdModuleStopeable = new CDModule();
    CDModule cdModuleNonStopeable = new CDModule();
    EffectReceiver myEffectReceiver;

    [Header("Spawn Options")]
    public EnemyBase_IntDictionary enemiesToSpawnDic = new EnemyBase_IntDictionary();
    [SerializeField] PlayObject trapToDie = null;
    [SerializeField] float trapDuration = 5;
    [SerializeField] Transform rootToTrap = null;
    public bool mandragoraIsTrap = false;
    [SerializeField] TriggerDispatcher trigger = null;
    public SpawnerSpot spawnerSpot = null;

    [Header("Feedback")]
    [SerializeField] AnimEvent anim = null;
    [SerializeField] Color onHitColor = Color.white;
    [SerializeField] float onHitFlashTime = 0.1f;
    [SerializeField] RagdollComponent ragdoll = null;

    float lineOfSightTimer;
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
        ///public AudioClip clip_walkEnt;
        public AudioClip mandragoraScream_Clip;
        public AudioClip mandragoraSpawn_Clip;
        public AudioClip mandragoraAttack_Clip;
        public AudioClip mandragoraDeath_Clip;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_SPAWN, new object[] { this });
        ParticlesManager.Instance.GetParticlePool(particles._spawnParticules.name, particles._spawnParticules, 5);
        ParticlesManager.Instance.GetParticlePool(particles.greenblood.name, particles.greenblood, 8);

        AudioManager.instance.GetSoundPool(sounds._takeHit_AC.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds._takeHit_AC);
        AudioManager.instance.GetSoundPool(sounds.mandragoraAttack_Clip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds.mandragoraAttack_Clip);
        AudioManager.instance.GetSoundPool(sounds.mandragoraScream_Clip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds.mandragoraScream_Clip);
        AudioManager.instance.GetSoundPool(sounds.mandragoraSpawn_Clip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds.mandragoraSpawn_Clip);
        AudioManager.instance.GetSoundPool(sounds.mandragoraDeath_Clip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds.mandragoraDeath_Clip);

        rb = GetComponent<Rigidbody>();
        combatComponent.Configure(AttackEntity);
        anim.Add_Callback("DealDamage", DealDamage);
        anim.Add_Callback("SpawnEnemy", SpawnBrothers);
        movement.Configure(rootTransform, rb, groundSensor);
        lineOfSight.Configurate(rootTransform);
        lineOfSight.viewDistance = combatDistance;

        StartDebug();

        Main.instance.AddEntity(this);

        IAInitialize(Main.instance.GetCombatDirector());
        if(trapToDie) PoolManager.instance.GetObjectPool(trapToDie.name, trapToDie);

        dmgReceiver.ChangeKnockback(movement.ApplyForceToVelocity, () => false);

        myEffectReceiver = GetComponent<EffectReceiver>();

        if (!mandragoraIsTrap) return;
        spawnerSpot.Initialize();

        foreach (var item in enemiesToSpawnDic)
            PoolManager.instance.GetObjectPool(item.Key.name == name ? "enemy_Mandragora" : item.Key.name, item.Key);
    }

    public override void SpawnEnemy()
    {
        animator.SetBool("entry", true);
        mandragoraIsTrap = false;
        AudioManager.instance.PlaySound(sounds.mandragoraSpawn_Clip.name,transform);
        trigger.gameObject.SetActive(false);
        base.SpawnEnemy();
    }

    public void AwakeMandragora()
    {
        AudioManager.instance.PlaySound(sounds.mandragoraScream_Clip.name, transform);
        animator.SetBool("entry", true);
        trigger.StopAllCoroutines();
        trigger.gameObject.SetActive(false);
    }

    void SpawnBrothers()
    {
        if (!mandragoraIsTrap) return;

        foreach (var item in enemiesToSpawnDic)
        {
            for (int i = 0; i < item.Value; i++)
            {
                var pos = spawnerSpot.GetSurfacePos();
                if (pos == Vector3.zero) pos = transform.position;
                spawnerSpot.SpawnPrefab(pos, item.Key, CurrentScene);
            }
        }

        mandragoraIsTrap = false;
        sm.SendInput(MandragoraInputs.IDLE);
    }

    void OnDead()
    {
        if (trapToDie)
        {
            var pool = PoolManager.instance.GetObjectPool(trapToDie.name);
            var trap = pool.GetPlayObject(trapDuration);
            trap.transform.position = rootToTrap.position;
        }
    }

    protected override void OnReset()
    {
        if (death)
        {
            ragdoll.Ragdoll(false, Vector3.zero);
            death = false;
            cdModuleStopeable.EndCDWithoutExecute("Dead");
        }
        trigger.gameObject.SetActive(true);
        cdModuleStopeable.ResetAll();
        cdModuleNonStopeable.ResetAll();
        sm.SendInput(MandragoraInputs.DISABLE);
    }
    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
            sm.SendInput(MandragoraInputs.IDLE);
    }

    protected override void OnUpdateEntity()
    {
        if (!mandragoraIsTrap)
        {
            if (!death)
            {
                if (combatElement.Target != null && combatElement.Combat)
                {
                    if (!lineOfSight.OnSight(combatElement.Target))
                        lineOfSightTimer += Time.deltaTime;
                    else
                        lineOfSightTimer = 0;
                    if (lineOfSightTimer >= 3)
                    {
                        combatElement.ExitCombat();
                    }
                }

                if (!combatElement.Combat && combatElement.Target == null)
                {
                    var possibleTarget = Main.instance.GetChar().transform;
                    if (lineOfSight.OnSight(possibleTarget))
                    {
                        combatElement.EnterCombat(Main.instance.GetChar().transform);
                    }
                }
            }
            if (!petrified) { sm?.Update(); cdModuleStopeable.UpdateCD(); }
            myEffectReceiver?.UpdateStates();
            movement.OnUpdate();
            cdModuleNonStopeable.UpdateCD();
        }
    }
    Vector3 force;
    protected override void OnPause()
    {
        base.OnPause();
        force = rb.velocity;
        rb.isKinematic = true;
        if (death) ragdoll.PauseRagdoll();
    }
    protected override void OnResume()
    {
        base.OnResume();
        rb.isKinematic = false;
        rb.velocity = force;
        if (death) ragdoll.ResumeRagdoll();
    }

    #region Attack
    public void DealDamage()
    {
        combatComponent.ManualTriggerAttack();
        sm.SendInput(MandragoraInputs.ATTACK);
        AudioManager.instance.PlaySound(sounds.mandragoraAttack_Clip.name,transform);
    }

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
    protected override void TakeDamageFeedback(DamageData data)
    {
        AudioManager.instance.PlaySound(sounds._takeHit_AC.name,transform);

        sm.SendInput(MandragoraInputs.TAKE_DAMAGE);

        ParticlesManager.Instance.PlayParticle(particles.greenblood.name, transform.position + Vector3.up);
        cooldown = true;
        cdModuleNonStopeable.AddCD("TakeDamageCD", () => cooldown = false, recallTime);

        StartCoroutine(OnHitted(onHitFlashTime, onHitColor));
    }

    protected override void Die(Vector3 dir)
    {
        groundSensor?.TurnOff();
        AudioManager.instance.PlaySound(sounds.mandragoraDeath_Clip.name,transform);
        sm.SendInput(MandragoraInputs.DIE);
        if (dir == Vector3.zero)
            ragdoll.Ragdoll(true, -rootTransform.forward);
        else
            ragdoll.Ragdoll(true, dir);
        death = true;
        mandragoraIsTrap = false;
        combatElement.ExitCombat();
        Main.instance.RemoveEntity(this);
        myEffectReceiver?.EndAllEffects();
    }

    protected override bool IsDamage()
    {
        if (cooldown || sm.Current.Name == "Die") return true;
        else return false;
    }
    #endregion

    protected override void OnFixedUpdate() { }
    protected override void OnTurnOff()
    {
        sm.SendInput(MandragoraInputs.DISABLE);
        combatElement.ExitCombat();
        groundSensor?.TurnOff();
    }

    protected override void OnTurnOn()
    {
        sm.SendInput(MandragoraInputs.IDLE);
        groundSensor?.TurnOn();
    }

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

        ConfigureState.Create(idle)
            .SetTransition(MandragoraInputs.GO_TO_POS, goToPos)
            .SetTransition(MandragoraInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(MandragoraInputs.DIE, die)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .SetTransition(MandragoraInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(goToPos)
            .SetTransition(MandragoraInputs.IDLE, idle)
            .SetTransition(MandragoraInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(MandragoraInputs.DIE, die)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .SetTransition(MandragoraInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(chasing)
            .SetTransition(MandragoraInputs.IDLE, idle)
            .SetTransition(MandragoraInputs.GO_TO_POS, goToPos)
            .SetTransition(MandragoraInputs.BEGIN_ATTACK, beginAttack)
            .SetTransition(MandragoraInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(MandragoraInputs.DIE, die)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(beginAttack)
            .SetTransition(MandragoraInputs.ATTACK, attack)
            .SetTransition(MandragoraInputs.DIE, die)
            .SetTransition(MandragoraInputs.PARRIED, parried)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(attack)
            .SetTransition(MandragoraInputs.IDLE, idle)
            .SetTransition(MandragoraInputs.DIE, die)
            .SetTransition(MandragoraInputs.PARRIED, parried)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(parried)
            .SetTransition(MandragoraInputs.IDLE, idle)
            .SetTransition(MandragoraInputs.DIE, die)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(MandragoraInputs.IDLE, idle)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .SetTransition(MandragoraInputs.DIE, die)
            .Done();

        ConfigureState.Create(die)
            .SetTransition(MandragoraInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(MandragoraInputs.IDLE, idle)
            .Done();

        sm = new EventStateMachine<MandragoraInputs>(idle, DebugState);

        var head = Main.instance.GetChar();

        new DummyIdleState(idle, sm, movement, distancePos, normalDistance, combatElement).SetAnimator(animator).SetRoot(rootTransform).SetDirector(director);

        new DummyFollowState(goToPos, sm, movement, normalDistance, distancePos, combatElement).SetAnimator(animator).SetRoot(rootTransform);

        new DummyChasing(chasing, sm, () => combatElement.Attacking, distancePos, movement, combatElement).SetDirector(director).SetRoot(rootTransform);

        new DummyAttAnt(beginAttack, sm, movement, combatElement).SetAnimator(animator).SetDirector(director).SetRoot(rootTransform);

        new DummyAttackState(attack, sm, cdToAttack, combatElement).SetAnimator(animator).SetDirector(director).SetCD(cdModuleStopeable);

        new DummyParried(parried, sm, parriedTime, combatElement).SetAnimator(animator).SetDirector(director).SetCD(cdModuleStopeable);

        new DummyTDState(takeDamage, sm, recallTime).SetAnimator(animator).SetCD(cdModuleStopeable);

        new DummyDieState<MandragoraInputs>(die, sm, ragdoll, OnDead, ReturnToSpawner,cdModuleStopeable);

        new DummyDisableState<MandragoraInputs>(disable, sm, EnableObject, DisableObject);
    }

    void DisableObject()
    {
        canupdate = false;
        movement.SetDefaultSpeed();
        combatElement.Combat = false;
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
