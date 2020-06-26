using System;
using UnityEngine;

namespace Tools.StateMachine
{
    public class JabaliStun : JabaliStates
    {
        Action<EState<JabaliEnemy.JabaliInputs>> EnterStun;
        Action<string> UpdateStun;
        Action<JabaliEnemy.JabaliInputs> ExitStun;

        public JabaliStun(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, Action<EState<JabaliEnemy.JabaliInputs>> _Enter,
                          Action<string> _Update, Action<JabaliEnemy.JabaliInputs> _Exit) : base(myState, _sm)
        {
            EnterStun = _Enter;
            UpdateStun = _Update;
            ExitStun = _Exit;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            base.Enter(input);

            EnterStun(input);
        }

        protected override void Update()
        {
            UpdateStun(lastState.Name);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            ExitStun(input);
        }
    }
}

