using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Idle : MonoStateBase
{
    protected override void OnOneAwake() { }
    protected override void OnBegin()
    {
        Get_Sensors.sensor_distance.StartSensor();
    }

    protected override void OnExit()
    {
        Get_Sensors.sensor_distance.StopSensor();
    }

    protected override void OnUpdate()
    {
        Debug.Log("IDLE: OnUpdate");
    }
}
