using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class DummyIdleState : DummyEnemyStates
    {
        float distanceMin;
        float distanceMax;
        ICombatDirector enemy;
        GenericEnemyMove move;

        public DummyIdleState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm, GenericEnemyMove _move,
                              float _disInCom, float _disNormal,  ICombatDirector _enemy) : base(myState, _sm)
        {
            move = _move;
            distanceMax = _disNormal;
            distanceMin = _disInCom;
            enemy = _enemy;
        }

        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> last)
        {
            base.Enter(last);
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);
        }

        protected override void Update()
        {
            base.Update();

            if(enemy.CurrentTarget() != null)
            {
                Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);

                move.Rotation(forwardRotation);

                if (enemy.IsInPos())
                {
                    Vector3 pos1 = new Vector3(root.position.x, 0, root.position.z);
                    Vector3 pos2 = new Vector3(enemy.CurrentTarget().transform.position.x, 0, enemy.CurrentTarget().transform.position.z);

                    if (Vector3.Distance(pos1, pos2) >= distanceMin)
                        sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.GO_TO_POS);
                    else
                        sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.CHASING);
                }
                else
                {
                    Vector3 pos1 = new Vector3(root.position.x, 0, root.position.z);
                    Vector3 pos2 = new Vector3(enemy.CurrentTarget().transform.position.x, 0, enemy.CurrentTarget().transform.position.z);

                    if (Vector3.Distance(pos1, pos2) >= distanceMax)
                        sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.GO_TO_POS);
                }
            }
        }
    }
}
