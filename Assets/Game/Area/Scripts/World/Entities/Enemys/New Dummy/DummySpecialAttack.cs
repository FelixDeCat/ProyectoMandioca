using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class DummySpecialAttack : DummyEnemyStates
    {
        //Acá se debería ejecutar la lógica de la habilidad. Tener la lógica o no dentro de este script es algo a elección. También podés dividirlo en más
        //estados si querés, pero es importante que cuando termine de atacar del todo, realice la lógica que te dejé en el Exit.
        ICombatDirector enemy;

        public DummySpecialAttack(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                                  ICombatDirector _enemy) : base(myState, _sm)
        {
            enemy = _enemy;
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);

            var myEnemy = (EnemyBase)enemy;
            myEnemy.attacking = false;
            combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
        }

        protected override void Update()
        {
        }
    }
}
