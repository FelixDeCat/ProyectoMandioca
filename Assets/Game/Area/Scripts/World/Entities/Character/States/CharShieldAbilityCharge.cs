using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class CharShieldAbilityCharge : CharacterStates
    { 
        Action toExecute;
        Action<bool> ChangeAttacking;
        public CharShieldAbilityCharge(EState<CharacterHead.PlayerInputs> myState, Action _toExecute, EventStateMachine<CharacterHead.PlayerInputs> _sm, Action<bool> _ChangeAttacking) : base(myState, _sm)
        {
            toExecute = _toExecute;
            ChangeAttacking = _ChangeAttacking;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);
            ChangeAttacking?.Invoke(true);
            //if (toExecute != null) toExecute.Invoke();
        }
        protected override void Update()
        {
            charMove.RotateHorizontal(LeftHorizontal());
            charMove.RotateVertical(LeftVertical());
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }

        protected override void Exit(CharacterHead.PlayerInputs input)
        {

        }
    }
}
