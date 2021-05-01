using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.EventClasses;

public class BrazaleteCondition : MonoBehaviour
{
    public EventCounterPredicate contrapredicado;

    public static BrazaleteCondition instance;
    private void Awake() => instance = this;

    bool iHaveBracelet = false;

    public static void TakeBracelet() => instance.iHaveBracelet = true;

    private void Start()
    {
        contrapredicado.Invoke(Condicion);
    }

    bool Condicion()
    {
        return iHaveBracelet;
    }
}
