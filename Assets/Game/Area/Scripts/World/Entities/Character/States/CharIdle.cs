using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharIdle : CharacterStates
    {
        float timer;
        float timeToAnim = 4;
        CharacterAnimator anim;

        public CharIdle(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, CharacterAnimator _anim) : base(myState, _sm)
        {
            anim = _anim;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);
        }

        protected override void Update()
        {
            if (LeftHorizontal()!=0 || LeftVertical() != 0)
            {
                sm.SendInput(CharacterHead.PlayerInputs.MOVE);
            }

            timer += Time.deltaTime;

            if(timer >= timeToAnim)
            {
                anim.IdleFancy();
                timer = 0;
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
            base.Exit(input);
            timer = 0;
        }
    }
}
