using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;
using UnityEngine.Serialization;

public class CrowEnemy : EnemyWithCombatDirector
{
    [SerializeField] bool showGizmoCombatDistance = false;
    [SerializeField] bool showGizmoDistancePos = false;
    public AnimationCurve animEmisive;


    [SerializeField] float rotationSpeed = 8;

    [Header("Combat Options")]
    [SerializeField] Throwable throwObject = null;
    [SerializeField] Transform shootPivot = null;
    [SerializeField] float throwForce = 6;
    [SerializeField] float cdToCast = 2;
    [SerializeField] int damage = 2;
    [SerializeField] float attackRecall = 2;
    [SerializeField] LineOfSight lineOfSight = null;
    CDModule cdModule = new CDModule();

    [Header("Life Options")]
    [SerializeField] float recallTime = 1;

    private bool cooldown = false;
    bool stopCD;
    bool castingOver;

    [Header("Feedback")]
    [SerializeField] AnimEvent anim = null;
    [SerializeField] Color onHitColor = Color.white;
    [SerializeField] float onHitFlashTime = 0.1f;
    [SerializeField] RagdollComponent ragdoll = null;
    ParticleSystem castPartTemp;

    [SerializeField] EffectBase petrifyEffect = null;
    EventStateMachine<CrowInputs> sm;

    public DataBaseCrowParticles particles;
    public DataBaseCrowSounds sounds;

    [System.Serializable]
    public class DataBaseCrowParticles
    {
        public ParticleSystem castParticles = null;
        public ParticleSystem takeDmg = null;
        public ParticleSystem attackParticles = null;
    }

    [System.Serializable]
    public class DataBaseCrowSounds
    {
        public AudioClip takeDmgSound;
        public AudioClip attackSound;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_SPAWN, new object[] { this });
        ParticlesManager.Instance.GetParticlePool(particles.castParticles.name, particles.castParticles, 2);
        ParticlesManager.Instance.GetParticlePool(particles.attackParticles.name, particles.attackParticles, 2);
        ParticlesManager.Instance.GetParticlePool(particles.takeDmg.name, particles.takeDmg, 3);

        AudioManager.instance.GetSoundPool(sounds.takeDmgSound.name, AudioGroups.GAME_FX, sounds.takeDmgSound);
        AudioManager.instance.GetSoundPool(sounds.attackSound.name, AudioGroups.GAME_FX, sounds.attackSound);

        rb = GetComponent<Rigidbody>();
        anim.Add_Callback("DealDamage", DealDamage);
        Main.instance.AddEntity(this);

        IAInitialize(Main.instance.GetCombatDirector());

        petrifyEffect?.AddStartCallback(() => sm.SendInput(CrowInputs.PETRIFIED));
        petrifyEffect?.AddEndCallback(() => sm.SendInput(CrowInputs.IDLE));

        ThrowablePoolsManager.instance.CreateAPool(throwObject.name, throwObject);

