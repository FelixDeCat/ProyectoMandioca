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
            anim.SetTrigger("HeadOk");

            AudioManager.instance.PlaySound(attackSound);
            if (attackTimes >= 1)
                cdModule.AddCD("RecallAttack", () => sm.SendInput(JabaliEnemy.JabaliInputs.IDLE), cdToAttackCurrent);
            else
                cdModule.AddCD("RecallAttack", () => sm.SendInput(JabaliEnemy.JabaliInputs.HEAD_ANTICIP), cdToAttackCurrent);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.HEAD_ANTICIP)
            {
                cdToAttackCurrent = cdToAttackMinus;
                var myEnemy = enemy;
                myEnemy.Attacking = false;
                combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
                attackTimes = 0;
                anim.SetInteger("AttackTime", 0);
            }
            else
            {
                cdToAttackCurrent = cdToAttackSum;
                anim.SetInteger("AttackTime", 1);
            }
            cdModule.EndCDWithoutExecute("RecallAttack");
        }
    }
}
