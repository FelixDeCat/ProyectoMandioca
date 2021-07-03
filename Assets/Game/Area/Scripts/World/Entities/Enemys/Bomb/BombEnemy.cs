using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

public class BombEnemy : EnemyBase
{
    [Header("Move Options")]
    [SerializeField] GenericEnemyMove movement = null;
    [SerializeField] CharacterGroundSensor groundSensor = null;

    [Header("Combat Options")]
    [SerializeField] int damage = 2;
    [SerializeField] float normalDistance = 8;
    [SerializeField] float combatDistance = 20;
    [SerializeField] float knockback = 20;
    [SerializeField] float recallTime = 0.1f;
    [SerializeField] GoatStompComponent explodeComponent = null;
    EntityBase target;

    private bool cooldown = false;
    bool petrify = false;

    [Header("Spawn Options")]
    [SerializeField] PlayObject trapToDie = null;
    [SerializeField] float trapDuration = 5;
    [SerializeField] Transform rootToTrap = null;

    [Header("Feedback")]
    [SerializeField] AnimEvent anim = null;
    [SerializeField] Color onHitColor = Color.white;
    [SerializeField] float onHitFlashTime = 0.1f;

    [SerializeField] Color explodeTick = Color.blue;
    [SerializeField] float initTickTime = 1;
    [SerializeField] float tickModifier = 0.8f;
    [SerializeField] float tickCount = 10;
    
    EventStateMachine<BombInputs> sm;
    CDModule cdModule = new CDModule();
    EffectReceiver myEffectReceiver;

    public BombParticles particles;
    public BombSounds sounds;
    [Serializable]
    public class BombParticles
    {
        public ParticleSystem _spawnParticules = null;
        public ParticleSystem takeDamagePart = null;
        public ParticleSystem explodePart = null;
    }

    [Serializable]
    public class BombSounds
    {
        public AudioClip _takeHit_AC;
        public AudioClip clip_walkEnt;
        public AudioClip mandragoraSpawn_Clip;
        public AudioClip explode_Clip;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_SPAWN, new object[] { this });
        ParticlesManager.Instance.GetParticlePool(particles._spawnParticules.name, particles._spawnParticules, 5);
        ParticlesManager.Instance.GetParticlePool(particles.takeDamagePart.name, particles.takeDamagePart, 8);
        ParticlesManager.Instance.GetParticlePool(particles.explodePart.name, particles.explodePart, 3);

        AudioManager.instance.GetSoundPool(sounds._takeHit_AC.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds._takeHit_AC);
        AudioManager.instance.GetSoundPool(sounds.clip_walkEnt.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds.clip_walkEnt, true);
        AudioManager.instance.GetSoundPool(sounds.explode_Clip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds.explode_Clip);
        AudioManager.instance.GetSoundPool(sounds.mandragoraSpawn_Clip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds.mandragoraSpawn_Clip);

        rb = GetComponent<Rigidbody>();
        explodeComponent.Configure(AttackEntity);
        anim.Add_Callback("Explode", DealDamage);
        movement.Configure(rootTransform, rb, groundSensor);

        Main.instance.AddEntity(this);
        myEffectReceiver = GetComponent<EffectReceiver>();

        SetStates();
        
        if (trapToDie) PoolManager.instance.GetObjectPool(trapToDie.name, trapToDie);

