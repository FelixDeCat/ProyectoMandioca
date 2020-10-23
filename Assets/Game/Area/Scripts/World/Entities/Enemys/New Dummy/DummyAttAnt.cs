using UnityEngine;

namespace Tools.StateMachine
{
    public class DummyAttAnt : DummyEnemyStates
    {
        GenericEnemyMove move;
        ICombatDirector enemy;

        public DummyAttAnt(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                           GenericEnemyMove _move, ICombatDirector _enemy) : base(myState, _sm)
        {
            enemy = _enemy;
            move = _move;
        }

        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> input)
        {
            base.Enter(input);

            anim.SetBool("Attack", true);
            anim.SetTrigger("AttackTrigger");
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);
        }

        protected override void Update()
        {
            base.Update();

            if (enemy.CurrentTarget() != null)
            {
                Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);
                move.Rotation(forwardRotation);
            }
        }
    }
}
