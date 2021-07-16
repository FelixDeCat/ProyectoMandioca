using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetoBoss : EnemyBase
{
    [SerializeField] BetoBrain brain = new BetoBrain();
    [SerializeField] float rotSpeed = 10;
    Transform target = null;

    [SerializeField] float attackCooldownTime = 3;
    [SerializeField] float spawnCooldown = 8;
    [SerializeField] float lakeCooldown = 20;
    [SerializeField] float flyCooldown = 30;

    [SerializeField] float recallTime = 0.2f;
    [SerializeField] Color onHitColor = Color.red;
    [SerializeField] float onHitFlashTime = 20;

    [SerializeField] float yMaxPos = 10.47f;
    [SerializeField] float ascendSpeed = 2;
    [SerializeField] float moveSpeed = 7;

    FinalPoisonLakeSkill poisonSkill;
    bool updatePoison;
    [SerializeField] GenericEnemyMove obsAvoid = null;
    [SerializeField] TriggerDispatcher trigger = null;

    [SerializeField] AudioClip bossBattleMusic = null;
    [SerializeField] ParticleSystem takeDamagePS = null;
    [SerializeField] SkinnedMeshRenderer[] myMeshes = new SkinnedMeshRenderer[0];

    #region Properties
    public int CurrentLife { get => lifesystem.Life; }
    public string MyAbilityMostUsed { get; private set; }
    public bool AttackOnCooldown { get; private set; }
    public bool SpawnCooldown { get; private set; }
    public bool LakeCooldown { get; private set; }
    public bool Stuned { get; set; }
    public bool FlyCooldown { get; set; }
    public bool Flying { get; set; }
    #endregion
    CDModule cdModule = new CDModule();
    bool cooldown;
    public bool onCombat;
    Vector3 initPos;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Debug.Log("me inicializo");
        target = Main.instance.GetChar().Root;
        MyAbilityMostUsed = "";
        brain.Initialize(this, StartCoroutine, rb);
        obsAvoid.Configure(rootTransform);
        dmgReceiver.ChangeKnockback((x) => { }, () => true);
        //ParticlesManager.Instance.GetParticlePool(takeDamagePS.name, takeDamagePS);
    }
    protected override void OnDeinitialize()
    {
        
    }

    public void StartCombat()
    {
        if (onCombat) return;
        brain.PlanAndExecute();
        BossBarGeneric.Open();
        BossBarGeneric.SetLife(lifesystem.Life, lifesystem.LifeMax);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ON_PLAYER_RESPAWN, ResetBossOnDead);
        Main.instance.eventManager.TriggerEvent(GameEvents.BETO_START);
        onCombat = true;
        initPos = transform.position;
        AudioAmbienceSwitcher.instance.EnterOnBossBattle(true, bossBattleMusic);
    }

    protected override void OnUpdateEntity()
    {
        cdModule.UpdateCD();

        if (updatePoison)
        {
            poisonSkill.OnUpdate();
        }

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
        cooldown = true;
        cdModule.AddCD("TakeDamageCD", () => { cooldown = false; animator.SetBool("takeDamage", false); }, recallTime);
        BossBarGeneric.SetLife(lifesystem.Life, lifesystem.LifeMax);
        //var part = ParticlesManager.Instance.PlayParticle(takeDamagePS.name, transform.position + Vector3.up);
        //part.transform.forward = (transform.position - data.owner_position).normalized;
        StartCoroutine(OnHitted(onHitFlashTime, onHitColor, myMeshes));
        animator.SetBool("takeDamage", true);


        if (data.ownerRoot == transform && !Stuned && Flying)
        {
            Debug.Log("entra");
            animator.SetFloat("Flying", 0);
            Flying = false;
            FlyCooldown = true;
            cdModule.AddCD("FlyCooldown", () => FlyCooldown = false, flyCooldown);
            Stuned = true;
        }
    }

    public void StartPoisonLake(FinalPoisonLakeSkill _skill)
    {
        poisonSkill = _skill;
        poisonSkill.UseSkill(EndPoisonLake);
        updatePoison = true;
    }

    void EndPoisonLake()
    { 
        updatePoison = false;
        Debug.Log("termina");
        LakeActive(false);
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
        Main.instance.eventManager.TriggerEvent(GameEvents.BETO_RESET);
        Main.instance.eventManager.TriggerEvent(GameEvents.BETO_DEFEATED);
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ON_PLAYER_RESPAWN, ResetBossOnDead);
        gameObject.SetActive(false);
    }

    void ResetBossOnDead()
    {
        AudioAmbienceSwitcher.instance.EnterOnBossBattle(false, bossBattleMusic);
        trigger.gameObject.SetActive(true);
        brain.ResetBrain();
        StopAllCoroutines();
        Flying = false;
        Stuned = false;
        StopMove();
        rb.useGravity = true;
        animator.Play("Idle");
        animator.SetFloat("Flying", 0);
        animator.SetBool("PoisonLake", false);
        animator.SetBool("Spawn", false);
        animator.SetBool("StartFly", false);
        lifesystem.ResetLifeSystem();
        BossBarGeneric.SetLife(lifesystem.Life, lifesystem.LifeMax);
        onCombat = false;
        cdModule.ResetAll();
        BossBarGeneric.Close();
        transform.position = initPos;
        Main.instance.eventManager.TriggerEvent(GameEvents.BETO_RESET);
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ON_PLAYER_RESPAWN, ResetBossOnDead);
    }

    #region Functions to States

    public bool Fly()
    {
        rb.velocity = Vector3.up * ascendSpeed;

        if (transform.localPosition.y >= yMaxPos)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, yMaxPos, transform.localPosition.z);
            rb.velocity = Vector3.zero;
            return true;
        }
        return false;
    }
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

    public bool DistanceToCharacter() => Vector3.Distance(transform.position, target.position) <= brain.minCharDistance;

    public bool WalkToNextNode(AStarNode starNode)
    {
        Vector3 nodePos = new Vector3(starNode.transform.position.x, 0, starNode.transform.position.z);
        Vector3 myPos = new Vector3(transform.position.x, 0, transform.position.z);

        Vector3 dirToNode = obsAvoid.ObstacleAvoidance((nodePos - myPos).normalized);

        rootTransform.forward = Vector3.Lerp(rootTransform.forward, dirToNode, Time.deltaTime * rotSpeed);

        rb.velocity = new Vector3(rootTransform.forward.x * moveSpeed, rb.velocity.y, rootTransform.forward.z * moveSpeed);

        if (Vector3.Distance(myPos, nodePos) < 0.5f) return true;

        return false;
    }

    public bool WalkToNextNode(Vector3 customNode)
    {
        Vector3 nodePos = new Vector3(customNode.x, 0, customNode.z);
        Vector3 myPos = new Vector3(transform.position.x, 0, transform.position.z);

        Vector3 dirToNode = obsAvoid.ObstacleAvoidance((nodePos - myPos).normalized);

        rootTransform.forward = Vector3.Lerp(rootTransform.forward, dirToNode, Time.deltaTime * rotSpeed);

        rb.velocity = new Vector3(rootTransform.forward.x * moveSpeed, rb.velocity.y, rootTransform.forward.z * moveSpeed);

        if (Vector3.Distance(myPos, nodePos) < 0.5f) return true;

        return false;
    }

    public void StopMove() => rb.velocity = Vector3.zero;

    public void AttackCooldown()
    {
        AttackOnCooldown = true;
        cdModule.AddCD("AttackCD", () => AttackOnCooldown = false, attackCooldownTime);
    }

    public void SpawnActive(bool b)
    {
        if(b)
            SpawnCooldown = true;
        else
            cdModule.AddCD("SpawnCD", () => SpawnCooldown = false, spawnCooldown);
    }

    public void LakeActive(bool b)
    {
        if (b)
            LakeCooldown = true;
        else
            cdModule.AddCD("LakeCD", () => LakeCooldown = false, lakeCooldown);
    }

    
    #endregion
}
