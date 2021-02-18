using System;
using System.Collections.Generic;
using UnityEngine;

namespace IA2Final.FSM
{
    public class BossShootAbility : MonoBaseState
    {
        public override event Action OnNeedsReplan;

        public BossShootAbility()
        {
        }

        public override void UpdateLoop()
        {
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            base.Enter(from, transitionParameters);
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
