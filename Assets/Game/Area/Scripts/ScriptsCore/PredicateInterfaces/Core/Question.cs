using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Gate { AND, OR }
public class Question
{
    private readonly Func<bool>[] predicates;
    private readonly Gate gate = Gate.AND;

    public Question(Gate gate = Gate.AND, params Func<bool>[] pred)
    {
        this.gate = gate;
        predicates = pred;
    }
    
    public bool Ask()
    {
        bool aux = false;

        if (gate == Gate.AND)
        {
            for (int i = 0; i < predicates.Length; i++)
            {
                if (predicates[i].Invoke())
                {
                    aux = true;
                }
                else
                {
                    aux = false;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < predicates.Length; i++)
            {
                if (predicates[i].Invoke())
                {
                    aux = true;
                    break;
                }
                else
                {
                    aux = false;
                    
                }
            }
        }

        return aux;
    }
}
