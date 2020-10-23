using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class GoatStomp : JabaliStates
    {
        float cdToAttack;
        string attackSound;

        public GoatStomp(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _cdToAttack, string _attackSound) : base(myState, _sm)
        {
            cdToAttack = _cdToAttack;
            attackSound = _attackSound;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            anim.SetTrigger("StompOk");

            AudioManager.instance.PlaySound(attackSound);
            cdModule.AddCD("RecallAttack", () => sm.SendInput(JabaliEnemy.JabaliInputs.IDLE), cdToAttack);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            var myEnemy = enemy;
            myEnemy.Attacking = false;
            combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
            cdModule.EndCDWithoutExecute("RecallAttack");
        }
    }
}
