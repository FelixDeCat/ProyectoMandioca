using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

public class JabaliEnemy : EnemyWithCombatDirector
{
    [Header("Move Options")]
    [SerializeField] GenericEnemyMove movement = null;
    [SerializeField] LineOfSight lineOfSight = null;
    [SerializeField] CharacterGroundSensor groundSensor = null;

    public AnimationCurve animEmisive;

    [Header("Combat Options")]
    [SerializeField] CombatComponent headAttack = null;
    [SerializeField] JabaliPushComponent pushAttack = null;
    [SerializeField] float normalDistance = 9;
    [SerializeField] float timeParried = 2;
    [SerializeField] bool friendly = false;

    [Header("NormalAttack")]
    [SerializeField] int normalDamage = 4;
    [SerializeField] float normalAttAnticipation = 0.5f;
    [SerializeField] float cdToHeadAttack = 1;
    [SerializeField] float normalKockback = 30;

    [Header("PushAttack")]
    [SerializeField] int pushDamage = 8;
    [SerializeField] float distanceToCharge = 7;
    [SerializeField] float chargeTime = 2;
    [SerializeField] float stunChargeTime = 2;
    [SerializeField] float timeToObtainCharge = 5;
    [SerializeField] float chargeSpeed = 12;
    [SerializeField] float chargeDuration = 5;
    [SerializeField] float pushKnockback = 80;
    [SerializeField] bool unequipShield = true;
    private bool chargeOk = false;
    private float cargeTimer;

    [Header("Life Options")]
    [SerializeField] float tdRecall = 0.5f;

    [Header("Stuns/Effects")]
    private bool cooldown = false;
    private float timercooldown = 0;
    float stunTimer;
    private Action<EState<JabaliInputs>> EnterStun;
    private Action<string> UpdateStun;
    private Action<JabaliInputs> ExitStun;

    [Header("Feedback")]
    [SerializeField] AnimEvent anim = null;
    [SerializeField] RagdollComponent ragdoll = null;
    [SerializeField] Color onHitColor = Color.white;
    [SerializeField] float onHitFlashTime = 0.1f;
    [SerializeField] EffectBase petrifyEffect = null;
    Material[] myMat;
    [SerializeField] GoatParticles particles = new GoatParticles();
    [SerializeField] GoatSounds sounds = new GoatSounds();

    [Serializable] public class GoatParticles
    {
        public ParticleSystem hitParticle;
        public GameObject chargeFeedback;
    }

    [Serializable] public class GoatSounds
    {
        public AudioClip takeDamage;
        public AudioClip pushAnticipation;
        public AudioClip pushEnter;
        public AudioClip pushLoop;
        public AudioClip headAttack;
        public AudioClip dead;
    }
    EventStateMachine<JabaliInputs> sm;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        var smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
            myMat = smr.materials;

        headAttack.Configure(HeadAttack);
        pushAttack.Configure(PushRelease, StunAfterCharge);
        lineOfSight.Configurate(rootTransform);
        AudioManager.instance.GetSoundPool(sounds.dead.name, AudioGroups.JABALI, sounds.dead);
        AudioManager.instance.GetSoundPool(sounds.headAttack.name, AudioGroups.JABALI, sounds.headAttack);
        AudioManager.instance.GetSoundPool(sounds.pushAnticipation.name, AudioGroups.JABALI, sounds.pushAnticipation, true);
        AudioManager.instance.GetSoundPool(sounds.pushEnter.name, AudioGroups.JABALI, sounds.pushEnter);
        AudioManager.instance.GetSoundPool(sounds.pushLoop.name, AudioGroups.JABALI, sounds.pushLoop, true);
        AudioManager.instance.GetSoundPool(sounds.takeDamage.name, AudioGroups.JABALI, sounds.takeDamage);
        ParticlesManager.Instance.GetParticlePool(particles.hitParticle.name, particles.hitParticle);
        movement.Configure(rootTransform, rb, groundSensor);

        anim.Add_Callback("DealDamage", DealDamage);
        Main.instance.AddEntity(this);

        IAInitialize(Main.instance.GetCombatDirector());

