using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class GoatAttack : JabaliStates
    {
        float cdToAttack;
        float timer;
        string attackSound;

        public GoatAttack(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _cdToAttack, string _attackSound) : base(myState, _sm)
        {
            cdToAttack = _cdToAttack;
            attackSound = _attackSound;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            if (input.Name != "Petrified")
                anim.SetTrigger("BasicAttackOk");

            AudioManager.instance.PlaySound(attackSound);
        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= cdToAttack)
                sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.PETRIFIED)
            {
                timer = 0;
                var myEnemy = enemy;
                myEnemy.Attacking = false;
                combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
            }
        }
    }
}
