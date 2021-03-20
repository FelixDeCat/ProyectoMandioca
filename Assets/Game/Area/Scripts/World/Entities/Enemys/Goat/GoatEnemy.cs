using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.StateMachine;
using System;

using GoatInputs = JabaliEnemy.JabaliInputs;

public enum GoatType { UnequipSword, Stomp, DoubleAttack }

public class GoatEnemy : EnemyWithCombatDirector
{
    [Header("Move Options")]
    [SerializeField] GenericEnemyMove movement = null;
    [SerializeField] LineOfSight lineOfSight = null;
    [SerializeField] CharacterGroundSensor groundSensor = null;
    
    [Header("Combat Options")]
    [SerializeField] CombatComponent headAttack = null;
    [SerializeField] JabaliPushComponent pushAttack = null;
    [SerializeField] GoatStompComponent stompAttack = null;
    [SerializeField] float normalDistance = 9;
    [SerializeField] float timeParried = 2;
    CDModule cdModuleStopeable = new CDModule();
    CDModule cdModuleNoStopeable = new CDModule();

    [Header("NormalAttack")]
    [SerializeField] GoatType goatType = GoatType.UnequipSword;
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
    private bool chargeOk = true;

    [Header("Life Options")]
    [SerializeField] float tdRecall = 0.5f;

    [Header("Stuns/Effects")]
    private bool cooldown = false;
    private Action<EState<GoatInputs>> EnterStun;
    private Action<string> UpdateStun;
    private Action<GoatInputs> ExitStun;
    EffectReceiver myEffectReceiver;

    [Header("Feedback")]
    [SerializeField] AnimEvent anim = null;
    [SerializeField] RagdollComponent ragdoll = null;
    [SerializeField] Color onHitColor = Color.white;
    [SerializeField] float onHitFlashTime = 0.1f;
    [SerializeField] EffectBase petrifyEffect = null;
    [SerializeField] GoatParticles particles = new GoatParticles();
    [SerializeField] GoatSounds sounds = new GoatSounds();

    [Serializable]
    public class GoatParticles
    {
        public ParticleSystem hitParticle;
        public GameObject chargeFeedback;
        public ParticleSystem stompParticle;
    }

    [Serializable]
    public class GoatSounds
    {
        public AudioClip takeDamage;
        public AudioClip pushAnticipation;
        public AudioClip pushEnter;
        public AudioClip pushLoop;
        public AudioClip headAttack;
        public AudioClip dead;
    }
    EventStateMachine<GoatInputs> sm;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        headAttack.Configure(HeadAttack);
        pushAttack.Configure(PushRelease, StunAfterCharge);
        stompAttack.Configure(StompRelease);
        lineOfSight.Configurate(rootTransform);
        AudioManager.instance.GetSoundPool(sounds.dead.name, AudioGroups.JABALI, sounds.dead);
        AudioManager.instance.GetSoundPool(sounds.headAttack.name, AudioGroups.JABALI, sounds.headAttack);
        AudioManager.instance.GetSoundPool(sounds.pushAnticipation.name, AudioGroups.JABALI, sounds.pushAnticipation, true);
        AudioManager.instance.GetSoundPool(sounds.pushEnter.name, AudioGroups.JABALI, sounds.pushEnter);
        AudioManager.instance.GetSoundPool(sounds.pushLoop.name, AudioGroups.JABALI, sounds.pushLoop, true);
        AudioManager.instance.GetSoundPool(sounds.takeDamage.name, AudioGroups.JABALI, sounds.takeDamage);

        ParticlesManager.Instance.GetParticlePool(particles.hitParticle.name, particles.hitParticle);
        ParticlesManager.Instance.GetParticlePool(particles.stompParticle.name, particles.stompParticle);
        movement.Configure(rootTransform, rb, groundSensor);

        anim.Add_Callback("DealDamage", DealDamage);
        anim.Add_Callback("StompDamage", StompAttack);
        Main.instance.AddEntity(this);

        IAInitialize(Main.instance.GetCombatDirector());

        dmgReceiver.ChangeKnockback(movement.ApplyForceToVelocity, () => sm.Current.Name == "Charge_Push" || sm.Current.Name == "Push" ? true : false);

