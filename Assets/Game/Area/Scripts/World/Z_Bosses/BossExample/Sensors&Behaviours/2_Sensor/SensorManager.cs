using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
    [Header("Valores a mano")]
    Transform own;
    public SensorDistance sensor_distance;
    public Sensor_HandRail sensor_death;
    public Sensor_HandRail sensor_hit;

    public void InitializeSensors(Transform root)
    {
        own = root;
        sensor_distance.Configure_Transforms(own, Main.instance.GetChar().transform);
    }

    public void  StartSensors()
    {
        sensor_distance.StartSensor();
    }
    public void StopSensors()
    {
        sensor_distance.StopSensor();
    }
}
