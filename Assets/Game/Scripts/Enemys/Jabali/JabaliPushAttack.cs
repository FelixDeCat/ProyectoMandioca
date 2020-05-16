using UnityEngine;
using System;

namespace ToolsMandioca.StateMachine
{
    public class JabaliPushAttack : JabaliStates
    {
        float pushSpeed;
        Action DealDamage;
        Action PlayCombat;
        float maxSpeed;
        GameObject feedbackCharge;

        public JabaliPushAttack(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _speed,
                                Action _DealDamage, GameObject _feedbackCharge, Action _PlayCombat) : base(myState, _sm)
        {
            maxSpeed = _speed;
            pushSpeed = maxSpeed / 2;
            DealDamage = _DealDamage;
            feedbackCharge = _feedbackCharge;
            PlayCombat = _PlayCombat;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            base.Enter(input);
            feedbackCharge.SetActive(true);
            feedbackCharge.GetComponent<ParticleSystem>().Play();
            PlayCombat();
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
            feedbackCharge.SetActive(false);
            feedbackCharge.GetComponent<ParticleSystem>().Stop();
            base.Exit(input);
            rb.velocity = Vector3.zero;
        }
    }
}

