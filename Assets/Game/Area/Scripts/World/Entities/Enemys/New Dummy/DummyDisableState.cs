using System;
using UnityEngine;

namespace Tools.StateMachine
{
    public class DummyDisableState<T> : StatesFunctions<T>
    {
        Action Active;
        Action Desactive;

        public DummyDisableState(EState<T> myState, EventStateMachine<T> _sm, Action _act, Action _desac) : base(myState, _sm)
        {
            Active += _act;
            Desactive += _desac;
        }

        protected override void Enter(EState<T> input)
        {
            //Desactivo el objeto con todo lo que tengo que hacer cuando salgo de la room
            Desactive();
        }

        protected override void Exit(T input)
        {
            //Activo el objeto con lo que tenga que hacer cuando entro a la room
            Active();
        }

        protected override void FixedUpdate()
        {

        }

        protected override void LateUpdate()
        {

        }

        protected override void Update()
        {
            Debug.Log("me quedo en el disable pá");
        }
    }
}
