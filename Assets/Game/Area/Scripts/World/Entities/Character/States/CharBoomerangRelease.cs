using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class CharBoomerangRelease : CharacterStates
    {
        Action throwShield;

        public CharBoomerangRelease(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, Action _throwShield ): base(myState, _sm)
        {
            throwShield = _throwShield;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            Main.instance.GetChar().ToggleBlock(false);
            Debug.Log("Entra CharBoomerangRelease");
            throwShield.Invoke();
        }


        protected override void Update()
        {
            Debug.Log("Entra Update CharBoomerangRelease");
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
            Debug.Log("Entra exit CharBoomerangRelease");

        }
    }
}
