using UnityEngine;

namespace Tools.StateMachine
{
    public class JabaliParried : JabaliStates
    {
        float timeParry;

        public JabaliParried(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _timeParried) : base(myState, _sm)
        {
            timeParry = _timeParried;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            base.Enter(input);

            anim.SetBool("Parried", true);
            cdModule.AddCD("ParriedRecall", () => sm.SendInput(JabaliEnemy.JabaliInputs.IDLE), timeParry);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            cdModule.EndCDWithoutExecute("ParriedRecall");
            anim.SetBool("Parried", false);
        }
    }
}

