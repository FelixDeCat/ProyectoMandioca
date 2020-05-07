using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProyectTools.StateMachine
{
    public class CharBlock : CharacterStates
    {
        public CharBlock(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charBlock.SetOnBlock(true);

            //if (charhead.ISLocoon())
            //{
            //    //nuestro funcioanmiento nuevo
            //    charMove.MovementHorizontal(input / 2);
            //    charMove.MovementVertical(input / 2);
            //}
            //else
            //{
            //    //charMove.MovementHorizontal(0);
            //   // charMove.MovementVertical(0);
            //}
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);
        }

        protected override void Update()
        {


            charMove.RotateHorizontal(RightHorizontal());
            charMove.RotateVertical(RightVertical());
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
