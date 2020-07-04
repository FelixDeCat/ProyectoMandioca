using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tools.StateMachine
{
    public class CharEndBlock : CharacterStates
    {
        public CharEndBlock(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charBlock.callback_UpBlock();
            charBlock.SetOnBlock(false);
        }

        protected override void Update()
        {
            if (LeftHorizontal() == 0 && LeftVertical() == 0)
            {
                sm.SendInput(CharacterHead.PlayerInputs.IDLE);
            }
            else
            {
                sm.SendInput(CharacterHead.PlayerInputs.MOVE);
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
