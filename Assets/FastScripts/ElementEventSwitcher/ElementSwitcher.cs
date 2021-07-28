using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElementSwitcher : MonoBehaviour
{
    public static ElementSwitcher instance;
    private void Awake() => instance = this;

    const string UNIQUE = "Zaraza123";
    const string TURN_ON = "prender";
    const string TURN_OFF = "apagar";

    public static void Subscribe(string elem, Action TurnOn, Action TurnOff) => instance.Subscribe_NewElement(elem, TurnOn, TurnOff);
    public static void On(string elem) => instance.Execute_TurnOn(elem);
    public static void Off(string elem) => instance.Execute_TurnOff(elem);

    void Subscribe_NewElement(string elem, Action TurnOn, Action TurnOff)
    {
        Main.instance.eventManager.SubscribeToEvent(UNIQUE + TURN_ON + elem, TurnOn);
        Main.instance.eventManager.SubscribeToEvent(UNIQUE + TURN_OFF + elem, TurnOff);
    }
    void Execute_TurnOn(string elem) => Main.instance.eventManager.TriggerEvent(UNIQUE + TURN_ON + elem);
    void Execute_TurnOff(string elem) => Main.instance.eventManager.TriggerEvent(UNIQUE + TURN_OFF + elem);
}