        myEffectReceiver = GetComponent<EffectReceiver>();
    }

    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
            sm.SendInput(GoatInputs.IDLE);
    }

    protected override void OnReset()
    {
        if (death)
        {
            ragdoll.Ragdoll(false, Vector3.zero);
            death = false;
        }
        cdModuleStopeable.ResetAll();
        cdModuleNoStopeable.ResetAll();
        sm.SendInput(GoatInputs.DISABLE);
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
        cdModuleNoStopeable.UpdateCD();
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
        sm.SendInput(GoatInputs.DISABLE);
        combatElement.ExitCombat();
        groundSensor?.TurnOff();
    }
    protected override void OnTurnOn()
    {
        sm.SendInput(GoatInputs.IDLE);
        groundSensor?.TurnOn();
    }

    #region Attack Things
    public void HeadAttack(DamageReceiver e)
    {
        dmgData.SetDamage(normalDamage).SetDamageTick(false).SetDamageType(Damagetype.Normal).SetKnockback(normalKockback).SetDamageInfo(DamageInfo.Normal)
            .SetPositionAndDirection(transform.position);
        Attack_Result takeDmg = e.TakeDamage(dmgData);

        if (takeDmg == Attack_Result.parried)
        {
            sm.SendInput(GoatInputs.PARRIED);
            return;
        }

        //if (goatType == GoatType.UnequipSword) e.GetComponent<CharacterHead>()?.UnequipSword(transform.forward);
    }

    void StompRelease(DamageReceiver e)
    {
        dmgData.SetDamage(normalDamage).SetDamageTick(false).SetDamageType(Damagetype.Normal).SetDamageInfo(DamageInfo.NonBlockAndParry).SetKnockback(normalKockback)
    .SetPositionAndDirection(transform.position, (e.transform.position - stompAttack.transform.position).normalized);
        e.TakeDamage(dmgData);
    }

    void PushRelease(DamageReceiver e)
    {
        dmgData.SetDamage(pushDamage).SetDamageTick(false).SetDamageType(Damagetype.StealShield).SetKnockback(pushKnockback).SetDamageInfo(DamageInfo.Normal)
            .SetPositionAndDirection(transform.position, transform.forward);
        Attack_Result takeDmg = e.TakeDamage(dmgData);

        //if (takeDmg == Attack_Result.parried || takeDmg == Attack_Result.blocked) e.GetComponent<CharacterHead>()?.UnequipShield(transform.forward);

        chargeOk = false;
        cdModuleStopeable.AddCD("ChargeAttack", () => chargeOk = true, timeToObtainCharge);
        pushAttack.Stop();
        sm.SendInput(GoatInputs.IDLE);
        animator.SetTrigger("PlayerCollision");
    }

    void PushAttack() => pushAttack.ManualTriggerAttack();

    void DealDamage() => headAttack.ManualTriggerAttack();

    void StompAttack() { stompAttack.ManualTriggerAttack(); ParticlesManager.Instance.PlayParticle(particles.stompParticle.name, stompAttack.transform.position); }
    #endregion

    #region TakeDamage Things
    protected override void TakeDamageFeedback(DamageData data)
    {
        AudioManager.instance.PlaySound(sounds.takeDamage.name, rootTransform);

        sm.SendInput(GoatInputs.TAKE_DMG);
        
        ParticlesManager.Instance.PlayParticle(particles.hitParticle.name, transform.position + Vector3.up).transform.forward = -data.attackDir;
        cooldown = true;
        cdModuleNoStopeable.AddCD("TakeDamageCD", () => cooldown = false, tdRecall);

        StartCoroutine(OnHitted(onHitFlashTime, onHitColor));
    }

    protected override void Die(Vector3 dir)
    {
        death = true;
        combatElement.ExitCombat();
        groundSensor?.TurnOff();
        sm.SendInput(GoatInputs.DEAD);
        if (dir == Vector3.zero)
            ragdoll.Ragdoll(true, -rootTransform.forward);
        else
            ragdoll.Ragdoll(true, dir);
        Main.instance.RemoveEntity(this);
        myEffectReceiver?.EndAllEffects();
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
            cdModuleStopeable.AddCD("StunAfterCharge", () => sm.SendInput(GoatInputs.IDLE), stunChargeTime);
        };

        UpdateStun = (name) => {
        };

        ExitStun += (input) => {
            chargeOk = true;
            cdModuleStopeable.EndCDWithoutExecute("StunAfterCharge");
            cdModuleStopeable.AddCD("ChargeAttack", () => chargeOk = true, timeToObtainCharge);
            animator.SetBool("Stun", false);
        };

        sm.SendInput(GoatInputs.PETRIFIED);
    }

    #endregion

    #region SET STATES

    void SetStates()
    {
        var idle = new EState<GoatInputs>("Idle");
        var chasing = new EState<GoatInputs>("Chasing");
        var persuit = new EState<GoatInputs>("Persuit");
        var chargePush = new EState<GoatInputs>("Charge_Push");
        var push = new EState<GoatInputs>("Push");
        var headAnt = new EState<GoatInputs>("Head_Anticipation");
        var headAttack = new EState<GoatInputs>("Head_Attack");
        var takeDamage = new EState<GoatInputs>("Take_DMG");
        var parried = new EState<GoatInputs>("Parried");
        var petrified = new EState<GoatInputs>("Petrified");
        var dead = new EState<GoatInputs>("Dead");
        var disable = new EState<GoatInputs>("Disable");

        ConfigureState.Create(idle)
            .SetTransition(GoatInputs.PERSUIT, persuit)
            .SetTransition(GoatInputs.TAKE_DMG, takeDamage)
            .SetTransition(GoatInputs.DEAD, dead)
            .SetTransition(GoatInputs.DISABLE, disable)
            .SetTransition(GoatInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(persuit)
            .SetTransition(GoatInputs.IDLE, idle)
            .SetTransition(GoatInputs.TAKE_DMG, takeDamage)
            .SetTransition(GoatInputs.DEAD, dead)
            .SetTransition(GoatInputs.DISABLE, disable)
            .SetTransition(GoatInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(chasing)
            .SetTransition(GoatInputs.IDLE, idle)
            .SetTransition(GoatInputs.PERSUIT, persuit)
            .SetTransition(GoatInputs.TAKE_DMG, takeDamage)
            .SetTransition(GoatInputs.CHARGE_PUSH, chargePush)
            .SetTransition(GoatInputs.HEAD_ANTICIP, headAnt)
            .SetTransition(GoatInputs.DEAD, dead)
            .SetTransition(GoatInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(chargePush)
            .SetTransition(GoatInputs.PUSH, push)
            .SetTransition(GoatInputs.DEAD, dead)
            .SetTransition(GoatInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(push)
            .SetTransition(GoatInputs.IDLE, idle)
            .SetTransition(GoatInputs.PARRIED, parried)
            .SetTransition(GoatInputs.PETRIFIED, petrified)
            .SetTransition(GoatInputs.DEAD, dead)
            .SetTransition(GoatInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(headAnt)
            .SetTransition(GoatInputs.HEAD_ATTACK, headAttack)
            .SetTransition(GoatInputs.DEAD, dead)
            .SetTransition(GoatInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(headAttack)
            .SetTransition(GoatInputs.IDLE, idle)
            .SetTransition(GoatInputs.PARRIED, parried)
            .SetTransition(GoatInputs.DEAD, dead)
            .SetTransition(GoatInputs.DISABLE, disable)
            .SetTransition(GoatInputs.HEAD_ANTICIP, headAnt)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(GoatInputs.IDLE, idle)
            .SetTransition(GoatInputs.DEAD, dead)
            .SetTransition(GoatInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(petrified)
            .SetTransition(GoatInputs.IDLE, idle)
            .SetTransition(GoatInputs.DEAD, dead)
            .SetTransition(GoatInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(parried)
            .SetTransition(GoatInputs.IDLE, idle)
            .SetTransition(GoatInputs.PETRIFIED, petrified)
            .SetTransition(GoatInputs.DEAD, dead)
            .SetTransition(GoatInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(dead)
            .SetTransition(GoatInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(GoatInputs.IDLE, idle)
            .Done();

        sm = new EventStateMachine<GoatInputs>(idle);

        new JabaliIdle(idle, sm, movement, distanceToCharge, distancePos, normalDistance, IsChargeOk).SetThis(combatElement).SetDirector(director)
            .SetRoot(rootTransform);

        new JabaliPersuit(persuit, sm, movement, lineOfSight.OnSight, IsChargeOk, normalDistance, distancePos, distanceToCharge).SetAnimator(animator)
            .SetThis(combatElement).SetRoot(rootTransform);

        new JabaliChasing(chasing, sm, () => combatElement.Attacking, IsChargeOk, distanceToCharge, distancePos, movement).SetDirector(director).SetThis(combatElement).SetRoot(rootTransform);

        new JabaliCharge(chargePush, sm, chargeTime, sounds.pushAnticipation.name, sounds.pushEnter.name, movement).SetAnimator(animator).SetDirector(director)
            .SetThis(combatElement).SetRigidbody(rb).SetRoot(rootTransform).SetCD(cdModuleStopeable);

        new JabaliPushAttack(push, sm, chargeSpeed, PushAttack, particles.chargeFeedback, pushAttack.Play, sounds.pushLoop.name, chargeDuration, groundSensor, true)
            .SetAnimator(animator).SetRigidbody(rb).SetRoot(rootTransform)
            .SetThis(combatElement).SetDirector(director).SetCD(cdModuleStopeable);

        if(goatType == GoatType.DoubleAttack)
        {
            new GoatTwoAttackAnt(headAnt, sm, movement, normalAttAnticipation).SetAnimator(animator).SetThis(combatElement).SetRoot(rootTransform).SetCD(cdModuleStopeable);
            new GoatTwoAttack(headAttack, sm, cdToHeadAttack, sounds.headAttack.name).SetAnimator(animator).SetDirector(director).SetThis(combatElement).SetCD(cdModuleStopeable);
        }
        else if(goatType == GoatType.Stomp)
        {
            new GoatStompAnt(headAnt, sm, normalAttAnticipation).SetAnimator(animator).SetThis(combatElement).SetRoot(rootTransform).SetCD(cdModuleStopeable);
            new GoatStomp(headAttack, sm, cdToHeadAttack, sounds.headAttack.name).SetAnimator(animator).SetDirector(director).SetThis(combatElement).SetCD(cdModuleStopeable);
        }
        else
        {
            new GoatAttackAnt(headAnt, sm, movement, normalAttAnticipation).SetAnimator(animator).SetThis(combatElement).SetRoot(rootTransform).SetCD(cdModuleStopeable);
            new GoatAttack(headAttack, sm, cdToHeadAttack, sounds.headAttack.name).SetAnimator(animator).SetDirector(director).SetThis(combatElement).SetCD(cdModuleStopeable);
        }

        new JabaliTD(takeDamage, sm, tdRecall).SetAnimator(animator).SetCD(cdModuleStopeable);

        new JabaliParried(parried, sm, timeParried).SetAnimator(animator).SetCD(cdModuleStopeable);

        new JabaliStun(petrified, sm, StartStun, TickStun, EndStun);

        new JabaliDeath(dead, sm, ragdoll, sounds.dead.name, ReturnToSpawner).SetThis(combatElement).SetDirector(director).SetCD(cdModuleStopeable);

        disable.OnEnter += (input) => DisableObject();

        disable.OnExit += (input) => EnableObject();
    }

    bool IsChargeOk() => chargeOk;

    void StartStun(EState<GoatInputs> input) => EnterStun?.Invoke(input);

    void TickStun(string name) => UpdateStun?.Invoke(name);

    void EndStun(GoatInputs input) { ExitStun?.Invoke(input); EnterStun = (x) => { }; ExitStun = (x) => { }; }

    void DisableObject()
    {
        canupdate = false;
        movement.SetDefaultSpeed();
        combatElement.Combat = false;
    }

    void EnableObject() => Initialize();

    #endregion
}
