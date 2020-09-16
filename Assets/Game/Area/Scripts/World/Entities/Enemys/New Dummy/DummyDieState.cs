using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class DummyDieState : DummyEnemyStates
    {
        RagdollComponent ragdoll;
        Action OnDead;
        float timer;
        bool desactive;
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
            combatDirector.DeadEntity(ragdoll.GetComponent<EnemyBase>(), ragdoll.GetComponent<EnemyBase>().CurrentTarget());
        }

        protected override void Update()
        {
            base.Update();

            timer += Time.deltaTime;

            if (timer >= 5)
            {
                if (!desactive)
                {
                    OnDead?.Invoke();
                    ragdoll.DesactiveBones();
                    desactive = true;
                }
            }

            if (timer >= 8)
            {
                timer = 0;
                desactive = false;
                OnDissappear?.Invoke();
            }
        }
    }
}
