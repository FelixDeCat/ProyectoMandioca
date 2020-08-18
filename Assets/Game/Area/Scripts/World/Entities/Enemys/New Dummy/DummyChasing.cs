using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class DummyChasing : DummyEnemyStates
    {
        Func<bool> IsAttack;
        Func<bool> IsSpecialAttack; //Este es el Func para checkear si es ataque especial 
        float distanceToNormalAttack;
        GenericEnemyMove move;
        EnemyBase enemy;

        public DummyChasing(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm, Func<bool> _IsAttack,
                            float _distanceToAttack, GenericEnemyMove _move, EnemyBase _enemy, Func<bool> _IsSpecialAttack = null) : base(myState, _sm)
        {
            IsAttack = _IsAttack;
            distanceToNormalAttack = _distanceToAttack;
            move = _move;
            enemy = _enemy;
            if (_IsSpecialAttack != null) IsSpecialAttack = _IsSpecialAttack; else IsSpecialAttack = () => false;
        }

        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> last)
        {
            base.Enter(last);
            combatDirector.PrepareToAttack(enemy, enemy.CurrentTarget());
        }

        protected override void Update()
        {
            base.Update();
            if (!enemy.CurrentTarget())
                sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);

            if (IsAttack())
            {
                if (IsSpecialAttack())
                    sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.SPECIAL_ATTACK);
                else
                    sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.BEGIN_ATTACK);
            }
            else
            {
                if (enemy.CurrentTarget() != null)
                {
                    if (IsSpecialAttack())
                    {
                        Vector3 dirForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                        Vector3 fowardRotation = move.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));

                        move.Rotation(fowardRotation.normalized);
                        move.MoveWRigidbodyV(fowardRotation);

                        if (Vector3.Distance(enemy.CurrentTarget().transform.position, root.position) <= distanceToNormalAttack)
                            move.StopMove();
                    }
                    else
                    {
                        Vector3 pos1 = new Vector3(root.position.x, 0, root.position.z);
                        Vector3 pos2 = new Vector3(enemy.CurrentTarget().transform.position.x, 0, enemy.CurrentTarget().transform.position.z);

                        Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                        Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);

                        move.Rotation(forwardRotation);

                        if (Vector3.Distance(pos1, pos2) >= distanceToNormalAttack)
                            sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.GO_TO_POS);
                    }
                }
            }
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);
            if(enemy.CurrentTarget() != null)
                combatDirector.DeleteToPrepare(enemy, enemy.CurrentTarget());
        }

    }
}
