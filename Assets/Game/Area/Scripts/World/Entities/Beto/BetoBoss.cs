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

    [SerializeField] float recallTime = 0.2f;
    [SerializeField] Color onHitColor = Color.red;
    [SerializeField] float onHitFlashTime = 20;

    [SerializeField] float yMaxPos = 10.47f;
    [SerializeField] float ascendSpeed = 2;
    [SerializeField] float moveSpeed = 7;

    FinalPoisonLakeSkill poisonSkill;
    bool updatePoison;

    #region Properties
    public int CurrentLife { get => lifesystem.Life; }
    public string MyAbilityMostUsed { get; private set; }
    public bool AttackOnCooldown { get; private set; }
    public bool SpawnCooldown { get; private set; }
    public bool LakeCooldown { get; private set; }
    public bool Stuned { get; set; }
    #endregion
    CDModule cdModule = new CDModule();
    bool cooldown;
    public bool onCombat;
    Vector3 initPos;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        target = Main.instance.GetChar().Root;
        MyAbilityMostUsed = "";
        //brain.Initialize(this, StartCoroutine);
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
    }

    protected override void OnUpdateEntity()
    {
        cdModule.UpdateCD();

        if (updatePoison)
            poisonSkill.OnUpdate();
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
        cdModule.AddCD("TakeDamageCD", () => cooldown = false, recallTime);
        BossBarGeneric.SetLife(lifesystem.Life, lifesystem.LifeMax);

        StartCoroutine(OnHitted(onHitFlashTime, onHitColor));
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
        LakeActive(false);
    }

    public bool Fly()
    {
        rb.velocity = transform.position + Vector3.up * ascendSpeed;

        if(transform.position.y >= yMaxPos)
        {
            transform.position = new Vector3(transform.position.x, yMaxPos, transform.position.z);
            return true;
        }
        return false;
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
        gameObject.SetActive(false);
    }

    void ResetBossOnDead()
    {
        brain.ResetBrain();
        StopAllCoroutines();
        animator.Play("Idle");
        animator.SetBool("OnSpawn", false);
        animator.SetBool("OnFlame", false);
        lifesystem.ResetLifeSystem();
        BossBarGeneric.SetLife(lifesystem.Life, lifesystem.LifeMax);
        onCombat = false;
        MyAbilityMostUsed = "";
        cdModule.ResetAll();
        BossBarGeneric.Close();
        transform.position = initPos;
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

    public bool DistanceToCharacter() => Vector3.Distance(transform.position, target.position) <= brain.minCharDistance;

    public bool WalkToNextNode(AStarNode starNode)
    {
        Vector3 dirToNode = (starNode.transform.position - transform.position).normalized;

        rootTransform.forward = Vector3.Lerp(rootTransform.forward, dirToNode, Time.deltaTime * rotSpeed);

        rb.velocity = new Vector3(rootTransform.forward.x * moveSpeed, rb.velocity.y, rootTransform.forward.z * moveSpeed);

        if (Vector3.Distance(transform.position, starNode.transform.position) < 0.1f)
            return true;

        return false;
    }

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