        lineOfSight.Configurate(rootTransform);
    }
    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
            sm.SendInput(CrowInputs.IDLE);

        stopCD = true;
    }
    protected override void OnUpdateEntity()
    {
        if (!death)
        {
            if (combatElement.Combat)
            {
                if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) > combatDistance + 2)
                {
                    combatElement.ExitCombat();

                    animator.SetBool("rotate", false);

                    if (castPartTemp != null)
                        ParticlesManager.Instance.StopParticle(particles.castParticles.name, castPartTemp);

                    sm.SendInput(CrowInputs.IDLE);
                }
            }

            if (!combatElement.Combat && combatElement.Target == null)
            {
                if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) <= combatDistance)
                {
                    combatElement.EnterCombat();

                    animator.SetBool("rotate", true);
                }
            }
        }

        if (sm != null)
            sm.Update();

        cdModule.UpdateCD();
    }
    protected override void OnPause()
    {
        base.OnPause();
        if (death) ragdoll.PauseRagdoll();
    }
    protected override void OnResume()
    {
        base.OnResume();
        if (death) ragdoll.ResumeRagdoll();
    }

    protected override void OnReset()
    {
        if (castPartTemp != null)
            ParticlesManager.Instance.StopParticle(particles.castParticles.name, castPartTemp);
        cdModule.ResetAll();
        ragdoll.Ragdoll(false, Vector3.zero);
        death = false;
        sm.SendInput(CrowInputs.DISABLE);
    }

    #region Attack
    void CastOver()
    {
        animator.SetTrigger("attack");
        animator.SetBool("rotate", false);
        castPartTemp = null;
        cdModule.AddCD("AttackRecall", () => sm.SendInput(CrowInputs.IDLE), attackRecall);
        dir = combatElement.CurrentTarget() ? (combatElement.CurrentTarget().transform.position + new Vector3(0, 1, 0) - shootPivot.position).normalized : Vector3.down;
        ParticlesManager.Instance.PlayParticle(particles.attackParticles.name, shootPivot.position);
    }

    Vector3 dir;
    public void DealDamage()
    {
        AudioManager.instance.PlaySound(sounds.attackSound.name);

        ThrowData newData = new ThrowData().Configure(shootPivot.position, dir, throwForce, damage, rootTransform);

        ThrowablePoolsManager.instance.Throw(throwObject.name, newData);
    }
    #endregion

    #region Life Things
    public GenericLifeSystem Life() => lifesystem;

    protected override void TakeDamageFeedback(DamageData data)
    {
        AudioManager.instance.PlaySound(sounds.takeDmgSound.name);

        sm.SendInput(CrowInputs.TAKE_DAMAGE);

        ParticlesManager.Instance.PlayParticle(particles.takeDmg.name, transform.position + Vector3.up);
        cooldown = true;
        cdModule.AddCD("takeDamageCooldown", () => cooldown = false, recallTime);

        StartCoroutine(OnHitted(onHitFlashTime, onHitColor));
    }

    protected override void Die(Vector3 dir)
    {
        sm.SendInput(CrowInputs.DIE);
        if (dir == Vector3.zero)
            ragdoll.Ragdoll(true, -rootTransform.forward);
        else
            ragdoll.Ragdoll(true, dir);

        if (castPartTemp != null)
            ParticlesManager.Instance.StopParticle(particles.castParticles.name, castPartTemp);
        else
        death = true;
        combatElement.ExitCombat();
        Main.instance.RemoveEntity(this);
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
        cdModule.ResetAll();
        sm.SendInput(CrowInputs.DISABLE);
        combatElement.ExitCombat();
    }
    protected override void OnTurnOn()
    {
        sm.SendInput(CrowInputs.IDLE);
    }

    public void OnDrawGizmos()
    {
        if (showGizmoDistancePos)
            Gizmos.DrawWireSphere(transform.position, distancePos);

        if (showGizmoCombatDistance)
            Gizmos.DrawWireSphere(transform.position, combatDistance);
    }

    #region STATE MACHINE THINGS
    public enum CrowInputs { IDLE, BEGIN_ATTACK, DIE, DISABLE, TAKE_DAMAGE, PETRIFIED, CHASING };
    void SetStates()
    {
        var idle = new EState<CrowInputs>("Idle");
        var chasing = new EState<CrowInputs>("Chasing");
        var attack = new EState<CrowInputs>("Begin_Attack");
        var takeDamage = new EState<CrowInputs>("Take_Damage");
        var die = new EState<CrowInputs>("Die");
        var disable = new EState<CrowInputs>("Disable");
        var petrified = new EState<CrowInputs>("Petrified");

        ConfigureState.Create(idle)
            .SetTransition(CrowInputs.DIE, die)
            .SetTransition(CrowInputs.PETRIFIED, petrified)
            .SetTransition(CrowInputs.DISABLE, disable)
            .SetTransition(CrowInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(chasing)
            .SetTransition(CrowInputs.IDLE, idle)
            .SetTransition(CrowInputs.BEGIN_ATTACK, attack)
            .SetTransition(CrowInputs.DIE, die)
            .SetTransition(CrowInputs.PETRIFIED, petrified)
            .SetTransition(CrowInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(attack)
            .SetTransition(CrowInputs.IDLE, idle)
            .SetTransition(CrowInputs.DIE, die)
            .SetTransition(CrowInputs.PETRIFIED, petrified)
            .SetTransition(CrowInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(petrified)
            .SetTransition(CrowInputs.IDLE, idle)
            .SetTransition(CrowInputs.BEGIN_ATTACK, attack)
            .SetTransition(CrowInputs.DIE, die)
            .SetTransition(CrowInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(CrowInputs.IDLE, idle)
            .SetTransition(CrowInputs.DISABLE, disable)
            .SetTransition(CrowInputs.PETRIFIED, petrified)
            .SetTransition(CrowInputs.DIE, die)
            .Done();

        ConfigureState.Create(die)
            .SetTransition(CrowInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(CrowInputs.IDLE, idle)
            .Done();

        sm = new EventStateMachine<CrowInputs>(idle, null);

        var head = Main.instance.GetChar();

        new CrowIdle(idle, sm, distancePos, rotationSpeed, combatElement, lineOfSight.OnSight, () => stopCD).SetAnimator(animator).SetRoot(rootTransform).SetDirector(director);

        new CrowChasing(chasing, sm, () => combatElement.Attacking, distancePos, rotationSpeed, combatElement, lineOfSight.OnSight).SetDirector(director).SetRoot(rootTransform);

        new CrowAttack(attack, sm, () => castPartTemp = ParticlesManager.Instance.PlayParticle(particles.castParticles.name, shootPivot.position, CastOver, shootPivot),
            () => castingOver, (x) => { castingOver = x; cdModule.AddCD("canAttack", () => stopCD = true, cdToCast);  stopCD = x; }, combatElement, rotationSpeed)
            .SetAnimator(animator).SetDirector(director).SetRoot(rootTransform).SetCD(cdModule);

        new CrowTakeDmg(takeDamage, sm, recallTime).SetAnimator(animator);

        new DummyStunState<CrowInputs>(petrified, sm);

        new CrowDead(die, sm, ragdoll, ReturnToSpawner).SetAnimator(animator).SetDirector(director).SetRigidbody(rb).SetCD(cdModule);

        new DummyDisableState<CrowInputs>(disable, sm, EnableObject, DisableObject);
    }

    void DisableObject()
    {
        canupdate = false;
        combatElement.Combat = false;
    }

    void EnableObject() => Initialize();

    #endregion
}