        petrifyEffect?.AddStartCallback(() => sm.SendInput(JabaliInputs.PETRIFIED));
        petrifyEffect?.AddEndCallback(() => sm.SendInput(JabaliInputs.IDLE));
    }

    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
            sm.SendInput(JabaliInputs.IDLE);
    }

    protected override void OnReset()
    {
        ragdoll.Ragdoll(false, Vector3.zero);
        death = false;
        sm.SendInput(JabaliInputs.DISABLE);
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
                }
            }

            if (!combatElement.Combat && combatElement.Target == null && !friendly)
            {
                if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) <= combatDistance)
                {
                    combatElement.EnterCombat();
                }
            }
        }

        if (sm != null)
            sm.Update();

        if (cooldown)
        {
            if (timercooldown < tdRecall) timercooldown = timercooldown + 1 * Time.deltaTime;
            else { cooldown = false; timercooldown = 0; }
        }

        if (!chargeOk)
        {
            cargeTimer += Time.deltaTime;

            if (cargeTimer >= timeToObtainCharge)
            {
                chargeOk = true;
                cargeTimer = 0;
            }
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

    protected override void OnFixedUpdate() { }
    protected override void OnTurnOff()
    {
        if (sm.Current.Name == "Die") gameObject.SetActive(false);

        sm.SendInput(JabaliInputs.DISABLE);
        combatElement.ExitCombat();
        groundSensor?.TurnOff();
    }
    protected override void OnTurnOn()
    {
        sm.SendInput(JabaliInputs.IDLE);
        groundSensor?.TurnOn();
    }

    #region Attack Things
    public void HeadAttack(DamageReceiver e)
    {
        dmgData.SetDamage(normalDamage).SetDamageTick(false).SetDamageType(Damagetype.Normal).SetKnockback(normalKockback)
            .SetPositionAndDirection(transform.position);
        Attack_Result takeDmg = e.TakeDamage(dmgData);

        if (takeDmg == Attack_Result.parried)
            sm.SendInput(JabaliInputs.PARRIED);
    }

    void PushRelease(DamageReceiver e)
    {
        dmgData.SetDamage(pushDamage).SetDamageTick(false).SetDamageType(Damagetype.StealShield).SetKnockback(pushKnockback)
            .SetPositionAndDirection(transform.position, transform.forward);
        Attack_Result takeDmg = e.TakeDamage(dmgData);


        if (takeDmg == Attack_Result.parried && unequipShield || takeDmg == Attack_Result.blocked && unequipShield) e.GetComponent<CharacterHead>().UnequipShield((e.transform.position - transform.position).normalized);

        if (e.GetComponent<CharacterHead>())
            pushAttack.Stop();
    }

    void PushAttack() => pushAttack.ManualTriggerAttack();

    public void DealDamage() => headAttack.ManualTriggerAttack();

    #endregion

    #region TakeDamage Things
    protected override void TakeDamageFeedback(DamageData data)
    {
        if (friendly && !combatElement.Combat)
        {
            ToCombat();

            var overlap = Physics.OverlapSphere(data.owner.transform.position, combatDistance);

            foreach (var item in overlap)
            {
                if (item.GetComponent<JabaliEnemy>() && item.GetComponent<JabaliEnemy>().friendly) item.GetComponent<JabaliEnemy>().ToCombat();
            }
        }

        AudioManager.instance.PlaySound(sounds.takeDamage.name, rootTransform);

        sm.SendInput(JabaliInputs.TAKE_DMG);

        ParticlesManager.Instance.PlayParticle(particles.hitParticle.name, transform.position);
        cooldown = true;

        StartCoroutine(OnHitted(myMat, onHitFlashTime, onHitColor));
    }

    public void ToCombat()
    {
        combatElement.EnterCombat();
        friendly = false;
    }

    protected override void Die(Vector3 dir)
    {
        death = true;
        combatElement.ExitCombat();
        groundSensor?.TurnOff();
        sm.SendInput(JabaliInputs.DEAD);
        if (dir == Vector3.zero)
            ragdoll.Ragdoll(true, -rootTransform.forward);
        else
            ragdoll.Ragdoll(true, dir);
        Main.instance.RemoveEntity(this);
    }

    protected override bool IsDamage()
    {
        if (cooldown || sm.Current.Name == "Dead" || sm.Current.Name == "Push") return true;
        else return false;
    }

    #endregion

    #region Effects Things

    void StunAfterCharge()
    {
        EnterStun += (input) => {
            animator.SetBool("Stun", true);
        };

        UpdateStun = (name) => {
            stunTimer += Time.deltaTime;

            if (stunTimer >= stunChargeTime)
                sm.SendInput(JabaliInputs.IDLE);
        };

        ExitStun += (input) => {
            animator.SetBool("Stun", false);
            stunTimer = 0;
            chargeOk = false;
        };

        sm.SendInput(JabaliInputs.PETRIFIED);
    }

    #endregion

    #region SET STATES

    public enum JabaliInputs { IDLE, PERSUIT, CHARGE_PUSH, PUSH, HEAD_ANTICIP, HEAD_ATTACK, TAKE_DMG, PARRIED, PETRIFIED, DEAD, DISABLE, CHASING }

    void SetStates()
    {
        var idle = new EState<JabaliInputs>("Idle");
        var chasing = new EState<JabaliInputs>("Chasing");
        var persuit = new EState<JabaliInputs>("Persuit");
        var chargePush = new EState<JabaliInputs>("Charge_Push");
        var push = new EState<JabaliInputs>("Push");
        var headAnt = new EState<JabaliInputs>("Head_Anticipation");
        var headAttack = new EState<JabaliInputs>("Head_Attack");
        var takeDamage = new EState<JabaliInputs>("Take_DMG");
        var parried = new EState<JabaliInputs>("Parried");
        var petrified = new EState<JabaliInputs>("Petrified");
        var dead = new EState<JabaliInputs>("Dead");
        var disable = new EState<JabaliInputs>("Disable");

        ConfigureState.Create(idle)
            .SetTransition(JabaliInputs.PERSUIT, persuit)
            .SetTransition(JabaliInputs.TAKE_DMG, takeDamage)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .SetTransition(JabaliInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(persuit)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.TAKE_DMG, takeDamage)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .SetTransition(JabaliInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(chasing)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.PERSUIT, persuit)
            .SetTransition(JabaliInputs.TAKE_DMG, takeDamage)
            .SetTransition(JabaliInputs.CHARGE_PUSH, chargePush)
            .SetTransition(JabaliInputs.HEAD_ANTICIP, headAnt)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(chargePush)
            .SetTransition(JabaliInputs.PUSH, push)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(push)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.PARRIED, parried)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(headAnt)
            .SetTransition(JabaliInputs.HEAD_ATTACK, headAttack)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(headAttack)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.PARRIED, parried)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(parried)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.PETRIFIED, petrified)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(petrified)
            .SetTransition(JabaliInputs.IDLE, idle)
            .SetTransition(JabaliInputs.CHARGE_PUSH, chargePush)
            .SetTransition(JabaliInputs.PUSH, push)
            .SetTransition(JabaliInputs.HEAD_ANTICIP, headAnt)
            .SetTransition(JabaliInputs.HEAD_ATTACK, headAttack)
            .SetTransition(JabaliInputs.DEAD, dead)
            .SetTransition(JabaliInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(dead)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(JabaliInputs.IDLE, idle)
            .Done();

        sm = new EventStateMachine<JabaliInputs>(idle);

        new JabaliIdle(idle, sm, movement, distanceToCharge, distancePos, normalDistance, IsChargeOk).SetThis(combatElement).SetDirector(director)
            .SetRoot(rootTransform);

        new JabaliPersuit(persuit, sm, movement, lineOfSight.OnSight, IsChargeOk, normalDistance, distancePos, distanceToCharge).SetAnimator(animator)
            .SetThis(combatElement).SetRoot(rootTransform);

        new JabaliChasing(chasing, sm, ()=>combatElement.Attacking, IsChargeOk, distanceToCharge, distancePos, movement).SetDirector(director).SetThis(combatElement).SetRoot(rootTransform);

        new JabaliCharge(chargePush, sm, chargeTime, sounds.pushAnticipation.name, sounds.pushEnter.name, movement).SetAnimator(animator).SetDirector(director)
            .SetThis(combatElement).SetRigidbody(rb).SetRoot(rootTransform);

        new JabaliPushAttack(push, sm, chargeSpeed, PushAttack, particles.chargeFeedback, pushAttack.Play, sounds.pushLoop.name, chargeDuration, groundSensor)
            .SetAnimator(animator).SetRigidbody(rb).SetRoot(rootTransform)
            .SetThis(combatElement).SetDirector(director);

        new JabaliAttackAnticipation(headAnt, sm, movement, normalAttAnticipation).SetAnimator(animator).SetDirector(director).SetThis(combatElement).SetRoot(rootTransform);

        new JabaliHeadAttack(headAttack, sm, cdToHeadAttack, sounds.headAttack.name).SetAnimator(animator).SetDirector(director).SetThis(combatElement);

        new JabaliTD(takeDamage, sm, tdRecall).SetAnimator(animator);

        new JabaliParried(parried, sm, timeParried).SetAnimator(animator);

        new JabaliStun(petrified, sm, StartStun, TickStun, EndStun);

        new JabaliDeath(dead, sm, ragdoll, sounds.dead.name).SetThis(combatElement).SetDirector(director);

        disable.OnEnter += (input) => DisableObject();

        disable.OnExit += (input) => EnableObject();
    }

    bool IsChargeOk() => chargeOk;

    void StartStun(EState<JabaliInputs> input) => EnterStun?.Invoke(input);

    void TickStun(string name) => UpdateStun?.Invoke(name);

    void EndStun(JabaliInputs input) { ExitStun?.Invoke(input); EnterStun = (x) => { }; ExitStun = (x) => { }; } 

    void DisableObject()
    {
        canupdate = false;
        movement.SetDefaultSpeed();
        combatElement.Combat = false;
    }

    void EnableObject() => Initialize();

    #endregion
}
