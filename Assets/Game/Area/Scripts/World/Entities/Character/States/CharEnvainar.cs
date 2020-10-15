using System;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharEnvainar : CharacterStates
    {
        Action Env;
        Action Desenv;
        Func<bool> Predicate;
        Func<bool> UpWeapons;

        public CharEnvainar(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, Action _Env, Action _Desenv,
                            Func<bool> _Predicate, Func<bool> _UpWeapons) : base(myState, _sm)
        {
            Env = _Env;
            Desenv = _Desenv;
            Predicate = _Predicate;
            UpWeapons = _UpWeapons;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            if (Predicate())
            {
                if (UpWeapons()) Env();
                else Desenv();
            }
            sm.SendInput(CharacterHead.PlayerInputs.IDLE);
        }

        protected override void Update()
        {
        }

        protected override void FixedUpdate() { }
        protected override void LateUpdate() { }
        protected override void Exit(CharacterHead.PlayerInputs input)
        {

        }
    }
}
