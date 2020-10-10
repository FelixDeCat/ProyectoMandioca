using System.Collections.Generic;
using UnityEngine;
using System;

public static class QuestionConnection
{
    public static Func<bool>[] Connect(IQuestionable questionable, Component[] posibleQuestions)
    {
        List<Func<bool>> selected = new List<Func<bool>>();
        for (int i = 0; i < posibleQuestions.Length; i++)
        {
            var answer = posibleQuestions[i].GetComponent<IAnswerable>();
            if (answer != null)
            {
                selected.Add(Convert(answer.Answer.Predicates, answer.Answer.CustomGate));
            }
        }
        Func<bool>[] aux;
        aux = new Func<bool>[selected.Count];
        for (int i = 0; i < selected.Count; i++)
        {
            aux[i] = selected[i];
        }
        return aux;
    }

    static Func<bool> Convert(Func<bool>[] funcs, Gate gate = Gate.AND)
    {
        bool aux = false;

        if (gate == Gate.AND)
        {
            for (int i = 0; i < funcs.Length; i++)
            {
                if (funcs[i].Invoke())
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
            for (int i = 0; i < funcs.Length; i++)
            {
                if (funcs[i].Invoke())
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

        return () => aux;
    }
}
