using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Idle : MonoStateBase
{
    [SerializeField] SensorManager sensors;

    protected override void OnBegin()
    {
        sensors.sensor_distance.StartSensor();
    }

    protected override void OnExit()
    {
        sensors.sensor_distance.StopSensor();
    }

    protected override void OnUpdate()
    {
        Debug.Log("IDLE: OnUpdate");
    }
}
