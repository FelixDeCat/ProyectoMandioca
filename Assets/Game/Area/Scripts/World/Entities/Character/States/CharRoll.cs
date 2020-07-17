using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    

    public class CharRoll : CharacterStates
    {
        public Action BeginBashDashCallback;
        public Action EndBashDashCallback;

        public CharRoll(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, Action _BeginbashDashCallback, Action _EndBashDash) : base(myState, _sm)
        {
            BeginBashDashCallback = _BeginbashDashCallback;
            EndBashDashCallback = _EndBashDash;
        }

        string _input;

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            feedbacks.particles.evadeParticle.Play();
            _input = input.Name;
            if (input.Name == "Begin_Block" || input.Name == "Block")
            {
                BeginBashDashCallback.Invoke();
            }
            charMove.Dash();
        }

        protected override void Update()
        {
            base.Update();
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
            feedbacks.particles.evadeParticle.Stop();

            if (_input == "Begin_Block" || _input == "Block")
            {
                EndBashDashCallback.Invoke();
                charBlock.callback_UpBlock();
                charBlock.SetOnBlock(false);
            }

            base.Exit(input);
        }
    }
}
