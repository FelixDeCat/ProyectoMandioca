﻿using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class DummyDieState : DummyEnemyStates
    {
        RagdollComponent ragdoll;
        Action OnDead;
        ParticleSystem particle;
        Action OnDissappear;

        public DummyDieState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm, RagdollComponent _ragdoll,
                             Action _OnDead, Action _OnDissappear) : base(myState, _sm)
        {
            OnDead = _OnDead;
            ragdoll = _ragdoll;
            OnDissappear = _OnDissappear;
        }

        public DummyDieState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm, RagdollComponent _ragdoll,
                     ParticleSystem _particle, Action _OnDissappear) : base(myState, _sm)
        {
            particle = _particle;
            ragdoll = _ragdoll;
            OnDissappear = _OnDissappear;
        }
        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> input)
        {
            particle?.gameObject.SetActive(false);
            combatDirector.DeadEntity(ragdoll.GetComponent<CombatDirectorElement>(), ragdoll.GetComponent<CombatDirectorElement>().CurrentTarget());
            cdModule.AddCD("DesactiveBones", () =>
            {
                OnDead?.Invoke();
                ragdoll.DesactiveBones();
            }, 5);

            cdModule.AddCD("Dead", () => OnDissappear?.Invoke(), 8);
        }
    }
}
