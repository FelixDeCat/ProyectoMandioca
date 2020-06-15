using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ToolsMandioca.StateMachine;
using UnityEngine.Serialization;

public class TrueDummyEnemy : EnemyBase
{
    [Header("Move Options")]
    [SerializeField] GenericEnemyMove movement;

    public AnimationCurve animEmisive;

    [Header("Combat Options")]
    [SerializeField] CombatComponent combatComponent = null;
    [SerializeField] int damage = 2;
    [SerializeField] float normalDistance = 8;
    [SerializeField] float cdToAttack = 1;
    [SerializeField] float parriedTime = 2;
    [SerializeField] float knockback = 20;
    private CombatDirector director;

    [Header("Life Options")]
    [SerializeField] float recallTime = 1;

    [Header("for knockback")]
    [SerializeField] float forceRecall = 4;
    [SerializeField] float explosionForce = 20;
    [Header("for petrify")]
    [SerializeField] float petrifiedTime = 2;
    private float stunTimer;
    private Action<EState<DummyEnemyInputs>> EnterStun;
    private Action<string> UpdateStun;
    private Action<DummyEnemyInputs> ExitStun;

    [Range(0,1)]
    [SerializeField] float freezeSpeedSlowed = 0.5f;
    [SerializeField] float freezeTime = 4;
    private bool cooldown = false;
    private float timercooldown = 0;

    [Header("Feedback")]
    [SerializeField] GameObject feedbackFireDot = null;
    [SerializeField] ParticleSystem greenblood = null;
    [SerializeField] AnimEvent anim = null;
    [SerializeField] Animator animator = null;
    
    [SerializeField] Material freeze_shader = null;
    [SerializeField] Color onHitColor;
    [SerializeField] float onHitFlashTime;
    [SerializeField] RagdollComponent ragdoll = null;
    [SerializeField] ParticleSystem myGroundParticle = null;
    [SerializeField] private AudioClip _takeHit_AC;
    private const string takeHit_audioName = "woodChop";
    [SerializeField] Material _petrifiedTex;
    [SerializeField] ParticleSystem Petrified;
    [SerializeField] ParticleSystem endPetrify;
    [SerializeField] AudioClip clip_PetrifyStand;
    [SerializeField] AudioClip clip_petrifyEnd;
    [SerializeField] AudioClip clip_walkEnt;
    [SerializeField] ParticleSystem _spawnParticules;

    public bool isOnFire { get; private set; }
    

