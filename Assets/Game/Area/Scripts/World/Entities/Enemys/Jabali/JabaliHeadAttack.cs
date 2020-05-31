using System;
using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class JabaliHeadAttack : JabaliStates
    {
        float cdToAttack;
        float timer;
        string attackSound;

        public JabaliHeadAttack(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _cdToAttack, string _attackSound) : base(myState, _sm)
        {
            cdToAttack = _cdToAttack;
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
                sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.PETRIFIED)
            {
                timer = 0;
                var myEnemy = (EnemyBase)enemy;
                myEnemy.attacking = false;
                combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
            }
        }
    }
}
