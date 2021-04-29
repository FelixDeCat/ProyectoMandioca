using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;
using UnityEngine.Serialization;

public class TrueDummyEnemy : EnemyWithCombatDirector
{
    [Header("Move Options")]
    [SerializeField] GenericEnemyMove movement = null;
    [SerializeField] CharacterGroundSensor groundSensor = null;

    [Header("Combat Options")]
    [SerializeField] CombatComponent combatComponent = null;
    [SerializeField] int damage = 2;
    [SerializeField] float normalDistance = 8;
    [SerializeField] float cdToAttack = 1;
    [SerializeField] float parriedTime = 2;
    [SerializeField] float knockback = 20;
    public DummySpecialAttack dummySpecialAttack;

    [Header("Life Options")]
    [SerializeField] float recallTime = 1;

    private bool cooldown = false;
    CDModule cdModuleStopeable = new CDModule();
    CDModule cdModuleNonStopeable = new CDModule();
    EffectReceiver myEffectReceiver;

    [Header("Feedback")]
    [SerializeField] AnimEvent anim = null;
    [SerializeField] Color onHitColor = Color.white;
    [SerializeField] float onHitFlashTime = 0.1f;
    [SerializeField] RagdollComponent ragdoll = null;
    [SerializeField] Transform warningAttack_pos = null;
    
    EventStateMachine<DummyEnemyInputs> sm;

    public DataBaseDummyParticles particles;
    public DummyAudioClipsDataBase sounds;

    [System.Serializable]
    public class DataBaseDummyParticles
    {
        public ParticleSystem _spawnParticules = null;
        public ParticleSystem myGroundParticle = null;
        public ParticleSystem greenblood = null;
        public ParticleSystem warningAttack = null;
    }

    [System.Serializable]
    public class DummyAudioClipsDataBase
    {
        public AudioClip _takeHit_AC;
        public AudioClip clip_walkEnt;
        public AudioClip entDeath_Clip;
        public AudioClip entAttack_Clip;
        public AudioClip entSpawn_clip;
    }

    [System.Serializable]
    public class DummySpecialAttack
    {
        public bool hasSpecialAttack;
        public bool isSpecialInCD;
        public CorruptedVine specialAttack_pf;
        public float _specialAttackCount_CD;
        public float specialAttack_CD;

        internal bool canDoSpecialAttack() => hasSpecialAttack && !isSpecialInCD;

        public void UpdateSpecialAttack()
        {
            if (hasSpecialAttack && isSpecialInCD)
            {
                _specialAttackCount_CD += Time.deltaTime;

                if (_specialAttackCount_CD >= specialAttack_CD)
                {
                    _specialAttackCount_CD = 0;
                    isSpecialInCD = false;
                }
            }
        }
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_SPAWN, new object[] { this });
        ParticlesManager.Instance.GetParticlePool(particles._spawnParticules.name, particles._spawnParticules, 5);
        ParticlesManager.Instance.GetParticlePool(particles.greenblood.name, particles.greenblood, 8);
        ParticlesManager.Instance.GetParticlePool(particles.warningAttack.name, particles.warningAttack, 2);

        AudioManager.instance.GetSoundPool(sounds._takeHit_AC.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds._takeHit_AC);
        AudioManager.instance.GetSoundPool(sounds.entDeath_Clip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds.entDeath_Clip);
        AudioManager.instance.GetSoundPool(sounds.entAttack_Clip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds.entAttack_Clip);
        AudioManager.instance.GetSoundPool(sounds.entSpawn_clip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds.entSpawn_clip);

        AudioManager.instance.GetSoundPool(sounds.clip_walkEnt.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds.clip_walkEnt, true);

        rb = GetComponent<Rigidbody>();
        combatComponent.Configure(AttackEntity);
        anim.Add_Callback("DealDamage", DealDamage);
        anim.Add_Callback("WarningAttack", WarningAttackParticle);
        movement.Configure(rootTransform, rb, groundSensor);

        StartDebug();

        Main.instance.AddEntity(this);

        IAInitialize(Main.instance.GetCombatDirector());
        myEffectReceiver = GetComponent<EffectReceiver>();
        
        //Hago el pool de las vines aca
        PoolManager.instance.GetObjectPool("CorruptedVines", dummySpecialAttack.specialAttack_pf);

