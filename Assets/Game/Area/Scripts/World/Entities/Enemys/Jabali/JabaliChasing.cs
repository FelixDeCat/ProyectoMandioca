using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ToolsMandioca.StateMachine
{
    public class JabaliChasing : JabaliStates
    {
        Func<bool> IsAttack;
        Func<bool> ChargeOk;
        float distanceToPush;
        float distanceToNormalAttack;
        GenericEnemyMove move;

        public JabaliChasing(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, Func<bool> _IsAttack,
                             Func<bool> _ChargeOk, float _distanceToPush, float _distanceToNormalAttack, GenericEnemyMove _move) : base(myState, _sm)
        {
            IsAttack = _IsAttack;
            ChargeOk = _ChargeOk;
            distanceToPush = _distanceToPush;
            distanceToNormalAttack = _distanceToNormalAttack;
            move = _move;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            base.Enter(input);
            combatDirector.PrepareToAttack(enemy, enemy.CurrentTarget());
        }

        protected override void Update()
        {
            if (!enemy.CurrentTarget())
                sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);

            if (IsAttack())
            {
                if (ChargeOk())
                    sm.SendInput(JabaliEnemy.JabaliInputs.CHARGE_PUSH);
                else
                    sm.SendInput(JabaliEnemy.JabaliInputs.HEAD_ANTICIP);
            }
            else
            {
                Vector3 pos1 = new Vector3(root.position.x, 0, root.position.z);
                Vector3 pos2 = new Vector3(enemy.CurrentTarget().transform.position.x, 0, enemy.CurrentTarget().transform.position.z);

                Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);

                move.Rotation(forwardRotation);
                if (ChargeOk())
                {
                    if (Vector3.Distance(pos1, pos2) >= distanceToPush)
                        sm.SendInput(JabaliEnemy.JabaliInputs.PERSUIT);
                }
                else
                {
                    if (Vector3.Distance(pos1, pos2) >= distanceToNormalAttack)
                        sm.SendInput(JabaliEnemy.JabaliInputs.PERSUIT);
                }
            }
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            base.Exit(input);
            if(enemy.CurrentTarget()!=null)
                combatDirector.DeleteToPrepare(enemy, enemy.CurrentTarget());
        }
    }
}
