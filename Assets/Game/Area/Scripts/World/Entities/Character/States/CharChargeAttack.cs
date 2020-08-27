using System;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharChargeAttack : CharacterStates
    {
        Animator anim;
        Action<bool> ChangeAttacking;

        public CharChargeAttack(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, Animator _anim, Action<bool> _ChangeAttacking) : base(myState, _sm)
        {
            anim = _anim;
            ChangeAttacking = _ChangeAttacking;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charAttack.AttackBegin();

            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);

            ChangeAttacking?.Invoke(true);
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
            if (input != CharacterHead.PlayerInputs.RELEASE_ATTACK)
                charAttack.AttackFail();
            else
                charAttack.AttackEnd();
        }
    }
}
