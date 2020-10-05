using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class GoatTwoAttack : JabaliStates
    {
        float cdToAttackCurrent;
        float cdToAttackMinus;
        float cdToAttackSum;
        float timer;
        string attackSound;
        int attackTimes;

        public GoatTwoAttack(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _cdToAttack, string _attackSound) : base(myState, _sm)
        {
            cdToAttackMinus = _cdToAttack/2;
            cdToAttackSum = _cdToAttack;
            cdToAttackCurrent = cdToAttackMinus;
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

            if (timer >= cdToAttackCurrent)
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
                    cdToAttackCurrent = cdToAttackMinus;
                    var myEnemy = enemy;
                    myEnemy.Attacking = false;
                    combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
                    attackTimes = 0;
                    anim.SetInteger("AttackTime", 0);
                }
                else
                {
                    timer = 0;
                    cdToAttackCurrent = cdToAttackSum;
                    anim.SetInteger("AttackTime", 1);
                }
            }
        }
    }
}
