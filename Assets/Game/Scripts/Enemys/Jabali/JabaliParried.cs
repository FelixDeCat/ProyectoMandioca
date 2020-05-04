using UnityEngine;

namespace Tools.StateMachine
{
    public class JabaliParried : JabaliStates
    {
        float timeParry;
        float timer;

        public JabaliParried(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _timeParried) : base(myState, _sm)
        {
            timeParry = _timeParried;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            base.Enter(input);

            anim.SetBool("Parried", true);
        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= timeParry)
                sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.PETRIFIED)
            {
                timer = 0;
                anim.SetBool("Parried", false);
            }
        }
    }
}

