using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class CharSwordAbilityCharge : CharacterStates
    {
        Action toExecute;
        Action<bool> ChangeAttacking;

        public CharSwordAbilityCharge(EState<CharacterHead.PlayerInputs> myState, Action _toExecute, EventStateMachine<CharacterHead.PlayerInputs> _sm, Action<bool> _ChangeAttacking) : base(myState, _sm)
        {
            toExecute = _toExecute;
            ChangeAttacking = _ChangeAttacking;
        }
        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            //if(toExecute != null) toExecute.Invoke();
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);
            ChangeAttacking?.Invoke(true);
        }
        protected override void Update()
        {
            //charMove.MovementHorizontal(LeftHorizontal());
            //charMove.MovementVertical(LeftVertical());
            charMove.RotateHorizontal(LeftHorizontal());
            charMove.RotateVertical(LeftVertical());
        }
      
        protected override void Exit(CharacterHead.PlayerInputs input)
        {
        }
    }
}
