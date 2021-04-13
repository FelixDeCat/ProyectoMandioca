using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IA2Final.FSM
{
    public class FinalBossFly : MonoBaseState
    {
        public override event Action OnNeedsReplan;

        Animator anim;
        Rigidbody rb;
        BetoBoss boss;
        bool flyOver;

        public FinalBossFly(BetoBoss _boss, Animator _anim, Rigidbody _rb)
        {
            boss = _boss;
            anim = _anim;
            rb = _rb;
        }

        public override void UpdateLoop()
        {

        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            flyOver = false;
            rb.useGravity = false;
            anim.Play("StartFly");
            anim.SetBool("StartFly", true);
            boss.Flying = true;
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            anim.SetBool("StartFly", false);
            anim.SetFloat("Flying", 1);
            return base.Exit(to);
        }

        public override IState ProcessInput()
        {
            return this;
        }
    }
}
