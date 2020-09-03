using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.StateMachine;

using CaronteInputs = TrueDummyEnemy.DummyEnemyInputs;

public class Caronte : EnemyBase
{
    [Header("Move Options")]
    [SerializeField] GenericEnemyMove movement = null;
    [SerializeField] CharacterGroundSensor groundSensor = null;

    public AnimationCurve animEmisive;

    [Header("Combat Options")]
    [SerializeField] CombatComponent combatComponent = null;
    [SerializeField] int damage = 2;
    [SerializeField] float normalDistance = 8;
    [SerializeField] float cdToAttack = 1;
    [SerializeField] float parriedTime = 2;
    [SerializeField] float knockback = 20;

    


    public event Action OnDefeatCaronte;
    private CombatDirector _director;

    EventStateMachine<CaronteInputs> sm;

    public override void IAInitialize(CombatDirector director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
            sm.SendInput(CaronteInputs.IDLE);

        director.AddNewTarget(this);

        canupdate = true;
    }

    void SetStates()
    {
        var sleeping = new EState<CaronteInputs>("Sleeping");
        var idle = new EState<CaronteInputs>("Idle");
        var goToPos = new EState<CaronteInputs>("Follow");
        var chasing = new EState<CaronteInputs>("Chasing");
        var beginAttack = new EState<CaronteInputs>("Begin_Attack");
        var attack = new EState<CaronteInputs>("Attack");
        var parried = new EState<CaronteInputs>("Parried");
        var takeDamage = new EState<CaronteInputs>("Take_Damage");
        var die = new EState<CaronteInputs>("Die");
        var disable = new EState<CaronteInputs>("Disable");
        var petrified = new EState<CaronteInputs>("Petrified");

        ConfigureState.Create(idle)
            .SetTransition(CaronteInputs.GO_TO_POS, goToPos)
            .SetTransition(CaronteInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(CaronteInputs.DIE, die)
            .SetTransition(CaronteInputs.PETRIFIED, petrified)
            .SetTransition(CaronteInputs.DISABLE, disable)
            .SetTransition(CaronteInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(goToPos)
            .SetTransition(CaronteInputs.IDLE, idle)
            .SetTransition(CaronteInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(CaronteInputs.DIE, die)
            .SetTransition(CaronteInputs.PETRIFIED, petrified)
            .SetTransition(CaronteInputs.DISABLE, disable)
            .SetTransition(CaronteInputs.CHASING, chasing)
            .Done();

        ConfigureState.Create(chasing)
            .SetTransition(CaronteInputs.IDLE, idle)
            .SetTransition(CaronteInputs.GO_TO_POS, goToPos)
            .SetTransition(CaronteInputs.BEGIN_ATTACK, beginAttack)
            .SetTransition(CaronteInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(CaronteInputs.DIE, die)
            .SetTransition(CaronteInputs.PETRIFIED, petrified)
            .SetTransition(CaronteInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(beginAttack)
            .SetTransition(CaronteInputs.ATTACK, attack)
            .SetTransition(CaronteInputs.DIE, die)
            .SetTransition(CaronteInputs.PETRIFIED, petrified)
            .SetTransition(CaronteInputs.PARRIED, parried)
            .SetTransition(CaronteInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(attack)
            .SetTransition(CaronteInputs.IDLE, idle)
            .SetTransition(CaronteInputs.DIE, die)
            .SetTransition(CaronteInputs.PETRIFIED, petrified)
            .SetTransition(CaronteInputs.PARRIED, parried)
            .SetTransition(CaronteInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(parried)
            .SetTransition(CaronteInputs.IDLE, idle)
            .SetTransition(CaronteInputs.PETRIFIED, petrified)
            .SetTransition(CaronteInputs.DIE, die)
            .SetTransition(CaronteInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(petrified)
            .SetTransition(CaronteInputs.IDLE, idle)
            .SetTransition(CaronteInputs.BEGIN_ATTACK, beginAttack)
            .SetTransition(CaronteInputs.DIE, die)
            .SetTransition(CaronteInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(CaronteInputs.IDLE, idle)
            .SetTransition(CaronteInputs.DISABLE, disable)
            .SetTransition(CaronteInputs.PETRIFIED, petrified)
            .SetTransition(CaronteInputs.DIE, die)
            .Done();

        ConfigureState.Create(die)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(CaronteInputs.IDLE, idle)
            .Done();

        //sm = new EventStateMachine<CaronteInputs>(idle, DebugState);

        //var head = Main.instance.GetChar();

        //new DummyIdleState(idle, sm, movement, distancePos, normalDistance, this).SetAnimator(animator).SetRoot(rootTransform).SetDirector(director);

        //new DummyFollowState(goToPos, sm, movement, normalDistance, distancePos, this).SetAnimator(animator).SetRoot(rootTransform);

        //new DummyChasing(chasing, sm, IsAttack, distancePos, movement, this).SetDirector(director).SetRoot(rootTransform);

        //new DummyAttAnt(beginAttack, sm, movement, this).SetAnimator(animator).SetDirector(director).SetRoot(rootTransform);

        //new DummyAttackState(attack, sm, cdToAttack, this).SetAnimator(animator).SetDirector(director);

        //new DummyParried(parried, sm, parriedTime, this).SetAnimator(animator).SetDirector(director);

        //new DummyTDState(takeDamage, sm, recallTime).SetAnimator(animator);

        //new DummyStunState<CaronteInputs>(petrified, sm);

        //new DummyDieState(die, sm, ragdoll, OnDead).SetAnimator(animator).SetDirector(director).SetRigidbody(rb);

        //new DummyDisableState<CaronteInputs>(disable, sm, EnableObject, DisableObject);
    }

    public void ReturnToLife()
    {
        OnDefeatCaronte?.Invoke();
    }

    public override void ToAttack()
    {
        throw new NotImplementedException();
    }

    protected override void Die(Vector3 dir)
    {
        throw new NotImplementedException();
    }

    protected override bool IsDamage()
    {
        throw new NotImplementedException();
    }

    protected override void OnFixedUpdate()
    {
        throw new NotImplementedException();
    }

    protected override void OnPause()
    {
        throw new NotImplementedException();
    }

    protected override void OnReset()
    {
        throw new NotImplementedException();
    }

    protected override void OnResume()
    {
        throw new NotImplementedException();
    }

    protected override void OnTurnOff()
    {
        throw new NotImplementedException();
    }

    protected override void OnTurnOn()
    {
        throw new NotImplementedException();
    }

    protected override void OnUpdateEntity()
    {
        throw new NotImplementedException();
    }

    protected override void TakeDamageFeedback(DamageData data)
    {
        throw new NotImplementedException();
    }
}
