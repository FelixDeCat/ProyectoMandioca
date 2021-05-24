using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class CharSwordAbilityCharge : CharacterStates
    {
        Action toExecute;

        public CharSwordAbilityCharge(EState<CharacterHead.PlayerInputs> myState, Action _toExecute, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
            toExecute = _toExecute;
        }
        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            //if(toExecute != null) toExecute.Invoke();
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);
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
