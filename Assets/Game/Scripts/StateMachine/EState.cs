using System.Collections.Generic;
using System;

namespace ProyectTools.StateMachine
{
    public class EState<T>
    {
        public Action<EState<T>> OnEnter = delegate { };
        public Action OnUpdate = delegate { };
        public Action OnLateUpdate = delegate { };
        public Action OnFixedUpdate = delegate { };
        public Action<T> OnExit = delegate { };
        Dictionary<T, TransitionState<T>> transitions;

        public void Enter(EState<T> lastState) { OnEnter(lastState); }
        public void Update() { OnUpdate(); }
        public void LateUpdate() { OnLateUpdate(); }
        public void FixedUpdate() { OnFixedUpdate(); }
        public void Exit(T input) { OnExit(input); }


        public string Name { get; private set; }

        public EState(string _name)
        {
            Name = _name;
        }

        public EState<T> Configure(Dictionary<T, TransitionState<T>> transitions)
        {
            this.transitions = transitions;
            return this;
        }

        //// Esto lo vamos a usar para que desde afuera nosotros 
        //// podamos asignarle funciones o Lambdas al Evento "OnTransition"
        //public TransitionState<T> GetTransition(T input)
        //{
        //    return transitions[input];
        //}

        public bool CheckInput(T input, out EState<T> next)
        {
            if (transitions.ContainsKey(input))
            {
                var transition = transitions[input];
                transition.OnTransitionExecute(input);
                next = transition.TargetState;
                return true;
            }
            next = this;
            return false;
        }
    }
}