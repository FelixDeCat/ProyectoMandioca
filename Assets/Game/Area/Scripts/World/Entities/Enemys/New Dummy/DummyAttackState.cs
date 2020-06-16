﻿using UnityEngine;
namespace ToolsMandioca.StateMachine
{
    public class DummyAttackState : DummyEnemyStates
    {
        float cd;
        float timer;
        ICombatDirector enemy;
        public DummyAttackState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                                float _cd, ICombatDirector _enemy) : base(myState, _sm)
        {
            cd = _cd;
            enemy = _enemy;
        }
        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
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
            timer += Time.deltaTime;
            if (timer >= cd) sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
        }
    }
}
