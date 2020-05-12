using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class DummyDieState : DummyEnemyStates
    {
        RagdollComponent ragdoll;
        float timer;
        bool desactive;

        public DummyDieState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm, RagdollComponent _ragdoll) : base(myState, _sm)
        {
            ragdoll = _ragdoll;
        }
        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> input) => ragdoll.Ragdoll(true);

        protected override void Update()
        {
            base.Update();

            timer += Time.deltaTime;

            if (timer >= 5)
            {
                if (!desactive)
                {
                    ragdoll.DesactiveBones();
                    desactive = true;
                }
            }

            if (timer >= 8)
            {
                ragdoll.gameObject.SetActive(false);
            }
        }
    }
}