    EventStateMachine<DummyEnemyInputs> sm;

    

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_SPAWN, new object[] { this });
        _spawnParticules.Play();
        var smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
        {
            myMat = smr.materials;
        }

        AudioManager.instance.GetSoundPool(takeHit_audioName, AudioGroups.GAME_FX, _takeHit_AC);
        AudioManager.instance.GetSoundPool("PetrifyStand", AudioGroups.GAME_FX, clip_PetrifyStand);
        AudioManager.instance.GetSoundPool("PetrifyEnd", AudioGroups.GAME_FX, clip_petrifyEnd);
        AudioManager.instance.GetSoundPool("WalkEnt", AudioGroups.GAME_FX, clip_walkEnt, true);

        rb = GetComponent<Rigidbody>();
        combatComponent.Configure(AttackEntity);
        anim.Add_Callback("DealDamage", DealDamage);
        movement.Configure(rootTransform, rb);

        StartDebug();

        Main.instance.AddEntity(this);

        IAInitialize(Main.instance.GetCombatDirector());
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
        sm.SendInput(DummyEnemyInputs.DISABLE);
    }

    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
        {
            sm.SendInput(DummyEnemyInputs.IDLE);
        }

        director.AddNewTarget(this);

        canupdate = true;
    }

    protected override void OnUpdateEntity()
    {
        if (canupdate)
        {
            EffectUpdate();

            if (!death)
            {
                if (combat)
                {
                    if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) > combatDistance + 2)
                    {
                        director.DeadEntity(this, entityTarget);
                        entityTarget = null;
                        combat = false;
                    }
                }

                if (!combat && entityTarget == null)
                {
                    if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) <= combatDistance)
                    {
                        director.AddToList(this, Main.instance.GetChar());
                        SetTarget(Main.instance.GetChar());
                        combat = true;
                    }
                }
            }

            if (sm != null)
            {
                sm.Update();
            }

            if (cooldown) {
                if (timercooldown < recallTime)  timercooldown = timercooldown + 1 * Time.deltaTime;
                else {  cooldown = false; timercooldown = 0; }
            }

        }
    }

    protected override void OnPause() { }
    protected override void OnResume() { }

    #region Attack
    public void DealDamage() { combatComponent.ManualTriggerAttack(); sm.SendInput(DummyEnemyInputs.ATTACK); }

    public void AttackEntity(DamageReceiver e)
    {
        dmgData.SetDamage(damage).SetDamageTick(false).SetDamageType(Damagetype.parriable).SetKnockback(knockback)
    .SetPositionAndDirection(transform.position);
        Attack_Result takeDmg = e.TakeDamage(dmgData);

        if (takeDmg == Attack_Result.parried)
        {
            combatComponent.Stop();
            sm.SendInput(DummyEnemyInputs.PARRIED);
            
        }
    }

    public override void ToAttack() => attacking = true;
    #endregion

    #region Effects
    float currentAnimSpeed;
    public override void OnPetrified()
    {
        base.OnPetrified();
       
        EnterStun = (input) => {
            currentAnimSpeed = animator.speed;
            animator.speed = 0;

            var smr = GetComponentInChildren<SkinnedMeshRenderer>();
            if (smr != null)
            {
                
                myMat = smr.materials;
                Petrified.Play();
                AudioManager.instance.PlaySound("PetrifyStand");
                Material[] mats = smr.materials;
                mats[0] = _petrifiedTex;
                smr.materials = mats;
                Invinsible = true;
                //cambiar las particulas de sangre a las de piedra
            }
        };

        UpdateStun = (name) => {
            stunTimer += Time.deltaTime;
            Debug.Log("petrified");
            if (stunTimer >= petrifiedTime)
            {
                if (name == "Begin_Attack")
                    sm.SendInput(DummyEnemyInputs.BEGIN_ATTACK);
                else if (name == "Attack")
                    sm.SendInput(DummyEnemyInputs.ATTACK);
                else
                    sm.SendInput(DummyEnemyInputs.IDLE);
            }
        };

        ExitStun = (input) => {
            animator.speed = currentAnimSpeed;
            stunTimer = 0;
            var smr2 = GetComponentInChildren<SkinnedMeshRenderer>();
            if (smr2 != null)
            {
                Material[] mats = smr2.materials;
                smr2.materials = myMat;
                Petrified.Stop();
                AudioManager.instance.PlaySound("PetrifyEnd");
                endPetrify.Play();
                Invinsible = false;
                //poner las particulas de sangre de vuelta
            }
        };

        Debug.Log("entra a enviar el petrify");
        sm.SendInput(DummyEnemyInputs.PETRIFIED);
    }

    public override float ChangeSpeed(float newSpeed)
    {
        if (newSpeed < 0)
            return movement.GetInitSpeed();

        movement.SetCurrentSpeed(newSpeed);

        return movement.GetInitSpeed();
    }

    public override void OnFire()
    {
        if (isOnFire)
            return;

        isOnFire = true;
        feedbackFireDot.SetActive(true);
        base.OnFire();

        lifesystem.DoTSystem(30, 2, 1, Damagetype.Fire, () =>
        {
            isOnFire = false;
            feedbackFireDot.SetActive(false);
        });
    }

    private Material[] myMat;
    public override void OnFreeze()
    {
        base.OnFreeze();

        //Debug.Log("entré al freeze");

        movement.MultiplySpeed(freezeSpeedSlowed);
        animator.speed *= freezeSpeedSlowed;

        var smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
        {
            myMat = smr.materials;

            Material[] mats = smr.materials;
            mats[0] = freeze_shader;
            smr.materials = mats;
        }

        AddEffectTick(() => { }, freezeTime, () => {
            movement.DivideSpeed(freezeSpeedSlowed);
            animator.speed /= freezeSpeedSlowed;
            var smr2 = GetComponentInChildren<SkinnedMeshRenderer>();
            if (smr2 != null)
            {
                Material[] mats = smr2.materials;
                smr2.materials = myMat;
            }
        });
    }
    

    #endregion

    #region Life Things

    public GenericLifeSystem Life() => lifesystem;
    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype dmgtype)
    {
        SetTarget(entityTarget);

        if (cooldown || Invinsible || sm.Current.Name == "Die") return Attack_Result.inmune;

        // Debug.Log("damagetype" + dmgtype.ToString()); ;

        Vector3 aux = this.transform.position - attack_pos;
        aux.Normalize();
        rb = GetComponent<Rigidbody>();
        if (dmgtype == Damagetype.explosion)
        {
            Debug.Log(rb);
            rb.AddForce(aux * explosionForce, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(aux * forceRecall, ForceMode.Impulse);
        }
        StartCoroutine(OnHitted(myMat, onHitFlashTime, onHitColor));

        sm.SendInput(DummyEnemyInputs.TAKE_DAMAGE);

        greenblood.Play();

        AudioManager.instance.PlaySound(takeHit_audioName);
        
        cooldown = true;
        bool death = lifesystem.Hit(dmg);
        return death ? Attack_Result.death : Attack_Result.sucessful;
    }

    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype, EntityBase owner_entity)
    {

        if (sm.Current.Name == "Die") return Attack_Result.inmune;

        if (sm.Current.Name != "Attack" && entityTarget != owner_entity)
        {
            attacking = false;
            //if (entityTarget == null) throw new System.Exception("entity target es null");//esto rompe cuando vengo desde el Damage in Room
            director.ChangeTarget(this, owner_entity, entityTarget);
        }

        return TakeDamage(dmg, attack_pos, damagetype);
    }

    protected override void TakeDamageFeedback(DamageData data)
    {
        if (sm.Current.Name == "Idle" || sm.Current.Name == "Persuit")
        {
            attacking = false;
            director.ChangeTarget(this, data.owner, entityTarget);
        }

        AudioManager.instance.PlaySound(takeHit_audioName);

        sm.SendInput(DummyEnemyInputs.TAKE_DAMAGE);

        greenblood.Play();
        cooldown = true;

        StartCoroutine(OnHitted(myMat, onHitFlashTime, onHitColor));
    }

    protected override void Die(Vector3 dir)
    {
        sm.SendInput(DummyEnemyInputs.DIE);
        if (dir == Vector3.zero)
            ragdoll.Ragdoll(true, -rootTransform.forward);
        else
            ragdoll.Ragdoll(true, dir);
        death = true;
        director.RemoveTarget(this);
        Main.instance.RemoveEntity(this);

        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_DEAD, new object[] { transform.position, petrified, expToDrop });
    }

    protected override bool IsDamage()
    {
        if (cooldown || Invinsible || sm.Current.Name == "Die") return true;
        else return false;
    }
    #endregion

    protected override void OnFixedUpdate() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }

    #region STATE MACHINE THINGS
    public enum DummyEnemyInputs { IDLE, BEGIN_ATTACK,ATTACK, GO_TO_POS, DIE, DISABLE, TAKE_DAMAGE, PETRIFIED, PARRIED, CHASING };
    void SetStates()
    {
        var idle = new EState<DummyEnemyInputs>("Idle");
        var goToPos = new EState<DummyEnemyInputs>("Follow");
        var chasing = new EState<DummyEnemyInputs>("Chasing");
        var beginAttack = new EState<DummyEnemyInputs>("Begin_Attack");
        var attack = new EState<DummyEnemyInputs>("Attack");
        var parried = new EState<DummyEnemyInputs>("Parried");
        var takeDamage = new EState<DummyEnemyInputs>("Take_Damage");
        var die = new EState<DummyEnemyInputs>("Die");
        var disable = new EState<DummyEnemyInputs>("Disable");
        var petrified = new EState<DummyEnemyInputs>("Petrified");

        ConfigureState.Create(idle)
            .SetTransition(DummyEnemyInputs.GO_TO_POS, goToPos)
            .SetTransition(DummyEnemyInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .SetTransition(DummyEnemyInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(goToPos)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .SetTransition(DummyEnemyInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(chasing)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.GO_TO_POS, goToPos)
            .SetTransition(DummyEnemyInputs.BEGIN_ATTACK, beginAttack)
            .SetTransition(DummyEnemyInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(beginAttack)
            .SetTransition(DummyEnemyInputs.ATTACK, attack)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.PARRIED, parried)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(attack)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.PARRIED, parried)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(parried)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(petrified)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            //.SetTransition(DummyEnemyInputs.ATTACK, attack)
            .SetTransition(DummyEnemyInputs.BEGIN_ATTACK, beginAttack)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .Done();

        ConfigureState.Create(die)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .Done();

        sm = new EventStateMachine<DummyEnemyInputs>(idle, DebugState);

        var head = Main.instance.GetChar();

        //Asignando las funciones de cada estado
        new DummyIdleState(idle, sm, movement, distancePos, normalDistance, this).SetAnimator(animator).SetRoot(rootTransform).SetDirector(director);

        new DummyFollowState(goToPos, sm, movement, normalDistance, distancePos, this).SetAnimator(animator).SetRoot(rootTransform);

        new DummyChasing(chasing, sm, IsAttack, distancePos, movement, this).SetDirector(director).SetRoot(rootTransform);

        new DummyAttAnt(beginAttack, sm, movement, this).SetAnimator(animator).SetDirector(director).SetRoot(rootTransform);

        new DummyAttackState(attack, sm, cdToAttack, this).SetAnimator(animator).SetDirector(director);

        new DummyParried(parried, sm, parriedTime, this).SetAnimator(animator).SetDirector(director);

        new DummyTDState(takeDamage, sm, recallTime).SetAnimator(animator);

        new DummyStunState(petrified, sm, StartStun, TickStun, EndStun);

        new DummyDieState(die, sm, ragdoll, myGroundParticle).SetAnimator(animator).SetDirector(director).SetRigidbody(rb);

        new DummyDisableState(disable, sm, EnableObject, DisableObject);
    }

    void StartStun(EState<DummyEnemyInputs> input) => EnterStun(input);

    void TickStun(string name) => UpdateStun(name);

    void EndStun(DummyEnemyInputs input) => ExitStun(input);

    void DisableObject()
    {
        canupdate = false;
        movement.SetDefaultSpeed();
        combat = false;
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
