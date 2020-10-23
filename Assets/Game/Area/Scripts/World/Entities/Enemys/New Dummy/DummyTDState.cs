using UnityEngine;

namespace Tools.StateMachine
{
    public class DummyTDState : DummyEnemyStates
    {
        float recallTime;

        public DummyTDState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                            float _recall) : base(myState, _sm)
        {
            recallTime = _recall;
        }

        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> input)
        {
            base.Enter(input);

            anim.SetBool("takeDamage", true);
            cdModule.AddCD("TakeDamageCD", () => sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE), recallTime);
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);
            cdModule.EndCDWithoutExecute("TakeDamageCD");
            anim.SetBool("takeDamage", false);
        }
    }
}
