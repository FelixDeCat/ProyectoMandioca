using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class CrowAttack : CrowStates
    {
        float timer;
        float cd;
        EnemyBase enemy;

        public CrowAttack(EState<CrowEnemy.CrowInputs> myState, EventStateMachine<CrowEnemy.CrowInputs> _sm, float _cd, EnemyBase _enemy) : base(myState, _sm)
        {
            cd = _cd;
            enemy = _enemy;
        }

        protected override void Enter(EState<CrowEnemy.CrowInputs> last)
        {
            base.Enter(last);
            anim.SetBool("Attack", true);
        }

        protected override void Exit(CrowEnemy.CrowInputs input)
        {
            base.Exit(input);

            timer = 0;
            anim.SetBool("Attack", false);
            var myEnemy = (EnemyBase)enemy;
            myEnemy.attacking = false;
            combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
        }

        protected override void Update()
        {
            base.Update();
            timer += Time.deltaTime;
            if (timer >= cd) sm.SendInput(CrowEnemy.CrowInputs.IDLE);
        }
    }
}
