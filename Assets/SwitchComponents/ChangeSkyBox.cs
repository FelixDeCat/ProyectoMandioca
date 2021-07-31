using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ChangeSkyBox : Switcheable
{
    public PostProcessVolume pp;

    public Material mat;

    public Color groundColor;
    public Color skyColor;

    public Color groundColor_2;
    public Color skyColor_2;

    public override void ABSOnFade(float f)
    {
        
    }

    public override void ABSOnTurnOff()
    {
        mat.SetColor("_GroundColor", groundColor_2);
        mat.SetColor("_SkyColor", skyColor_2);
    }

    public override void ABSOnTurnON()
    {
        mat.SetColor("_GroundColor", groundColor);
        mat.SetColor("_SkyColor", skyColor);
    }
}
