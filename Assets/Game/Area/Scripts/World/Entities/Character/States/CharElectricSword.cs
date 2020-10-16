using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharElectricSword : CharacterStates
    {
        CharacterAnimator anim = null;

        public CharElectricSword(EState<CharacterHead.PlayerInputs> myState, CharacterAnimator _anim, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
            anim = _anim;
        }
        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            Main.instance.GetChar().GetCharMove().SetSpeed(Main.instance.GetChar().GetCharMove().GetDefaultSpeed/2f);
            //Empezar animacion
        }
        protected override void Update()
        {
            charMove.MovementHorizontal(LeftHorizontal());
            charMove.MovementVertical(LeftVertical());
            charMove.RotateHorizontal(LeftHorizontal());
            charMove.RotateVertical(LeftVertical());
        }
      
        protected override void Exit(CharacterHead.PlayerInputs input)
        {
            // Cortar animacion
        }
    }
}
