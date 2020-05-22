using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ToolsMandioca.StateMachine
{
    public class CharStun : CharacterStates
    {
        float timerstun = 0;
        Func<float> GetTimeToStun;
        bool anim;
        GameObject go_feedback;
        public CharStun(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
        }
        public CharStun Configurate(Func<float> Callback_GetTimeToStun, GameObject _go_feedback)
        {
            go_feedback = _go_feedback;
            go_feedback.SetActive(false);
            GetTimeToStun = Callback_GetTimeToStun;
            return this;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input) 
        {
            charMove.RotateHorizontal(0);
            charMove.RotateVertical(0);
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);

            go_feedback.SetActive(true);
            charAnim.Stun(true); 
            anim = true;
        }
        protected override void Update()
        {
            if (anim)
            {
                if (timerstun < GetTimeToStun())
                {
                    Debug.Log("Hace animacion");
                    timerstun = timerstun + 1 * Time.deltaTime;
                }
                else
                {
                    Debug.Log("Voy a IDLE");
                    timerstun = 0;
                    anim = false;
                    sm.SendInput(CharacterHead.PlayerInputs.IDLE);
                }
            }
        }

        protected override void FixedUpdate() { }
        protected override void LateUpdate() { }
        protected override void Exit(CharacterHead.PlayerInputs input)
        {
            go_feedback.SetActive(false);
            charAnim.Stun(false);
        }
    }
}