        dmgReceiver.ChangeKnockback(movement.ApplyForceToVelocity, () => false);
    }

    public override void SpawnEnemy()
    {
        animator.SetBool("entry", true);
        AudioManager.instance.PlaySound(sounds.mandragoraSpawn_Clip.name, animator.transform);
        base.SpawnEnemy();
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
        death = false;
        sm.SendInput(BombInputs.DISABLE);
    }

    protected override void OnUpdateEntity()
    {
        if (!death)
        {
            if (target != null && Vector3.Distance(target.transform.position, transform.position) > combatDistance + 2)
            {
                target = null;
                sm.SendInput(BombInputs.IDLE);
            }

            if (target == null && Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) <= combatDistance)
            {
                target = Main.instance.GetChar();
                sm.SendInput(BombInputs.PERSUIT);
            }
        }
        myEffectReceiver?.UpdateStates();
        sm?.Update();
        movement.OnUpdate();
        cdModule.UpdateCD();
    }
    Vector3 force;
    protected override void OnPause()
    {
        base.OnPause();
        force = rb.velocity;
        rb.isKinematic = true;
    }
    protected override void OnResume()
    {
        base.OnResume();
        rb.isKinematic = false;
        rb.velocity = force;
    }

    #region Attack
    public void DealDamage()
    {
        explodeComponent.ManualTriggerAttack();
        AudioManager.instance.PlaySound(sounds.explode_Clip.name, animator.transform);
        ParticlesManager.Instance.PlayParticle(particles.explodePart.name, transform.position);
        AchievesManager.instance.CompleteAchieve("ExplosiveMandragora");
        OnDead();

        Desactive();
    }

    public void AttackEntity(DamageReceiver e)
    {
        dmgData.SetDamage(damage).SetDamageTick(false).SetDamageType(Damagetype.Explosion).SetKnockback(knockback).SetDamageInfo(DamageInfo.NonBlockAndParry)
    .SetPositionAndDirection(transform.position);
        e.TakeDamage(dmgData);
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

    public GenericLifeSystem Life() => lifesystem;

    protected override void TakeDamageFeedback(DamageData data)
    {
        AudioManager.instance.PlaySound(sounds._takeHit_AC.name, rootTransform);

        ParticlesManager.Instance.PlayParticle(particles.takeDamagePart.name, transform.position + Vector3.up);
        cooldown = true;
        cdModule.AddCD("TakeDamageCD", () => cooldown = false, recallTime);

        StartCoroutine(OnHitted(onHitFlashTime, onHitColor));
    }

    protected override void Die(Vector3 dir)
    {
        sm.SendInput(BombInputs.EXPLODE);
        death = true;
        Main.instance.RemoveEntity(this);
        myEffectReceiver?.EndAllEffects();
    }

    void Desactive()
    {
        groundSensor?.TurnOff();
        ReturnToSpawner();
    }

    protected override bool IsDamage()
    {
        if (cooldown || sm.Current.Name == "Explode") return true;
        else return false;
    }
    #endregion

    protected override void OnFixedUpdate() { }
    protected override void OnTurnOff()
    {
        sm.SendInput(BombInputs.DISABLE);
        groundSensor?.TurnOff();
    }
    protected override void OnTurnOn() { sm.SendInput(BombInputs.IDLE); groundSensor?.TurnOn(); }

    #region STATE MACHINE THINGS

    public enum BombInputs { IDLE, PERSUIT, PREP_EXPLODE, EXPLODE, DISABLE }


    void SetStates()
    {
        var idle = new EState<BombInputs>("Idle");
        var goToPos = new EState<BombInputs>("Follow");
        var prepare = new EState<BombInputs>("PrepareExplode");
        var explode = new EState<BombInputs>("Explode");
        var disable = new EState<BombInputs>("Disable");

        ConfigureState.Create(idle)
            .SetTransition(BombInputs.PERSUIT, goToPos)
            .SetTransition(BombInputs.EXPLODE, explode)
            .SetTransition(BombInputs.DISABLE, disable)
            .SetTransition(BombInputs.PREP_EXPLODE, prepare)
            .Done();

        ConfigureState.Create(goToPos)
            .SetTransition(BombInputs.IDLE, idle)
            .SetTransition(BombInputs.EXPLODE, explode)
            .SetTransition(BombInputs.DISABLE, disable)
            .SetTransition(BombInputs.PREP_EXPLODE, prepare)
            .Done();

        ConfigureState.Create(prepare)
            .SetTransition(BombInputs.EXPLODE, explode)
            .SetTransition(BombInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(explode)
            .SetTransition(BombInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(BombInputs.IDLE, idle)
            .Done();

        sm = new EventStateMachine<BombInputs>(idle);

        new BombEnemyIdle(idle, sm, normalDistance, () => target, rootTransform);

        new BombEnemyPersuit(goToPos, sm, movement, normalDistance, () => target, animator, sounds.clip_walkEnt.name, rootTransform);

        new BombEnemyPrepExplode(prepare, sm, PrepareToExplosion);

        new BombEnemyPrepExplode(explode, sm, Explosion);

        new DummyDisableState<BombInputs>(disable, sm, EnableObject, DisableObject);
    }
    Coroutine coroutineFeedback;

    void Explosion()
    {
        if(coroutineFeedback != null)
        {
            StopCoroutine(coroutineFeedback);
            var smr = GetComponentInChildren<SkinnedMeshRenderer>();
            Material[] mats = smr.materials;
            mats[0].SetColor("_EmissionColor", Color.black);
            coroutineFeedback = null;
        }
        animator.SetTrigger("Explode");
    }

    void PrepareToExplosion()
    {
        coroutineFeedback = StartCoroutine(ExplosionFeedback());
    }

    IEnumerator ExplosionFeedback()
    {
        float tickTimer = initTickTime;

        var smr = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] mats = smr.materials;

        for (int i = 0; i < tickCount; i++)
        {
            for (int e = 0; e < 20; e++)
            {
                if (petrify)
                {
                    e -= 1;
                    yield return new WaitForSeconds(0.01f);
                    continue;
                }
                if (e < 10)
                {
                    mats[0].SetColor("_EmissionColor", Color.Lerp(Color.black, explodeTick, e / 10));
                }
                else
                {
                    mats[0].SetColor("_EmissionColor", Color.Lerp(explodeTick, Color.black, (e - 10) / 10));
                }
                yield return new WaitForSeconds(0.01f);
            }

            tickTimer *= tickModifier;

            yield return new WaitForSeconds(tickTimer);
        }

        sm.SendInput(BombInputs.EXPLODE);
    }

    void DisableObject()
    {
        movement.SetDefaultSpeed();
    }

    void EnableObject() => Initialize();

    #endregion
}
