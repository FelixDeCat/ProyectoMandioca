using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Minion : Companion
{
    [SerializeField] CombatComponent combatComponent;
    [SerializeField] int damage = 5;

    [SerializeField] ParticleSystem greenblood;

    public float time_stun;

    public AnimEvent anim;
    StatesMachine sm;
    public Animator animator;
    [SerializeField] private float _speedMovement = 5;
    [SerializeField] private float _rotSpeed = 3;
    [SerializeField] private float _petrifiedTime = 4;
    [SerializeField] private float _distance = 3;
    //public Follow follow;

    public Rigidbody _rb;

    [Header("Life Options")]
    [SerializeField] GenericLifeSystem lifesystem = null;

    StatesAttack attackState;
    StatesFollow followState;

    protected override void OnInitialize()
    {
        Main.instance.eventManager.TriggerEvent(GameEvents.MINION_SPAWN, new object[] { this });

        _rb = GetComponent<Rigidbody>();
        combatComponent.Configure(AttackEntity);
        Main.instance.GetCombatDirector().AddNewTarget(this);
        lifesystem.AddEventOnDeath(OnDead);
        anim.Add_Callback("DealDamage", DealDamage);
        
    }

    void SetStateMachine()
    {
        sm = new StatesMachine();
        sm.Addstate(new StatesWander(sm));
        followState = new StatesFollow(sm, transform, _rb, FindObjectOfType<TrueDummyEnemy>().transform, animator, _rotSpeed, _distance, _speedMovement);
        sm.Addstate(followState);
        attackState = new StatesAttack(sm, animator, transform, FindObjectOfType<TrueDummyEnemy>().transform, _rotSpeed, _distance);
        sm.Addstate(attackState);
        sm.Addstate(new StatesPetrified(sm, _petrifiedTime));
        sm.ChangeState<StatesWander>();
    }

    public void ChangeToAttackState(Transform parriedEnemy)
    {
        attackState.ChangeTarget(parriedEnemy);
        followState.ChangeTarget(parriedEnemy);
        sm.ChangeState<StatesFollow>();
    }
    
    protected override void OnUpdateEntity() => sm.Update();
    public void DealDamage() => combatComponent.ManualTriggerAttack();
    public void EndStun() => combatComponent.Play();
    public void Petrified() => sm.ChangeState<StatesPetrified>();
    public void AttackEntity(EntityBase e) => e.TakeDamage(damage, transform.position, Damagetype.parriable, this);

    public override Attack_Result TakeDamage(int dmg, Vector3 dir, Damagetype dmgtype)
    {
        greenblood.Play();
        bool death = lifesystem.Hit(dmg);
        return death ? Attack_Result.death : Attack_Result.sucessful;
    }

    public float ChangeSpeed(float newSpeed)
    {
        //Si le mando negativo me devuelve la original
        //para guardarla en el componente WebSlowedComponent
        if (newSpeed < 0)
            return _speedMovement;

        //Busco el estado follow para poder cambiarle la velocidad
        StatesFollow statesFollow = sm.GetState<StatesFollow>();
        if (statesFollow != null)
        {
            statesFollow.ChangeSpeed(newSpeed);
        }

        return _speedMovement;
    }

    /////////////////////////////////////////////////////////////////
    //////  En desuso
    /////////////////////////////////////////////////////////////////
    void OnDead() { }
    protected override void OnResume() { }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
}
