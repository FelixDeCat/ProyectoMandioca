using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ToolsMandioca.StateMachine
{
    using input = BasicMinion.BasicMinionInput;
    public class BStun : BMinionStates
    {
        Action<EState<input>> EnterStun;
        Action<string> UpdateStun;
        Action<input> ExitStun;

        public BStun(EState<input> myState, EventStateMachine<input> _sm, Action<EState<input>> _Enter,
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
