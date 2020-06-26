using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Tools.StateMachine
{
    public class BGoToTarget : BMinionStates
    {
        GenericEnemyMove move;
        float maxDistance;
        float minDistance;
        float distanceOwner;
        EntityBase owner;

        public BGoToTarget(EState<BasicMinion.BasicMinionInput> myState, EventStateMachine<BasicMinion.BasicMinionInput> _sm, GenericEnemyMove _move,
                           float _distance, float _minDistance,float _distanceOwner, EntityBase _owner) : base(myState, _sm)
        {
            move = _move;
            maxDistance = _distance;
            minDistance = _minDistance;
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
            if (!minion.IsInPos())
            {
                if (minion.CurrentTarget() != null)
                {
                    Vector3 dirForward = (minion.CurrentTarget().transform.position - root.position).normalized;
                    Vector3 fowardRotation = move.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));

                    move.Rotation(fowardRotation.normalized);
                    move.MoveWRigidbodyV(fowardRotation);

                    if (Vector3.Distance(minion.CurrentTarget().transform.position, root.position) <= maxDistance)
                        sm.SendInput(BasicMinion.BasicMinionInput.IDLE);
                }
                else
                {
                    Vector3 dirForward = (owner.transform.position - root.position).normalized;
                    Vector3 fowardRotation = move.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));

                    move.Rotation(fowardRotation.normalized);
                    move.MoveWRigidbodyV(fowardRotation);

                    if (Vector3.Distance(owner.transform.position, root.position) <= distanceOwner)
                        sm.SendInput(BasicMinion.BasicMinionInput.IDLE);
                }
            }
            else
            {
                Vector3 dirForward = (minion.CurrentTarget().transform.position - root.position).normalized;
                Vector3 fowardRotation = move.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));

                move.Rotation(fowardRotation.normalized);
                move.MoveWRigidbodyV(fowardRotation);

                Vector3 targetPos = minion.CurrentTarget().transform.position;
                Vector3 myPos = root.position;

                if (Vector3.Distance(new Vector3(targetPos.x,0,targetPos.z), new Vector3(myPos.x, 0, myPos.z)) <= minDistance)
                    sm.SendInput(BasicMinion.BasicMinionInput.CHASING);
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
