using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Fade_PostProcess : Switcheable
{
    public PostProcessVolume pp;

    protected void UNITY_EVENT_OnFade(float val) => pp.weight = val;
    protected void UNITY_EVENT_OnTurnOff() => pp.weight = 0;
    protected void UNITY_EVENT_OnTurnOn() => pp.weight = 1;

    public override void ABSOnFade(float f)=> pp.weight = f;
    public override void ABSOnTurnOff()=> pp.weight = 0;
    public override void ABSOnTurnON() => pp.weight = 1;

}
