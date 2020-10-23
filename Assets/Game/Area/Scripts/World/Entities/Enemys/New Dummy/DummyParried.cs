using UnityEngine;

namespace Tools.StateMachine
{
    public class DummyParried : DummyEnemyStates
    {
        float cd;
        CombatDirectorElement enemy;

        public DummyParried(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                                float _cd, CombatDirectorElement _enemy) : base(myState, _sm)
        {
            cd = _cd;
            enemy = _enemy;
        }

        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> input)
        {
            base.Enter(input);

            anim.SetBool("Stun", true);
            anim.SetBool("Attack", false);
            cdModule.AddCD("ParriedCD", () => sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE), cd);
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);

            cdModule.EndCDWithoutExecute("ParriedCD");
            combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
            anim.SetBool("Stun", false);
        }
    }
}
