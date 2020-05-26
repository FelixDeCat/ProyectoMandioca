using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ToolsMandioca.StateMachine
{
    public class CharOnSkill : CharacterStates
    {
        Action canExcecute;

        public CharOnSkill(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, Action callbackCanExecute) : base(myState, _sm) 
        {
            canExcecute = callbackCanExecute;
        }
        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);
            canExcecute.Invoke();
        }
        protected override void Update() { }
        protected override void FixedUpdate() { }
        protected override void LateUpdate() { }
        protected override void Exit(CharacterHead.PlayerInputs input) { }
    }
}
