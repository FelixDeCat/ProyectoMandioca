using System;

namespace ProyectTools.StateMachine
{
    public class DummyDisableState : DummyEnemyStates
    {
        Action Active;
        Action Desactive;

        public DummyDisableState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm, Action _act, Action _desac) : base(myState, _sm)
        {
            Active += _act;
            Desactive += _desac;
        }

        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> input)
        {
            base.Enter(input);

            //Desactivo el objeto con todo lo que tengo que hacer cuando salgo de la room
            Desactive();
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);

            //Activo el objeto con lo que tenga que hacer cuando entro a la room
            Active();
        }
    }
}
