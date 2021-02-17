using System.Collections.Generic;
using IA2Final.FSM;
using UnityEngine;
using System;

namespace IA2Final { 
public class GOAPAction {

    public List<Func<GOAPValue, bool>> preconditions { get; private set; }
    public List<Action<GOAPValue>> effects       { get; private set; }
    public string                   name          { get; private set; }
    public float                    cost          { get; private set; }
    public IState                   linkedState   { get; private set; }


    public GOAPAction(string name) {
        this.name     = name;
        cost          = 1f;
            preconditions = new List<Func<GOAPValue, bool>>();
            effects = new List<Action<GOAPValue>>();
    }

    public GOAPAction Cost(float cost) {
        if (cost < 1f) {
            //Costs < 1f make the heuristic non-admissible. h() could overestimate and create sub-optimal results.
            //https://en.wikipedia.org/wiki/A*_search_algorithm#Properties
            Debug.Log(string.Format("Warning: Using cost < 1f for '{0}' could yield sub-optimal results", name));
        }

        this.cost = cost;
        return this;
    }

    public GOAPAction Pre(string s, Func<GOAPValue, bool> value) {
        preconditions.Add(value);
        return this;
    }
        public GOAPAction Effect(string s, Action<GOAPValue> changeValue) {
        effects.Add(changeValue);
        return this;
        }

    public GOAPAction LinkedState(IState state) {
        linkedState = state;
        return this;
    }
}
}
