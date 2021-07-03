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
            charMove.SnorlaxateCharacter(true);
            interactSensor.CanInteract(false);
            SpatialGrid_handler.instance.StopSpatialGrid();
            var entities = Main.instance.AllEntities;
            foreach (var e in entities)
            {
                if (e.gameObject.activeSelf) e.Off();
            }
        }
        protected override void Update()
        {
            charMove.MovementVertical(0);
            charMove.MovementHorizontal(0);
        }
        protected override void FixedUpdate() => base.FixedUpdate();
        protected override void LateUpdate() => base.LateUpdate();
        protected override void Exit(CharacterHead.PlayerInputs input) {
            base.Exit(input);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            charMove.SnorlaxateCharacter(false);
            interactSensor.CanInteract(true);
            SpatialGrid_handler.instance.ResumeCheck();
            var entities = Main.instance.AllEntities;
            foreach (var e in entities)
            {
                if(e.gameObject.activeSelf) e.On();
            }
        }
    }
}
