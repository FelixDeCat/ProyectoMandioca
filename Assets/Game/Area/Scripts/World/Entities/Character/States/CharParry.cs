using System;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharParry : CharacterStates
    {
        float timer;
        float parryRecall;
        Action<bool> ChangeAttacking;

        public CharParry(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, float recall, Action<bool> _ChangeAttacking) : base(myState, _sm)
        {
            parryRecall = recall;
            ChangeAttacking = _ChangeAttacking;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);
            charBlock.callback_OnParry();
        }

        protected override void Update()
        {
            timer += Time.deltaTime;
            if (timer >= parryRecall)
                sm.SendInput(CharacterHead.PlayerInputs.END_BLOCK);
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
            charBlock.callback_UpBlock();
            ChangeAttacking?.Invoke(false);
        }
    }
}
