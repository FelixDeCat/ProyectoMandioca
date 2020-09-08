using System;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharFalling : CharacterStates
    {
        float timer;
        bool cd;

        public CharFalling(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charAnim.Falling(true);
        }

        protected override void Update()
        {
            base.Update();
            if (cd)
            {
                timer += Time.deltaTime;
                if (timer >= 2)
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

        public void ActivateCD()
        {
            if(sm.Current == state) cd = true;
        }

        protected override void Exit(CharacterHead.PlayerInputs input)
        {
            charAnim.Falling(false);
            timer = 0;
            cd = false;
            base.Exit(input);
        }
    }
}
