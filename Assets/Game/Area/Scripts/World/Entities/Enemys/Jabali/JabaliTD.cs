using UnityEngine;

namespace Tools.StateMachine
{
    public class JabaliTD : JabaliStates
    {
        float timeToRecall;

        public JabaliTD(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _recall) : base(myState, _sm)
        {
            timeToRecall = _recall;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            anim.SetBool("TakeDamage", true);
            cdModule.AddCD("TakeDamageRecall", () => sm.SendInput(JabaliEnemy.JabaliInputs.IDLE), timeToRecall);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            anim.SetBool("TakeDamage", false);
            cdModule.EndCDWithoutExecute("TakeDamageRecall");
        }
    }
}
