using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class CharShieldAbilityRelease : CharacterStates
    {
        float timer = 0;

        public CharShieldAbilityRelease(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm): base(myState, _sm)
        {

        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            Main.instance.GetChar().ToggleBlock(false);            
        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer > 1)
            {
                timer = 0;
                if (LeftHorizontal() == 0 && LeftVertical() == 0)
                {
                    sm.SendInput(CharacterHead.PlayerInputs.IDLE);
                }
                else
                {
                    sm.SendInput(CharacterHead.PlayerInputs.MOVE);
                }
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
