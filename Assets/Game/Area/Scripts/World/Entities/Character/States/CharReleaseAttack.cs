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
        Animator anim;
        Func<bool> WaitAttack;
        ParticleSystem dashAttackParticles;
        float timer;
        bool enter;

        public CharReleaseAttack(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, float recall,
                                 Func<bool> _isHeavy, Action<bool> _ChangeHeavy, Animator _anim, Func<bool> _WaitAttack, ParticleSystem moveHeavyDash) : base(myState, _sm)
        {
            attackRecall = recall;
            IsHeavy = _isHeavy;
            ChangeHeavy = _ChangeHeavy;
            anim = _anim;
            WaitAttack = _WaitAttack;
            dashAttackParticles = moveHeavyDash;

        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            if (IsHeavy())
            {
                charMove.MovementHorizontal(0);
                charMove.MovementVertical(0);
                dashAttackParticles.Play();
            }
            enter = true;
        }

        protected override void Update()
        {
            if (!IsHeavy())
            {
                charMove.MovementHorizontal(LeftHorizontal());
                charMove.MovementVertical(LeftVertical());
                charMove.RotateHorizontal(RightHorizontal());
                charMove.RotateVertical(RightVertical());
            }

            var info = anim.GetCurrentAnimatorStateInfo(2);

            if (info.IsName("CentralState"))
            {
                if (LeftHorizontal() == 0 && LeftVertical() == 0)
                {
                    sm.SendInput(CharacterHead.PlayerInputs.IDLE);
                }
                else
                {
                    sm.SendInput(CharacterHead.PlayerInputs.MOVE);
                }
            }

            if (enter)
            {
                if (timer < attackRecall)
                {
                    timer = timer + 1 * Time.deltaTime;
                }
                else
                {
                    sm.SendInput(CharacterHead.PlayerInputs.IDLE);
                    enter = false;
                    timer = 0;
                }
            }
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
            enter = true;
            timer = 0;
            ChangeHeavy(false);
            dashAttackParticles.Stop();
        }
    }
}
