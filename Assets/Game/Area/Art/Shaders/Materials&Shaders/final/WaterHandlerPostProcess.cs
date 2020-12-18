using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHandlerPostProcess : MonoBehaviour
{
    public void Blend(float blend)
    {
        WaterfallPostProcess.instance.Blend(blend);
    }
}
