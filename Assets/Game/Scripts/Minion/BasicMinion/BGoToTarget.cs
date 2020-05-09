using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class BGoToTarget : BMinionStates
    {
        GenericEnemyMove move;
        float distance;
        float distanceOwner;
        EntityBase owner;

        public BGoToTarget(EState<BasicMinion.BasicMinionInput> myState, EventStateMachine<BasicMinion.BasicMinionInput> _sm, GenericEnemyMove _move,
                           float _distance, float _distanceOwner, EntityBase _owner) : base(myState, _sm)
        {
            move = _move;
            distance = _distance;
            distanceOwner = _distanceOwner;
            owner = _owner;
        }

        protected override void Enter(EState<BasicMinion.BasicMinionInput> lastState)
        {
            base.Enter(lastState);
            anim.SetFloat("move", 0.3f);
        }

        protected override void Update()
        {
            base.Update();
            if (minion.CurrentTargetPos() == null)
            {
                if (minion.CurrentTarget() != null)
                {
                    Vector3 dirForward = (minion.CurrentTarget().transform.position - root.position).normalized;
                    Vector3 fowardRotation = move.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));

                    move.Rotation(fowardRotation);
                    move.MoveWRigidbodyV(fowardRotation);

                    if (Vector3.Distance(minion.CurrentTarget().transform.position, root.position) <= distance)
                        sm.SendInput(BasicMinion.BasicMinionInput.IDLE);
                }
                else
                {
                    Vector3 dirForward = (owner.transform.position - root.position).normalized;
                    Vector3 fowardRotation = move.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));

                    move.Rotation(fowardRotation);
                    move.MoveWRigidbodyV(fowardRotation);

                    if (Vector3.Distance(minion.CurrentTarget().transform.position, root.position) <= distanceOwner)
                        sm.SendInput(BasicMinion.BasicMinionInput.IDLE);
                }
            }
            else
            {
                Vector3 dir = minion.CurrentTargetPos().position - root.position;
                dir.Normalize();

                Vector3 dirFix = move.ObstacleAvoidance(new Vector3(dir.x, 0, dir.z));

                move.Rotation(dirFix);
                move.MoveWRigidbodyV(dirFix);

                float distanceX = Mathf.Abs(minion.CurrentTargetPos().transform.position.x - root.position.x);
                float distanceZ = Mathf.Abs(minion.CurrentTargetPos().transform.position.z - root.position.z);

                if (distanceX < 0.7f && distanceZ < 0.7f)
                    sm.SendInput(BasicMinion.BasicMinionInput.IDLE);
            }

        }

        protected override void Exit(BasicMinion.BasicMinionInput input)
        {
            base.Exit(input);
            move.StopMove();
            anim.SetFloat("move", 0);
        }
    }
}
