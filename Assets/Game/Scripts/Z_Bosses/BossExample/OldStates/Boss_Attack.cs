using UnityEngine;
namespace ToolsMandioca.StateMachine
{
    public class Boss_Attack : DummyEnemyStates
    {
        float cd;
        float timer;
        ICombatDirector enemy;
        public Boss_Attack(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                                float _cd, ICombatDirector _enemy) : base(myState, _sm)
        {
            cd = _cd;
            enemy = _enemy;
        }
        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);

            if (input != TrueDummyEnemy.DummyEnemyInputs.PETRIFIED)
            {
                timer = 0;
                anim.SetBool("Attack", false);
                var myEnemy = (EnemyBase)enemy;
                myEnemy.attacking = false;
                combatDirector.AddToAttack(enemy, enemy.CurrentTarget());
            }
        }

        protected override void Update()
        {
            timer += Time.deltaTime;
            if (timer >= cd) sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
        }
    }
}
