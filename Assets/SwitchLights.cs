using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLights : MonoBehaviour
{
    public void EnableMainDirectionalLight()
    {
        DirectionalMain.instance?.EnableDirectional(true);
    }
    public void DisableMainDirectionalLight()
    {
        DirectionalMain.instance?.EnableDirectional(false);
    }
}
