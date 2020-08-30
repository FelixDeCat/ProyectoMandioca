using System;

namespace Tools.StateMachine
{
    public class DummyStunState<T> : StatesFunctions<T>
    {
        public DummyStunState(EState<T> myState, EventStateMachine<T> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(EState<T> lastState)
        {
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
