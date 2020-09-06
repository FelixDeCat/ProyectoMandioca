using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;
using UnityEngine.Serialization;

public class CrowEnemy : EnemyBase
{
    public AnimationCurve animEmisive;

    [SerializeField] float rotationSpeed = 8;

    [Header("Combat Options")]
    [SerializeField] Throwable throwObject = null;
    [SerializeField] Transform shootPivot = null;
    [SerializeField] float throwForce = 6;
    [SerializeField] float cdToCast = 2;
    [SerializeField] int damage = 2;
    [SerializeField] float knockback = 20;
    [SerializeField] float attackRecall = 2;
    private CombatDirector director;
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
    [SerializeField] Animator animator = null;
    private Material[] myMat;
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
        ParticlesManager.Instance.GetParticlePool(particles.takeDmg.name, particles.takeDmg, 8);

        var smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
            myMat = smr.materials;

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
        sm.SendInput(CrowInputs.DISABLE);
    }
    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
            sm.SendInput(CrowInputs.IDLE);

        //director.AddNewTarget(this);

        canupdate = true;

        timerToCast = cdToCast;
    }
    protected override void OnUpdateEntity()
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

                    animator.SetBool("rotate", false);

                    if (castPartTemp != null)
                        ParticlesManager.Instance.StopParticle(castPartTemp.name, castPartTemp);

                    sm.SendInput(CrowInputs.IDLE);
                }
            }

            if (!combat && entityTarget == null)
            {
                if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) <= combatDistance)
                {
                    director.AddToList(this, Main.instance.GetChar());
                    SetTarget(Main.instance.GetChar());
                    combat = true;

                    animator.SetBool("rotate", true);
                }
            }

            if (!stopCD)
            {
                if (cdToCast > timerToCast) timerToCast += Time.deltaTime;
                else
                {
                    timerToCast = 0;
                    stopCD = true;
                }
            }
        }

        if (sm != null)
            sm.Update();

        if (cooldown)
        {
            if (timercooldown < recallTime) timercooldown = timercooldown + 1 * Time.deltaTime;
            else { cooldown = false; timercooldown = 0; }
        }
    }

    protected override void OnPause() { }
    protected override void OnResume() { }

    #region Attack
    void CastOver()
    {
        animator.SetTrigger("attack");
        animator.SetBool("rotate", false);
        castPartTemp = null;
        castingOver = true;
    }

    public void DealDamage()
    {
        AudioManager.instance.PlaySound(sounds.attackSound.name);

        Vector3 dir = CurrentTarget() ? (CurrentTarget().transform.position - shootPivot.position).normalized : Vector3.zero;

        ThrowData newData = new ThrowData().Configure(shootPivot.position, dir, throwForce, damage, rootTransform);

        ThrowablePoolsManager.instance.Throw(throwObject.name, newData);

        stopCD = false;
    }

    public override void ToAttack() => attacking = true;
    #endregion

    #region Life Things
    public GenericLifeSystem Life() => lifesystem;

    protected override void TakeDamageFeedback(DamageData data)
    {
        if (sm.Current.Name == "Idle")
        {
            attacking = false;
            director.ChangeTarget(this, data.owner, entityTarget);
        }

        AudioManager.instance.PlaySound(sounds.takeDmgSound.name);

        sm.SendInput(CrowInputs.TAKE_DAMAGE);

        ParticlesManager.Instance.PlayParticle(particles.takeDmg.name, transform.position + Vector3.up);
        cooldown = true;

        StartCoroutine(OnHitted(myMat, onHitFlashTime, onHitColor));
    }

    protected override void Die(Vector3 dir)
    {
        sm.SendInput(CrowInputs.DIE);
        if (dir == Vector3.zero)
            ragdoll.Ragdoll(true, -rootTransform.forward);
        else
            ragdoll.Ragdoll(true, dir);
        death = true;
        director.DeadEntity(this, entityTarget);
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
        if (sm.Current.Name == "Die") gameObject.SetActive(false);

        sm.SendInput(CrowInputs.DISABLE);
        if (combat)
        {
            director.DeadEntity(this, entityTarget);
            entityTarget = null;
            combat = false;
        }
    }
    protected override void OnTurnOn()
    {
        sm.SendInput(CrowInputs.IDLE);
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
            .SetTransition(CrowInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(CrowInputs.DIE, die)
            .SetTransition(CrowInputs.PETRIFIED, petrified)
            .SetTransition(CrowInputs.DISABLE, disable)
            .SetTransition(CrowInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(chasing)
            .SetTransition(CrowInputs.IDLE, idle)
            .SetTransition(CrowInputs.BEGIN_ATTACK, attack)
            .SetTransition(CrowInputs.TAKE_DAMAGE, takeDamage)
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
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(CrowInputs.IDLE, idle)
            .Done();

        sm = new EventStateMachine<CrowInputs>(idle, null);

        var head = Main.instance.GetChar();

        new CrowIdle(idle, sm, distancePos, rotationSpeed, this, lineOfSight.OnSight, () => stopCD).SetAnimator(animator).SetRoot(rootTransform).SetDirector(director);

        new CrowChasing(chasing, sm, IsAttack, distancePos, rotationSpeed, this, lineOfSight.OnSight).SetDirector(director).SetRoot(rootTransform);

        new CrowAttack(attack, sm, attackRecall, () => castPartTemp = ParticlesManager.Instance.PlayParticle(particles.castParticles.name, shootPivot.position, CastOver, shootPivot),
            () => castingOver, (x) => castingOver = x, this, rotationSpeed).SetAnimator(animator).SetDirector(director).SetRoot(rootTransform);

        new CrowTakeDmg(takeDamage, sm, recallTime).SetAnimator(animator);

        new DummyStunState<CrowInputs>(petrified, sm);

        new CrowDead(die, sm, ragdoll).SetAnimator(animator).SetDirector(director).SetRigidbody(rb);

        new DummyDisableState<CrowInputs>(disable, sm, EnableObject, DisableObject);
    }

    void DisableObject()
    {
        canupdate = false;
        combat = false;
    }

    void EnableObject() => Initialize();

    #endregion
}
