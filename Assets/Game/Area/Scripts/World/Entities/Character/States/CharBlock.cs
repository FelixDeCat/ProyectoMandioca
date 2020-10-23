using System;

namespace Tools.StateMachine
{
    public class CharBlock : CharacterStates
    {
        ////float _speedPenalty = .75f;
        //private float initSpeed;

        public CharBlock(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charBlock.SetOnBlock(true);
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
            //ResetSpeed();
        }
        
        //void ResetSpeed()
        //{
        //    charMove.SetSpeed(initSpeed);
        //}
    }
}
