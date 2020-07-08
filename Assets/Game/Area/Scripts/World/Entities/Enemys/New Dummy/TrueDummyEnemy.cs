using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;
using UnityEngine.Serialization;

public class TrueDummyEnemy : EnemyBase
{
    //PARA FRANCISCO (o el que tome la activa)
    //podés ir directamente a la region STATE MACHINE THINGS, ahí dejé algunas cosas que podés modificar y cosas que deberías agregar para que funcione.
    //La lógica de la habilidad podes tenerla donde quieras, pero deberías ejecutarla en el estado que te dejé creado. También si querés hacerlo de otra
    //manera o modificar cierto comportamiento, podés hacerlo, pero es un re quilombo, así que lo dejo a tu criterio. Con lo que te dejé armado, en teoría
    //debería de funcionar si sólo agregas la lógica de la activa y hacés lo que te dejé comentado. Dejo alto comentario acá, pero lo más seguro es que esté
    //en las reuniones del viernes, sábado y domingo, pero por las dudas. 


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
    //[SerializeField] private bool hasSpecialAttack;
    //[SerializeField] public bool isSpecialInCD;
    //[SerializeField] private CorruptedVine specialAttack_pf;
    //[SerializeField] private float _specialAttackCount_CD;
    //[SerializeField] private float specialAttack_CD;
    public DummySpecialAttack dummySpecialAttack;

    private CombatDirector director;

    [Header("Life Options")]
    [SerializeField] float recallTime = 1;
    private Action<EState<DummyEnemyInputs>> EnterStun;
    private Action<string> UpdateStun;
    private Action<DummyEnemyInputs> ExitStun;

    private bool cooldown = false;
    private float timercooldown = 0;

    [Header("Feedback")]
    [SerializeField] AnimEvent anim = null;
    [SerializeField] Animator animator = null;
    private Material[] myMat;
    [SerializeField] Color onHitColor;
    [SerializeField] float onHitFlashTime;
    [SerializeField] RagdollComponent ragdoll = null;
    private const string takeHit_audioName = "woodChop";

    [SerializeField] EffectBase petrifyEffect;
    EventStateMachine<DummyEnemyInputs> sm;

    public DataBaseDummyParticles particles;
    public DummyAudioClipsDataBase sounds;

    [System.Serializable]
    public class DataBaseDummyParticles
    {
        public ParticleSystem _spawnParticules = null;
        public ParticleSystem myGroundParticle = null;
        public ParticleSystem greenblood = null;
    }

    [System.Serializable]
    public class DummyAudioClipsDataBase
    {
        public AudioClip _takeHit_AC;
        public AudioClip clip_walkEnt;
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
        particles._spawnParticules.Play();
        var smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
            myMat = smr.materials;

        AudioManager.instance.GetSoundPool(takeHit_audioName, AudioGroups.GAME_FX, sounds._takeHit_AC);
        AudioManager.instance.GetSoundPool("WalkEnt", AudioGroups.GAME_FX, sounds.clip_walkEnt, true);

        rb = GetComponent<Rigidbody>();
        combatComponent.Configure(AttackEntity);
        anim.Add_Callback("DealDamage", DealDamage);
        movement.Configure(rootTransform, rb);

        StartDebug();

        Main.instance.AddEntity(this);

        IAInitialize(Main.instance.GetCombatDirector());
        
        //Hago el pool de las vines aca
        PoolManager.instance.GetObjectPool("CorruptedVines", dummySpecialAttack.specialAttack_pf);

        petrifyEffect?.AddStartCallback(() => sm.SendInput(DummyEnemyInputs.PETRIFIED));
        petrifyEffect?.AddEndCallback(() => sm.SendInput(DummyEnemyInputs.IDLE));
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

            //special attack CD no funciono como queria

            dummySpecialAttack.UpdateSpecialAttack();
        }
    }
    protected override void OnPause() { }
    protected override void OnResume() { }

    #region Attack
    public void DealDamage() { combatComponent.ManualTriggerAttack(); sm.SendInput(DummyEnemyInputs.ATTACK); }

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

    public override void ToAttack() => attacking = true;
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

    public override void Bashed()
    {
        sm.SendInput(DummyEnemyInputs.PARRIED);
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

        particles.greenblood.Play();
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
    public enum DummyEnemyInputs { IDLE, BEGIN_ATTACK,ATTACK, GO_TO_POS, DIE, DISABLE, TAKE_DAMAGE, PETRIFIED, PARRIED, CHASING, SPECIAL_ATTACK };
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
            .SetTransition(DummyEnemyInputs.SPECIAL_ATTACK, specialAttack) //transición del ataque especial
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

        ConfigureState.Create(specialAttack)  //Las transiciones del nuevo estado
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
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
            .SetTransition(DummyEnemyInputs.SPECIAL_ATTACK, specialAttack) //transición del ataque especial
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

        Func<bool> SpecialAttackReady = CanDoSpecialAttack;
        //Este Func lo tenes que modificar a tu antojo para que me dé true cuando está implicitamente listo para tirar la habilidad, y falso para cuando no.
        //Un ejemplo sería: () => isPlayerNear && isCdOver ? true : false;
        //Hacelo como se te haga más cómodo.

        new DummyIdleState(idle, sm, movement, distancePos, normalDistance, this).SetAnimator(animator).SetRoot(rootTransform).SetDirector(director);

        new DummyFollowState(goToPos, sm, movement, normalDistance, distancePos, this, SpecialAttackReady).SetAnimator(animator).SetRoot(rootTransform); //Estado que necesita el Func

        new DummyChasing(chasing, sm, IsAttack, distancePos, movement, this, SpecialAttackReady).SetDirector(director).SetRoot(rootTransform); //Estado que necesita el Func

        new DummyAttAnt(beginAttack, sm, movement, this).SetAnimator(animator).SetDirector(director).SetRoot(rootTransform);

        new DummyAttackState(attack, sm, cdToAttack, this).SetAnimator(animator).SetDirector(director);

        new Tools.StateMachine.DummySpecialAttack(this, specialAttack, sm, this).SetDirector(director); //Seteando el estado que nos compete. Agregale todas las variables que necesites.

        new DummyParried(parried, sm, parriedTime, this).SetAnimator(animator).SetDirector(director);

        new DummyTDState(takeDamage, sm, recallTime).SetAnimator(animator);

        new DummyStunState(petrified, sm, StartStun, TickStun, EndStun);

        new DummyDieState(die, sm, ragdoll, particles.myGroundParticle).SetAnimator(animator).SetDirector(director).SetRigidbody(rb);

        new DummyDisableState(disable, sm, EnableObject, DisableObject);
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

    void SpecialAttackCoolDown()
    {
        
    }

    void StartStun(EState<DummyEnemyInputs> input) => EnterStun?.Invoke(input);

    void TickStun(string name) => UpdateStun?.Invoke(name);

    void EndStun(DummyEnemyInputs input) => ExitStun?.Invoke(input);

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
