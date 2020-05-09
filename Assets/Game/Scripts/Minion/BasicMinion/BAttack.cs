using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class BAttack : BMinionStates
    {
        float cd;
        float timer;

        public BAttack(EState<BasicMinion.BasicMinionInput> myState, EventStateMachine<BasicMinion.BasicMinionInput> _sm, float _cd) : base(myState, _sm)
        {
            cd = _cd;
        }

        protected override void Enter(EState<BasicMinion.BasicMinionInput> lastState)
        {
            base.Enter(lastState);
        }

        protected override void Update()
        {
            base.Update();

            timer += Time.deltaTime;
            if (timer >= cd) sm.SendInput(BasicMinion.BasicMinionInput.IDLE);
        }

        protected override void Exit(BasicMinion.BasicMinionInput input)
        {
            base.Exit(input);

            if (input != BasicMinion.BasicMinionInput.STUN)
            {
                timer = 0;
                anim.SetBool("Attack", false);
                var myEnemy = minion;
                myEnemy.attacking = false;
                combatDirector.AddToAttack(minion, minion.CurrentTarget());
            }
        }
    }
}
