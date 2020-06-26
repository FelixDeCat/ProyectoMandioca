using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class CharacterStates : StatesFunctions<CharacterHead.PlayerInputs>
    {
        protected Func<float> LeftHorizontal;
        protected Func<float> LeftVertical;
        protected Func<float> RightHorizontal;
        protected Func<float> RightVertical;
        protected CharacterAttack charAttack;
        protected CharacterBlock charBlock;
        protected CharacterMovement charMove;
        protected CharacterAnimator charAnim;
        protected LockOn _myLockOn;

        public CharacterStates(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
        }

        #region Builder
        public CharacterStates SetAnimator(CharacterAnimator anim)
        {
            charAnim = anim;
            return this;
        }
        public CharacterStates SetLeftAxis(Func<float> h, Func<float> v)
        {
            LeftHorizontal += h;
            LeftVertical += v;
            return this;
        }

        public CharacterStates SetRightAxis(Func<float> h, Func<float> v)
        {
            RightHorizontal += h;
            RightVertical += v;
            return this;
        }

        public CharacterStates SetAttack(CharacterAttack attack)
        {
            charAttack = attack;
            return this;
        }

        public CharacterStates SetBlock(CharacterBlock block)
        {
            charBlock = block;
            return this;
        }

        public CharacterStates SetMovement(CharacterMovement move)
        {
            charMove = move;
            return this;
        }
        #endregion

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
        }

        protected override void Exit(CharacterHead.PlayerInputs input)
        {

        }

        protected override void FixedUpdate()
        {
        }

        protected override void LateUpdate()
        {

        }

        protected override void Update()
        {
        }
    }
}
