using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class BTakeDmg : BMinionStates
    {
        float timer;
        float recallTime;

        public BTakeDmg(EState<BasicMinion.BasicMinionInput> myState, EventStateMachine<BasicMinion.BasicMinionInput> _sm, float _recall) : base(myState, _sm)
        {
            recallTime = _recall;
        }

        protected override void Enter(EState<BasicMinion.BasicMinionInput> lastState)
        {
            base.Enter(lastState);
            anim.SetBool("takeDamage", true);
        }

        protected override void Update()
        {
            base.Update();
            timer += Time.deltaTime;

            if (timer >= recallTime)
                sm.SendInput(BasicMinion.BasicMinionInput.IDLE);
        }

        protected override void Exit(BasicMinion.BasicMinionInput input)
        {
            base.Exit(input);

            anim.SetBool("takeDamage", false);
            timer = 0;
        }
    }
}
