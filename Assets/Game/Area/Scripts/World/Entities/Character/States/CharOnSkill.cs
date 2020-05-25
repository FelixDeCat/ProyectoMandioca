using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class CharOnSkill : CharacterStates
    {
        public CharOnSkill(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, LockOn myLockOn) : base(myState, _sm) { }
        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);
        }

        protected override void Update() { }
        protected override void FixedUpdate() { }
        protected override void LateUpdate() { }
        protected override void Exit(CharacterHead.PlayerInputs input) { }
    }
}
