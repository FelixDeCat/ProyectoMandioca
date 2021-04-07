using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IA2Final.FSM
{
    public class FinalBossStun : MonoBaseState
    {
        public override event Action OnNeedsReplan;

        Rigidbody rb;


        public FinalBossStun()
        {
        }

        public override void UpdateLoop()
        {
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            return base.Exit(to);
        }

        public override IState ProcessInput()
        {
            return this;
        }
    }
}
