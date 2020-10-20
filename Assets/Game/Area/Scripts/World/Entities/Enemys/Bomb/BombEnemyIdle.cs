using System;
using UnityEngine;

namespace Tools.StateMachine
{
    public class BombEnemyIdle : StatesFunctions<BombEnemy.BombInputs>
    {
        float distanceToExplode;
        Func<EntityBase> Target;
        Transform root;

        public BombEnemyIdle(EState<BombEnemy.BombInputs> myState, EventStateMachine<BombEnemy.BombInputs> _sm, float _distanceToExplode, Func<EntityBase> _Target, Transform _root) : base(myState, _sm)
        {
            distanceToExplode = _distanceToExplode;
            Target = _Target;
            root = _root;
        }

        protected override void Enter(EState<BombEnemy.BombInputs> lastState)
        {
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
            if (Target() != null)
            {
                Vector3 myForward = (Target().transform.position - root.position).normalized;
                Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);

                Vector3 pos1 = new Vector3(root.position.x, 0, root.position.z);
                Vector3 pos2 = new Vector3(Target().transform.position.x, 0, Target().transform.position.z);

                if (Vector3.Distance(pos1, pos2) >= distanceToExplode)
                    sm.SendInput(BombEnemy.BombInputs.PERSUIT);
                else 
                    sm.SendInput(BombEnemy.BombInputs.PREP_EXPLODE);
            }
        }
    }
}
