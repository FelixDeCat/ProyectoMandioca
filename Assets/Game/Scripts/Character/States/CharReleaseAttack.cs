using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class CharReleaseAttack : CharacterStates
    {
        float attackRecall;
        Func<bool> IsHeavy;
        Action<bool> ChangeHeavy;

        public CharReleaseAttack(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, float recall,
                                 Func<bool> _isHeavy, Action<bool> _ChangeHeavy) : base(myState, _sm)
        {
            attackRecall = recall;
            IsHeavy = _isHeavy;
            ChangeHeavy = _ChangeHeavy;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            if (IsHeavy())
            {
                Debug.Log("Hice un Heavy re loco");
                charMove.MovementHorizontal(0);
                charMove.MovementVertical(0);
            }
        }

        protected override void Update()
        {
            if (!IsHeavy())
            {
                charMove.MovementHorizontal(LeftHorizontal());
                charMove.MovementVertical(LeftVertical());
            }
            //timer += Time.deltaTime;

            //if (timer >= attackRecall)
            //    sm.SendInput(CharacterHead.PlayerInputs.IDLE);
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
            ChangeHeavy(false);
        }
    }
}
