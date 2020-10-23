using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class CharSwordAbilityCharge : CharacterStates
    {
        Action toExecute;
        Func<float> speed;

        public CharSwordAbilityCharge(EState<CharacterHead.PlayerInputs> myState, Action _toExecute, Func<float> _speed, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
            toExecute = _toExecute;
            speed = _speed;
        }
        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            Main.instance.GetChar().GetCharMove().SetSpeed(speed());
            //if(toExecute != null) toExecute.Invoke();
        }
        protected override void Update()
        {
            charMove.MovementHorizontal(LeftHorizontal());
            charMove.MovementVertical(LeftVertical());
            charMove.RotateHorizontal(LeftHorizontal());
            charMove.RotateVertical(LeftVertical());
        }
      
        protected override void Exit(CharacterHead.PlayerInputs input)
        {
        }
    }
}
