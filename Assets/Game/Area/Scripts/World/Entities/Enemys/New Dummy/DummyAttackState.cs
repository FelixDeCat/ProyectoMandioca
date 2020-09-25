using UnityEngine;
namespace Tools.StateMachine
{
    public class DummyAttackState : DummyEnemyStates
    {
        float cd;
        float timer;
        CombatDirectorElement enemy;
        public DummyAttackState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                                float _cd, CombatDirectorElement _enemy) : base(myState, _sm)
        {
            cd = _cd;
            enemy = _enemy;
        }
        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);

            timer = 0;
            anim.SetBool("Attack", false);
            var myEnemy = enemy;
            myEnemy.Attacking = false;
            combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
        }

        protected override void Update()
        {
            timer += Time.deltaTime;
            if (timer >= cd) sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
        }
    }
}
