using UnityEngine;
namespace Tools.StateMachine
{
    public class DummyAttackState : DummyEnemyStates
    {
        float cd;
        CombatDirectorElement enemy;
        public DummyAttackState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                                float _cd, CombatDirectorElement _enemy) : base(myState, _sm)
        {
            cd = _cd;
            enemy = _enemy;
        }
        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> last)
        {
            base.Enter(last);
            cdModule.AddCD("AttackRecall", () => sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE), cd);
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);
            cdModule.EndCDWithoutExecute("AttackRecall");
            anim.SetBool("Attack", false);
            var myEnemy = enemy;
            myEnemy.Attacking = false;
            combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
        }
    }
}
