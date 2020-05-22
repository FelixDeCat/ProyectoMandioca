using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ToolsMandioca.StateMachine
{
    public class BIdle : BMinionStates
    {
        float distanceMin;
        float distanceMax;
        float distanceToOwner;
        GenericEnemyMove move;
        EntityBase owner;

        public BIdle(EState<BasicMinion.BasicMinionInput> myState, EventStateMachine<BasicMinion.BasicMinionInput> _sm, GenericEnemyMove _move,
                              float _disInCom, float _disNormal, EntityBase _owner, float _distanceToOwner) : base(myState, _sm)
        {
            owner = _owner;
            move = _move;
            distanceMax = _disNormal;
            distanceMin = _disInCom;
            distanceToOwner = _distanceToOwner;
        }

        protected override void Enter(EState<BasicMinion.BasicMinionInput> lastState)
        {
            base.Enter(lastState);
        }

        protected override void Update()
        {
            base.Update();

            if (minion.CurrentTarget() != null)
            {
                Vector3 myForward = (minion.CurrentTarget().transform.position - root.position).normalized;
                Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);

                move.Rotation(forwardRotation);

                if (minion.IsInPos())
                {
                    Vector3 pos1 = new Vector3(root.position.x, 0, root.position.z);
                    Vector3 pos2 = new Vector3(minion.CurrentTarget().transform.position.x, 0, minion.CurrentTarget().transform.position.z);

                    if (Vector3.Distance(pos1, pos2) >= distanceMin)
                        sm.SendInput(BasicMinion.BasicMinionInput.GO_TO_POS);
                    else
                        sm.SendInput(BasicMinion.BasicMinionInput.CHASING);
                }
                else
                {
                    Vector3 pos1 = new Vector3(root.position.x, 0, root.position.z);
                    Vector3 pos2 = new Vector3(minion.CurrentTarget().transform.position.x, 0, minion.CurrentTarget().transform.position.z);

                    if (Vector3.Distance(pos1, pos2) >= distanceMax)
                        sm.SendInput(BasicMinion.BasicMinionInput.GO_TO_POS);
                }
            }
            else
            {
                if(Vector3.Distance(root.position, owner.transform.position) >= distanceToOwner)
                    sm.SendInput(BasicMinion.BasicMinionInput.GO_TO_POS);
            }
        }

        protected override void Exit(BasicMinion.BasicMinionInput input)
        {
            base.Exit(input);
        }
    }
}
