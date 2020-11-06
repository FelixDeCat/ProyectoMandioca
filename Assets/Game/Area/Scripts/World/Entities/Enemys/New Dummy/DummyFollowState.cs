﻿using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace Tools.StateMachine
{
    public class DummyFollowState : DummyEnemyStates
    {
        GenericEnemyMove move;

        CombatDirectorElement noObs;

        float normalDistance;
        float minDistance;
        string soundName;

        Func<bool> IsSpecialAttack;

        public DummyFollowState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm, GenericEnemyMove _move,
                                float distance, float _minDistance, CombatDirectorElement me, string _soundName, Func<bool> _IsSpecialAttack = null) : base(myState, _sm)
        {
            move = _move;
            normalDistance = distance;
            minDistance = _minDistance;
            noObs = me;
            soundName = _soundName;

            if (_IsSpecialAttack != null) IsSpecialAttack = _IsSpecialAttack; else IsSpecialAttack = () => false;
        }

        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> input)
        {
            base.Enter(input);
            anim.SetFloat("move", 0.3f);
            
            AudioManager.instance.PlaySound(soundName,root);
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);

            move.StopMove();
            anim.SetFloat("move", 0);
            
            AudioManager.instance.StopAllSounds(soundName);
        }

        protected override void Update()
        {
            base.Update();

            if (!noObs.IsInPos())
            {
                if (noObs.CurrentTarget() != null)
                {
                    Vector3 dirForward = (noObs.CurrentTarget().transform.position - root.position).normalized;
                    Vector3 fowardRotation = move.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));

                    move.Rotation(fowardRotation.normalized);
                    move.MoveWRigidbodyV(fowardRotation);

                    if (Vector3.Distance(noObs.CurrentTarget().transform.position, root.position) <= normalDistance)
                        sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
                }
                else
                    sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
            }
            else
            {
                if(IsSpecialAttack())
                    sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.CHASING);

                Vector3 dirForward = (noObs.CurrentTarget().transform.position - root.position).normalized;
                Vector3 fowardRotation = move.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));

                move.Rotation(fowardRotation.normalized);
                move.MoveWRigidbodyV(fowardRotation);

                Vector3 targetPos = noObs.CurrentTarget().transform.position;
                Vector3 myPos = root.position;

                if (Vector3.Distance(new Vector3(targetPos.x, 0, targetPos.z), new Vector3(myPos.x, 0, myPos.z)) <= minDistance)
                    sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.CHASING);
            }

        }
    }
}
