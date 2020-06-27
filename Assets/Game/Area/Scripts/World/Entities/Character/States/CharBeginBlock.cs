using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharBeginBlock : CharacterStates
    {
        Animator anim;
        private float initSpeed;
        public CharBeginBlock(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, Animator _anim) : base(myState, _sm)
        {
            anim = _anim;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charBlock.OnBlock();
            initSpeed = charMove.GetDefaultSpeed;
            charMove.SetSpeed(charMove.GetDefaultSpeed * .5f);
        }

        protected override void Update()
        {
            charMove.RotateHorizontal(RightHorizontal());
            charMove.RotateVertical(RightVertical());
            
            
            charMove.MovementHorizontal(LeftHorizontal());
            charMove.MovementVertical(LeftVertical());

            
            
            var info = anim.GetCurrentAnimatorStateInfo(1);
            if (info.IsName("BlockStay"))
            {
                Debug.Log("blockeo");
                sm.SendInput(CharacterHead.PlayerInputs.BLOCK);
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
            if (input != CharacterHead.PlayerInputs.BLOCK)
            {
                charBlock.UpBlock();
            }
            else
                charBlock.OnBlockSuccessful();
            
            ResetSpeed();
        }

        void ResetSpeed()
        {
            charMove.SetSpeed(initSpeed);
        }
    }
}
