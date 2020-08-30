using System.Collections;
using System.Collections.Generic;
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

            anim.SetBool("TakeDamage", true);
        }

        protected override void Exit(CrowEnemy.CrowInputs input)
        {
            base.Exit(input);
            anim.SetBool("TakeDamage", false);
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
