using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharOnMenues : CharacterStates
    {
        public CharOnMenues(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm) { }
        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            charMove.MovementVertical(0);
            charMove.MovementHorizontal(0);
            charMove.RotateHorizontal(0);
            charMove.RotateVertical(0);
            interactSensor.CanInteract(false);
        }
        protected override void Update() { }
        protected override void FixedUpdate() => base.FixedUpdate();
        protected override void LateUpdate() => base.LateUpdate();
        protected override void Exit(CharacterHead.PlayerInputs input) {
            base.Exit(input);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            interactSensor.CanInteract(true);
        }
    }
}
