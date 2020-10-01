using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharBoomerangCharge : CharacterStates
    { 
        Animator anim;

        public CharBoomerangCharge(EState<CharacterHead.PlayerInputs> myState, Animator _anim, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
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
        }
    }
}
