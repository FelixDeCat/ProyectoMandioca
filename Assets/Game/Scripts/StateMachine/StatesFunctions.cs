using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsMandioca.StateMachine;
using System;

namespace ToolsMandioca.StateMachine
{
    public abstract class StatesFunctions<T>
    {
        protected EventStateMachine<T> sm;

        public StatesFunctions(EState<T> myState, EventStateMachine<T> _sm)
        {
            myState.OnEnter += Enter;

            myState.OnUpdate += Update;

            myState.OnLateUpdate += LateUpdate;

            myState.OnFixedUpdate += FixedUpdate;

            myState.OnExit += Exit;

            sm = _sm;
        }

        protected abstract void Enter(EState<T> lastState);

        protected abstract void Update();

        protected abstract void LateUpdate();

        protected abstract void FixedUpdate();

        protected abstract void Exit(T input);

    }
}
