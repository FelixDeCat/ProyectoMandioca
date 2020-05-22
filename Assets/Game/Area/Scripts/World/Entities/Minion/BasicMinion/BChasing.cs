using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ToolsMandioca.StateMachine
{
    public class BChasing : BMinionStates
    {
        Func<bool> IsAttack;
        float distanceToNormalAttack;
        GenericEnemyMove move;

        public BChasing(EState<BasicMinion.BasicMinionInput> myState, EventStateMachine<BasicMinion.BasicMinionInput> _sm, Func<bool> _IsAttack,
                            float _distanceToAttack, GenericEnemyMove _move) : base(myState, _sm)
        {
            IsAttack = _IsAttack;
            distanceToNormalAttack = _distanceToAttack;
            move = _move;
        }

        protected override void Enter(EState<BasicMinion.BasicMinionInput> _lastState)
        {
            base.Enter(_lastState);
            combatDirector.PrepareToAttack(minion, minion.CurrentTarget());
        }

        protected override void Update()
        {
            base.Update();
            if (!minion.CurrentTarget())
                sm.SendInput(BasicMinion.BasicMinionInput.IDLE);
            else
            {
                if (IsAttack())
                    sm.SendInput(BasicMinion.BasicMinionInput.BEGIN_ATTACK);
                else
                {
                    Vector3 pos1 = new Vector3(root.position.x, 0, root.position.z);
                    Vector3 pos2 = new Vector3(minion.CurrentTarget().transform.position.x, 0, minion.CurrentTarget().transform.position.z);

                    Vector3 myForward = (minion.CurrentTarget().transform.position - root.position).normalized;
                    Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);

                    move.Rotation(forwardRotation);

                    if (Vector3.Distance(pos1, pos2) >= distanceToNormalAttack)
                        sm.SendInput(BasicMinion.BasicMinionInput.GO_TO_POS);
                }
            }
        }

        protected override void Exit(BasicMinion.BasicMinionInput input)
        {
            base.Exit(input);

            if(minion.CurrentTarget() != null)
                combatDirector.DeleteToPrepare(minion, minion.CurrentTarget());
        }

    }
}
