using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharChargeAttack : CharacterStates
    {
        public CharChargeAttack(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charAttack.OnAttackBegin(); 
        }

        protected override void Update()
        {
            charMove.RotateHorizontal(RightHorizontal());
            charMove.RotateVertical(RightVertical());
            charMove.MovementHorizontal(LeftHorizontal());
            charMove.MovementVertical(LeftVertical());
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
                charAttack.OnAttackEnd();
        }
    }
}
