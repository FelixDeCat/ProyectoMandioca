using UnityEngine;

namespace ProyectTools.StateMachine
{
    public class DummyParried : DummyEnemyStates
    {
        float cd;
        float timer;
        ICombatDirector enemy;


        public DummyParried(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                                float _cd, ICombatDirector _enemy) : base(myState, _sm)
        {
            cd = _cd;
            enemy = _enemy;
        }

        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> input)
        {
            base.Enter(input);

            anim.SetBool("Stun", true);
            anim.SetBool("Attack", false);
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);

            if (input != TrueDummyEnemy.DummyEnemyInputs.PETRIFIED)
            {
                timer = 0;
                anim.SetBool("Stun", false);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }

        protected override void Update()
        {
            base.Update();

            timer += Time.deltaTime;

            if (timer >= cd)
            {
                sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
                combatDirector.AddToAttack(enemy, enemy.CurrentTarget());
            }
        }
    }
}
