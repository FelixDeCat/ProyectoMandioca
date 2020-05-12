using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace ToolsMandioca.StateMachine
{
    public class DummyFollowState : DummyEnemyStates
    {
        GenericEnemyMove move;

        TrueDummyEnemy noObs;

        float normalDistance;

        public DummyFollowState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm, GenericEnemyMove _move,
                                float distance, TrueDummyEnemy me) : base(myState, _sm)
        {
            move = _move;
            normalDistance = distance;
            noObs = me;
        }

        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> input)
        {
            base.Enter(input);
            anim.SetFloat("move", 0.3f);
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);

            move.StopMove();
            anim.SetFloat("move", 0);
        }

        protected override void Update()
        {
            base.Update();

            if (noObs.CurrentTargetPosDir() == null)
            {
                if (noObs.CurrentTarget() != null)
                {
                    Vector3 dirForward = (noObs.CurrentTarget().transform.position - root.position).normalized;
                    Vector3 fowardRotation = move.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));

                    move.Rotation(fowardRotation);
                    move.MoveWRigidbodyV(fowardRotation);

                    if (Vector3.Distance(noObs.CurrentTarget().transform.position, root.position) <= normalDistance)
                        sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
                }
            }
            else
            {
                Vector3 dir = noObs.CurrentTargetPos() - root.position;
                dir.Normalize();

                Vector3 dirFix = move.ObstacleAvoidance(new Vector3(dir.x, 0, dir.z));

                move.Rotation(dirFix);
                move.MoveWRigidbodyV(dirFix);

                float distanceX = Mathf.Abs(noObs.CurrentTargetPos().x - root.position.x);
                float distanceZ = Mathf.Abs(noObs.CurrentTargetPos().z - root.position.z);

                if (distanceX < 0.7f && distanceZ < 0.7f)
                    sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
            }

        }
    }
}
