using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
    [Header("Valores a mano")]
    Transform own;
    public SensorDistance sensor_distance;

    public void InitializeSensors(Transform root)
    {
        own = root;
        sensor_distance.Configure_Transforms(own, Main.instance.GetChar().transform);
    }
}
