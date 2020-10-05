using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class GoatTwoAttack : JabaliStates
    {
        float cdToAttack;
        float timer;
        string attackSound;
        int attackTimes;

        public GoatTwoAttack(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _cdToAttack, string _attackSound) : base(myState, _sm)
        {
            cdToAttack = _cdToAttack/2;
            attackSound = _attackSound;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            if (input.Name != "Petrified")
                anim.SetTrigger("HeadOk");

            AudioManager.instance.PlaySound(attackSound);
        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= cdToAttack)
            {
                if(attackTimes >= 1) sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
                else
                {
                    attackTimes += 1;
                    sm.SendInput(JabaliEnemy.JabaliInputs.HEAD_ANTICIP);
                }
            }
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.PETRIFIED)
            {
                if (input != JabaliEnemy.JabaliInputs.HEAD_ANTICIP)
                {
                    timer = 0;
                    cdToAttack /= 2;
                    var myEnemy = enemy;
                    myEnemy.Attacking = false;
                    combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
                    attackTimes = 0;
                    anim.SetInteger("AttackTime", 0);
                }
                else
                {
                    timer = 0;
                    cdToAttack *= 2;
                    anim.SetInteger("AttackTime", 1);
                }
            }
        }
    }
}
