using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossModel : EnemyBase
{
    [SerializeField] UnityEvent OnResetCaronteIfPlayerIsDead;

    [SerializeField] BossBrain brain = new BossBrain();
    public int maxStamina = 9;
    [SerializeField] float rotSpeed = 10;
    Transform target = null;
    [SerializeField] Transform shootPosition = null;
    [SerializeField] AnimEvent animEvent = null;
    [SerializeField] ThrowData throwData = new ThrowData();
    [SerializeField] Throwable projectile = null;
    [SerializeField] int meleDamage = 4;
    [SerializeField] float meleKnockback = 10;
    [SerializeField] CombatComponent meleAttack;

    [SerializeField] float attackCooldownTime = 3;
    [SerializeField] float tpCooldownTime = 10;
    [SerializeField] float abilityCooldownTime = 8;

    [SerializeField] float recallTime = 0.2f;
    [SerializeField] Color onHitColor = Color.red;
    [SerializeField] float onHitFlashTime = 20;

    [SerializeField] CaronteSounds sounds = new CaronteSounds();
    [SerializeField] CaronteParticles particles = new CaronteParticles();

    [SerializeField] AudioClip bossBattleMusic = null;
    [SerializeField] TriggerDispatcher trigger;
    

    public float yMaxPos = 10.47f;
    public float yMinPos = 5.47f;
    public float ascendSpeed = 2;

    [System.Serializable]
    public class CaronteSounds
    {
        public AudioClip takeDamage_sound = null;
    }
    [System.Serializable]
    public class CaronteParticles
    {
        public ParticleSystem takeDamage_particle = null;
    }

    #region Properties
    public int CurrentStamina { get; private set; }
    public int CurrentLife { get => lifesystem.Life; }
    public string MyAbilityMostUsed { get; private set; }
    public bool AttackOnCooldown { get; private set; }
    public bool AbilityOnCooldown { get; private set; }
    public bool TPOnCooldown { get; private set; }
    public bool ShieldActive { get; set; }
    #endregion
    CDModule cdModule = new CDModule();
    bool cooldown;
    public bool onCombat;
    bool inSecondPhase;
    Vector3 initPos;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        target = Main.instance.GetChar().Root;
        CurrentStamina = maxStamina;
        MyAbilityMostUsed = "";
        ThrowablePoolsManager.instance.CreateAPool(projectile.name, projectile);
        animEvent.Add_Callback("NormalShoot", ShootEvent);
        animEvent.Add_Callback("MeleAttack", MeleEvent);
        meleAttack.Configure(MeleAttack);
        dmgData.SetDamage(meleDamage).SetDamageInfo(DamageInfo.NonBlockAndParry).SetKnockback(meleKnockback);
        brain.Initialize(this, StartCoroutine);

        AudioManager.instance.GetSoundPool(sounds.takeDamage_sound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, sounds.takeDamage_sound);

        ParticlesManager.Instance.GetParticlePool(particles.takeDamage_particle.name, particles.takeDamage_particle);
    }

    public void StartCombat() 
    {
        if (onCombat) return;
        brain.PlanAndExecute();
        BossBarGeneric.Open();
        BossBarGeneric.SetLife(lifesystem.Life, lifesystem.LifeMax);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ON_PLAYER_RESPAWN, ResetBossOnDead);
        onCombat = true;
        initPos = transform.position;
        AudioAmbienceSwitcher.instance.EnterOnBossBattle(true, bossBattleMusic);
    }

    void ShootEvent()
    {
        throwData.Position = shootPosition.position;
        throwData.Direction = rootTransform.forward;
        ThrowablePoolsManager.instance.Throw(projectile.name, throwData);
    }

    void MeleEvent()
    {
        meleAttack.ManualTriggerAttack();
    }

    void MeleAttack(DamageReceiver dmgReceiver)
    {
        dmgReceiver.TakeDamage(dmgData.SetPositionAndDirection(rootTransform.position, rootTransform.forward));
    }

    protected override void OnUpdateEntity()
    {
        cdModule.UpdateCD();
    }

    protected override void OnFixedUpdate()
    {
    }

    protected override void OnPause()
    {
        base.OnPause();
        brain.DesactiveFSM();
    }

    protected override void OnResume()
    {
        base.OnResume();
        brain.ActiveFSM();
    }

    protected override void OnReset()
    {
    }

    protected override void OnTurnOff()
    {
    }

    protected override void OnTurnOn()
    {
    }

    protected override void TakeDamageFeedback(DamageData data)
    {
        AudioManager.instance.PlaySound(sounds.takeDamage_sound.name, rootTransform);

        ParticlesManager.Instance.PlayParticle(particles.takeDamage_particle.name, transform.position + Vector3.up);
        cooldown = true;
        cdModule.AddCD("TakeDamageCD", () => cooldown = false, recallTime);
        BossBarGeneric.SetLife(lifesystem.Life, lifesystem.LifeMax);

        StartCoroutine(OnHitted(onHitFlashTime, onHitColor));

        if (!inSecondPhase && CurrentLife <= lifesystem.LifeMax * brain.lifeToChangePhase)
            ChangePhase();
    }

    void ChangePhase()
    {
        brain.ChangePhase();
        animator.Play("Idle");
        animator.SetBool("OnSpawn", false);
        animator.SetBool("OnFlame", false);
        StartCoroutine(Fly());
        inSecondPhase = true;
    }

    IEnumerator Fly()
    {
        while (transform.localPosition.y < yMaxPos)
        {
            transform.position += Vector3.up * ascendSpeed * Time.deltaTime;
            yield return new WaitForSeconds(0.001f);
        }
        cdModule.ResetAll();
        brain.PlanAndExecute();
    }

    protected override bool IsDamage()
    {
        if (cooldown) return true;
        else return false;
    }

    protected override void Die(Vector3 dir)
    {
        brain.ResetBrain();
        StopAllCoroutines();
        BossBarGeneric.Close();
        AudioAmbienceSwitcher.instance.EnterOnBossBattle(false, bossBattleMusic);
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ON_PLAYER_RESPAWN, ResetBossOnDead);
        gameObject.SetActive(false);
    }

    void ResetBossOnDead()
    {
        AudioAmbienceSwitcher.instance.EnterOnBossBattle(false, bossBattleMusic);
        brain.ResetBrain();
        StopAllCoroutines();
        animator.Play("Idle");
        animator.SetBool("OnSpawn", false);
        animator.SetBool("OnFlame", false);
        lifesystem.ResetLifeSystem();
        BossBarGeneric.SetLife(lifesystem.Life, lifesystem.LifeMax);
        onCombat = false;
        CurrentStamina = maxStamina;
        MyAbilityMostUsed = "";
        cdModule.ResetAll();
        BossBarGeneric.Close();
        trigger.gameObject.SetActive(true);
        transform.position = initPos;
        inSecondPhase = false;
        OnResetCaronteIfPlayerIsDead.Invoke();
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ON_PLAYER_RESPAWN, ResetBossOnDead);
    }

    #region Functions to States
    public void ChangeLastAbility(string last)
    {
        MyAbilityMostUsed = last;
    }

    public void RotateToChar()
    {
        Vector3 newForward = (target.position - transform.position).normalized;
        newForward.y = 0;
        rootTransform.forward = Vector3.Slerp(rootTransform.forward, newForward, rotSpeed * Time.deltaTime);
    }

    public bool DistanceToCharacter() => Vector3.Distance(transform.position, target.position) <= brain.distanceToMele && !inSecondPhase ? true : false;

    public void AttackCooldown()
    {
        AttackOnCooldown = true;
        cdModule.AddCD("AttackCD", () => AttackOnCooldown = false, attackCooldownTime);
    }

    public void AbilityCooldown(int restStamina)
    {
        if (inSecondPhase) { CurrentStamina -= restStamina; if (CurrentStamina < 0) CurrentStamina = 0; }
        AbilityOnCooldown = true;
        cdModule.AddCD("AbilityCD", () => AbilityOnCooldown = false, abilityCooldownTime);
    }

    public void TPCooldown()
    {
        TPOnCooldown = true;
        cdModule.AddCD("TPCD", () => TPOnCooldown = false, tpCooldownTime);
    }

    public void RestartStamina() => CurrentStamina = maxStamina;

    #endregion
}