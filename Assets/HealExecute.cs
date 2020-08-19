using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;

public class HealExecute : MonoBehaviour
{
    public int healval;

    public EventCounterPredicate pred;

    private void Start()
    {
        pred.Invoke(Pred);
    }

    public void Heal()
    {
        Main.instance.GetChar().Life.Heal(healval);
    }

    public bool Pred()
    {
        return Main.instance.GetChar().Life.CanHeal();
    }

}
