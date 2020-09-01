using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CrowTakeDmg : CrowStates
    {
        float timer;
        float tdRecall;

        public CrowTakeDmg(EState<CrowEnemy.CrowInputs> myState, EventStateMachine<CrowEnemy.CrowInputs> _sm, float _tdRecall) : base(myState, _sm)
        {
            tdRecall = _tdRecall;
        }

        protected override void Enter(EState<CrowEnemy.CrowInputs> last)
        {
            base.Enter(last);

            anim.SetTrigger("takeDmg");
            anim.SetBool("rotate", false);
        }

        protected override void Exit(CrowEnemy.CrowInputs input)
        {
            base.Exit(input);

            timer = 0;
        }

        protected override void Update()
        {
            base.Update();

            timer += Time.deltaTime;

            if (timer >= tdRecall) sm.SendInput(CrowEnemy.CrowInputs.IDLE);
        }
    }
}
