using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class GoatStompAnt : JabaliStates
    {
        float anticipationTime;
        float timer = 0;
        public GoatStompAnt(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float antTime) : base(myState, _sm)
        {
            anticipationTime = antTime;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            anim.SetBool("StompAttack", true);
        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= anticipationTime)
                sm.SendInput(JabaliEnemy.JabaliInputs.HEAD_ATTACK);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            anim.SetBool("StompAttack", false);
            timer = 0;
        }
    }
}
