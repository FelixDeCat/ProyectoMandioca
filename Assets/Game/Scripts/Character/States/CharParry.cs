using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharParry : CharacterStates
    {
        float timer;
        float parryRecall;


        public CharParry(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, float recall) : base(myState, _sm)
        {
            parryRecall = recall;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);
            charBlock.SetOnBlock(false);
            charBlock.OnParry();
        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= parryRecall)
                sm.SendInput(CharacterHead.PlayerInputs.IDLE);
        }

        protected override void FixedUpdate()
        {

        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }

        protected override void Exit(CharacterHead.PlayerInputs input)
        {
            timer = 0;
            charBlock.UpBlock();
        }
    }
}
