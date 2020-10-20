using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class BombEnemyPrepExplode : StatesFunctions<BombEnemy.BombInputs>
    {
        Action PrepareExplosion;
        public BombEnemyPrepExplode(EState<BombEnemy.BombInputs> myState, EventStateMachine<BombEnemy.BombInputs> _sm, Action _PrepareExplosion) : base(myState, _sm)
        {
            PrepareExplosion = _PrepareExplosion;
        }

        protected override void Enter(EState<BombEnemy.BombInputs> lastState)
        {
            PrepareExplosion();
        }

        protected override void Exit(BombEnemy.BombInputs input)
        {
        }

        protected override void FixedUpdate()
        {
        }

        protected override void LateUpdate()
        {
        }

        protected override void Update()
        {
        }
    }
}
