using System;

namespace Tools.StateMachine
{
    using input = TrueDummyEnemy.DummyEnemyInputs;
    public class DummyStunState : DummyEnemyStates
    {
        public DummyStunState(EState<input> myState, EventStateMachine<input> _sm) : base(myState, _sm)
        {
        }
    }
}
