using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProyectTools.StateMachine
{
    public class JabaliCharge : JabaliStates
    {
        float chargeTime;
        float timer = 0;

        public JabaliCharge(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _chargeTime) : base(myState, _sm)
        {
            chargeTime = _chargeTime;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            if (input.Name != "Petrified")
            {
                anim.SetBool("ChargeAttack", true);
                combatDirector.RemoveToAttack(enemy, enemy.CurrentTarget());
            }
        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= chargeTime)
                sm.SendInput(JabaliEnemy.JabaliInputs.PUSH);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.PETRIFIED)
            {
                anim.SetBool("ChargeAttack", false);
                timer = 0;
            }
        }
    }
}
