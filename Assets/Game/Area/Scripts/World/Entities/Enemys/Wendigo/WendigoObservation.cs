﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class WendigoObservation : WendigoStates
    {
        GenericEnemyMove moveComp;
        CombatDirectorElement enemy;
        public WendigoObservation(EState<WendigoEnemy.WendigoInputs> myState, WendigoView _view, GenericEnemyMove _moveComp, CombatDirectorElement _enem, EventStateMachine<WendigoEnemy.WendigoInputs> _sm) : base(myState, _sm)
        {
            view = _view;
            moveComp = _moveComp;
            enemy = _enem;
        }

        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            if (last.Name == "Idle")
            {
                view.Awakening();
            }
        }

        protected override void Update()
        {
            view.DebugText("Observation");

            if (enemy.CurrentTarget() != null)
            {
                Vector3 dirForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                Vector3 fowardRotation = moveComp.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));
                moveComp.Rotation(fowardRotation.normalized);
                moveComp.MoveWRigidbodyV(moveComp.ObstacleAvoidance(fowardRotation));

                view.Movement(fowardRotation.magnitude);

            }
        }
        protected override void Exit(WendigoEnemy.WendigoInputs input)
        {
            view.ExitMov();
        }
    }
}