        dmgReceiver.ChangeKnockback(movement.ApplyForceToVelocity, () => false);
    }
    protected override void OnReset()
    {
        if (death)
        {
            ragdoll.Ragdoll(false, Vector3.zero);
            death = false;
            cdModuleStopeable.EndCDWithoutExecute("Dead");
        }
        sm.SendInput(DummyEnemyInputs.DISABLE);
        cdModuleNonStopeable.ResetAll();
        cdModuleStopeable.ResetAll();
        particles.myGroundParticle.gameObject.SetActive(true);
    }
    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
            sm.SendInput(DummyEnemyInputs.IDLE);
    }
    public override void SpawnEnemy()
    {
        base.SpawnEnemy();
        ParticlesManager.Instance.PlayParticle(particles._spawnParticules.name, transform.position);
        AudioManager.instance.PlaySound(sounds.entSpawn_clip.name,transform);
    }
    protected override void OnUpdateEntity()
    {
        if (!death)
        {
            if (combatElement.Target != null && combatElement.Combat)
            {
                if (Vector3.Distance(combatElement.Target.position, transform.position) > combatDistance + 2)
                {
                    combatElement.ExitCombat();
                }
            }

            if (!combatElement.Combat && combatElement.Target == null)
            {
                if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) <= combatDistance)
                {
                    combatElement.EnterCombat(Main.instance.GetChar().transform);
                }
            }
        }

        if (!petrified)
        {
            sm?.Update();
            cdModuleStopeable.UpdateCD();
        }
        myEffectReceiver?.UpdateStates();
        movement.OnUpdate();
        cdModuleNonStopeable.UpdateCD();
        dummySpecialAttack.UpdateSpecialAttack();
    }
    Vector3 force;
    protected override void OnPause()
    {
        base.OnPause();
        force = rb.velocity; rb.isKinematic = true;
        particles.myGroundParticle.Pause();
        if (death) ragdoll.PauseRagdoll();
    }
    protected override void OnResume()
    {
        base.OnResume();
        rb.isKinematic = false;
        rb.velocity = force;
        particles.myGroundParticle.Play();
        if (death) ragdoll.ResumeRagdoll();
    }

    #region Attack
    public void DealDamage()
    {
        combatComponent.ManualTriggerAttack();
        sm.SendInput(DummyEnemyInputs.ATTACK);
        AudioManager.instance.PlaySound(sounds.entAttack_Clip.name,transform);
    }

    public void WarningAttackParticle()
    {
        ParticlesManager.Instance.PlayParticle(particles.warningAttack.name, warningAttack_pos.position);
    }

    public void AttackEntity(DamageReceiver e)
    {
        dmgData.SetDamage(damage).SetDamageTick(false).SetDamageType(Damagetype.Normal).SetKnockback(knockback)
    .SetPositionAndDirection(transform.position);
        Attack_Result takeDmg = e.TakeDamage(dmgData);

        if (takeDmg == Attack_Result.parried)
        {
            combatComponent.Stop();
            sm.SendInput(DummyEnemyInputs.PARRIED);
        }
    }
    #endregion

    #region Life Things
    protected override void TakeDamageFeedback(DamageData data)
    {
        AudioManager.instance.PlaySound(sounds._takeHit_AC.name,transform);

        sm.SendInput(DummyEnemyInputs.TAKE_DAMAGE);

        ParticlesManager.Instance.PlayParticle(particles.greenblood.name, transform.position + Vector3.up);
        cooldown = true;
        cdModuleNonStopeable.AddCD("TakeDamageCD", () => cooldown = false, recallTime);

        StartCoroutine(OnHitted(onHitFlashTime, onHitColor));
    }

    protected override void Die(Vector3 dir)
    {
        AudioManager.instance.PlaySound(sounds.entDeath_Clip.name,transform);
        groundSensor?.TurnOff();
        sm.SendInput(DummyEnemyInputs.DIE);
        if (dir == Vector3.zero)
            ragdoll.Ragdoll(true, -rootTransform.forward);
        else
            ragdoll.Ragdoll(true, dir);
        death = true;
        combatElement.ExitCombat();
        Main.instance.RemoveEntity(this);
        myEffectReceiver?.EndAllEffects();
        particles.myGroundParticle.gameObject.SetActive(false);
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
        sm.SendInput(DummyEnemyInputs.DISABLE);
        combatElement.ExitCombat();
        groundSensor?.TurnOff();
    }
    protected override void OnTurnOn()
    {
        sm.SendInput(DummyEnemyInputs.IDLE);
        groundSensor?.TurnOn();
    }

    #region STATE MACHINE THINGS
    public enum DummyEnemyInputs { AWAKE, IDLE, BEGIN_ATTACK,ATTACK, GO_TO_POS, DIE, DISABLE, TAKE_DAMAGE, PARRIED, CHASING, SPECIAL_ATTACK };
    void SetStates()
    {
        var idle = new EState<DummyEnemyInputs>("Idle");
        var goToPos = new EState<DummyEnemyInputs>("Follow");
        var chasing = new EState<DummyEnemyInputs>("Chasing");
        var beginAttack = new EState<DummyEnemyInputs>("Begin_Attack");
        var attack = new EState<DummyEnemyInputs>("Attack");
        var specialAttack = new EState<DummyEnemyInputs>("Special_Attack");
        var parried = new EState<DummyEnemyInputs>("Parried");
        var takeDamage = new EState<DummyEnemyInputs>("Take_Damage");
        var die = new EState<DummyEnemyInputs>("Die");
        var disable = new EState<DummyEnemyInputs>("Disable");

        ConfigureState.Create(idle)
            .SetTransition(DummyEnemyInputs.GO_TO_POS, goToPos)
            .SetTransition(DummyEnemyInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .SetTransition(DummyEnemyInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(goToPos)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .SetTransition(DummyEnemyInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(chasing)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.GO_TO_POS, goToPos)
            .SetTransition(DummyEnemyInputs.BEGIN_ATTACK, beginAttack)
            .SetTransition(DummyEnemyInputs.SPECIAL_ATTACK, specialAttack) //transición del ataque especial
            .SetTransition(DummyEnemyInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(beginAttack)
            .SetTransition(DummyEnemyInputs.ATTACK, attack)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PARRIED, parried)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(attack)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PARRIED, parried)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(specialAttack)  //Las transiciones del nuevo estado
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(parried)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .Done();

        ConfigureState.Create(die)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .Done();

        sm = new EventStateMachine<DummyEnemyInputs>(idle, DebugState);

        var head = Main.instance.GetChar();

        Func<bool> SpecialAttackReady = CanDoSpecialAttack;

        new DummyIdleState(idle, sm, movement, distancePos, normalDistance, combatElement).SetAnimator(animator).SetRoot(rootTransform).SetDirector(director);

        new DummyFollowState(goToPos, sm, movement, normalDistance, distancePos, combatElement, SpecialAttackReady).SetAnimator(animator).SetRoot(rootTransform);

        new DummyChasing(chasing, sm, () => combatElement.Attacking, distancePos, movement, combatElement, SpecialAttackReady).SetDirector(director).SetRoot(rootTransform);

        new DummyAttAnt(beginAttack, sm, movement, combatElement).SetAnimator(animator).SetDirector(director).SetRoot(rootTransform);

        new DummyAttackState(attack, sm, cdToAttack, combatElement).SetAnimator(animator).SetDirector(director).SetCD(cdModuleStopeable);

        new Tools.StateMachine.DummySpecialAttack(this, specialAttack, sm, combatElement).SetDirector(director);

        new DummyParried(parried, sm, parriedTime, combatElement).SetAnimator(animator).SetDirector(director).SetCD(cdModuleStopeable);

        new DummyTDState(takeDamage, sm, recallTime).SetAnimator(animator).SetCD(cdModuleStopeable);

        new DummyDieState<DummyEnemyInputs>(die, sm, ragdoll, ()=> { }, ReturnToSpawner, cdModuleStopeable);

        new DummyDisableState<DummyEnemyInputs>(disable, sm, EnableObject, DisableObject);
    }

    bool CanDoSpecialAttack()
    {
        var character = Main.instance.GetChar();

        bool aux = false;
        if (dummySpecialAttack.canDoSpecialAttack() && Vector3.Distance(character.transform.position, transform.position) <= 10 &&
            Vector3.Distance(character.transform.position, transform.position) >= 4 && character.Slowed == false)
        {
            //isSpecialInCD = true;
            aux = true;
            return aux;
        }

        return aux;
    }

    void DisableObject()
    {
        canupdate = false;
        movement.SetDefaultSpeed();
        combatElement.Combat = false;
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
        else {
            if (txt_debug != null)
                txt_debug.enabled = val;
        }
    }
    void StartDebug() { if (txt_debug != null) txt_debug.enabled = DevelopToolsCenter.instance.EnemyDebuggingIsActive(); }// inicializacion
    #endregion    
}
