using UnityEngine;
using System;

namespace ProyectTools.StateMachine
{
    public class JabaliPushAttack : JabaliStates
    {
        float pushSpeed;
        Action DealDamage;
        float maxSpeed;

        public JabaliPushAttack(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _speed,
                                Action _DealDamage) : base(myState, _sm)
        {
            maxSpeed = _speed;
            pushSpeed = maxSpeed / 1.5f;
            DealDamage = _DealDamage;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            base.Enter(input);

            anim.SetTrigger("ChargeOk");
        }

        protected override void Update()
        {
            if (pushSpeed < maxSpeed)
            {
                pushSpeed += Time.deltaTime;

                if (pushSpeed > maxSpeed)
                    pushSpeed = maxSpeed;
            }

            rb.velocity = root.forward * pushSpeed;
            DealDamage();

            base.Update();
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            base.Exit(input);
            StopMove();
        }
    }
}

