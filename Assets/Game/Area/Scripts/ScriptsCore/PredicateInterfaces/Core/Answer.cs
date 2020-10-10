using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Answer
{
    Func<bool>[] predicates;
    Gate gate = Gate.AND;
    public Gate CustomGate { get { return gate; } }
    public Func<bool>[] Predicates { get { return predicates; } }
    public Answer(Gate gate = Gate.AND, params Func<bool>[] pred)
    {
        this.gate = gate;
        predicates = pred;
    }
}
