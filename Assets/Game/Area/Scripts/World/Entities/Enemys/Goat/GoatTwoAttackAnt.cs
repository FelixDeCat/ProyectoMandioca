using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class GoatTwoAttackAnt : JabaliStates
    {
        float anticipationTime;
        GenericEnemyMove move;

        public GoatTwoAttackAnt(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm,
                                        GenericEnemyMove _move, float antTime) : base(myState, _sm)
        {
            move = _move;
            anticipationTime = antTime;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            anim.SetBool("HeadAttack", true);
            cdModule.AddCD("AnticipationDuration", () => sm.SendInput(JabaliEnemy.JabaliInputs.HEAD_ATTACK), anticipationTime);
        }

        protected override void Update()
        {
            if (enemy.CurrentTarget() != null)
            {
                Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);
                move.Rotation(forwardRotation);
            }
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            anim.SetBool("HeadAttack", false);
            cdModule.EndCDWithoutExecute("AnticipationDuration");
        }
    }
}
