using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GoapAction
    {
        //public Func<WorldStateSnapShot, bool> preconditions { get; private set; }
        public Func<GoapState, bool> preconditions { get; private set; }
        public Action<GoapState> effects { get; private set; }
        //public Action<WorldStateSnapShot> effects { get; private set; }
        public string Name { get; private set; }
        public float Cost { get; private set; }

        public GoapAction(string name)
        {
            this.Name = name;
            Cost = 1f;
            preconditions = (ws) => true;
            effects = delegate { };
        }

        public GoapAction SetCost(float cost)
        {
            if (cost < 1f)
            {
                Debug.Log(string.Format("Warning: Using cost < 1f for '{0}' could yield sub-optimal results", Name));
            }
            this.Cost = cost;
            return this;
        }
        public GoapAction Pre(Func<GoapState, bool> pre)//(Func<WorldStateSnapShot, bool> pre)
        {
            preconditions = pre;
            return this;
        }
        public GoapAction Effect(Action<GoapState> eff)//(Action<WorldStateSnapShot> eff)
        {
            effects = eff;
            return this;
        }
    }
}