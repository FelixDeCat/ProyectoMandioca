using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharLookOverSholder : CharacterStates
    {

        public CharLookOverSholder(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {

        }
        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            base.Enter(input);
            _camera.inputToOverTheSholder(false);
        }

        protected override void Update()
        {
           
            //charMove.MovementHorizontal(0);
            //charMove.MovementVertical(0);
           
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
            base.Exit(input);
            _camera.inputToOverTheSholder(true);
        }

    }
}

