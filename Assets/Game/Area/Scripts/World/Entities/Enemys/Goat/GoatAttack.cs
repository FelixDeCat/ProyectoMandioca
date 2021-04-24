using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class GoatAttack : JabaliStates
    {
        float cdToAttack;
        string attackSound;

        public GoatAttack(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _cdToAttack, string _attackSound) : base(myState, _sm)
        {
            cdToAttack = _cdToAttack;
            attackSound = _attackSound;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            anim.SetTrigger("BasicAttackOk");
            cdModule.AddCD("RecallAttack", () => sm.SendInput(JabaliEnemy.JabaliInputs.IDLE), cdToAttack);
            AudioManager.instance.PlaySound(attackSound, anim.transform);
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
