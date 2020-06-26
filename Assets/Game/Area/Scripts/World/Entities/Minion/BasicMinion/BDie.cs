using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class BDie : BMinionStates
    {
        public BDie(EState<BasicMinion.BasicMinionInput> myState, EventStateMachine<BasicMinion.BasicMinionInput> _sm) : base(myState, _sm) { }
        protected override void Enter(EState<BasicMinion.BasicMinionInput> input) => anim.SetBool("dead", true);
    }
}

