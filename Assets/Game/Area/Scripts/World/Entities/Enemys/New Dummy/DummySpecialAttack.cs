using System.Collections;
using System.Collections.Generic;
using DevelopTools;
using UnityEngine;

namespace Tools.StateMachine
{
    public class DummySpecialAttack : DummyEnemyStates
    {
        //Acá se debería ejecutar la lógica de la habilidad. Tener la lógica o no dentro de este script es algo a elección. También podés dividirlo en más
        //estados si querés, pero es importante que cuando termine de atacar del todo, realice la lógica que te dejé en el Exit.
        ICombatDirector enemy;
        private TrueDummyEnemy _smOwner;


        private float _count = 3;
        
        private CharacterHead _hero;
        private SingleObjectPool<PlayObject> vinesPool;

        public DummySpecialAttack(TrueDummyEnemy e, EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                                  ICombatDirector _enemy) : base(myState, _sm)
        {
            enemy = _enemy;
            _smOwner = e;
        }

        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> last)
        {
            
            SummonCorruptedVines();
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
            _count -= Time.deltaTime;
            
            if(_count <= 0)
            {
                _count = 1;
                _smOwner.isSpecialInCD = true;
                sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
            }
        }

        void SummonCorruptedVines()
        {
            _hero = Main.instance.GetChar();
            //Iniciar animacion de enmarañar

            vinesPool = PoolManager.instance.GetObjectPool("CorruptedVines");

            var vine = vinesPool.Get();
            vine.Initialize();
            vine.transform.position = _hero.transform.position;
            
        }
    }
}
