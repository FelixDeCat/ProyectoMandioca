using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSwitchComponent : Switcheable
{
    public override void ABSOnFade(float f) { Debug.Log("On Fade usando la Interfaz: val: " + f); }
    public override void ABSOnTurnOff() { Debug.Log("On Turn OFF usando la interfaz"); }
    public override void ABSOnTurnON() { Debug.Log("OnTurnON usando UNITY_EVENTS"); }

    //un ejemplo de como lo usaria si lo uso con UNITY EVENTS
    public void UNITY_EVENT_OnFade(float val) { Debug.Log("On Fade usando UNITY_EVENTS: val: " + val); }
    public void UNITY_EVENT_OnTurnOff() { Debug.Log("OnTurnON usando UNITY_EVENTS"); }
    public void UNITY_EVENT_OnTurnOn() { Debug.Log("OnTurnON usando UNITY_EVENTS"); }
}
