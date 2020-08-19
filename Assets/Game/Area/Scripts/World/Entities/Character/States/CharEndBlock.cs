using System;


namespace Tools.StateMachine
{
    public class CharEndBlock : CharacterStates
    {
        Action<bool> ChangeAttacking;

        public CharEndBlock(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, Action<bool> _ChangeAttacking) : base(myState, _sm)
        {
            ChangeAttacking = _ChangeAttacking;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charBlock.callback_UpBlock();
            charBlock.SetOnBlock(false);
            ChangeAttacking?.Invoke(false);
        }

        protected override void Update()
        {
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
}
