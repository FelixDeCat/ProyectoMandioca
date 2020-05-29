using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class DummyDieState : DummyEnemyStates
    {
        RagdollComponent ragdoll;
        ParticleSystem particle;
        float timer;
        bool desactive;

        public DummyDieState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm, RagdollComponent _ragdoll,
                             ParticleSystem _particle) : base(myState, _sm)
        {
            particle = _particle;
            ragdoll = _ragdoll;
        }
        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> input)
        {
            particle.gameObject.SetActive(false);
            combatDirector.DeadEntity(ragdoll.GetComponent<EnemyBase>(), ragdoll.GetComponent<EnemyBase>().CurrentTarget());
        }

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
