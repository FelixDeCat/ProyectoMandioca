using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class GoatStompAnt : JabaliStates
    {
        float anticipationTime;
        public GoatStompAnt(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float antTime) : base(myState, _sm)
        {
            anticipationTime = antTime;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            anim.SetBool("StompAttack", true);
            cdModule.AddCD("AnticipationDuration", () => sm.SendInput(JabaliEnemy.JabaliInputs.HEAD_ATTACK), anticipationTime);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            anim.SetBool("StompAttack", false);
            cdModule.EndCDWithoutExecute("AnticipationDuration");
        }
    }
}
