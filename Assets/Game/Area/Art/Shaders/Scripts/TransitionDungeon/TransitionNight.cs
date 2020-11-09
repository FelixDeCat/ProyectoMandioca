using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class TransitionNight : MonoBehaviour
{
    public PostProcessVolume firstZone;
    public PostProcessVolume night;


    public void NightStart()
    {
        firstZone.isGlobal = false;
        night.isGlobal = true;
        
    }
}
