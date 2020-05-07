using UnityEngine;

namespace ProyectTools.StateMachine
{
    public class DummyAttAnt : DummyEnemyStates
    {
        float cd;
        float timer;
        float rotationSpeed;
        ICombatDirector enemy;

        public DummyAttAnt(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                                float _rotSpeed, ICombatDirector _enemy) : base(myState, _sm)
        {
            cd = 5;
            enemy = _enemy;
            rotationSpeed = _rotSpeed;
        }

        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> input)
        {
            base.Enter(input);

            if (input.Name != "Petrified")
            {
                anim.SetBool("Attack", true);
                combatDirector.RemoveToAttack(enemy, enemy.CurrentTarget());
            }
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);

            if (input != TrueDummyEnemy.DummyEnemyInputs.PETRIFIED)
                timer = 0;
        }

        protected override void Update()
        {
            base.Update();

            Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
            Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);

            root.forward = Vector3.Lerp(root.forward, forwardRotation, rotationSpeed * Time.deltaTime);

            timer += Time.deltaTime;

            if (timer >= cd)
                sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.ATTACK);
        }
    }
}
