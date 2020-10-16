using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharElectricSword : CharacterStates
    {
        CharacterAnimator anim = null;
        float speed;

        public CharElectricSword(EState<CharacterHead.PlayerInputs> myState, float _speed , CharacterAnimator _anim, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
            anim = _anim;
            speed = _speed;
        }
        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            Main.instance.GetChar().GetCharMove().SetSpeed(speed);
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
