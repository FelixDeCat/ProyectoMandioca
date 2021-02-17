using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class DummyDieState<T> : StatesFunctions<T>
    {
        RagdollComponent ragdoll;
        Action OnDead;
        Action OnDissappear;
        CDModule cdModule;

        public DummyDieState(EState<T> myState, EventStateMachine<T> _sm, RagdollComponent _ragdoll,
                             Action _OnDead, Action _OnDissappear, CDModule _cdModule) : base(myState, _sm)
        {
            OnDead = _OnDead;
            ragdoll = _ragdoll;
            OnDissappear = _OnDissappear;
            cdModule = _cdModule;
        }

        protected override void Enter(EState<T> lastState)
        {
            cdModule.AddCD("DesactiveBones", () =>
            {
                OnDead?.Invoke();
                ragdoll.DesactiveBones();
            }, 5);

            cdModule.AddCD("Dead", () => OnDissappear?.Invoke(), 8);
        }

        protected override void Exit(T input)
        {
        }

        protected override void FixedUpdate()
        {
        }

        protected override void LateUpdate()
        {
        }

        protected override void Update()
        {
        }
    }
}
