using System;

namespace Tools.StateMachine
{
    using input = TrueDummyEnemy.DummyEnemyInputs;
    public class DummyStunState : DummyEnemyStates
    {
        Action<EState<input>> EnterStun;
        Action<string> UpdateStun;
        Action<input> ExitStun;

        public DummyStunState(EState<input> myState, EventStateMachine<input> _sm, Action<EState<input>> _Enter,
                          Action<string> _Update, Action<input> _Exit) : base(myState, _sm)
        {
            EnterStun = _Enter;
            UpdateStun = _Update;
            ExitStun = _Exit;
        }

        protected override void Enter(EState<input> input) { base.Enter(input); EnterStun(input); }
        protected override void Update() => UpdateStun(lastState.Name);
        protected override void Exit(input input) => ExitStun(input);
    }
}
