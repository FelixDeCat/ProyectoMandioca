using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IA2Final.FSM
{
    public class FinalBossIdle : MonoBaseState
    {
        public override event Action OnNeedsReplan;

        public FinalBossIdle()
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
