using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    

    public class CharRoll : CharacterStates
    {
        public CharRoll(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            feedbacks.particles.evadeParticle.Play();
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

            base.Exit(input);
        }
    }
}
