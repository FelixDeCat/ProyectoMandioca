using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProyectTools.StateMachine
{
    public class CharMove : CharacterStates
    {
        public CharMove(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            base.Enter(input);
        }

        protected override void Update()
        {
            charMove.RotateHorizontal(RightHorizontal());
            charMove.RotateVertical(RightVertical());
            charMove.MovementHorizontal(LeftHorizontal());
            charMove.MovementVertical(LeftVertical());

            if (LeftVertical() == 0 && LeftHorizontal() == 0)
            {
                sm.SendInput(CharacterHead.PlayerInputs.IDLE);
            }
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
